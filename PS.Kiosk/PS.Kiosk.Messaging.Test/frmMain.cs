using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fanap;
using System.Diagnostics;
using System.Collections;
using Fanap.Messaging;
using Fanap.Messaging.Iso8583;
using System.IO;
using PS.Kiosk.Messaging.Operations;

using PS.Kiosk.Common.Model;
using PS.Kiosk.Messaging.MsgCall;
using PS.Kiosk.DeviceController.Services;

namespace PS.Kiosk.Messaging
{
    public partial class frmMain : Form
    {
        private delegate void ShowMessageDelegate(string msg);
        private volatile int  req, res , total;
        private  Properties.Settings cnf = new Properties.Settings();
        private volatile System.Threading.Timer tm;
        private int seq = 1;
        private int stan = 118;
        private int refNo = 23654;
        private CsTransaction dfs;
        private CsUtil csUtil = new CsUtil();
        //private byte[] key ;
        private Hashtable Collect = new Hashtable();
        private const string securityKeysFileName = "Security.Ini";

        public frmMain()
        {
            InitializeComponent();
        }


        private string GetYYYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        private string GetYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
        
        private void ShowMessage(string msg)
        {
            ShowMessageDelegate smd = new ShowMessageDelegate(ShowMessageInvoker);
            this.Invoke(smd, new object[] { msg });
        }

        private void ShowMessageInvoker(string msg)
        {
            listBox1.Items.Add(msg);
        }

        private void getParams(bool isSave)
        {
            if (isSave)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("Data.txt");
                sw.WriteLine(stan.ToString().PadLeft(6, '0') + refNo.ToString().PadLeft(12, '0'));
                sw.Close();
            }
            else
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("Data.txt");
                string str = sr.ReadLine();
                stan = Convert.ToInt32(str.Substring(0, 6));
                refNo = Convert.ToInt32(str.Substring(6, 12));

                sr.Close();
            }
        }

        private void btnRemain_Click(object sender, EventArgs e)
        {
               DateTime datetime = new DateTime(Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(0, 4)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(4, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(6, 2)),
                    Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(8, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(10, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(12, 2)));

            FinancialParameters finance = new FinancialParameters();
            finance.MsgType = Enums.MsgType.Financial;
            finance.CardNumberP2 = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            finance.ProcessCode = Enums.ProcessCode.Remain;
            finance.TranAmountP4 = Convert.ToInt64( txtAmnt.Text);
            finance.TranRefNumber = refNo;
            finance.TranTime = datetime.TimeOfDay;
            finance.TranDate = datetime.Date;
            finance.DeviceCodeP25 = 14;
            finance.BankAcceptorId = "628023";
            finance.IsoTrack = "";
            finance.Stan = stan;
            finance.TerminalAcceptorId = "5537";
            finance.CardAcceptorId = "22869";
            finance.TerminalSerialNumberP53 = "29533925";

            CsAgent csAg = new CsAgent();
            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);
            finance.PinBlockP52 = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);
            finance.TranCurrencyCodeP49 = 364;

            //Transactions Tran = new Transactions();
            //Tran.GetBalanceInquery(finance);

           // for (int i = 0; i < 100; i++)
           // {

                //bool ret;
                //CsTransaction dfs = CreateTransaction();
                //dfs.CsParam.Amount = "0";


                //getParams(false);
                //dfs.CsParam.isoTrack = txtIso.Text;
                //dfs.CsParam.messageType = 0200;
                //dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
                //dfs.CsParam.Language = 1;
                //dfs.CsParam.trxType = 1;
                //dfs.CsParam.CVV2 = "    ";
                //CsAgent csAg = new CsAgent();

                //dfs.CsParam.AuditNumber = ++stan;
                //dfs.CsParam.RefrenceNo = (++refNo).ToString();


                //int index = 0;
                //string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
                //byte[] key = csUtil.HexToBin(keyStr);


                //dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

                //req = dfs.CsParam.AuditNumber;
                //getParams(true);




                //ret = dfs.Trx2Do();
                //try
                //{
                //    listBox1.Items.Add(dfs._messageout.ToString());
                //}
                //catch
                //{
                //}
                //getParams(true);
           // }
           
        }

        private void btnPurch_Click(object sender, EventArgs e)
        {


            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = txtAmnt.Text;


            getParams(false);
            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType = 22;
            dfs.CsParam.CVV2 = "    ";
            CsAgent csAg = new CsAgent();

            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();


            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;
            getParams(true);

           

            ret = dfs.Trx2Do();
            dfs.CsParam.trxType = 22;
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }

            dfs.CsParam.trxType = 15;
            ret = dfs.Trx2Do();

            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
           
             
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {            
            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = txtAmnt.Text;
            getParams(false);
            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));;
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType =4;
            dfs.CsParam.BillID = txtBillID.Text;
            dfs.CsParam.PayID = txtPayID.Text;
            cmbBill.SelectedIndex = 0;

            dfs.CsParam.BillType = cmbBill.Text.Substring(0,2);
            byte companyID = 0;
            string companyCode = "";

            string docCode = "";
            byte cycleCode = 0;
            byte yearCode = 0;
            double value = 0;
            BillCtrl.Bill csBill = new BillCtrl.Bill();
            csBill.ExtractBillID(dfs.CsParam.BillID, ref companyID, ref companyCode, ref docCode);
            csBill.ExtractPaymentID(dfs.CsParam.PayID, ref cycleCode, ref yearCode, ref value);
            dfs.CsParam.Amount = "10000";

            CsAgent csAg = new CsAgent();
            
            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();

            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);

            
 
                dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;

            getParams(true);

            

            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }


        }

        private CsTransaction CreateTransaction()
        {
            CsTransaction dfs = null;
                dfs = new CsTransaction(cnf.ip, cnf.port, cnf.Terminal, cnf.CardAcceptor, "Persian Switch", "581672011", 50000);
            return dfs;
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            //if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            //{
            //    Process.GetCurrentProcess().Kill();
            //    return;
            //}
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = appPath + "\\" + securityKeysFileName;
            CsSecurityKeys.getInstance().setFilePath(fileName);

            
         
        }

        private void btnSignOn_Click(object sender, EventArgs e)
        {

            try
            {
                byte[] key = new byte[8] { 0xf2, 0x63, 0xb2, 0x72, 0x83, 0xc2, 0x70, 0xb4 };
                EppService.Instance.SetKeysForLogin(key);

                SignOnParameters signon = new SignOnParameters();

                //DateTime datetime = new DateTime(Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(0, 4)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(4, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(6, 2)),
                //    Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(8, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(10, 2)), Convert.ToInt32(GetYYYYMMDDhhmmss().Substring(12, 2)));

                signon.TranTime = DateTime.Now.TimeOfDay;
                signon.TranDate = DateTime.Now;

                
                signon.MsgType = Enums.MsgType.SignOn;
                signon.ProcessCode = Enums.ProcessCode.SignOn;
                signon.IP = "192.168.7.235";
                signon.Port = 16000;
                signon.TerminalAcceptorId = "5537";
                signon.TerminalAcceptorName = "Persian Switch";
                signon.CardAcceptorId = "22869";
                signon.CardAcceptorBinCode = "581672011";
                signon.BankAcceptorId = "628023";
                signon.TerminalSerialNumberP48 = "29533925";
                signon.TimeOut = 50000;
                signon.TranRefNumber = 1;
                signon.Stan = 2;
                signon.DeviceCodeP25 = 14;
                signon.IsoTrack = "93045061351133300001";
                
                SignOnReplyParameters reply = Transactions.TransactionsInstance.SignOn(signon);


                #region old
                /*
                bool ret;
                byte[] bte2 = { 0x00, 0x85, 0x60, 0x00, 0x13, 0x00, 0x01 };
                CsTransaction dfs = CreateTransaction();
                dfs.CsParam.Amount = "0";
                getParams(false);
                dfs.CsParam.isoTrack = "";
                dfs.CsParam.messageType = 0800;
                dfs.CsParam.PAN = "";
                dfs.CsParam.Language = 1;
                dfs.CsParam.trxType = 26;
                dfs.CsParam.CVV2 = "";
                CsAgent csAg = new CsAgent();
                dfs.CsParam.PINBlock = null;
                dfs.CsParam.AuditNumber = ++stan;
                dfs.CsParam.RefrenceNo = (++refNo).ToString();
                req = dfs.CsParam.AuditNumber;
                getParams(true);

                CsUtil util = new CsUtil();



                byte[] mck = csUtil.HexToBin("f263b27283c270b4");
                string ss = util.BinToHex(util.DesBuffer(util.HexToBin("a393767f55e195d1"), mck, EncryptDecrypt.Decrypt));

                ret = dfs.Trx2Do();

                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                string[] sss = dfs._messageout.Fields[48].ToString().Split(str[0]);
                string MACK = sss[sss.Length - 3];
                string PINK = sss[sss.Length - 2];

                byte[] pnk;
                util.ExtractPinMacKeys(out mck, out pnk, "29533925");
                MACK = util.BinToHex(util.DesBuffer(util.HexToBin(MACK), mck, EncryptDecrypt.Decrypt));
                PINK = util.BinToHex(util.DesBuffer(util.HexToBin(PINK), mck, EncryptDecrypt.Decrypt));
                CsSecurityKeys.getInstance().updateMakKey(0, MACK);
                CsSecurityKeys.getInstance().updatePinKey(0, PINK);
                try
                {
                    listBox1.Items.Add(dfs._messageout.ToString());
                }
                catch
                {
                }
                getParams(true);
                */
                #endregion old
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btnReconcil_Click(object sender, EventArgs e)
        {
            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.trxType = 10;
            getParams(true);
            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();
            req = dfs.CsParam.AuditNumber;
           

         
            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
           
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {

            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = "0";


            getParams(false);
            dfs.CsParam.DestinationPAN = txtDestPAN.Text;

            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType = 3;
            dfs.CsParam.CVV2 = "    ";
            CsAgent csAg = new CsAgent();

            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();


            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;
            getParams(true);

            

            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
            
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {

            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = "1000";


            getParams(false);
            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType = 5;

            dfs.CsParam.DestinationPAN = txtDestPAN.Text;
            dfs.CsParam.CVV2 = "    ";
            CsAgent csAg = new CsAgent();

            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();


            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;
            getParams(true);

            

            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
            
        }

        private void btnSignOff_Click(object sender, EventArgs e)
        {
            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = "20000";


            getParams(false);
            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType = 6;

            dfs.CsParam.DestinationPAN = "9935";
            dfs.CsParam.CVV2 = "    ";
            CsAgent csAg = new CsAgent();

            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();


            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;
            getParams(true);



            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
        }

        private void btnState_Click(object sender, EventArgs e)
        {
            bool ret;
            CsTransaction dfs = CreateTransaction();
            dfs.CsParam.Amount = "0";


            getParams(false);
            dfs.CsParam.isoTrack = txtIso.Text;
            dfs.CsParam.messageType = 0200;
            dfs.CsParam.PAN = txtIso.Text.Remove(txtIso.Text.IndexOf('='));
            dfs.CsParam.Language = 1;
            dfs.CsParam.trxType = 7;
            dfs.CsParam.CVV2 = "    ";
            CsAgent csAg = new CsAgent();

            dfs.CsParam.AuditNumber = ++stan;
            dfs.CsParam.RefrenceNo = (++refNo).ToString();


            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            dfs.CsParam.PINBlock = csAg.EncryptPIN(txtPIN.Text, txtIso.Text.Remove(txtIso.Text.IndexOf('=')), key);

            req = dfs.CsParam.AuditNumber;
            getParams(true);




            ret = dfs.Trx2Do();
            try
            {
                listBox1.Items.Add(dfs._messageout.ToString());
            }
            catch
            {
            }
            getParams(true);
        }

      
      
    }
}
