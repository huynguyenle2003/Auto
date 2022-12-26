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
            return KeyDirectX.Space;
        }
    }
    public class ComboKey
    {
        Keys keys;
        string multikeys;
        List<KeyDirectX> combo;
        DateTime lastCall;
        string callWithMouse;
        bool qMustBeCall;
        public Keys Keys { get => keys; set => keys = value; }
        public string Multikeys { get => multikeys; set => multikeys = value; }
        public List<KeyDirectX> Combo { get => combo; set => combo = value; }
        public DateTime LastCall { get => lastCall; set => lastCall = value; }
        public string CallWithMouse { get => callWithMouse; set => callWithMouse = value; }
        public bool QMustBeCall { get => qMustBeCall; set => qMustBeCall = value; }

        public ComboKey(Keys m_key, string m_combo)
        {
            keys = m_key;
            Combo = new List<KeyDirectX>();
            foreach (char item in m_combo.ToLower())
            {
                Combo.Add(ComboSettings.GetKeyDirectXSelect(item));
            }
            LastCall = DateTime.Now;
            CallWithMouse = "default";
        }
        public ComboKey(string m_multikey, string m_combo)
        {
            multikeys = m_multikey;
            Combo = new List<KeyDirectX>();
            foreach (char item in m_combo.ToLower())
            {
                Combo.Add(ComboSettings.GetKeyDirectXSelect(item));
            }
            LastCall = DateTime.Now;
            CallWithMouse = "default";
        }
    }
    public class ComboOption
    {
        Point pointQ;
        Size sizeQ;
        bool saveImg;

        public Point PointQ { get => pointQ; set => pointQ = value; }
        public Size SizeQ { get => sizeQ; set => sizeQ = value; }
        public bool SaveImg { get => saveImg; set => saveImg = value; }
        public ComboOption()
        {
            sizeQ = new Size(15, 15);
            pointQ = new Point(810, 965);
            saveImg = false;
        }
    }
}
