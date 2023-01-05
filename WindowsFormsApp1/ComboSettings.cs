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

        public ComboSettings()
        {

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
                default:
                    break;
            }
            return Keys.Space;

        }
    }
    public class ComboKey
    {
        Keys keys;
        string multikeys;
        List<KeyDirectX> combo;
        DateTime lastCall;
        ComboKeyOption option;

        public Keys Keys { get => keys; set => keys = value; }
        public string Multikeys { get => multikeys; set => multikeys = value; }
        public List<KeyDirectX> Combo { get => combo; set => combo = value; }
        public DateTime LastCall { get => lastCall; set => lastCall = value; }
        public ComboKeyOption Option { get => option; set => option = value; }


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
            Combo = new List<KeyDirectX>();
            foreach (char item in m_combo.ToLower())
            {
                Combo.Add(ComboSettings.GetKeyDirectXSelect(item));
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
        }
    }
    public class ComboOption
    {
        Point pointQ;
        Size sizeQ;
        bool saveImg;
        int blinkMin;
        int blinkDelay;

        public Point PointQ { get => pointQ; set => pointQ = value; }
        public Size SizeQ { get => sizeQ; set => sizeQ = value; }
        public bool SaveImg { get => saveImg; set => saveImg = value; }
        public int BlinkMin { get => blinkMin; set => blinkMin = value; }
        public int BlinkDelay { get => blinkDelay; set => blinkDelay = value; }

        public ComboOption()
        {
            sizeQ = new Size(15, 15);
            pointQ = new Point(810, 965);
            saveImg = false;
            blinkMin = 200;
            BlinkDelay = 0;
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
        public ComboKeyOption()
        {

        }
    }
}
