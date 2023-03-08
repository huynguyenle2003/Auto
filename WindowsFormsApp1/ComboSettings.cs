using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class ComboSettings
    {
        private string key1;
        private string key2;
        private string key3;
        private string key4;
        private string key5;
        private string key6;
        private string key7;
        private string key8;
        private string key9;
        private string key10;
        private int delayOut;
        private int delayIn;
        private List<LineTuongBang> l_tuongbang;

        public string Key1 { get => key1; set => key1 = value; }
        public string Key2 { get => key2; set => key2 = value; }
        public string Key3 { get => key3; set => key3 = value; }
        public string Key4 { get => key4; set => key4 = value; }
        public string Key5 { get => key5; set => key5 = value; }
        public string Key6 { get => key6; set => key6 = value; }
        public string Key7 { get => key7; set => key7 = value; }
        public string Key8 { get => key8; set => key8 = value; }
        public string Key9 { get => key9; set => key9 = value; }
        public string Key10 { get => key10; set => key10 = value; }
        public int DelayOut { get => delayOut; set => delayOut = value; }
        public int DelayIn { get => delayIn; set => delayIn = value; }
        public List<LineTuongBang> L_tuongbang { get => l_tuongbang; set => l_tuongbang = value; }

        public ComboSettings()
        {
            #region khai bao tuong bang
            L_tuongbang = new List<LineTuongBang>();

            L_tuongbang.Add(new LineTuongBang(0, new Point(130, 300), new Point(209, -390)));
            L_tuongbang.Add(new LineTuongBang(6, new Point(480, 36), new Point(-353, -352)));
            L_tuongbang.Add(new LineTuongBang(12, new Point(353, -352), new Point(-455, 66)));
            L_tuongbang.Add(new LineTuongBang(18, new Point(-190, -447), new Point(-120, -287)));
            L_tuongbang.Add(new LineTuongBang(24, new Point(-482, -64), new Point(279, 228)));
            L_tuongbang.Add(new LineTuongBang(30, new Point(-271, 238), new Point(438, -63)));

            L_tuongbang.Add(new LineTuongBang(1, new Point(196, 254), new Point(104, -394)));
            L_tuongbang.Add(new LineTuongBang(7, new Point(495, -35), new Point(-460, -312)));
            L_tuongbang.Add(new LineTuongBang(13, new Point(273, -412), new Point(-393, 100)));
            L_tuongbang.Add(new LineTuongBang(19, new Point(-267, -391), new Point(-47, 305)));
            L_tuongbang.Add(new LineTuongBang(25, new Point(-453, -3), new Point(299, 184)));
            L_tuongbang.Add(new LineTuongBang(31, new Point(-232, 270), new Point(456, -128)));

            L_tuongbang.Add(new LineTuongBang(2, new Point(252, 240), new Point(20, -389)));
            L_tuongbang.Add(new LineTuongBang(8, new Point(484, -118), new Point(-434, -230)));
            L_tuongbang.Add(new LineTuongBang(14, new Point(148, -397), new Point(-377, 146)));
            L_tuongbang.Add(new LineTuongBang(20, new Point(-362, -364), new Point(23, 291)));
            L_tuongbang.Add(new LineTuongBang(26, new Point(-437, 36), new Point(358, 162)));
            L_tuongbang.Add(new LineTuongBang(32, new Point(-146, 286), new Point(436, -210)));

            L_tuongbang.Add(new LineTuongBang(3, new Point(334, 217), new Point(-66, -412)));
            L_tuongbang.Add(new LineTuongBang(9, new Point(483, -171), new Point(-457, -171)));
            L_tuongbang.Add(new LineTuongBang(15, new Point(60, -415), new Point(-305, 223)));
            L_tuongbang.Add(new LineTuongBang(21, new Point(-468, -342), new Point(105, 293)));
            L_tuongbang.Add(new LineTuongBang(27, new Point(-422, 111), new Point(422, 111)));
            L_tuongbang.Add(new LineTuongBang(33, new Point(-85, 305), new Point(432, -300)));

            L_tuongbang.Add(new LineTuongBang(4, new Point(379, 152), new Point(-169, -396)));
            L_tuongbang.Add(new LineTuongBang(10, new Point(494, -258), new Point(-442, -93)));
            L_tuongbang.Add(new LineTuongBang(16, new Point(-42, -399), new Point(-260, 235)));
            L_tuongbang.Add(new LineTuongBang(22, new Point(-502, -245), new Point(139, 265)));
            L_tuongbang.Add(new LineTuongBang(28, new Point(-384, 148), new Point(405, 53)));
            L_tuongbang.Add(new LineTuongBang(34, new Point(-29, 318), new Point(349, -364)));

            L_tuongbang.Add(new LineTuongBang(5, new Point(413, 98), new Point(-293, -402)));
            L_tuongbang.Add(new LineTuongBang(11, new Point(461, -344), new Point(-464, -22)));
            L_tuongbang.Add(new LineTuongBang(17, new Point(-128, -402), new Point(-202, 300)));
            L_tuongbang.Add(new LineTuongBang(23, new Point(-520, -168), new Point(201, 242)));
            L_tuongbang.Add(new LineTuongBang(29, new Point(-351, 203), new Point(418, 2)));
            L_tuongbang.Add(new LineTuongBang(35, new Point(55, 313), new Point(262, -398)));
            #endregion
        }

        public static KeyDirectX GetKeyDirectXSelect(char item)
        {
            switch (item)
            {
                case '0':
                    return KeyDirectX.D0;
                case '1':
                    return KeyDirectX.D1;
                case '2':
                    return KeyDirectX.D2;
                case '3':
                    return KeyDirectX.D3;
                case '4':
                    return KeyDirectX.D4;
                case '5':
                    return KeyDirectX.D5;
                case '6':
                    return KeyDirectX.D6;
                case '7':
                    return KeyDirectX.D7;
                case '8':
                    return KeyDirectX.D8;
                case '9':
                    return KeyDirectX.D9;
                case 'q':
                    return KeyDirectX.Q;
                case 'w':
                    return KeyDirectX.W;
                case 'e':
                    return KeyDirectX.E;
                case 'r':
                    return KeyDirectX.R;
                case 't':
                    return KeyDirectX.T;
                case 'y':
                    return KeyDirectX.Y;
                case 'u':
                    return KeyDirectX.U;
                case 'i':
                    return KeyDirectX.I;
                case 'o':
                    return KeyDirectX.O;
                case 'p':
                    return KeyDirectX.P;
                case 'a':
                    return KeyDirectX.A;
                case 's':
                    return KeyDirectX.S;
                case 'd':
                    return KeyDirectX.D;
                case 'f':
                    return KeyDirectX.F;
                case 'g':
                    return KeyDirectX.G;
                case 'h':
                    return KeyDirectX.H;
                case 'j':
                    return KeyDirectX.J;
                case 'k':
                    return KeyDirectX.K;
                case 'l':
                    return KeyDirectX.L;
                case 'z':
                    return KeyDirectX.Z;
                case 'x':
                    return KeyDirectX.X;
                case 'c':
                    return KeyDirectX.C;
                case 'v':
                    return KeyDirectX.V;
                case 'b':
                    return KeyDirectX.B;
                case 'n':
                    return KeyDirectX.N;
                case 'm':
                    return KeyDirectX.M;
                case '*':
                    return KeyDirectX.NumPadStar;
                case '`':
                    return KeyDirectX.Grave;
                case '.':
                    return KeyDirectX.Decimal;
                case '-':
                    return KeyDirectX.NumPadMinus;
                case '+':
                    return KeyDirectX.NumPadPlus;
                case '/':
                    return KeyDirectX.Divide;
                default:
                    break;
            }
            return KeyDirectX.Nothing;
        }
        public static Keys GetKeySelect(char item)
        {
            switch (item)
            {
                case '0':
                    return Keys.D0;
                case '1':
                    return Keys.D1;
                case '2':
                    return Keys.D2;
                case '3':
                    return Keys.D3;
                case '4':
                    return Keys.D4;
                case '5':
                    return Keys.D5;
                case '6':
                    return Keys.D6;
                case '7':
                    return Keys.D7;
                case '8':
                    return Keys.D8;
                case '9':
                    return Keys.D9;
                case 'q':
                    return Keys.Q;
                case 'w':
                    return Keys.W;
                case 'e':
                    return Keys.E;
                case 'r':
                    return Keys.R;
                case 't':
                    return Keys.T;
                case 'y':
                    return Keys.Y;
                case 'u':
                    return Keys.U;
                case 'i':
                    return Keys.I;
                case 'o':
                    return Keys.O;
                case 'p':
                    return Keys.P;
                case 'a':
                    return Keys.A;
                case 's':
                    return Keys.S;
                case 'd':
                    return Keys.D;
                case 'f':
                    return Keys.F;
                case 'g':
                    return Keys.G;
                case 'h':
                    return Keys.H;
                case 'j':
                    return Keys.J;
                case 'k':
                    return Keys.K;
                case 'l':
                    return Keys.L;
                case 'z':
                    return Keys.Z;
                case 'x':
                    return Keys.X;
                case 'c':
                    return Keys.C;
                case 'v':
                    return Keys.V;
                case 'b':
                    return Keys.B;
                case 'n':
                    return Keys.N;
                case 'm':
                    return Keys.M;
                case '*':
                    return Keys.Multiply;
                case '`':
                    return Keys.Oemtilde;
                case '.':
                    return Keys.Decimal;
                case '-':
                    return Keys.Subtract;
                case '+':
                    return Keys.Add;
                case '/':
                    return Keys.OemQuestion;
                default:
                    break;
            }
            return Keys.None;
            
        }
    }
    public class ComboKey
    {
        Keys keys;
        string multikeys;
        List<KeyDirectX> combo;
        DateTime lastCall;
        ComboKeyOption option;
        List<List<KeyDirectX>> l_code_combo;
        public Keys Keys { get => keys; set => keys = value; }
        public string Multikeys { get => multikeys; set => multikeys = value; }
        public List<KeyDirectX> Combo { get => combo; set => combo = value; }
        public DateTime LastCall { get => lastCall; set => lastCall = value; }
        public ComboKeyOption Option { get => option; set => option = value; }
        public List<List<KeyDirectX>> L_code_combo { get => l_code_combo; set => l_code_combo = value; }


        //public ComboKey(Keys m_key, string m_combo)
        //{
        //    keys = m_key;
        //    Combo = new List<KeyDirectX>();
        //    foreach (char item in m_combo.ToLower())
        //    {
        //        Combo.Add(ComboSettings.GetKeyDirectXSelect(item));
        //    }
        //    LastCall = DateTime.Now;
        //    //SpecialHero = "";
        //    //CallWithMouse = "default";
        //    //LockMouseUntilPressThis = KeyDirectX.Nothing;
        //}
        public ComboKey(string inputkeys, string m_combo, object m_option)
        {
            if (inputkeys.Length == 1)
            {
                keys = ComboSettings.GetKeySelect(inputkeys[0]);
            }
            else
            {
                multikeys = inputkeys;
            }
            LastCall = DateTime.Now;
            if (m_option == null)
            {
                Option = new ComboKeyOption();
            }
            else
            {
                Option = (ComboKeyOption)m_option;
            }
            Combo = new List<KeyDirectX>();
            L_code_combo = new List<List<KeyDirectX>>();
            if (m_combo.StartsWith("code"))
            {
                string[] list_cb = m_combo.Split(';');
                Option.ComboName = list_cb[0];
                switch (Option.SpecialHero)
                {
                    case "Invoker":
                        //if (Option.ComboName == ("code1") || Option.ComboName == ("code2") || Option.ComboName == ("code3"))
                        {
                            //List<string> list_cb = new List<string>() { "ew-qq-sff-d-dd-wwer*ftx-eq*x", "-qq-sff-d-dd-wwer*ftx-eq*x" };
                            //List<string> list_cb = new List<string>() { "ew-qq-sew", "-qq-sew" };

                            //foreach (string cb in list_cb)
                            for (int i = 1; i < list_cb.Length; i++)
                            {
                                string cb = list_cb[i];
                                List<KeyDirectX> temp_combo = new List<KeyDirectX>();
                                foreach (char item in cb.ToLower())
                                {
                                    temp_combo.Add(ComboSettings.GetKeyDirectXSelect(item));
                                }
                                L_code_combo.Add(temp_combo);
                            }
                        }
                        break;
                    case "Tinker-code":
                        {
                            for (int i = 1; i < list_cb.Length; i++)
                            {
                                string cb = list_cb[i];
                                List<KeyDirectX> temp_combo = new List<KeyDirectX>();
                                foreach (char item in cb.ToLower())
                                {
                                    temp_combo.Add(ComboSettings.GetKeyDirectXSelect(item));
                                }
                                L_code_combo.Add(temp_combo);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                foreach (char item in m_combo.ToLower())
                {
                    Combo.Add(ComboSettings.GetKeyDirectXSelect(item));
                }
            }
        }
    }
    public class ComboOption
    {
        Point pointQ;
        Size sizeQ;
        bool saveImg;
        int blinkMin;
        int blinkDelay;
        string onOff;
        bool continueAfterFailed;
        Point pointR;
        Size sizeR;
        Point pointD;
        Size sizeD;
        int lanThu;
        int lanThuDelay;
        Point pointF;
        Size sizeF;

        public Point PointQ { get => pointQ; set => pointQ = value; }
        public Size SizeQ { get => sizeQ; set => sizeQ = value; }
        public bool SaveImg { get => saveImg; set => saveImg = value; }
        public int BlinkMin { get => blinkMin; set => blinkMin = value; }
        public int BlinkDelay { get => blinkDelay; set => blinkDelay = value; }
        public string OnOff { get => onOff; set => onOff = value; }
        public bool ContinueAfterFailed { get => continueAfterFailed; set => continueAfterFailed = value; }
        public Point PointR { get => pointR; set => pointR = value; }
        public Size SizeR { get => sizeR; set => sizeR = value; }
        public Point PointD { get => pointD; set => pointD = value; }
        public Size SizeD { get => sizeD; set => sizeD = value; }
        public int LanThu { get => lanThu; set => lanThu = value; }
        public int LanThuDelay { get => lanThuDelay; set => lanThuDelay = value; }
        public Point PointF { get => pointF; set => pointF = value; }
        public Size SizeF { get => sizeF; set => sizeF = value; }

        public ComboOption()
        {
            sizeQ = new Size(15, 15);
            pointQ = new Point(800, 950);
            saveImg = false;
            blinkMin = 200;
            BlinkDelay = 0;
            OnOff = "`";
            ContinueAfterFailed = false;
            SizeR = new Size(15, 15);
            PointR = new Point(1100, 950);
            SizeD = new Size(15, 15);
            PointD = new Point(1035, 950);
            LanThu = 5;
            LanThuDelay = 200;
            SizeF = new Size(15, 15);
            PointF = new Point(1015, 950);
        }
    }
    public class ComboKeyOption
    {
        string specialHero = "";// tinker - dam bao Q duoc moi combo tiep
        string callWithMouse = "default";//kiem tra hinh dang chuot truoc khi goi combo
        KeyDirectX lockMouseUntilPressThis = KeyDirectX.Nothing; // khoa chuot o vi tri hien tai den khi key nay duoc send
        public string CallWithMouse { get => callWithMouse; set => callWithMouse = value; }
        public string SpecialHero { get => specialHero; set => specialHero = value; }
        public KeyDirectX LockMouseUntilPressThis { get => lockMouseUntilPressThis; set => lockMouseUntilPressThis = value; }
        string comboName;
        public string ComboName { get => comboName; set => comboName = value; }
        public ComboKeyOption()
        {
            ComboName = "";
        }
    }
    public class LineTuongBang
    {
        private int goc_i;
        private Point start;
        private Point end;

        public int Goc_i { get => goc_i; set => goc_i = value; }
        public Point Start { get => start; set => start = value; }
        public Point End { get => end; set => end = value; }
        public LineTuongBang(int m_goc_i, Point m_start, Point m_end)
        {
            Goc_i = m_goc_i;
            Start = m_start;
            End = m_end;
        }
        public static double TichVoHuong(Point input, Point start, Point end)
        {
            double vt_start_x = start.X - input.X;
            double vt_start_y = start.Y - input.Y;
            double vt_start_length = Math.Sqrt(vt_start_x * vt_start_x + vt_start_y * vt_start_y);
            vt_start_x = vt_start_x / vt_start_length;
            vt_start_y = vt_start_y / vt_start_length;

            double vt_end_x = end.X - input.X;
            double vt_end_y = end.Y - input.Y;
            double vt_end_length = Math.Sqrt(vt_end_x * vt_end_x + vt_end_y * vt_end_y);
            vt_end_x = vt_end_x / vt_end_length;
            vt_end_y = vt_end_y / vt_end_length;

            double result = vt_start_x * vt_end_x + vt_start_y * vt_end_y;
            return result;
        }
        private double TichVoHuongMoRong(Point input, double heso)
        {
            //if (heso==1)
            //{
            //    ;
            //}
            double center_x = 0.5 * (start.X + end.X);
            double center_y = 0.5 * (start.Y + end.Y);
            double vt_start_x = start.X - center_x;
            double vt_start_y = start.Y - center_y;
            int new_start_x = (int)(center_x + vt_start_x * heso);
            int new_start_y = (int)(center_y + vt_start_y * heso);
            int new_end_x = (int)(center_x - vt_start_x * heso);
            int new_end_y = (int)(center_y - vt_start_y * heso);
            return TichVoHuong(input, new Point(new_start_x, new_start_y), new Point(new_end_x, new_end_y));
        }
        private double KhoangCachVuongGoc(Point input)
        {
            double dientich = 0.5 * Math.Abs((End.X - Start.X) * (input.Y - Start.Y) - (input.X - Start.X) * (End.Y - Start.Y));
            double x = start.X - end.X;
            double y = start.Y - end.Y;
            double length_start_end = Math.Sqrt(x * x + y * y);
            return dientich / length_start_end;
        }
        public bool IsApproval(Point input, out double khoangcach)
        {
            double khoangCachChoPhep = 25;
            double MoRongChoPhep = 1.3;
            khoangcach = KhoangCachVuongGoc(input);
            if (khoangcach < khoangCachChoPhep)
            {
                if (TichVoHuongMoRong(input, 1) <= 0)
                {
                    return true;
                }
                else if (TichVoHuongMoRong(input, MoRongChoPhep) <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
