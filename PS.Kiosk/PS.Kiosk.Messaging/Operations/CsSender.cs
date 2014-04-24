using System;
using System.IO;
//using System.Windows.Forms;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Globalization;
using Fanap.Messaging;
using Fanap.Messaging.Iso8583;
using System.Collections;
using PS.Kiosk.Framework;


namespace PS.Kiosk.Messaging.Operations
{
    enum StateSend { Connect, Send, Receive, Close, Finish, Error, Reversal };

    enum ReadState
    {
        RS_PacketLength,
        RS_ReadPacket
    }

    public delegate void OnReceivedDataDelegate(Fanap.Messaging.Message message);

    public class CsSender
    {
        #region Variable
        private const int HEADER_LEN = 4;
        private FormatterContext _formatterContext;
        private ParserContext _parserContext;
        private static Socket socket;//= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private string _ip;
        private int _port;
        private byte[] _header = new byte[16];
        private Fanap.Messaging.Message _messageout = null;
        private bool _isConnected = false;
        private bool _isReversal = false;
        private byte[] _MacKey;
        private Message Mymessage;

        #endregion

        #region Properties
        public byte[] MACKey
        {
            set
            {
                _MacKey = value;
            }
        }
        public bool isReversal
        {
            get
            {
                return _isReversal;
            }
        }
        public Fanap.Messaging.Message TrxMessage
        {
            get
            {
                return _messageout;
            }
        }
        #endregion

        #region Constructors
        public CsSender(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
            _parserContext = new ParserContext(ParserContext.DefaultBufferSize);
            _isConnected = false;
            //_formatter = new Fanap.Messaging.Iso8583.Iso8583Ascii1987MessageFormatter();
            Connect();
        }


        #endregion

        #region Methods


        private bool Connect()
        {
            if (socket != null && socket.Connected)
                return true;
            try
            {
                if (socket != null && !socket.Connected)
                    Close();
            }
            catch
            {
            }
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(_ip, _port);
                //Using SSL
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                socket.ReceiveBufferSize = Int32.MaxValue;
                socket.SendBufferSize = Int32.MaxValue;
                _isConnected = true;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(ex, "Error in Socket Connection Method");
                try
                {
                    socket.Close();
                }
                catch
                {
                }
                socket = null;
                _isConnected = false;

                return false;
            }
            return socket.Connected;
        }

        private bool SendMessage(Fanap.Messaging.Message message)
        {
            KioskLogger.Instance.LogMessage("Ready For Sending Message in SendMessage Method");
          
            if (message == null)
            {
                return false;
            }
            if (socket == null || !socket.Connected || !_isConnected)
            {
                return false;
            }

            try
            {
                _messageout = null;
                int plen = 2;
                _parserContext.Clear();
                _formatterContext.Clear();
                message.Formatter = message.Formatter;// _formatter;
                message.Formatter.Format(message, ref _formatterContext);
                byte[] PacketHeader;
                int llk = 7;
                int LEN = 0;
                //be jaye 0x16 - > 0x61 = server testi mishavad
                byte[] Switch = { 0x60, 0x00, 0x61, 0x00, 0x00 } ;
                byte[] Fanap = { 0x60, 0x00, 0x18, 0x00, 0x00 } ;
                byte[] bte = new byte[5];
                if (((Iso8583Message)message).MessageTypeIdentifier < 1000)
                    LEN = 4;

                byte[] PacketWithHeader = new byte[LEN + _formatterContext.DataLength + llk];

                if(((Iso8583Message)message).MessageTypeIdentifier > 1000)
                   Array.Copy(Switch , bte , Switch.Length) ;
                else
                    Array.Copy(Fanap, bte, Fanap.Length);

                Array.Copy(bte, 0, PacketWithHeader, plen, bte.Length);
                CsUtil util = new CsUtil();
                bte = util.HexToBin((PacketWithHeader.Length - 2).ToString().PadLeft(4, '0'));
                Array.Copy(bte, 0, PacketWithHeader, 0, plen);
                Array.Copy(_formatterContext.GetBuffer(), 0, PacketWithHeader, LEN + llk, _formatterContext.DataLength);
                if (((Iso8583Message)message).MessageTypeIdentifier < 1000)
                {
                    PacketHeader = Utilities.UtilityMethods.StrtoByte(_formatterContext.DataLength.ToString().PadLeft(4, '0'));
                    Array.Copy(PacketHeader, 0, PacketWithHeader, llk, 4);
                }
                int sendByte = 0;
                socket.SendTimeout = 10000;


                
                sendByte = socket.Send(PacketWithHeader, 0, PacketWithHeader.Length, SocketFlags.None);
                string sss = System.Text.Encoding.ASCII.GetString(PacketWithHeader);
                KioskLogger.Instance.LogMessage("Packet Send Byte Count = " + sendByte.ToString());
                if (sendByte == 0)
                {
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(ex, "Error in SendMessage");
                return false;
            }
            return true;
        }

        private bool Receive(IMessageFormatter _formatter)
        {

            KioskLogger.Instance.LogMessage("Ready For Receive Message in Receive Method");
            _messageout = null;

            try
            {
                byte[] buff = new byte[2048];
                byte[] buff2 = new byte[2048];
                int PacketLength = 4;

                CsTimer timer = new CsTimer(50000);
                timer.Start();
                while (socket.Available == 0 && timer.isActive() && !timer.isTimeOut()) Thread.Sleep(50);

                socket.ReceiveTimeout = 60000;

                if (socket.Available == 0)
                {
                    KioskLogger.Instance.LogMessage("Socket is Not Available -Error in Receive Message");
                    return false;
                }
                int len = 0;
                if (_formatter.GetType() == typeof(Iso8583Ascii1993MessageFormatter))
                {
                    len = socket.Receive(buff, 2, SocketFlags.None);
                    len = Utilities.UtilityMethods.Byte2IntBCD(buff, 2);

                    len = socket.Receive(buff, len, SocketFlags.None);
                    Array.Copy(buff, 5, buff2, 0, len - 5);
                    buff = buff2;
                    len = len - 5;
                }
                else
                {
                    len = socket.Receive(buff, 7, SocketFlags.None);
                    len = socket.Receive(buff, PacketLength, SocketFlags.None);
                    len = Utilities.UtilityMethods.Byte2Int(buff, PacketLength);
                    len = socket.Receive(buff, 0, len, SocketFlags.None);
                }
                _formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
                _parserContext = new ParserContext(ParserContext.DefaultBufferSize);
                Array.Copy(buff, 0, _parserContext.GetBuffer(), 0, len);
                _parserContext.UpperDataBound = len;
                if (_parserContext.DataLength > 0)
                    _messageout = _formatter.Parse(ref _parserContext);
                if (_messageout == null)
                {

                    if (_parserContext.FreeBufferSpace == 0)
                        _parserContext.ResizeBuffer(ParserContext.DefaultBufferSize);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(ex, "Error in Receive Message");
                return false;
            }

        }
      
        public bool SendPacketSync(Iso8583Message inMessage)
        {
            try
            {
                StateSend state = StateSend.Connect;
                while (state != StateSend.Finish)
                {
                    switch (state)
                    {
                        case StateSend.Connect:
                            state = StateSend.Send;
                            if (!Connect())
                                state = StateSend.Error;
                            break;
                        case StateSend.Send:
                            state = StateSend.Receive;
                            if (!SendMessage(inMessage))
                                state = StateSend.Error;
                            break;
                        case StateSend.Receive:
                            state = StateSend.Close;
                            if (!Receive(inMessage.Formatter))
                                state = StateSend.Reversal;
                            break;
                        case StateSend.Reversal:
                            state = StateSend.Error;
                            break;
                        case StateSend.Close:
                            Close();
                            state = StateSend.Finish;
                            break;
                        default:
                            Close();
                            return false;
                    }
                }
                return true;
            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX, "Exception Came in SendPacketSync in csSender Class");
                throw EX;
            }
        }

        private void Close()
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                }
                socket = null;
            }
            _parserContext.Clear();
            _formatterContext.Clear();
        }

        #endregion

    }
}