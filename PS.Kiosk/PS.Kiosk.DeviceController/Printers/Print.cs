using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace PS.Kiosk.DeviceController.Printers
{
    internal class Print
    {
        private  Printer.Status prnStatus = new Printer.Status();

        public static bool? IsPrinterOk;


        public bool ChekStatus()
        {
            bool res = prnStatus.Init();


            if (res)
            {
                bool res2 = prnStatus.ReadForceStatus();
                return !prnStatus.PaperNotPresent && !prnStatus.PaperRollNearEnd && !prnStatus.UnrecoverableError && !prnStatus.AutoCutterError;
            }
            else
                return false;


        }
    }

    //internal class Print
    //{
    //    #region Properties

    //    //public static explicit operator IntPtr(int value);

    //    Int32 hDev = 0;
    //    const int WRITE_PIPENUM = 0;
    //    object LockObj = new object();


    //    public bool Online = false;
    //    public bool AutoCutterError = false;
    //    public bool UnrecoverableError = false;
    //    public bool AutomaticallyRecoverableError = false;
    //    public bool PaperRollNearEnd = false;
    //    public bool PaperNotPresent = false;
    //    public bool Ready = false;


    //    delegate void CheckedStatusEventHandler();
    //    event CheckedStatusEventHandler CheckedStatus;

    //    static Print _PrintInstance;
    //    internal static Print PrintInstance
    //    {
    //        get
    //        {
    //            if (Print._PrintInstance == null)
    //                Print._PrintInstance = new Print();
    //            return Print._PrintInstance;
    //        }
           
    //    }


    //    #endregion

    //    private Print()
    //    {

    //    }

    //    #region ExternalMethods

    //    [DllImport("ByUsbInt.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    //    private static extern bool CloseDevice(Int32 hdev);
    //    [DllImport("ByUsbInt.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    //    private static extern int OpenDevice();
    //    [DllImport("ByUsbInt.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    //    private static extern Int32 uWrite(Int32 hdev, Int32 pipeNum, ref byte buf, Int32 bufSize);
    //    [DllImport("ByUsbInt.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    //    private static extern Int32 GetSpecStatus(Int32 hdev, IntPtr buf, Int32 bufSize, ref Int32 nBytes);

    //    #endregion ExternalMethods

    //    #region PrivateMethods

    //    private int VarPtr(object e)
    //    {
    //        GCHandle GC = GCHandle.Alloc(e, GCHandleType.Pinned);
    //        int GC2 = GC.AddrOfPinnedObject().ToInt32();
    //        GC.Free();
    //        return GC2;
    //    }

    //    private  bool ReadStatus1()
    //    {
    //        byte[] pRecvBuf = new byte[7];
    //        bool ret = true;

    //        lock (LockObj)
    //        {
    //            try
    //            {
    //                if (hDev > 1)
    //                    CloseDevice(hDev);
    //                hDev = OpenDevice();

    //                if (hDev < 1)
    //                {
    //                    Online = false;
    //                    Online = false;
    //                    AutoCutterError = true;
    //                    UnrecoverableError = true;
    //                    AutomaticallyRecoverableError = false;
    //                    PaperRollNearEnd = false;
    //                    PaperNotPresent = false;
    //                    Ready = false;
    //                    ret = false;
    //                    CheckedStatus();
    //                    return false;
    //                }

    //                int nBytes = 0;
    //                GetSpecStatus(hDev, new IntPtr(VarPtr(pRecvBuf)), 8, ref nBytes);

    //                Online = (pRecvBuf[0] & 8) != 8;
    //                AutoCutterError = (pRecvBuf[1] & 8) == 8;
    //                UnrecoverableError = (pRecvBuf[1] & 32) == 32;
    //                AutomaticallyRecoverableError = (pRecvBuf[1] & 64) == 64;

    //                PaperRollNearEnd = ((pRecvBuf[2] & 3) == 3);
    //                PaperNotPresent = ((pRecvBuf[2] & 12) == 12);
    //                Ready = Online & (!AutoCutterError) & (!UnrecoverableError) & (!PaperNotPresent);

    //                if (CloseDevice(hDev))
    //                    hDev = 0;

    //                CheckedStatus();
    //            }
    //            catch (Exception EX)
    //            {

    //                Online = false;
    //                AutoCutterError = true;
    //                UnrecoverableError = true;
    //                AutomaticallyRecoverableError = false;
    //                PaperRollNearEnd = false;
    //                PaperNotPresent = false;
    //                Ready = false;
    //                ret = false;

    //                throw EX;
    //            }

    //        }

    //        return ret;
    //    }

    //    #endregion PrivateMethods

    //    #region PublicMethods

    //    public bool Init()
    //    {
    //        bool ret;
    //        try
    //        {
    //            byte[] sBuf = new byte[4] { 0x1D, 0x61, 0xF, 0x0 };

    //            if (hDev > 1)
    //                CloseDevice(hDev);

    //            if (hDev < 1)
    //            {
    //                CloseDevice(hDev);
    //                return false;
    //            }



    //            ret = uWrite(hDev, WRITE_PIPENUM, ref sBuf[0], 4) > 0 ? true : false;

    //            if (CloseDevice(hDev))
    //                hDev = 0;
    //        }
    //        catch (Exception EX)
    //        {
    //            throw EX;
    //            //return false;
    //        }

    //        return ret;
    //    }

    //    public  bool ReadStatus()
    //    {
    //        try
    //        {
    //            Thread NewThread = new Thread(() => ReadStatus1());
    //            NewThread.Name = "PrinterReadStatus";
    //            NewThread.IsBackground = true;
    //            NewThread.Start();
    //        }
    //        catch (Exception EX)
    //        {
                
    //            throw EX;
    //        }
    //        return true;
    //    }

    //    public  bool ReadForceStatus() 
    //    {
    //        return ReadStatus1();
    //    }

    //    #endregion PublicMethods
    //}
}
