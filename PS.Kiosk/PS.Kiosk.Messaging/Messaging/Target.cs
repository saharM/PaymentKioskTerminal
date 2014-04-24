using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Fanap
{
    public class csTarget
    {
        private bool flag = false;
        public csTarget()
        {
            try
            {
                Start();
            }
            catch
            {
            }
        }
        public void Start()
        {
            Thread _thTarg = new Thread(new ThreadStart(Listen));
            _thTarg.Start();
        }
        private void Listen()
        {
           
        }
    }
}
