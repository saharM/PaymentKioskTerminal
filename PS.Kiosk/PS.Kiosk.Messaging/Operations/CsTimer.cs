using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PS.Kiosk.Messaging.Operations
{
	public class CsTimer
	{
		private bool _started;
        private long _timeOut;
        private DateTime _time;
        public long TimeOut
		{
			get
			{
				return _timeOut;
			}
			set
			{
				_timeOut = value;
			}
		}
		public CsTimer(long timeout)
		{
			_timeOut = timeout;
			_started = false;
		}
		public void Start()
		{
			_started = true;
			_time = System.DateTime.Now;	 		
		}
		public void Stop()
		{
			_started = false;
		}
		public bool isTimeOut()
		{
			return (_started && ElapsedTime() > _timeOut);
		}
		public bool isActive()
		{
			return _started;
		}
		private long ElapsedTime()
		{
			TimeSpan TS = System.DateTime.Now - _time;
			return TS.Ticks / 10000;
		}
	}
}
