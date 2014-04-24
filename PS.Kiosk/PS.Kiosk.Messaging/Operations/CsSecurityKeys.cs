using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PS.Kiosk.Messaging.Operations
{
    public class CsSecurityKeys
    {
        private static CsSecurityKeys instance = null;
        private IniFile iniObj = null;

        private CsSecurityKeys()
        {
        }

        public bool setFilePath(string path)
        {
            if (!File.Exists(path))
                return false;
            iniObj = new IniFile(path);
            return true;
        }

        public static CsSecurityKeys getInstance()
        {
            if (instance == null)
                instance = new CsSecurityKeys();
            return instance;
        }
        
        public string getMasterKey()
        {
            if (iniObj == null)
                return "";
            return iniObj.IniReadValue("Public", "PublicKey");
        }

        public int getPinKeyCount()
        {
            if (iniObj == null)
                return -1;
            string str = iniObj.IniReadValue("Pin", "Count");
            int count;
            if (int.TryParse(str, out count))
                return count;
            return -1;
        }

        public string getPinKey(int index)
        {
            if (iniObj == null)
                return "";
            StringBuilder str = new StringBuilder().Append("PinKey").Append(index);
            return iniObj.IniReadValue("Pin", str.ToString());
        }

        public int getMakKeyCount()
        {
            if (iniObj == null)
                return -1;
            string str = iniObj.IniReadValue("Mak", "Count");
            int count;
            if (int.TryParse(str, out count))
                return count;
            return -1;
        }

        public string getMakKey(int index)
        {
            if (iniObj == null)
                return "";
            StringBuilder str = new StringBuilder().Append("MakKey").Append(index);
            return iniObj.IniReadValue("Mak", str.ToString());
        }

        public bool updatePinKeyCoutn(int count)
        {
            if (iniObj == null)
                return false;
            iniObj.IniWriteValue("Pin", "Count", count.ToString());
            return true;
        }

        public bool updatePinKey(int index, string value)
        {
            if (iniObj == null)
                return false;
            StringBuilder str = new StringBuilder().Append("PinKey").Append(index);
            iniObj.IniWriteValue("Pin", str.ToString(), value);
            return true;
        }

        public bool updateMakKeyCoutn(int count)
        {
            if (iniObj == null)
                return false;
            iniObj.IniWriteValue("Mak", "Count", count.ToString());
            return true;
        }

        public bool updateMakKey(int index, string value)
        {
            if (iniObj == null)
                return false;
            StringBuilder str = new StringBuilder().Append("MakKey").Append(index);
            iniObj.IniWriteValue("Mak", str.ToString(), value);
            return true;
        }
    }
}
