using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Fanap.Messaging;
using Fanap.Messaging.Iso8583;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;


namespace PS.Kiosk.Messaging.Operations
{
    public enum EncryptDecrypt
    {
        Encrypt,
        Decrypt
    }
    public class CsUtil
    {
        private byte[] IV = new byte[8];
        public CsUtil()
        {
        }
        public int Hex2Int(char inputC)
        {
            if (inputC >= '0' && inputC <= '9')
                return inputC - 0x30;
            if (inputC >= 'A' && inputC <= 'F')
                return inputC - 'A' + 10;
            if (inputC >= 'a' && inputC <= 'f')
                return inputC - 'a' + 10;
            throw new Exception();
        }
        public bool ExtractPinMacKeys(out byte[] ucMacKey, out byte[] ucPinKey, string tmpSerial)
        {
            ucMacKey = null;
            ucPinKey = null;
            try
            {
                int i = 0;
                byte[] tmpKey = {0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,
                          0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08};

                byte[] tmpSN;
                byte[] tmpBinSN = new byte[16];
                byte[] keyBlk = new byte[16];
                byte[] motherKey;
                byte[] tmpMacKey = new byte[8];
                byte[] tmpPinKey = new byte[8];
                byte[] bSerial = new byte[16];


                for (i = 0; i < bSerial.Length; i++)
                    bSerial[i] = 0;
                tmpSN = System.Text.Encoding.ASCII.GetBytes(tmpSerial);
                Array.Copy(tmpSN, 0, bSerial, 16 - tmpSN.Length - 1, tmpSN.Length);

                for (i = 0; i < 16; i++)
                    keyBlk[i] = (byte)((int)bSerial[i] ^ (int)tmpKey[i]);

                motherKey = SHA1.Create().ComputeHash(keyBlk);


                for (i = 0; i < 8; i++)
                {
                    tmpMacKey[i] = motherKey[2 * i];
                    tmpPinKey[i] = motherKey[2 * i + 1];
                }
                ucMacKey = tmpMacKey;
                ucPinKey = tmpPinKey;
                return true;
            }
            catch (Exception ex)
            {
               

                return false;
            }
        }
        public byte[] HexToBin(string inputStr)
        {
            byte[] Res = new byte[inputStr.Length /2 ];
            int Size = 0, Len = 0;
            if (inputStr.Length == 0)
		        return null;
            while (Len < inputStr.Length)
	        {
		        int	k;
		        if(inputStr[Len]	>=	'0'	&&	inputStr[Len]	<=	'9')
			        k	=	inputStr[Len]	-	0x30;
		        else if(inputStr[Len]	>=	'A'	&&	inputStr[Len]	<=	'F')
			        k	=	inputStr[Len]	-	'A'	+	10;
		        else if(inputStr[Len]	>=	'a'	&&	inputStr[Len]	<=	'f')
			        k	=	inputStr[Len]	-	'a'	+	10;
		        else
                    return null;
		        if((Size	&	(1 != 0?1:0))!=0)
                    Res[Size >> 1] += (byte)k;
		        else
                    Res[Size >> 1] =(byte)( k << 4);
		        Len++;
		        Size++;
	        }
            Size = Size >> 1;
	        return	Res;
        }
        public  string BinToHex(byte[] inputB)
        {
            int Count = inputB.Length;
	        string	Hex	=	"0123456789ABCDEF";
	        string	inputStr	=	"";
	        for(int	i=0;i<Count;i++)
	        {
                inputStr += Hex[inputB[i] >> 4];
                inputStr += Hex[inputB[i] & 0xf];
	        }

            return inputStr;
        }
        public byte[] DesBuffer(byte[] _Data, byte[] _Key, EncryptDecrypt _Operation)
        {
            byte[] sourceArray = PadRight(_Data);
            byte[] destinationArray = new byte[sourceArray.Length];
            byte[] buffer3 = new byte[8];
            byte[] buffer4 = new byte[8];
            for (int i = 0; i < sourceArray.Length; i += 8)
            {
                Array.Copy(sourceArray, i, buffer4, 0, 8);
                if ((buffer3 = Des(buffer4, _Key, _Operation)) == null)
                {
                    return null;
                }
                Array.Copy(buffer3, 0, destinationArray, i, 8);
            }
           return destinationArray;
        }
        private  byte[] Des(byte[] _Data, byte[] _Key, EncryptDecrypt _Operation)
        {
            CryptoStream stream2;
            byte[] rgbKey = PadRight(_Key);
            byte[] buffer = PadRight(_Data);
            byte[]  _Res = new byte[buffer.Length];
            if (_Key.Length == 0)
            {
                return null;
            }
            DES des = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            if (_Operation == EncryptDecrypt.Encrypt)
            {
                stream2 = new CryptoStream(stream, des.CreateEncryptor(rgbKey, IV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, 8);
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(_Res, 0, 8);
                stream.Close();
            }
            else
            {
                stream2 = new CryptoStream(stream, des.CreateDecryptor(rgbKey, IV), CryptoStreamMode.Write);
                byte[] array = new byte[0x10];
                buffer.CopyTo(array, 0);
                stream2.Write(array, 0, array.Length);
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(_Res, 0, 8);
                stream.Close();
            }
            return _Res;
        }

        private  byte[] PadRight(byte[] DataIn)
        {
            int num = ((DataIn.Length % 8) == 0) ? DataIn.Length : (((DataIn.Length + 8) / 8) * 8);
            byte[] array = new byte[num];
            DataIn.CopyTo(array, 0);
            return array;
        }

      

        public string irsys2win(string irsysText)
        {
            char[] Rev = new char[irsysText.Length];
            int len = irsysText.Length;
            int l = 0;
            byte[] Iran = { 0x002F, 0x0020, 0x00FF, 0x008D, 0x0090, 0x0091, 0x0092, 0x0093, 0x0094, 0x0095, 0x0096, 0x0097, 0x0098, 0x0099, 0x009A, 0x009B, 0x009C, 0x009D, 0x009E, 0x009F, 0x00A0, 0x00A1, 0x00A2, 0x00A3, 0x00A4, 0x00A5, 0x00A6, 0x00A7, 0x00A8, 0x00A9, 0x00AA, 0x00AB, 0x00AC, 0x00AD, 0x00AE, 0x00AF, 0x00E0, 0x00E1, 0x00E2, 0x00E3, 0x00E4, 0x00E5, 0x00E6, 0x00E7, 0x00E8, 0x00E9, 0x00EA, 0x00EB, 0x00EC, 0x00ED, 0x00EE, 0x00EF, 0x00F0, 0x00F1, 0x00F3, 0x00F4, 0x00F5, 0x00F6, 0x00F7, 0x00F8, 0x00F9, 0x00FA, 0x00FB, 0x00FD, 0x00FC, 0x00FE, 0x008F, 0x008E, 0x00BA, 0x00BC, 0x00BB, 0x0080, 0x0081, 0x0082, 0x0083, 0x0084, 0x0085, 0x0086, 0x0087, 0x0088, 0x0089 };
            char[] Iran2 = { '/', ' ', ' ', 'آ', 'ا', 'ا', 'ب', 'ب', 'پ', 'پ', 'ت', 'ت', 'ث', 'ث', 'ج', 'ج', 'چ', 'چ', 'ح', 'ح', 'خ', 'خ', 'د', 'ذ', 'ر', 'ز', 'ژ', 'س', 'س', 'ش', 'ش', 'ص', 'ص', 'ض', 'ض', 'ط', 'ظ', 'ع', 'ع', 'ع', 'ع', 'غ', 'غ', 'غ', 'غ', 'ف', 'ف', 'ق', 'ق', 'ك', 'ك', 'گ', 'گ', 'ل', 'ل', 'م', 'م', 'ن', 'ن', 'و', 'ه', 'ه', 'ه', 'ي', 'ي', 'ي', 'ء', 'ئ', '،', '؟', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int i = 0, j = 0;
            l = len;
            for (i = 0; i < irsysText.Length; i++)
                for (j = 0; j < 81; j++)
                    if (irsysText[i] == Iran[j])
                    {
                        Rev[i] = Iran2[j];
                        --len;
                    }
            string str = "";
            for (i = 0; i < Rev.Length; i++)
            {
                str += Rev[i].ToString();
            }
            return str;
        }

    }
}
