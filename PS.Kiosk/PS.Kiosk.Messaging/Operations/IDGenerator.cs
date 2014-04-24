using System;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace PS.Kiosk.Messaging.Operations
{
    public class IDGenerator
    {
        private IniFile iniFileObj;
        //private int stanNo;
        private static IDGenerator instance = null;
        
        public static IDGenerator getInstance()
        {
            if (instance == null)
                instance = new IDGenerator();
            return instance;
        }

        //private IDGenerator()
        //{
        //    string path = Application.ExecutablePath.ToString();
        //    int index = path.LastIndexOf('\\');
        //    path = path.Substring(0, index + 1);
        //    string fileName = path + "CSTransactionIDs.ini";
        //    iniFileObj = new IniFile(fileName);
        //}

        public int getNextID(string portNumber)
        {
            int stanNo = 0;
            string lastDateStr = iniFileObj.IniReadValue(portNumber, "Date");
            if (lastDateStr == DateTime.Now.Date.ToString())
                stanNo = Convert.ToInt32(iniFileObj.IniReadValue(portNumber, "StanNo"));
            stanNo++;
            updateIniFile(portNumber, stanNo);
            return stanNo;
        }

        private void updateIniFile(string portNumber, int stanNo)
        {
            iniFileObj.IniWriteValue(portNumber, "Date", DateTime.Now.Date.ToString());
            iniFileObj.IniWriteValue(portNumber, "StanNo", stanNo.ToString());
        }
    }
}
