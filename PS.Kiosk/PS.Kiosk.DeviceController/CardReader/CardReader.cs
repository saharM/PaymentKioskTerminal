using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace PS.Kiosk.DeviceController
{
    public enum CardPosition
    {
        There_is_card_at_the_front_side = 0,
        There_is_card_at_the_front_card_hold_position = 1,
        There_is_card_in_the_reader = 2,
        There_is_card_at_IC_card_operation_position = 3,
        There_is_card_at_the_rear_card_hold_position = 4,
        There_is_card_at_the_rear_side = 5,
        No_Card_In_The_Reader = 6
    }

    internal class CardReader : IDisposable
    {
        public bool CanRaise = false;

        #region enum
        private enum MoveCardPos
        {
            Stop_at_the_Front = 0x30,
            Stop_at_the_front_hold_the_card = 0x31,
            Stop_at_the_RF_card_operation_position = 0x2E,
            Stop_at_the_IC_card_operation_position = 0x2F,
            Stop_at_the_rear_hold_the_card = 0x32,
            Stop_at_the_rear = 0x33,
            Eject_abnormal_card_from_rear_side = 0x35,
            Clean_Reader = 0x37
        }

        private enum ResetDevice
        {
            Reset_Without_Eject = 0x30,
            Reset_and_Eject_to_Front = 0x31,
            Reset_and_Eject_to_Rear = 0x32
        }


        public enum CardPermit
        {
            Prohibit_the_cards_in = 0x31,
            Card_in_by_mag_signal_and_switch = 0x32,
            Permit_all_cards_in = 0x33,
            Card_in_by_mag_signal = 0x34
        }

        public enum RearPermit
        {
            Permit_all_cards_in_from_rear = 0x30,
            Prohibit_the_cards_in_from_rear = 0x31
        }
        #endregion

        #region Property And Variable
        public object[] Logger;

        public bool HaveError = false;

        private const int ENQ = 0x5;
        private SerialPort CSerial = new SerialPort();
        private string _Setting = "Com1,9600,N,8,1,N";
        public bool Shutdown = false;
        public string PortName = "COM1";
        public int BaudRate = 9600;
        public Parity Parity = Parity.None;
        public int DataBits = 8;
        public StopBits StopBits = StopBits.One;
        public Handshake Handshake = Handshake.None;

        public int LastErrorNumber = 0;
        public string LastErrorDescription = "";
        public CardPosition LastCardPosition;
        private CardPosition _CurrentCardPosition = CardPosition.No_Card_In_The_Reader;
        public CardPosition CurrentCardPosition
        {
            get
            {
                return _CurrentCardPosition;
            }
            set
            {
                _CurrentCardPosition = value;
            }
        }
        public RearPermit CurrentRearPermit;
        private CardPermit _CurrentCardPermit;
        public CardPermit CurrentCardPermit
        {
            get
            {
                return _CurrentCardPermit;
            }
            set
            {
                _CurrentCardPermit = value;
                //SetAccess(_CurrentCardPermit, CurrentRearPermit);
            }
        }

        public int CaptureCardAfter = 30;
        public string Track1 = "";
        public string Track2 = "";
        public string Track3 = "";
        public string CardTrack = "";
        public string CardID = "";
        public string CardName = "";
        public bool ISConnected
        {
            get
            {
                return CSerial.IsOpen;
            }
        }


        public event CardPositionChangedEventHandler CardPositionChanged;
        public delegate void CardPositionChangedEventHandler();
        public event CardCaptureEventHandler CardCapture;
        public delegate void CardCaptureEventHandler();
        private Thread AutoEject;
        public string Setting
        {
            get
            {
                return _Setting;
            }
            set
            {
                _Setting = value.ToUpper();
                if (_Setting == "")
                {
                    return;
                }
                string[] v = _Setting.Split(',');
                PortName = v[0];
                BaudRate = int.Parse(v[1]);
                switch (v[2])
                {
                    case "N":
                        Parity = Parity.None;
                        break;
                    case "O":
                        Parity = Parity.Odd;
                        break;
                    case "M":
                        Parity = Parity.Mark;
                        break;
                    case "S":
                        Parity = Parity.Space;
                        break;
                }
                DataBits = int.Parse(v[3]);
                switch (v[4])
                {
                    case "0":
                        StopBits = StopBits.None;
                        break;
                    case "1":
                        StopBits = StopBits.One;
                        break;
                    case "1.5":
                        StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        StopBits = StopBits.Two;
                        break;
                }
                if (v.Length > 5)
                {
                    switch (v[5])
                    {
                        case "N":
                            Handshake = Handshake.None;
                            break;
                        case "RTS":
                            Handshake = Handshake.RequestToSend;
                            break;
                        case "RTSX":
                            Handshake = Handshake.RequestToSendXOnXOff;
                            break;
                        case "X":
                            Handshake = Handshake.XOnXOff;
                            break;
                    }
                }
                else
                {
                    Handshake = Handshake.None;
                }
            }
        }


        #endregion

        #region Methods


        public bool Connect()
        {
            if (string.Compare(PortName, "None", true) == 0)
            {
                return true;
            }

            try
            {
                CSerial.PortName = PortName;
                CSerial.BaudRate = BaudRate;
                CSerial.Parity = Parity;
                CSerial.DataBits = DataBits;
                CSerial.StopBits = StopBits;
                CSerial.Handshake = Handshake;
                CSerial.ReadTimeout = 500;
                CSerial.WriteTimeout = 500;
                try
                {
                    CSerial.Open();
                    if (ResetReader() == "OK")
                    {
                        ResetReader1();
                        EnabledShutter(true);
                        //MoveCard();
                        Shutdown = false;
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                catch (Exception ex)
                {
                    Disconnect();
                }
            }

            catch (Exception ex)
            {

            }
            HaveError = !CSerial.IsOpen;
            return CSerial.IsOpen;
        }


        public bool Disconnect()
        {
            try
            {
                CardEject();
            }
            catch (Exception ex)
            {
            }

            while (AutoEject.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(1000);
            }
            Shutdown = true;
            EnabledShutter(false);
            CSerial.Close();

            HaveError = CSerial.IsOpen;
            return !CSerial.IsOpen;
        }

        public bool CardEject()
        {
            bool Ret;
            Ret = CardEjectS();
            return Ret;
        }

        private bool CardEjectS()
        {
            bool Ret;
            Ret = MoveCard(MoveCardPos.Stop_at_the_front_hold_the_card);
            if (CaptureCardAfter > 0)
            {
                AutoEject = new Thread(AutoEjectCheck);
                AutoEject.Name = "CardReaderAutoEject";
                AutoEject.Start();
            }
            return Ret;
        }

        private void AutoEjectCheck()
        {
            DateTime EndTime;
            EndTime = DateTime.Now.AddSeconds(CaptureCardAfter);
            while (EndTime > DateTime.Now)
            {
                DeviceStatus();
                if (CurrentCardPosition == CardPosition.No_Card_In_The_Reader || !CSerial.IsOpen)
                {
                    break;
                }
            }
            if (CurrentCardPosition == CardPosition.There_is_card_at_the_front_card_hold_position)
            {
                CaptureCard();
            }
            Thread.EndThreadAffinity();
        }

        public bool CaptureCard()
        {
            bool Ret;
            Ret = MoveCard(MoveCardPos.Stop_at_the_rear);
            if (CardCapture != null)
            {
                CardCapture();
            }
            return Ret;
        }

        private string LastCommand = "";

        #region Read Track

        private bool ReadTracks()
        {
            bool Noback = true;
            int BackLen = 0;
            byte[] Re = new byte[1024];
            bool Ret = false;

            if (!CSerial.IsOpen)
            {
                LastErrorNumber = 900;
                LastErrorDescription = "Comm. Port Setup First";
                HaveError = !Ret;
                return Ret;
            }

            Track1 = "";
            Track2 = "";
            Track3 = "";
            LastErrorNumber = 0;
            LastErrorDescription = "";

            lock (CSerial)
            {
                try
                {
                    byte[] Sendbuf = { 2, 0, 4, 0x45, 0x30, 0x30, 0x37, 3, 0 };
                    Sendbuf[8] = CheckXOR(Sendbuf, 8);
                    CSerial.Write(Sendbuf, 0, Sendbuf.Length);

                    BackLen = 0;
                    Thread.Sleep(100);
                    if (CSerial.BytesToRead > 0)
                    {
                        BackLen = CSerial.BytesToRead;
                        CSerial.Read(Re, 0, BackLen);
                        if (Re[0] == 6)
                        {
                            CSerial.Write(((char)ENQ).ToString());
                            Ret = lReadTraks();
                        }
                    }
                    else
                    {
                        Noback = true;
                    }
                }
                catch (Exception ex)
                {
                    //'Logger.Write(ex)

                }
                if (Noback)
                {
                    if (LastErrorNumber == 0)
                    {
                        LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                    }
                }
                CardTrack = "";
                CardID = "";
                CardName = "";
                if (Track2 != "")
                {
                    CardTrack = Track2.Replace(";", "").Replace("?", "").Replace(" ", "").Trim();
                    if (CardTrack.IndexOf("=") > 0)
                    {
                        CardID = CardTrack.Substring(0, CardTrack.IndexOf("="));
                    }
                }
                if (Track1.IndexOf("^") > -1)
                {
                    CardName = Track1.Substring(Track1.IndexOf("^") + 1);
                    CardName = CardName.Substring(0, CardName.IndexOf("^"));
                    CardName = CardName.Replace("/", " ");
                }
            }
            return Ret;
        }

        private bool lReadTraks()
        {
            bool Noback = true;
            int BackLen = 0;
            byte[] Re = new byte[1024];
            DateTime EndTime;
            int Temp;
            int weizhi1 = 0;
            int weizhi2 = 0;
            int weizhi3 = 0;
            string TrackTemp = "";

            try
            {
                Noback = true;
                BackLen = 0;
                EndTime = DateTime.Now.AddSeconds(4);
                while (EndTime > DateTime.Now)
                {
                    if (CSerial.BytesToRead >= 3)
                    {
                        BackLen = 3;
                        CSerial.Read(Re, 0, BackLen);
                        Noback = false;
                        break;
                    }
                }

                Temp = Re[0] ^ Re[1] ^ Re[2];

                double len1;
                if (Re[1] != 0)
                {
                    string Re1Hex = "0" + Re[1].ToString("X");
                    string Re2Hex = "0" + Re[2].ToString("X");
                    len1 = double.Parse("0x" + Re1Hex.Substring(Re1Hex.Length - 2) + Re2Hex.Substring(Re2Hex.Length - 2));
                }
                else
                {
                    len1 = Re[2];
                }
                len1 = len1 + 2;

                if (!Noback)
                {
                    EndTime = DateTime.Now.AddSeconds(3);

                    while (EndTime > DateTime.Now)
                    {
                        if (CSerial.BytesToRead >= len1)
                        {
                            BackLen = CSerial.BytesToRead;
                            Array.Clear(Re, 0, Re.Length);
                            CSerial.Read(Re, 0, BackLen);
                            break;
                        }
                    }

                    Temp = Temp ^ CheckXOR(Re, BackLen - 1);
                    if (BackLen > 5)
                    {
                        weizhi1 = 4;
                        for (int i = 5; i <= BackLen - 2; i++)
                        {
                            if (Re[i] == 31)
                            {
                                weizhi2 = i;
                                break;
                            }
                        }
                        for (int i = weizhi2 + 1; i <= BackLen - 2; i++)
                        {
                            if (Re[i] == 31)
                            {
                                weizhi3 = i;
                                break;
                            }
                        }

                        switch (Re[weizhi1 + 1])
                        {
                            case 89:
                                TrackTemp = "";
                                for (int i = weizhi1 + 2; i <= weizhi2 - 1; i++)
                                {
                                    TrackTemp += (char)Re[i];
                                }
                                Track1 = TrackTemp;
                                break;
                            case 78:
                                Track1 = "";
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 1: Read/Parity Error. ";
                                switch (Re[weizhi1 + 2])
                                {
                                    case 225:
                                        LastErrorDescription += "No start bits (STX)";
                                        break;
                                    case 226:
                                        LastErrorDescription += "No stop bits (ETX)";
                                        break;
                                    case 227:
                                        LastErrorDescription += "Byte Parity Error(Parity))";
                                        break;
                                    case 228:
                                        LastErrorDescription += "Parity Bit Error(LRC)";
                                        break;
                                    case 229:
                                        LastErrorDescription += "Card Track Data is Blank";
                                        break;
                                }
                                break;
                            case 79:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 1: No Read for this Track. Card Track Data is 0xE0";
                                break;
                            case 0:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 1: No Read Operation. Card Track Data is 0x00";
                                break;
                        }
                        switch (Re[weizhi2 + 1])
                        {
                            case 89:
                                TrackTemp = "";
                                for (int i = weizhi2 + 2; i <= weizhi3 - 1; i++)
                                {
                                    TrackTemp += (char)Re[i];
                                }
                                Track2 = TrackTemp;
                                break;
                            case 78:
                                Track2 = "";
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 2: Read/Parity Error. ";
                                switch (Re[weizhi2 + 2])
                                {
                                    case 225:
                                        LastErrorDescription += "No start bits (STX)";
                                        break;
                                    case 226:
                                        LastErrorDescription += "No stop bits (ETX)";
                                        break;
                                    case 227:
                                        LastErrorDescription += "Byte Parity Error(Parity))";
                                        break;
                                    case 228:
                                        LastErrorDescription += "Parity Bit Error(LRC)";
                                        break;
                                    case 229:
                                        LastErrorDescription += "Card Track Data is Blank";
                                        break;
                                }
                                break;
                            case 79:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 2: No Read for this Track. Card Track Data is 0xE0";
                                break;
                            case 0:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 2: No Read Operation. Card Track Data is 0x00";
                                break;
                        }

                        switch (Re[weizhi3 + 1])
                        {
                            case 89:
                                TrackTemp = "";
                                for (int i = weizhi3 + 2; i <= BackLen - 3; i++)
                                {
                                    TrackTemp += (char)Re[i];
                                }
                                Track3 = TrackTemp;
                                break;
                            case 78:
                                Track3 = "";
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 3: Read/Parity Error. ";
                                switch (Re[weizhi3 + 2])
                                {
                                    case 225:
                                        LastErrorDescription += "No start bits (STX)";
                                        break;
                                    case 226:
                                        LastErrorDescription += "No stop bits (ETX)";
                                        break;
                                    case 227:
                                        LastErrorDescription += "Byte Parity Error(Parity))";
                                        break;
                                    case 228:
                                        LastErrorDescription += "Parity Bit Error(LRC)";
                                        break;
                                    case 229:
                                        LastErrorDescription += "Card Track Data is Blank";
                                        break;
                                }
                                break;
                            case 79:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 3: No Read for this Track. Card Track Data is 0xE0";
                                break;
                            case 0:
                                LastErrorNumber = 1009;
                                LastErrorDescription += "Track 3: No Read Operation. Card Track Data is 0x00";
                                break;
                        }
                    }
                    else
                    {
                        if (Re[0] == 78)
                        {
                            switch (Re[2])
                            {
                                case 0:
                                    LastErrorNumber = 1000;
                                    LastErrorDescription = "Undefined Command";
                                    break;
                                case 1:
                                    LastErrorNumber = 1001;
                                    LastErrorDescription = "Parameter Wrong";
                                    break;
                                case 2:
                                    LastErrorNumber = 1002;
                                    LastErrorDescription = "Command Can Not Be Executed";
                                    break;
                                case 4:
                                    LastErrorNumber = 1004;
                                    LastErrorDescription = "Command Data Wrong";
                                    break;
                                case 5:
                                    LastErrorNumber = 1005;
                                    LastErrorDescription = "The input power supply is not in the operation range";
                                    break;
                                case 6:
                                    LastErrorNumber = 1006;
                                    LastErrorDescription = "Not Standard Length Card Inside";
                                    break;
                                case 7:
                                    LastErrorNumber = 1007;
                                    LastErrorDescription = "Power-off Protection Status";
                                    break;
                            }
                        }
                        Noback = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //'Logger.Write(ex)
                LastErrorNumber = -1;
                LastErrorDescription = ex.Message;
            }

            if (Noback)
            {
                if (LastErrorNumber == 0)
                {
                    LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                }
            }
            return !Noback;
        }
        #endregion

        private string ResetReader()
        {
            int BackLen;
            byte[] Re = new byte[1024];
            bool Noback;
            DateTime EndTime;
            int Temp;
            string Ret = "OK";

            LastErrorNumber = 0;
            LastErrorDescription = "";

            //'N4=H,02 00 02 31 30 03 00
            byte[] Sendbuf = { 0x2, 0x0, 0x2, 0x31, 0x30, 0x3, 0 };
            Sendbuf[6] = CheckXOR(Sendbuf, 6);
            CSerial.Write(Sendbuf, 0, Sendbuf.Length);
            Thread.Sleep(100);
            if (CSerial.BytesToRead > 0)
            {
                BackLen = CSerial.BytesToRead;
                CSerial.Read(Re, 0, BackLen);
                if (Re[0] == 6)
                {
                    CSerial.Write(((char)ENQ).ToString());

                    Noback = true;
                    BackLen = 0;
                    EndTime = DateTime.Now.AddSeconds(8);
                    while (EndTime > DateTime.Now)
                    {
                        if (CSerial.BytesToRead >= 3)
                        {
                            BackLen = 3;
                            CSerial.Read(Re, 0, BackLen);
                            Noback = false;
                            break;
                        }
                    }

                    Temp = Re[0] ^ Re[1] ^ Re[2];

                    double len1;
                    if (Re[1] != 0)
                    {
                        string Re1Hex = "0" + Re[1].ToString("X");
                        string Re2Hex = "0" + Re[2].ToString("X");
                        len1 = double.Parse("0x" + Re1Hex.Substring(Re1Hex.Length - 2) + Re2Hex.Substring(Re2Hex.Length - 2));
                    }
                    else
                    {
                        len1 = Re[2];
                    }
                    len1 = len1 + 2;

                    if (!Noback)
                    {
                        EndTime = DateTime.Now.AddSeconds(3);

                        while (EndTime > DateTime.Now)
                        {
                            if (CSerial.BytesToRead >= len1)
                            {
                                BackLen = CSerial.BytesToRead;
                                Array.Clear(Re, 0, Re.Length);
                                CSerial.Read(Re, 0, BackLen);
                                break;
                            }
                        }

                        Temp = Temp ^ CheckXOR(Re, BackLen - 1);
                        if (Temp == Re[BackLen - 1])
                        {
                            switch (Re[2])
                            {
                                case 89:
                                    Ret = "OK";
                                    break;
                                case 78:
                                    Ret = "OK";
                                    break;
                            }
                            if (Re[0] == 78)
                            {
                                switch (Re[2])
                                {
                                    case 0:
                                        LastErrorNumber = 1000;
                                        LastErrorDescription = "Undefined Command";
                                        break;
                                    case 1:
                                        LastErrorNumber = 1001;
                                        LastErrorDescription = "Parameter Wrong";
                                        break;
                                    case 2:
                                        LastErrorNumber = 1002;
                                        LastErrorDescription = "Command Can Not Be Executed";
                                        break;
                                    case 4:
                                        LastErrorNumber = 1004;
                                        LastErrorDescription = "Command Data Wrong";
                                        break;
                                    case 5:
                                        LastErrorNumber = 1005;
                                        LastErrorDescription = "The input power supply is not in the operation range";
                                        break;
                                    case 6:
                                        LastErrorNumber = 1006;
                                        LastErrorDescription = "Not Standard Length Card Inside";
                                        break;
                                    case 7:
                                        LastErrorNumber = 1007;
                                        LastErrorDescription = "Power-off Protection Status";
                                        break;
                                }
                                Ret = "ERROR";
                            }
                            else
                            {
                                Ret = "OK";
                            }
                        }
                        else
                        {
                            Ret = "ERROR";
                        }
                    }
                    else
                    {
                        Ret = "ERROR";
                    }
                }
                else
                {
                    Ret = "ERROR";
                }
            }
            else
            {
                Ret = "ERROR";
            }
            return Ret.ToUpper();
        }

        private string ResetReader1()
        {
            int BackLen;
            byte[] Re = new byte[1024];
            bool Noback;
            DateTime EndTime;
            int Temp;
            string Ret = "OK";

            LastErrorNumber = 0;
            LastErrorDescription = "";

            byte[] Sendbuf = { 0x2, 0x0, 0x2, 0x30, 0x30, 0x3, 0 };
            Sendbuf[6] = CheckXOR(Sendbuf, 6);
            CSerial.Write(Sendbuf, 0, Sendbuf.Length);
            Thread.Sleep(100);
            if (CSerial.BytesToRead > 0)
            {
                BackLen = CSerial.BytesToRead;
                CSerial.Read(Re, 0, BackLen);
                if (Re[0] == 6)
                {
                    CSerial.Write(((char)ENQ).ToString());

                    Noback = true;
                    BackLen = 0;
                    EndTime = DateTime.Now.AddSeconds(3);
                    while (EndTime > DateTime.Now)
                    {
                        if (CSerial.BytesToRead >= 20)
                        {
                            BackLen = CSerial.BytesToRead;
                            CSerial.Read(Re, 0, BackLen);
                            Noback = false;
                            break;
                        }
                    }

                    Temp = CheckXOR(Re, BackLen - 1);

                    if (Temp == Re[BackLen - 1])
                    {
                        if (Re[3] == 78)
                        {
                            switch (Re[5])
                            {
                                case 0:
                                    LastErrorNumber = 1000;
                                    LastErrorDescription = "Undefined Command";
                                    break;
                                case 1:
                                    LastErrorNumber = 1001;
                                    LastErrorDescription = "Parameter Wrong";
                                    break;
                                case 2:
                                    LastErrorNumber = 1002;
                                    LastErrorDescription = "Command Can Not Be Executed";
                                    break;
                                case 4:
                                    LastErrorNumber = 1004;
                                    LastErrorDescription = "Command Data Wrong";
                                    break;
                                case 5:
                                    LastErrorNumber = 1005;
                                    LastErrorDescription = "The input power supply is not in the operation range";
                                    break;
                                case 6:
                                    LastErrorNumber = 1006;
                                    LastErrorDescription = "Not Standard Length Card Inside";
                                    break;
                                case 7:
                                    LastErrorNumber = 1007;
                                    LastErrorDescription = "Power-off Protection Status";
                                    break;
                            }
                            Ret = "ERROR";
                        }
                        else
                        {
                            Ret = "OK";
                        }
                    }
                    else
                    {
                        Ret = "ERROR";
                    }
                }
                else
                {
                    Ret = "ERROR";
                }
            }
            else
            {
                Ret = "ERROR";
            }
            return Ret.ToUpper();
        }

        private byte CheckXOR(byte[] bDataBuf, int Count)
        {
            byte DataXOR = 0;
            long ii;

            for (ii = 0; ii <= Count - 1; ii++)
            {
                DataXOR ^= bDataBuf[ii];
            }
            return DataXOR;
        }

        public bool EnabledShutter(bool mEnabled)
        {
            bool returnValue = false;
            if (mEnabled)
            {
                returnValue = SetAccessS(CardPermit.Permit_all_cards_in, RearPermit.Permit_all_cards_in_from_rear);
                SetCardPostition(MoveCardPos.Stop_at_the_rear_hold_the_card);
            }
            else
            {
                returnValue = SetAccessS(CardPermit.Prohibit_the_cards_in, RearPermit.Prohibit_the_cards_in_from_rear);
            }
            return returnValue;
        }

        private bool SetAccess(CardPermit mCardFrontPermit = CardPermit.Permit_all_cards_in, RearPermit mCardRearPermit = RearPermit.Permit_all_cards_in_from_rear)
        {
            bool Ret;
            _CurrentCardPermit = mCardFrontPermit;
            CurrentRearPermit = mCardRearPermit;

            Ret = SetAccessS(CurrentCardPermit, CurrentRearPermit);

            return Ret;
        }

        private bool SetAccessS(CardPermit CardFrontPermit = CardPermit.Permit_all_cards_in, RearPermit CardRearPermit = RearPermit.Permit_all_cards_in_from_rear)
        {
            int BackLen;
            byte[] Re = new byte[1024];
            int Temp;
            bool Noback;
            DateTime EndTime;
            bool Ret = false;

            LastErrorNumber = 0;
            LastErrorDescription = "";

            if (!CSerial.IsOpen)
            {
                LastErrorNumber = 900;
                LastErrorDescription = "Comm. Port Setup First";
                HaveError = !Ret;
                return Ret;
            }

            lock (CSerial)
            {

                try
                {
                    byte[] Sendbuf = { 2, 0, 3, 0x2F, (byte)CardFrontPermit, (byte)CardRearPermit, 3, 0 };
                    Sendbuf[7] = CheckXOR(Sendbuf, 7);
                    CSerial.Write(Sendbuf, 0, Sendbuf.Length);

                    BackLen = 0;
                    Thread.Sleep(100);
                    if (CSerial.BytesToRead > 0)
                    {
                        BackLen = CSerial.BytesToRead;
                        CSerial.Read(Re, 0, BackLen);
                        if (Re[0] == 6)
                        {
                            CSerial.Write(((char)ENQ).ToString());

                            Noback = true;
                            BackLen = 0;
                            EndTime = DateTime.Now.AddSeconds(8);
                            while (EndTime > DateTime.Now)
                            {
                                if (CSerial.BytesToRead >= 3)
                                {
                                    BackLen = 3;
                                    CSerial.Read(Re, 0, BackLen);
                                    Noback = false;
                                    break;
                                }
                            }

                            Temp = Re[0] ^ Re[1] ^ Re[2];

                            double len1;
                            if (Re[1] != 0)
                            {
                                string Re1Hex = "0" + Re[1].ToString("X");
                                string Re2Hex = "0" + Re[2].ToString("X");
                                len1 = double.Parse("0x" + Re1Hex.Substring(Re1Hex.Length - 2) + Re2Hex.Substring(Re2Hex.Length - 2));
                            }
                            else
                            {
                                len1 = Re[2];
                            }
                            len1 = len1 + 2;

                            if (!Noback)
                            {
                                EndTime = DateTime.Now.AddSeconds(3);

                                while (EndTime > DateTime.Now)
                                {
                                    if (CSerial.BytesToRead >= len1)
                                    {
                                        BackLen = CSerial.BytesToRead;
                                        Array.Clear(Re, 0, Re.Length);
                                        CSerial.Read(Re, 0, BackLen);
                                        break;
                                    }
                                }

                                Temp = Temp ^ CheckXOR(Re, BackLen - 1);

                                if (Temp == Re[BackLen - 1])
                                {
                                    if (Re[0] != 78)
                                    {
                                        switch (Re[3])
                                        {
                                            case 89:
                                                Ret = true;
                                                break;
                                            case 78:
                                                Ret = false;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (Re[0] == 78)
                                        {
                                            switch (Re[2])
                                            {
                                                case 225:
                                                    LastErrorDescription += "No start bits (STX)";
                                                    break;
                                                case 226:
                                                    LastErrorDescription += "No stop bits (ETX)";
                                                    break;
                                                case 227:
                                                    LastErrorDescription += "Byte Parity Error(Parity))";
                                                    break;
                                                case 228:
                                                    LastErrorDescription += "Parity Bit Error(LRC)";
                                                    break;
                                                case 229:
                                                    LastErrorDescription += "Card Track Data is Blank";
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                { }
                            }
                            else
                            {
                                Noback = true;
                            }
                        }
                        else
                        {
                            Noback = true;
                        }
                    }
                    else
                    {
                        Noback = true;
                    }
                }
                catch (Exception ex)
                {
                    //'Logger.Write(ex)
                    Noback = false;
                    LastErrorNumber = -1;
                    LastErrorDescription = ex.Message;
                }

                if (Noback)
                {
                    if (LastErrorNumber == 0)
                    {
                        LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                    }
                }
                HaveError = !Ret;
            }
            return Ret;
        }

        private bool SetCardPostition(MoveCardPos Position = MoveCardPos.Stop_at_the_RF_card_operation_position)
        {
            int BackLen;
            byte[] Re = new byte[1024];
            int Temp;
            bool Noback = false;
            bool Ret = false;

            LastErrorNumber = 0;
            LastErrorDescription = "";
            lock (CSerial)
            {
                try
                {
                    byte[] Sendbuf = { 2, 0, 2, 0x2E, (byte)Position, 3, 0 };
                    Sendbuf[6] = CheckXOR(Sendbuf, 6);
                    CSerial.Write(Sendbuf, 0, Sendbuf.Length);

                    BackLen = 0;
                    Thread.Sleep(100);
                    if (CSerial.BytesToRead > 0)
                    {
                        BackLen = CSerial.BytesToRead;
                        CSerial.Read(Re, 0, BackLen);
                        if (Re[0] == 6)
                        {
                            CSerial.Write(((char)ENQ).ToString());
                            Thread.Sleep(250);
                            BackLen = 0;
                            if (CSerial.BytesToRead > 0)
                            {
                                BackLen = CSerial.BytesToRead;
                                CSerial.Read(Re, 0, BackLen);

                                Temp = CheckXOR(Re, BackLen - 1);

                                if (Temp == Re[BackLen - 1])
                                {
                                    switch (Re[5])
                                    {
                                        case 89:
                                            Ret = true;
                                            break;
                                        case 78:
                                            Ret = false;
                                            break;
                                    }
                                    if (Re[3] == 78)
                                    {
                                        switch (Re[5])
                                        {
                                            case 225:
                                                LastErrorDescription += "No start bits (STX)";
                                                break;
                                            case 226:
                                                LastErrorDescription += "No stop bits (ETX)";
                                                break;
                                            case 227:
                                                LastErrorDescription += "Byte Parity Error(Parity))";
                                                break;
                                            case 228:
                                                LastErrorDescription += "Parity Bit Error(LRC)";
                                                break;
                                            case 229:
                                                LastErrorDescription += "Card Track Data is Blank";
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Noback = true;
                            }
                        }
                        else
                        {
                            Noback = true;
                        }
                    }
                    else
                    {
                        Noback = true;
                    }
                }
                catch (Exception ex)
                {
                    //'Logger.Write(ex)
                    LastErrorNumber = int.Parse(ex.InnerException.Message);
                    LastErrorDescription = ex.Message;
                }
                if (Noback == true)
                {
                    if (LastErrorNumber == 0)
                    {
                        LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                    }
                }
                //'DeviceStatus()
                HaveError = !Ret;

            }
            return Ret;
        }

        private bool DeviceStatus()
        {
            int BackLen;
            byte[] Re = new byte[1024];
            int Temp;
            bool Noback;

            LastErrorNumber = 0;
            LastErrorDescription = "";

            lock (CSerial)
            {
                if (!CSerial.IsOpen)
                {
                    LastErrorNumber = 900;
                    LastErrorDescription = "Comm. Port Setup First";
                    HaveError = true;
                    return false;
                }
                try
                {
                    byte[] Sendbuf = { 2, 0, 2, 0x31, 0x30, 3, 0 };
                    Sendbuf[6] = CheckXOR(Sendbuf, 6);
                    CSerial.Write(Sendbuf, 0, Sendbuf.Length);

                    BackLen = 0;
                    Thread.Sleep(100);
                    if (CSerial.BytesToRead > 0)
                    {
                        BackLen = CSerial.BytesToRead;
                        CSerial.Read(Re, 0, BackLen);
                        if (Re[0] == 6)
                        {
                            CSerial.Write(((char)ENQ).ToString());
                            Thread.Sleep(200);
                            Noback = true;
                            BackLen = 0;
                            if (CSerial.BytesToRead > 0)
                            {
                                BackLen = CSerial.BytesToRead;
                                Array.Clear(Re, 0, Re.Length);
                                CSerial.Read(Re, 0, BackLen);
                                Temp = CheckXOR(Re, BackLen - 1);

                                if (Temp == Re[BackLen - 1])
                                {
                                    if (BackLen > 8)
                                    {
                                        Noback = false;
                                        if (Re[5] > 71 && Re[5] < 79)
                                        {
                                            //'CanReadStatus = True
                                            LastCardPosition = CurrentCardPosition;
                                            switch (Re[5])
                                            {
                                                case 72:
                                                    CurrentCardPosition = CardPosition.There_is_card_at_the_front_side;
                                                    break;
                                                case 73:
                                                    CurrentCardPosition = CardPosition.There_is_card_at_the_front_card_hold_position;
                                                    break;
                                                case 74:
                                                    CurrentCardPosition = CardPosition.There_is_card_in_the_reader;
                                                    //'CanReadStatus = False
                                                    break;
                                                case 75:
                                                    CurrentCardPosition = CardPosition.There_is_card_at_IC_card_operation_position;
                                                    break;
                                                case 76:
                                                    CurrentCardPosition = CardPosition.There_is_card_at_the_rear_card_hold_position;
                                                    break;
                                                case 77:
                                                    CurrentCardPosition = CardPosition.There_is_card_at_the_rear_side;
                                                    break;
                                                case 78:
                                                    CurrentCardPosition = CardPosition.No_Card_In_The_Reader;
                                                    break;
                                            }

                                            switch (Re[6])
                                            {
                                                case 73:
                                                    _CurrentCardPermit = CardPermit.Card_in_by_mag_signal_and_switch;
                                                    break;
                                                case 74:
                                                    _CurrentCardPermit = CardPermit.Permit_all_cards_in;
                                                    break;
                                                case 75:
                                                    _CurrentCardPermit = CardPermit.Card_in_by_mag_signal;
                                                    break;
                                                case 78:
                                                    _CurrentCardPermit = CardPermit.Prohibit_the_cards_in;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //'MsgBox(Re(5))
                                        }
                                    }
                                    else
                                    {
                                        if (Re[3] == 78)
                                        {
                                            switch (Re[5])
                                            {
                                                case 225:
                                                    LastErrorDescription += "No start bits (STX)";
                                                    break;
                                                case 226:
                                                    LastErrorDescription += "No stop bits (ETX)";
                                                    break;
                                                case 227:
                                                    LastErrorDescription += "Byte Parity Error(Parity))";
                                                    break;
                                                case 228:
                                                    LastErrorDescription += "Parity Bit Error(LRC)";
                                                    break;
                                                case 229:
                                                    LastErrorDescription += "Card Track Data is Blank";
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                Noback = true;
                            }
                        }
                        else
                        {
                            Noback = true;
                        }
                    }
                    else
                    {
                        Noback = true;
                    }
                }
                catch (Exception ex)
                {
                    //'Logger.Write(ex)
                    Noback = false;
                    LastErrorNumber = -1;
                    LastErrorDescription = ex.Message;
                }
                if (Noback)
                {
                    if (LastErrorNumber == 0)
                    {
                        LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                    }
                }
                if (!Noback)
                {
                    if (LastCardPosition != CurrentCardPosition)
                    {
                        CanRaise = true;
                    }
                    else
                    {
                        CanRaise = false;
                    }
                }
                HaveError = Noback;

            }
            return !Noback;
        }

        private bool MoveCard(MoveCardPos Position = MoveCardPos.Stop_at_the_Front)
        {
            int BackLen;
            byte[] Re = new byte[1024];
            int Temp;
            bool Noback = false;
            bool Ret = false;
            DateTime EndTime;

            LastErrorNumber = 0;
            LastErrorDescription = "";
            lock (CSerial)
            {
                try
                {
                    byte[] Sendbuf = { 2, 0, 2, 0x32, (byte)Position, 3, 0 };
                    Sendbuf[6] = CheckXOR(Sendbuf, 6);
                    CSerial.Write(Sendbuf, 0, Sendbuf.Length);

                    BackLen = 0;
                    Thread.Sleep(300);
                    if (CSerial.BytesToRead > 0)
                    {
                        BackLen = CSerial.BytesToRead;
                        CSerial.Read(Re, 0, BackLen);
                        if (Re[0] == 6)
                        {
                            CSerial.Write(((char)ENQ).ToString());
                            Noback = true;
                            BackLen = 0;
                            EndTime = DateTime.Now.AddSeconds(8);
                            while (EndTime > DateTime.Now)
                            {
                                if (CSerial.BytesToRead >= 3)
                                {
                                    BackLen = 3;
                                    CSerial.Read(Re, 0, BackLen);
                                    Noback = false;
                                    break;
                                }
                            }

                            Temp = Re[0] ^ Re[1] ^ Re[2];

                            double len1;
                            if (Re[1] != 0)
                            {
                                string Re1Hex = "0" + Re[1].ToString("X");
                                string Re2Hex = "0" + Re[2].ToString("X");
                                len1 = double.Parse("0x" + Re1Hex.Substring(Re1Hex.Length - 2) + Re2Hex.Substring(Re2Hex.Length - 2));
                            }
                            else
                            {
                                len1 = Re[2];
                            }
                            len1 = len1 + 2;

                            if (!Noback)
                            {
                                EndTime = DateTime.Now.AddSeconds(3);

                                while (EndTime > DateTime.Now)
                                {
                                    if (CSerial.BytesToRead >= len1)
                                    {
                                        BackLen = CSerial.BytesToRead;
                                        Array.Clear(Re, 0, Re.Length);
                                        CSerial.Read(Re, 0, BackLen);
                                        break;
                                    }
                                }

                                Temp = Temp ^ CheckXOR(Re, BackLen - 1);
                                if (Temp == Re[BackLen - 1])
                                {
                                    Ret = true;
                                    switch (Re[2])
                                    {
                                        case 89:
                                            if (Sendbuf[4] == 0x37)
                                            {
                                                LastErrorDescription = "Set 'Clean Reader' Success. Please insert the Cleaning card";
                                                LastErrorNumber = -1;
                                            }
                                            break;
                                        case 78:
                                            if (Sendbuf[4] == 0x37)
                                            {
                                                LastErrorDescription = "Set 'Clean Reader' Error";
                                                LastErrorNumber = 1010;
                                            }
                                            else
                                            {
                                                LastErrorDescription = "Move card Error";
                                                LastErrorNumber = 1011;
                                            }
                                            break;
                                        case 69:
                                            LastErrorDescription = "No Card In The Reader";
                                            LastErrorNumber = 1012;
                                            break;
                                        case 87:
                                            LastErrorDescription = "The card is not on the IC card operation position";
                                            LastErrorNumber = 1013;
                                            break;
                                    }
                                    if (Re[0] == 78)
                                    {
                                        switch (Re[2])
                                        {
                                            case 0:
                                                LastErrorNumber = 1000;
                                                LastErrorDescription = "Undefined Command";
                                                break;
                                            case 1:
                                                LastErrorNumber = 1001;
                                                LastErrorDescription = "Parameter Wrong";
                                                break;
                                            case 2:
                                                LastErrorNumber = 1002;
                                                LastErrorDescription = "Command Can Not Be Executed";
                                                break;
                                            case 4:
                                                LastErrorNumber = 1004;
                                                LastErrorDescription = "Command Data Wrong";
                                                break;
                                            case 5:
                                                LastErrorNumber = 1005;
                                                LastErrorDescription = "The input power supply is not in the operation range";
                                                break;
                                            case 6:
                                                LastErrorNumber = 1006;
                                                LastErrorDescription = "Not Standard Length Card Inside";
                                                break;
                                            case 7:
                                                LastErrorNumber = 1007;
                                                LastErrorDescription = "Power-off Protection Status";
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Noback = true;
                            }
                        }
                        else
                        {
                            Noback = true;
                        }
                    }
                    else
                    {
                        Noback = true;
                    }
                }
                catch (Exception ex)
                {
                    //'Logger.Write(ex)
                    LastErrorNumber = int.Parse(ex.InnerException.Message);
                    LastErrorDescription = ex.Message;
                }


                if (Noback)
                {
                    if (LastErrorNumber == 0)
                    {
                        LastErrorDescription = "Communication Error, Possible Causes: 1)Communication Setup Error 2)Wrong Model Selected 3)No connected 4)No Power On Unit";
                    }
                }

            }
            DeviceStatus();
            return Ret;
        }
        #endregion

        public void ReadStatus()
        {
            //'If CurrentCardPosition <> CardPosition.There_is_card_in_the_reader Then

            DeviceStatus();
            if (CanRaise && CurrentCardPosition == CardPosition.There_is_card_in_the_reader)
            {
                ReadTracks();
            }
            if (CanRaise)
                if (CardPositionChanged != null)
                {
                    CardPositionChanged();
                }
        }

        private void ReadStatusThread()
        {
            while (!Shutdown)
            {
                //'If CurrentCardPosition <> CardPosition.There_is_card_in_the_reader Then

                if (!CanRaise)
                {
                    DeviceStatus();
                }
                if (CanRaise && CurrentCardPosition == CardPosition.There_is_card_in_the_reader)
                {
                    ReadTracks();
                }
                if (CanRaise)
                {
                    if (CardPositionChanged != null)
                    {
                        CardPositionChanged();
                    }
                }
                CanRaise = false;
                //'End If
                Thread.Sleep(0);
                Thread.Sleep(300);
            }
        }

        #region Class Methods
        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }
        #endregion

        public CardReader(string PortN)
        {
            PortName = PortN;
        }
    }
}
