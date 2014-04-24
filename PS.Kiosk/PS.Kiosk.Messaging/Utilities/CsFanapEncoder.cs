using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Messaging.Operations;
using PS.Kiosk.Messaging.Utilities;

namespace Fanap.Utilities
{
    public class CsFanapEncoder
    {
        #region Vars
        public string[,] P_Code = new string[12, 16];
        #endregion

        #region Construct
        public CsFanapEncoder()
        {
        }
        #endregion

        #region Methods
        public string FanapEncoding(string input)
        {
            string result = "", before = "", after = "", Temp = input;
            setValue();
            for (int k = 0; k <= input.Length - 1; k++)
            {
                Temp = (iEncode(input.Substring(k, 1))).PadRight(2, '0');
                if (k > 0) before = (iEncode(input.Substring(k - 1, 1))).PadRight(2, '0'); else before = "";
                if (k < (input.Length - 1)) after = (iEncode(input.Substring(k + 1, 1))).PadRight(2, '0'); else after = "";
                int place = ChrPlace(before, after);
                result += fEncode(Temp, place);
            }
            CsUtil util = new CsUtil();
            byte[] Resu = util.HexToBin(result);
            return System.Text.Encoding.UTF7.GetString(Resu);
        }

        private string rev(string s)
        {
            char[] c = s.ToCharArray();
            string ts = "";
            for (int i = s.Length - 1; i >= 0; i--)
            {
                ts += c[i].ToString();
            }

            return (ts);

        }

        private string DectoHex(string s)
        {
            int temp = int.Parse(s);
            string str = "";

            while (temp > 0)
            {
                int i = (int)temp % 16;
                switch (i)
                {
                    case 10: str += 'A'; break;
                    case 11: str += 'B'; break;
                    case 12: str += 'C'; break;
                    case 13: str += 'D'; break;
                    case 14: str += 'E'; break;
                    case 15: str += 'F'; break;
                    default: str += i.ToString(); break;
                }
                temp /= 16;
            }

            return (rev(str));
        }
        private string HextoDec(string s)
        {
            char[] c = s.ToCharArray();
            int p = 1;
            int temp = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                char t = c[i];
                switch (t)
                {
                    case 'A': temp += 10 * p; break;
                    case 'B': temp += 11 * p; break;
                    case 'C': temp += 12 * p; break;
                    case 'D': temp += 13 * p; break;
                    case 'E': temp += 14 * p; break;
                    case 'F': temp += 15 * p; break;
                    default: temp += (int.Parse(c[i].ToString())) * p; break;
                }
                p *= 16;
            }

            return (temp.ToString());
        }
        private void setValue()
        {

            P_Code[3, 0] = "0"; P_Code[3, 1] = "1"; P_Code[3, 2] = "2"; P_Code[3, 3] = "3"; P_Code[3, 4] = "4"; P_Code[3, 5] = "5";
            P_Code[3, 6] = "6"; P_Code[3, 7] = "7"; P_Code[3, 8] = "8"; P_Code[3, 9] = "9"; P_Code[3, 10] = "،"; P_Code[3, 11] = "-";
            P_Code[3, 12] = "؟"; P_Code[3, 13] = "آ"; P_Code[3, 14] = "ﺋ"; P_Code[3, 15] = "ء"; P_Code[4, 0] = "ا"; P_Code[4, 1] = "ﺎ";
            P_Code[4, 2] = "ب"; P_Code[4, 3] = "ﺑ"; P_Code[4, 4] = "پ"; P_Code[4, 5] = "ﭘ"; P_Code[4, 6] = "ت"; P_Code[4, 7] = "ﺗ";
            P_Code[4, 8] = "ث"; P_Code[4, 9] = "ﺛ"; P_Code[4, 10] = "ج"; P_Code[4, 11] = "ﺟ"; P_Code[4, 12] = "چ"; P_Code[4, 13] = "ﭼ";
            P_Code[4, 14] = "ح"; P_Code[4, 15] = "ﺣ"; P_Code[5, 0] = "خ"; P_Code[5, 1] = "ﺧ"; P_Code[5, 2] = "د"; P_Code[5, 3] = "ذ";
            P_Code[5, 1] = "ﺧ"; P_Code[5, 2] = "د"; P_Code[5, 3] = "ذ"; P_Code[5, 4] = "ر"; P_Code[5, 5] = "ز"; P_Code[5, 6] = "ژ";
            P_Code[5, 7] = "س"; P_Code[5, 8] = "ﺳ"; P_Code[5, 9] = "ش"; P_Code[5, 10] = "ﺷ"; P_Code[5, 11] = "ص"; P_Code[5, 12] = "ﺻ";
            P_Code[5, 13] = "ض"; P_Code[5, 14] = "ﺿ"; P_Code[5, 15] = "ط"; P_Code[6, 0] = "!"; P_Code[6, 1] = "%"; P_Code[6, 2] = ")";
            P_Code[6, 3] = "("; P_Code[6, 4] = "+"; P_Code[6, 5] = ","; P_Code[6, 6] = "÷"; P_Code[6, 7] = "."; P_Code[6, 8] = "/";
            P_Code[6, 9] = ":"; P_Code[6, 10] = "="; P_Code[6, 11] = "]"; P_Code[6, 12] = "["; P_Code[6, 13] = "}"; P_Code[6, 14] = "{";
            P_Code[6, 15] = "»"; P_Code[7, 0] = "«"; P_Code[7, 1] = "×"; P_Code[7, 2] = "›"; P_Code[7, 3] = "‹"; P_Code[7, 4] = "ﺒ";
            P_Code[7, 5] = "ﺘ"; P_Code[7, 6] = "ﺜ"; P_Code[7, 7] = "ﺞ"; P_Code[7, 8] = "ﺠ"; P_Code[7, 9] = "ﺢ"; P_Code[7, 10] = "ﺤ";
            P_Code[7, 11] = "ﺦ"; P_Code[7, 12] = "ﺨ"; P_Code[7, 13] = "ﺪ"; P_Code[7, 14] = "ﺬ"; P_Code[7, 15] = "ﺮ"; P_Code[8, 0] = "ﺰ";
            P_Code[8, 1] = "ﮋ"; P_Code[8, 2] = "ﺴ"; P_Code[8, 3] = "ﺸ"; P_Code[8, 4] = "ﺺ"; P_Code[8, 5] = "ﺼ"; P_Code[8, 6] = "ﺾ";
            P_Code[8, 7] = "ﻀ"; P_Code[8, 8] = "ﻄ"; P_Code[8, 9] = "ﻆ"; P_Code[8, 10] = "ﻔ"; P_Code[8, 11] = "ﻘ"; P_Code[8, 12] = "ﻜ";
            P_Code[8, 13] = "ﮕ"; P_Code[8, 14] = "ﻨ"; P_Code[8, 15] = "ﻪ"; P_Code[9, 0] = "ظ"; P_Code[9, 1] = "ع"; P_Code[9, 2] = "ﻊ";
            P_Code[9, 3] = "ﻌ"; P_Code[9, 4] = "ﻋ"; P_Code[9, 5] = "غ"; P_Code[9, 6] = "ﻎ"; P_Code[9, 7] = "ﻐ"; P_Code[9, 8] = "ﻏ";
            P_Code[9, 9] = "ف"; P_Code[9, 10] = "ﻓ"; P_Code[9, 11] = "ق"; P_Code[9, 12] = "ﻗ"; P_Code[9, 13] = "ک"; P_Code[9, 14] = "ﻛ";
            P_Code[9, 15] = "گ"; P_Code[10, 0] = "ﮔ"; P_Code[10, 1] = "ل"; P_Code[10, 2] = "لا"; P_Code[10, 3] = "ﻟ"; P_Code[10, 4] = "م";
            P_Code[10, 5] = "ﻣ"; P_Code[10, 6] = "ن"; P_Code[10, 7] = "ﻧ"; P_Code[10, 8] = "و"; P_Code[10, 9] = "ه"; P_Code[10, 10] = "ﻬ";
            P_Code[10, 11] = "ﻫ"; P_Code[10, 12] = "ﻲ"; P_Code[10, 13] = "ی"; P_Code[10, 14] = "ﻳ"; P_Code[10, 15] = " "; P_Code[11, 0] = "ي";
            /*****************************************************************************************************************************/
        }
        private string iEncode(string _input)
        {
            string result = "";
            if (_input == "ي")
                _input = "ی";
            for (int i = 3; i <= 10; i++)
            {
                for (int j = 0; j <= 15; j++)
                {

                    if (_input == P_Code[i, j])
                    {
                        result = DectoHex(i.ToString()) + DectoHex(j.ToString());
                    }
                }
            }
            return result;
        }
        private string fEncode(string _code, int _place)
        {
            switch (_code)
            {
                case "3D":
                    {
                        return "3D";
                    }
                    break;
                case "40":
                    {
                        if (_place == 1 || _place == 4) return "40";// break;
                        if (_place == 2 || _place == 3) return "41";
                    }
                    break;

                case "42":
                    {
                        if (_place == 1) return "43"; //break;
                        if (_place == 2) return "74"; //break;
                        if (_place == 3 || _place == 4) return "42";
                    }
                    break;
                case "44":
                    {
                        if (_place == 1 || _place == 2) return "45"; //break;
                        if (_place == 3 || _place == 4) return "44";
                    }
                    break;
                case "46":
                    {
                        if (_place == 1) return "47"; //break;
                        if (_place == 2) return "75"; //break;
                        if (_place == 3 || _place == 4) return "46";
                    }
                    break;
                case "48":
                    {
                        if (_place == 1) return "49"; //break;
                        if (_place == 2) return "76"; //break;
                        if (_place == 3 || _place == 4) return "48";
                    }
                    break;
                case "4A":
                    {
                        if (_place == 1) return "4B"; //break;
                        if (_place == 2) return "78"; //break;
                        if (_place == 3) return "77"; //break;
                        if (_place == 4) return "4A";
                    }
                    break;
                case "4C":
                    {
                        if (_place == 1 || _place == 2) return "4D"; //break;
                        if (_place == 3 || _place == 4) return "4C";
                    }
                    break;
                case "4E":
                    {
                        if (_place == 1) return "4F"; //break;
                        if (_place == 2) return "7A"; //break;
                        if (_place == 3) return "79"; //break;
                        if (_place == 4) return "4E";
                    }
                    break;
                case "50":
                    {
                        if (_place == 1) return "51"; //break;
                        if (_place == 2) return "7C"; //break;
                        if (_place == 3) return "7B"; //break;
                        if (_place == 4) return "50";
                    }
                    break;
                case "52":
                    {
                        if (_place == 1 || _place == 4) return "52"; //break;
                        if (_place == 2 || _place == 3) return "7D";
                    }
                    break;
                case "53":
                    {
                        if (_place == 1 || _place == 4) return "53"; //break;
                        if (_place == 2 || _place == 3) return "7E";
                    }
                    break;
                case "54":
                    {
                        if (_place == 1 || _place == 4) return "54"; //break;
                        if (_place == 2 || _place == 3) return "7F";
                    }
                    break;
                case "55":
                    {
                        if (_place == 1 || _place == 4) return "55"; //break;
                        if (_place == 2 || _place == 3) return "80";
                    }
                    break;
                case "56":
                    {
                        if (_place == 1 || _place == 4) return "56"; //break;
                        if (_place == 2 || _place == 3) return "81";
                    }
                    break;
                case "57":
                    {
                        if (_place == 1) return "58"; //break;
                        if (_place == 2) return "82"; //break;
                        if (_place == 3 || _place == 4) return "57";
                    }
                    break;
                case "59":
                    {
                        if (_place == 1) return "5A"; //break;
                        if (_place == 2) return "83"; //break;
                        if (_place == 3 || _place == 4) return "59";
                    }
                    break;
                case "5B":
                    {
                        if (_place == 1) return "5C"; //break;
                        if (_place == 2) return "85"; //break;
                        if (_place == 3) return "84"; //break;
                        if (_place == 4) return "5B";
                    }
                    break;
                case "5D":
                    {
                        if (_place == 1) return "5E"; //break;
                        if (_place == 2) return "87"; //break;
                        if (_place == 3) return "86"; //break;
                        if (_place == 4) return "5D";
                    }
                    break;
                case "5F":
                    {
                        if (_place == 1 || _place == 4) return "5F"; //break;
                        if (_place == 2 || _place == 3) return "88";
                    }
                    break;
                case "90":
                    {
                        if (_place == 1 || _place == 4) return "90"; //break;
                        if (_place == 2 || _place == 3) return "89";
                    }
                    break;
                case "91":
                    {
                        if (_place == 1) return "94"; //break;
                        if (_place == 2) return "93"; //break;
                        if (_place == 3) return "92"; //break;
                        if (_place == 4) return "91";
                    }
                    break;
                case "95":
                    {
                        if (_place == 1) return "98"; //break;
                        if (_place == 2) return "97"; //break;
                        if (_place == 3) return "96"; //break;
                        if (_place == 4) return "95";
                    }
                    break;
                case "99":
                    {
                        if (_place == 1) return "9A"; //break;
                        if (_place == 2) return "8A"; //break;
                        if (_place == 3 || _place == 4) return "99";
                    }
                    break;
                case "9B":
                    {
                        if (_place == 1) return "9C"; //break;
                        if (_place == 2) return "8B"; //break;
                        if (_place == 3 || _place == 4) return "9B";
                    }
                    break;
                case "9D":
                    {
                        if (_place == 1) return "9E"; //break;
                        if (_place == 2) return "8C"; //break;
                        if (_place == 3 || _place == 4) return "9D";
                    }
                    break;
                case "9F":
                    {
                        if (_place == 1) return "A0"; //break;
                        if (_place == 2) return "8D"; //break;
                        if (_place == 3 || _place == 4) return "9F";
                    }
                    break;
                case "A1":
                    {
                        if (_place == 1 || _place == 2) return "A3"; //break;
                        if (_place == 3 || _place == 4) return "A1";
                    }
                    break;
                case "A4":
                    {
                        if (_place == 1 || _place == 2) return "A5"; //break;
                        if (_place == 3 || _place == 4) return "A4";
                    }
                    break;
                case "A6":
                    {
                        if (_place == 1) return "A7"; //break;
                        if (_place == 2) return "8E"; //break;
                        if (_place == 3 || _place == 4) return "A6";
                    }
                    break;
                case "A8":
                    return "A8";
                    break;
                case "A9":
                    {
                        if (_place == 1) return "AB"; //break;
                        if (_place == 2) return "AA"; //break;
                        if (_place == 3) return "8F"; //break;
                        if (_place == 4) return "A9";
                    }
                    break;
                case "AD":
                    {
                        if (_place == 1 || _place == 2) return "AE"; //break;
                        if (_place == 3) return "AC"; //break;
                        if (_place == 4) return "AD";
                    }
                    break;
                case "3F":
                    {
                        if (_place == 1 || _place == 2) return "3E"; //break;
                        if (_place == 3 || _place == 4) return "3F";
                    }
                    break;

            }
            return _code;
        }
        private int ChrPlace(string _before, string _after)
        {
            if ((_before == "AF" || _before == "52" || _before == "53" || _before == "54" || _before == "55" || _before == "56" || _before == "7D" || _before == "7E" || _before == "7F" || _before == "80" || _before == "81" || _before == "A8" || _before == "40" || _before == "41" || _before == "3D" || _before == "" || _before == " ") && !(_after == "AF" || _after == "")) return 1;
            if (!(_before == "AF" || _before == "52" || _before == "53" || _before == "54" || _before == "55" || _before == "56" || _before == "7D" || _before == "7E" || _before == "7F" || _before == "80" || _before == "81" || _before == "A8" || _before == "40" || _before == "41" || _before == "3D" || _before == "" || _before == " ") && !(_after == "AF" || _after == "")) return 2;
            if (!(_before == "AF" || _before == "52" || _before == "53" || _before == "54" || _before == "55" || _before == "56" || _before == "7D" || _before == "7E" || _before == "7F" || _before == "80" || _before == "81" || _before == "A8" || _before == "40" || _before == "41" || _before == "3D" || _before == "" || _before == " ") && (_after == "AF" || _after == "")) return 3;
            //if ((_before == "AF" || _before == "52" || _before == "53" || _before == "54" || _before == "55" || _before == "56" || _before == "7D" || _before == "7E" || _before == "7F" || _before == "80" || _before == "81" || _before == "A8" || _before == "40" || _before == "41" || _before == "3D" || _before == "" || _before == " ") && (_after == " " || _after == "")) return "Last";
            return 4;
        }
        public string Decode(string _input)
        {
            string NewHex = UtilityMethods.Ascii2Hex(_input);
            setValue();
            //byte[] bte = System.Text.Encoding.ASCII.GetBytes(_input) ;
            //CsUtil util = new CsUtil();
            //_input = util.BinToHex(bte);
            _input = NewHex.ToUpper();
            //_input = _input.ToUpper();
            string _result = "";
            for (int i = 0; i < _input.Length - 1; i += 2)
            {
                String str = _input.Substring(i, 2);
                String b = str.Substring(0, 1);
                String c = str.Substring(1, 1);
                string res = P_Code[int.Parse(HextoDec(b)), int.Parse(HextoDec(c))];
                if (string.IsNullOrEmpty( res))
                {
                    
                    _result += Decode_1256(string.Concat(b,c)).Replace("\0","");
                }
                else
                    _result += res;
            }
            return _result;
        }




        string Decode_1256(string input)
        {
            int b = input.Length;
            byte[] c = new byte[b];
            for (int i = 0; i < input.Length; i += 2)
                c[(i / 2) ] = byte.Parse(input.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            return Encoding.GetEncoding(1256).GetString(c);
        }

        #endregion
    }
}
