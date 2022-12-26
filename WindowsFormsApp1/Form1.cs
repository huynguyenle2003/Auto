using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KAutoHelper;
using Gma.UserActivityMonitor;
using Microsoft.DirectX.DirectInput;
using System.Reflection;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        int NoAcc = 0;
        Point nutDN = new Point();
        Image nutDN_image;
        String duongdan = null;
        Image Dis;
        Size dis_size;
        Point dis_point;
        bool isCtroldown = false;
        bool isSdown = false;
        static bool isQdown = false;
        bool stop = false;
        Image Sever;
        Point sub_point;
        Image Sub1;
        Size sub1_size;
        Point sub1_point;
        Image Sub2;
        Size sub2_size;
        Point sub2_point;
        Image Sub3;
        Size sub3_size;
        Point sub3_point;
        Image Sub4;
        Size sub4_size;
        Point sub4_point;
        Point sv_point;

        ThreadStart ts_combo;
        Thread thread_combo;
        bool AutoCombo_on = false;
        ThreadStart ts_autoQ;
        Thread thread_autoQ;
        bool AutoQ_on = false;
        //private static Bitmap CaptureScreen(Screen window)
        //{

        //    Rectangle s_rect = window.Bounds;
        //    Bitmap bmp = new Bitmap(s_rect.Width, s_rect.Height);

        //    using (Graphics gScreen = Graphics.FromImage(bmp))
        //        gScreen.CopyFromScreen(s_rect.Location, Point.Empty, s_rect.Size);
        //    return bmp;
        //    // bmp.Save(file, System.Drawing.Imaging.ImageFormat.Png);

        //}
        public static void HuyBamPhim(KeyCode key, int miligiay)
        {
            AutoControl.SendKeyDown(key);
            System.Threading.Thread.Sleep(miligiay);
            AutoControl.SendKeyUp(key);
            System.Threading.Thread.Sleep(miligiay);
        }
        public static void HuySendText(String text, int time)
        {
            string text2 = text.ToLower();
            for (int i = 0; i < text2.Length; i++)
            {
                char c = text2[i];
                KeyCode key = KeyCode.SPACE_BAR;
                if (c != ' ')
                {
                    switch (c)
                    {
                        case '0':
                            key = KeyCode.KEY_0;
                            break;
                        case '1':
                            key = KeyCode.KEY_1;
                            break;
                        case '2':
                            key = KeyCode.KEY_2;
                            break;
                        case '3':
                            key = KeyCode.KEY_3;
                            break;
                        case '4':
                            key = KeyCode.KEY_4;
                            break;
                        case '5':
                            key = KeyCode.KEY_5;
                            break;
                        case '6':
                            key = KeyCode.KEY_6;
                            break;
                        case '7':
                            key = KeyCode.KEY_7;
                            break;
                        case '8':
                            key = KeyCode.KEY_8;
                            break;
                        case '9':
                            key = KeyCode.KEY_9;
                            break;
                        default:
                            switch (c)
                            {
                                case 'a':
                                    key = KeyCode.KEY_A;
                                    break;
                                case 'b':
                                    key = KeyCode.KEY_B;
                                    break;
                                case 'c':
                                    key = KeyCode.KEY_C;
                                    break;
                                case 'd':
                                    key = KeyCode.KEY_D;
                                    break;
                                case 'e':
                                    key = KeyCode.KEY_E;
                                    break;
                                case 'f':
                                    key = KeyCode.KEY_F;
                                    break;
                                case 'g':
                                    key = KeyCode.KEY_G;
                                    break;
                                case 'h':
                                    key = KeyCode.KEY_H;
                                    break;
                                case 'i':
                                    key = KeyCode.KEY_I;
                                    break;
                                case 'j':
                                    key = KeyCode.KEY_J;
                                    break;
                                case 'k':
                                    key = KeyCode.KEY_K;
                                    break;
                                case 'l':
                                    key = KeyCode.KEY_L;
                                    break;
                                case 'm':
                                    key = KeyCode.KEY_M;
                                    break;
                                case 'n':
                                    key = KeyCode.KEY_N;
                                    break;
                                case 'o':
                                    key = KeyCode.KEY_O;
                                    break;
                                case 'p':
                                    key = KeyCode.KEY_P;
                                    break;
                                case 'q':
                                    key = KeyCode.KEY_Q;
                                    break;
                                case 'r':
                                    key = KeyCode.KEY_R;
                                    break;
                                case 's':
                                    key = KeyCode.KEY_S;
                                    break;
                                case 't':
                                    key = KeyCode.KEY_T;
                                    break;
                                case 'u':
                                    key = KeyCode.KEY_U;
                                    break;
                                case 'v':
                                    key = KeyCode.KEY_V;
                                    break;
                                case 'w':
                                    key = KeyCode.KEY_W;
                                    break;
                                case 'x':
                                    key = KeyCode.KEY_X;
                                    break;
                                case 'y':
                                    key = KeyCode.KEY_Y;
                                    break;
                                case 'z':
                                    key = KeyCode.KEY_Z;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    key = KeyCode.SPACE_BAR;
                }
                HuyBamPhim(key, time);
            }
        }
        public static void HuyMouseClick(int x, int y, EMouseKey mouseKey = EMouseKey.LEFT)
        {
            Cursor.Position = new Point(x, y);
            Form1.HuyClick(mouseKey);
        }

        public static void HuyMouseClick(Point point, EMouseKey mouseKey = EMouseKey.LEFT)
        {
            Cursor.Position = point;
            Form1.HuyClick(mouseKey);
        }
        public static void HuyClick(EMouseKey mouseKey = EMouseKey.LEFT)
        {
            switch (mouseKey)
            {
                case EMouseKey.LEFT:
                    AutoControl.mouse_event(32770u, 0u, 0u, 0u, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(100);
                    AutoControl.mouse_event(32772u, 0u, 0u, 0u, UIntPtr.Zero);
                    //System.Threading.Thread.Sleep(100);
                    //AutoControl.mouse_event(32770u, 0u, 0u, 0u, UIntPtr.Zero);
                    //System.Threading.Thread.Sleep(100);
                    //AutoControl.mouse_event(32772u, 0u, 0u, 0u, UIntPtr.Zero);
                    return;
                case EMouseKey.RIGHT:
                    AutoControl.mouse_event(0x08, 0u, 0u, 0u, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(100);
                    AutoControl.mouse_event(0x10, 0u, 0u, 0u, UIntPtr.Zero);

                    return;
                case EMouseKey.DOUBLE_LEFT:
                    AutoControl.mouse_event(32774u, 0u, 0u, 0u, UIntPtr.Zero);
                    AutoControl.mouse_event(32774u, 0u, 0u, 0u, UIntPtr.Zero);
                    return;
                case EMouseKey.DOUBLE_RIGHT:
                    AutoControl.mouse_event(32792u, 0u, 0u, 0u, UIntPtr.Zero);
                    AutoControl.mouse_event(32792u, 0u, 0u, 0u, UIntPtr.Zero);
                    return;
                default:
                    return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = IntPtr.Zero;
            hWnd = AutoControl.FindWindowHandle(null, "Mu Ha Noi Xua");
            AutoControl.BringToFront(hWnd);
            this.Hide();
            //  var screen = KAutoHelper.CaptureHelper.CaptureScreen();
            //screen.Save("mainScreen.PNG");
            // System.Drawing.Bitmap subBitmap = WindowsFormsApp1.Properties.Resources.Lorencia;
            //  var loren = ImageScanOpenCV.FindOutPoint((Bitmap)screen, (Bitmap)Sever, 0.8);
            // sv_point = loren.Value;

            Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
            Thread.Sleep(1000);
            //  Bitmap bitkq;
            stop = false;
            // this.Show();
            ThreadStart ts = new ThreadStart(dangnhap);
            Thread thread = new Thread(ts);
            thread.IsBackground = true;
            thread.Start();
            if (chb_dis.Checked)
            {
                ThreadStart ts_dis = new ThreadStart(dis);
                Thread thread_dis = new Thread(ts_dis);
                thread_dis.IsBackground = true;
                thread_dis.Start();
            }

            // bitkq.Save("KQ.PNG");

            this.Show();
            // screen.Save("windows.PNG");
            // Form1.LeftClick(754, 460);


            //Process[] processes = Process.GetProcessesByName("main (32 bit)");

            //foreach (Process proc in processes)
            //{
            //    SetForegroundWindow(proc.MainWindowHandle);
            //    SendKeys.SendWait("{C}");
            //}

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                Sever = CaptureHelper.CaptureImage(new Size(frm2.selectWidth, frm2.selectHeight), new Point(frm2.selectX, frm2.selectY));
                //MessageBox.Show(System.IO.Directory.GetCurrentDirectory() + @"\Image\Server.PNG");
                //if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Image\\Server.PNG"))
                //{
                //    System.IO.File.Delete(System.IO.Directory.GetCurrentDirectory() + @"\Image\\Server.PNG");
                //}

                Sever.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Server.PNG");
                pictureBox1.Image = Sever;
                sv_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Server_point = sv_point;
                Properties.Settings.Default.Save();
            }
            pictureBox1.Refresh();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                sub1_size = new Size(frm2.selectWidth, frm2.selectHeight);
                Properties.Settings.Default.Sub1_size = sub1_size;
                sub1_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Sub1_point = sub1_point;
                Properties.Settings.Default.Save();
                Sub1 = CaptureHelper.CaptureImage(sub1_size, sub1_point);
                Sub1.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub1.PNG");
                pictureBox2.Image = Sub1;
            }
            pictureBox2.Refresh();
            this.Show();
        }






        #region commnet
        // IntPtr hWnd = IntPtr.Zero;
        // hWnd = AutoControl.FindWindowHandle(null, "Mu Ha Noi Xua");
        // // //var childhWnd = AutoControl.FindHandle(hWnd, null, "hgabel2");
        // AutoControl.BringToFront(hWnd);
        //// Cursor.Position = new Point(300, 300);

        //    System.Threading.Thread.Sleep(2000);
        // Cursor.Position = new Point(500, 500);
        // MessageBox.Show(Cursor.Position.ToString());
        // IntPtr lParam = AutoControl.MakeLParamFromXY(x, y);
        // AutoControl.SendMessage(IntPtr.Zero, 0x0201, 0x0001, 0u);
        //AutoControl.SendKeyDown(KeyCode.KEY_C);
        // AutoControl.SendKeyFocus(KeyCode.KEY_C);

        // Form1.Click(EMouseKey.LEFT);
        //  Form1.MouseClick(754, 460, EMouseKey.LEFT);
        // AutoControl.BringToFront(hWnd);
        // System.Threading.Thread.Sleep(2000);

        //  AutoControl.mouse_event(32770u, 0u, 0u, 0u, UIntPtr.Zero);
        //System.Threading.Thread.Sleep(100);
        //AutoControl.SendKeyPress(KeyCode.KEY_Q);
        //if (hWnd != null)
        //{
        //    //AutoControl.BringToFront(hWnd);

        //    //  Form1.MouseClick(600, 360, EMouseKey.LEFT);


        //    // var screen = CaptureScreen(Screen.PrimaryScreen);
        //    var screen = CaptureHelper.CaptureScreen();
        //    System.Drawing.Bitmap subBitmap = WindowsFormsApp1.Properties.Resources.Lorencia;
        //    // var subBitmap = ImageScanOpenCV.GetImage("template.PNG");

        //    var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitmap, 0.5);
        //    var Bi = ImageScanOpenCV.Find((Bitmap)screen, subBitmap, 0.5);
        //    screen.Save("mainScreen.PNG");

        //    if (resBitmap != null)
        //    {
        //        Bi.Save("test.PNG");
        //        // ((Bitmap)resBitmap).Save("test.PNG");
        //        //var pointToClick = AutoControl.GetGlobalPoint(hWnd, resBitmap);
        //        Form1.MouseClick(resBitmap.Value, EMouseKey.LEFT);
        //     //   MessageBox.Show(resBitmap.Value.ToString());
        //    }
        //}
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            ts_combo = new ThreadStart(autocombo);

            ts_autoQ = new ThreadStart(autobomQ);

            HookManager.KeyPress += HookManager_KeyPress;
            if (!System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Image\"))
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Image\");
            // HookManager.KeyPress += hotkey_press;
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Server.PNG"))
                {
                    Sever = new Bitmap(bmpTemp);
                }
                pictureBox1.Image = Sever;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub1.PNG"))
                {
                    Sub1 = new Bitmap(bmpTemp);
                }
                pictureBox2.Image = Sub1;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub2.PNG"))
                {
                    Sub2 = new Bitmap(bmpTemp);
                }
                pictureBox3.Image = Sub2;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub3.PNG"))
                {
                    Sub3 = new Bitmap(bmpTemp);
                }
                pictureBox4.Image = Sub3;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub4.PNG"))
                {
                    Sub4 = new Bitmap(bmpTemp);
                }
                pictureBox5.Image = Sub4;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\NutDN.PNG"))
                {
                    nutDN_image = new Bitmap(bmpTemp);
                }
                pictureBox7.Image = nutDN_image;
            }
            catch (Exception)
            {
            }
            try
            {
                using (var bmpTemp = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Image\Dis.PNG"))
                {
                    Dis = new Bitmap(bmpTemp);
                }

                pictureBox6.Image = Dis;
            }
            catch (Exception)
            {
            }
            duongdan = Properties.Settings.Default.duongdan;
            label3.Text = duongdan;
            sv_point = Properties.Settings.Default.Server_point;
            sub1_point = Properties.Settings.Default.Sub1_point;
            sub1_size = Properties.Settings.Default.Sub1_size;
            sub2_point = Properties.Settings.Default.Sub2_point;
            sub2_size = Properties.Settings.Default.Sub2_size;
            sub3_point = Properties.Settings.Default.Sub3_point;
            sub3_size = Properties.Settings.Default.Sub3_size;
            sub4_point = Properties.Settings.Default.Sub4_point;
            sub4_size = Properties.Settings.Default.Sub4_size;
            nutDN = Properties.Settings.Default.NutDangNhap_point;
            dis_point = Properties.Settings.Default.Dis_point;
            dis_size = Properties.Settings.Default.Dis_size;

        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey)
            {
                //MessageBox.Show("duoc ne");
                isCtroldown = true;

            }
            if (e.KeyCode == Keys.S)
            {
                isSdown = true;
                if (isCtroldown)
                {
                    stop = true;
                    NoAcc = 0;

                    if (chbAutoCombo.Checked)
                    {
                        //if (e.KeyChar == tbAutoCombo.Text.First<Char>())
                        //{
                        if (AutoCombo_on)
                        {
                            AutoCombo_on = false;
                            thread_combo.Join();

                        }
                        else
                        {
                            AutoCombo_on = true;
                            //   AutoCombo_on = true;
                            thread_combo = new Thread(ts_combo);
                            thread_combo.IsBackground = true;
                            thread_combo.Start();
                        }
                        //  autocombo();
                        // }
                    }
                }
            }
            if (e.KeyCode == Keys.Q)
            {
                if (AutoQ_on)
                {
                    AutoQ_on = false;
                    thread_autoQ.Join();
                }
                AutoQ_on = true;
                //ts_combo = new ThreadStart(autocombo);

                //ts_autoQ = new ThreadStart(autobomQ);
                thread_autoQ = new Thread(ts_autoQ);
                thread_autoQ.IsBackground = true;
                thread_autoQ.Start();
                //thread_autoQ.Start();
                //autobomQ();

            }
            if (e.KeyCode == Keys.LShiftKey)
            {
                if (isCtroldown)
                {
                    checkBox3.Checked = !(checkBox3.Checked);
                }
            }
        }
        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey)
            {
                isCtroldown = false;

            }
            if (e.KeyCode == Keys.S)
            {
                isSdown = false;
            }
            if (e.KeyCode == Keys.S)
            {
                isQdown = false;
            }
        }
        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (checkBox2.Checked)
            {
                //if (e.KeyChar == 'q')
                //{
                //    HookManager.KeyPress -= HookManager_KeyPress;
                //    HookManager.KeyDown -= HookManager_KeyDown;
                //    for (int i = 1; i < numericUpDown1.Value; i++)
                //    {
                //        HuyBamPhim(KeyCode.KEY_Q, 50);
                //    }
                //    HookManager.KeyPress += HookManager_KeyPress;
                //    HookManager.KeyDown += HookManager_KeyDown;

                //}
            }

        }
        //private void hotkey_press(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 's')
        //    {
        //        stop = true ;
        //    }
        //    if (e.KeyChar == 'e')
        //    {
        //        checkBox3.Checked = true;
        //    }
        //}
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked)
            //{
            //    HookManager.KeyPress += HookManager_KeyPress;
            //}
            //else
            //{
            //    HookManager.KeyPress -= HookManager_KeyPress;
            //}
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(SpaceLienTuc);
            Thread thread = new Thread(ts);
            thread.IsBackground = true;
            thread.Start();
        }
        public void SpaceLienTuc()
        {
            while (checkBox3.Checked)
            {
                HuyBamPhim(KeyCode.SPACE_BAR, 50);
            }
        }
        public void autobomQ()
        {
            isQdown = true;
            //HookManager.KeyDown -= HookManager_KeyDown;
            //HookManager.KeyPress -= HookManager_KeyPress;
            for (int i = 1; i < numericUpDown1.Value; i++)
            {
                HuyBamPhim(KeyCode.KEY_W, 50);
                if (!AutoQ_on)
                {
                    break;
                }
            }
            //HookManager.KeyDown += HookManager_KeyDown;
            //HookManager.KeyPress += HookManager_KeyPress;

            //  AutoQ_on = false;
        }
        public void autocombo()
        {
            AutoControl.SendKeyDown(KeyCode.LCONTROL);
            AutoControl.mouse_event(0x08, 0u, 0u, 0u, UIntPtr.Zero);
            while (AutoCombo_on)
            {


                HuyBamPhim(KeyCode.KEY_1, 100);
                // Form1.HuyClick(EMouseKey.LEFT);
                HuyBamPhim(KeyCode.KEY_2, 100);
                //  Form1.HuyClick(EMouseKey.RIGHT);
                //  Form1.HuyClick(EMouseKey.LEFT);
                HuyBamPhim(KeyCode.KEY_3, 100);
                //  Form1.HuyClick(EMouseKey.RIGHT);
                //  Form1.HuyClick(EMouseKey.LEFT);
                System.Threading.Thread.Sleep(100);





                //AutoCombo_on = false;
                //thread_combo.Join();
            }
            AutoControl.mouse_event(0x10, 0u, 0u, 0u, UIntPtr.Zero);
            AutoControl.SendKeyUp(KeyCode.LCONTROL);

        }
        public void dangnhap()
        {
            Point? kq;
            Size new_sub1_size = new Size();
            Point new_sub1_point = new Point();
            Size new_sub2_size = new Size();
            Point new_sub2_point = new Point();
            Size new_sub3_size = new Size();
            Point new_sub3_point = new Point();
            Size new_sub4_size = new Size();
            Point new_sub4_point = new Point();
            if (chb_s1.Checked)
            {
                new_sub1_size = new Size(sub1_size.Width / 2, sub1_size.Height / 2);
                new_sub1_point = new Point(sub1_point.X + new_sub1_size.Width / 2, sub1_point.Y + new_sub1_size.Height / 2);
            }
            if (chb_s2.Checked)
            {
                new_sub2_size = new Size(sub2_size.Width / 2, sub2_size.Height / 2);
                new_sub2_point = new Point(sub2_point.X + new_sub2_size.Width / 2, sub2_point.Y + new_sub2_size.Height / 2);
            }
            if (chb_s3.Checked)
            {
                new_sub3_size = new Size(sub3_size.Width / 2, sub3_size.Height / 2);
                new_sub3_point = new Point(sub3_point.X + new_sub3_size.Width / 2, sub3_point.Y + new_sub3_size.Height / 2);
            }
            if (chb_s4.Checked)
            {
                new_sub4_size = new Size(sub4_size.Width / 2, sub4_size.Height / 2);
                new_sub4_point = new Point(sub4_point.X + new_sub4_size.Width / 2, sub4_point.Y + new_sub4_size.Height / 2);
            }

            do
            {
                Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
                System.Threading.Thread.Sleep(200);
                if (chb_s1.Checked)
                {
                    var new_sub1 = CaptureHelper.CaptureImage(new_sub1_size, new_sub1_point);
                    kq = ImageScanOpenCV.FindOutPoint((Bitmap)Sub1, (Bitmap)new_sub1, 0.8);
                    if (!(kq.HasValue))
                    {
                        sub_point = sub1_point;
                        break;
                    }
                }
                if (chb_s2.Checked)
                {
                    var new_sub2 = CaptureHelper.CaptureImage(new_sub2_size, new_sub2_point);
                    kq = ImageScanOpenCV.FindOutPoint((Bitmap)Sub2, (Bitmap)new_sub2, 0.8);
                    if (!(kq.HasValue))
                    {
                        sub_point = sub2_point;
                        break;
                    }
                }
                if (chb_s3.Checked)
                {
                    var new_sub3 = CaptureHelper.CaptureImage(new_sub3_size, new_sub3_point);
                    kq = ImageScanOpenCV.FindOutPoint((Bitmap)Sub3, (Bitmap)new_sub3, 0.8);
                    if (!(kq.HasValue))
                    {
                        sub_point = sub3_point;
                        break;
                    }
                }
                if (chb_s4.Checked)
                {
                    var new_sub4 = CaptureHelper.CaptureImage(new_sub4_size, new_sub4_point);
                    kq = ImageScanOpenCV.FindOutPoint((Bitmap)Sub4, (Bitmap)new_sub4, 0.8);
                    if (!(kq.HasValue))
                    {
                        sub_point = sub4_point;
                        break;
                    }
                }
                //var new_sub = CaptureHelper.CaptureImage(new_sub_size, new_sub_point);
                //kq = ImageScanOpenCV.FindOutPoint((Bitmap)Sub, (Bitmap)new_sub, 0.8);
                //   bitkq = ImageScanOpenCV.Find((Bitmap)Sub, (Bitmap)new_sub, 0.8);
                // stop = isCtroldown && isSdown;
                //if (stop)
                //{
                //    break;
                //}
            } while (!(stop));
            if (!stop)
            {
                Form1.HuyMouseClick(sub_point, EMouseKey.LEFT);
                if (checkBox1.Checked)
                {
                    System.Threading.Thread.Sleep(500);
                    HuyBamPhim(KeyCode.ENTER, 100);
                    System.Threading.Thread.Sleep(500);
                    HuyBamPhim(KeyCode.ENTER, 100);
                    System.Threading.Thread.Sleep(500);
                    HuySendText(textBox1.Text.Split(';')[NoAcc], 100);
                    HuyBamPhim(KeyCode.TAB, 100);
                    HuySendText(textBox2.Text.Split(';')[NoAcc], 100);
                    HuyBamPhim(KeyCode.ENTER, 100);
                    NoAcc += 1;
                    Thread.Sleep(2000);
                    HuyBamPhim(KeyCode.F12, 100);
                    if (NoAcc < (textBox1.Text.Split(';').Count<String>()))
                    {
                        Process.Start(duongdan);
                        //Thread.Sleep(1000);
                        //HuyBamPhim(KeyCode.LEFT, 100);
                        //HuyBamPhim(KeyCode.ENTER, 100);
                        Thread.Sleep(15000);
                        HuyMouseClick(nutDN, EMouseKey.LEFT);
                        Thread.Sleep(25000);
                        if (!(stop))
                        {
                            button1.PerformClick();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thành công " + NoAcc.ToString() + " Acc");
                    }
                }
            }
        }

        public void dis()
        {

            Point? kq;
            Size new_dis_size = new Size();
            Point new_dis_point = new Point();
            new_dis_size = new Size(dis_size.Width / 2, dis_size.Height / 2);
            new_dis_point = new Point(dis_point.X + new_dis_size.Width / 2, dis_point.Y + new_dis_size.Height / 2);

            do
            {
                Thread.Sleep(15000);
                var new_dis = CaptureHelper.CaptureImage(new_dis_size, new_dis_point);
                kq = ImageScanOpenCV.FindOutPoint((Bitmap)Dis, (Bitmap)new_dis, 0.8);
                if ((kq.HasValue))
                {
                    break;
                }
            } while (!(stop));
            if (!(stop))
            {
                stop = true;
                HuyBamPhim(KeyCode.ENTER, 100);
                Thread.Sleep(1000);
                Process.Start(duongdan);
                //  Thread.Sleep(1000);
                //HuyBamPhim(KeyCode.LEFT, 100);
                //HuyBamPhim(KeyCode.ENTER, 100);
                Thread.Sleep(15000);
                HuyMouseClick(nutDN, EMouseKey.LEFT);
                Thread.Sleep(25000);
                button1.PerformClick();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                sub2_size = new Size(frm2.selectWidth, frm2.selectHeight);
                Properties.Settings.Default.Sub2_size = sub2_size;
                sub2_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Sub2_point = sub2_point;
                Properties.Settings.Default.Save();
                Sub2 = CaptureHelper.CaptureImage(sub2_size, sub2_point);
                Sub2.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub2.PNG");
                pictureBox3.Image = Sub2;
            }
            pictureBox3.Refresh();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                sub3_size = new Size(frm2.selectWidth, frm2.selectHeight);
                Properties.Settings.Default.Sub3_size = sub3_size;
                sub3_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Sub3_point = sub3_point;
                Properties.Settings.Default.Save();
                Sub3 = CaptureHelper.CaptureImage(sub3_size, sub3_point);
                Sub3.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub3.PNG");
                pictureBox4.Image = Sub3;
            }
            pictureBox4.Refresh();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                sub4_size = new Size(frm2.selectWidth, frm2.selectHeight);
                Properties.Settings.Default.Sub4_size = sub4_size;
                sub4_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Sub4_point = sub_point;
                Properties.Settings.Default.Save();
                Sub4 = CaptureHelper.CaptureImage(sub4_size, sub4_point);
                Sub4.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Sub4.PNG");
                pictureBox5.Image = Sub4;
            }
            pictureBox5.Refresh();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPEN = new OpenFileDialog();
            if (DialogResult.OK == OPEN.ShowDialog())
            {
                duongdan = OPEN.FileName;
                Properties.Settings.Default.duongdan = duongdan;
                Properties.Settings.Default.Save();
                label3.Text = duongdan;
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {

                nutDN = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.NutDangNhap_point = nutDN;
                Properties.Settings.Default.Save();
                nutDN_image = CaptureHelper.CaptureImage(new Size(frm2.selectWidth, frm2.selectHeight), nutDN);
                nutDN_image.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\NutDN.PNG");
                pictureBox7.Image = nutDN_image;
            }
            pictureBox2.Refresh();
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                dis_size = new Size(frm2.selectWidth, frm2.selectHeight);
                Properties.Settings.Default.Dis_size = dis_size;
                dis_point = new Point(frm2.selectX, frm2.selectY);
                Properties.Settings.Default.Dis_point = dis_point;
                Properties.Settings.Default.Save();
                Dis = CaptureHelper.CaptureImage(dis_size, dis_point);
                Dis.Save(System.IO.Directory.GetCurrentDirectory() + @"\Image\Dis.PNG");
                pictureBox6.Image = Dis;
            }
            pictureBox6.Refresh();
            this.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void chbAutoCombo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCtrl.Checked)
            {
                AutoControl.SendKeyDown(KeyCode.LCONTROL);
            }
            else
            {
                AutoControl.SendKeyUp(KeyCode.LCONTROL);
            }
        }

        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        const Int32 WM_SETTEXT = 0x000c;
        const int VK_F4 = 0x73;
        const int VK_MENU = 0x12;
        const int VK_F5 = 0x74;
        const int VK_F1 = 0x70;
        const int VK_F3 = 0x72;
        const int VK_A = 0x41;
        const int VK_ESCAPE = 0x1B;
        //   IntPtr val = new IntPtr((Int32)'A');

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);


        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern string SendMessage(int hWnd, int msg, string wParam, IntPtr lParam);
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        public void dangnhapVLTK()
        {
            Process[] procs = Process.GetProcessesByName("vggame");

            IntPtr hWnd = procs.First().MainWindowHandle;
            AutoControl.BringToFront(hWnd);
            //  Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            this.Close();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = @"D:\GAME\[HanhSon_06.03]_FULL_MATKHAU_123456\game.exe";
                myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(myProcess.StartInfo.FileName);
                myProcess.Start();
            }
            catch (Exception eX)
            {
                Console.WriteLine(eX.Message);
            }

            // Form1.HuyMouseClick(sub1_point, EMouseKey.DOUBLE_LEFT);

            Thread.Sleep(4000);
            Process proc = Process.Start(@"D:\4. Lap Trinh\Project\Auto\WindowsFormsApp1\LoginVLTK_2\bin\Debug\LoginVLTK_2.exe");
            Thread.Sleep(1000);
            IntPtr hWnd = proc.MainWindowHandle;
            AutoControl.BringToFront(hWnd);
            AutoControl.HuySendText("camyve03");
            AutoControl.SendKeyPress(KeyCode.TAB);
            AutoControl.HuySendText("1233041990");
            AutoControl.SendKeyPress(KeyCode.TAB);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            //Form1.HuyMouseClick(sub1_point, EMouseKey.LEFT);

            //AutoControl.SendKeyPress(KeyCode.ENTER);
            //AutoControl.SendKeyPress(KeyCode.ENTER);

            //try
            //{
            //    // Start the process with the info we specified.
            //    // Call WaitForExit and then the using statement will close.
            //    using (Process exeProcess = Process.Start(startInfo))
            //    {
            //        exeProcess.WaitForExit();
            //    }
            //}
            //catch
            //{
            //    // Log error.
            //}


            //IntPtr hWnd = proc.MainWindowHandle;
            //AutoControl.BringToFront(hWnd);


            //Process[] procs = Process.GetProcessesByName("vggame");

            //foreach (Process proc in procs)
            //{
            //    IntPtr hWnd = proc.MainWindowHandle;
            //    AutoControl.BringToFront(hWnd);
            //    AutoControl.SendKeyPress(KeyCode.ENTER);
            //}

            // MessageBox.Show(proc.ProcessName);
            // get handle to Notepad's edit window 
            //  IntPtr hWnd = FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, "Sword3 Class", null);

            // IntPtr hWnd = FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, "Edit", null);

            //IntPtr hWnd = proc.MainWindowHandle;

            //AutoControl.BringToFront(hWnd);

            //for (int i = 0; i < 10; i++)
            //{
            //    // Key ScanCode = Microsoft.DirectX.DirectInput.Key.W;
            //    // SendKeyPress(ScanCode);
            //    AutoControl.SendKeyPress(KeyCode.KEY_H);
            //    Thread.Sleep(1000);
            //}

            //MessageBox.Show((0x000031D8 - 0x120B20E4).ToString());
            //MessageBox.Show((0x00004CDC - 0x1290F0E4).ToString());

            //IntPtr NPC_BASE_ADD = (IntPtr)0x00E172F0;
            //int NPC_HP_MAX_OFF = 0x00001060;
            //IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, proc.Id);

            //int bytesRead = 0;

            //byte[] buffer = new byte[24]; //To read a 24 byte unicode string

            //ReadProcessMemory(processHandle, NPC_BASE_ADD, buffer, buffer.Length, out bytesRead);

            //MessageBox.Show(Encoding.Unicode.GetString(buffer) +
            //      " (" + bytesRead.ToString() + "bytes)");





            //AutoControl.SendKeyUp(KeyCode.KEY_H);
            HuySendText("Dcl me may", 10);



            //AutoControl.SendKeyDown(KeyCode.LWIN);
            //AutoControl.SendKeyDown(KeyCode.KEY_R);

            //  AutoControl.SendKeyDown(KeyCode.KEY_H);
            //// post "o" to notepad
            ///
            //const int VK_O = 0x4F;

            // 

            // PostMessage(hWnd, WM_KEYDOWN, VK_ESCAPE, 0);

            //PostMessage(hWnd, WM_KEYDOWN, VK_F5, 0);maydcl me maydcl me maydcl me maydcl me maydcl me maydcl me maydcl me maydcl me d
            //  SendMessage(hWnd, WM_SETTEXT,0, "Hello World!33333");
            //}
            //Thread.Sleep(5000);
            //ShoutHello();
            // PostMessage(hWnd, WM_KEYDOWN, VK_A, 0);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //ThreadStart ts = new ThreadStart(dangnhapVLTK);
            //Thread thread = new Thread(ts);
            //thread.IsBackground = true;
            //thread.Start();

            Process[] procs = Process.GetProcessesByName("vggame");

            IntPtr hWnd = procs.First().MainWindowHandle;
            AutoControl.BringToFront(hWnd);
            //  Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            this.Close();
        }
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        //[DllImport("kernel32.dll")]
        //public static extern bool ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        //public byte[] ReadProcessMemory(int hProcess, IntPtr MemoryAddress, uint bytesToRead, out int bytesReaded)
        //{
        //    IntPtr ptr;
        //    byte[] buffer = new byte[bytesToRead];
        //    ReadProcessMemory(hProcess, MemoryAddress, buffer, bytesToRead, out ptr);
        //    bytesReaded = ptr.ToInt32();
        //    return buffer;
        //}
        public static void SendKeyPress(Key keyCode)
        {
            uint KEYEVENTF_SCANCODE = (uint)0x0008;
            uint KEYEVENTF_KEYUP = (uint)0x0002;
            INPUT iNPUT = new INPUT
            {
                Type = 1u
            };
            iNPUT.Data.Keyboard = new KEYBDINPUT
            {
                Vk = 0,
                //  Vk = (ushort)keyCode,
                //   Scan = 0,
                Scan = (ushort)keyCode,
                Flags = KEYEVENTF_SCANCODE,
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            INPUT iNPUT2 = new INPUT
            {
                Type = 1u
            };
            iNPUT2.Data.Keyboard = new KEYBDINPUT
            {
                // Vk = (ushort)keyCode,
                Vk = 0,
                //  Scan = 0,
                Scan = (ushort)keyCode,
                Flags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP,
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            INPUT[] inputs = new INPUT[]
            {
                iNPUT,
                iNPUT2
            };
            if (SendInput(2u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        //public void ShoutHello()
        //{
        //    InputSimulator inputSimulator = new InputSimulator();
        //    //  inputSimulator.Keyboard.KeyDown.SimulateKeyPress(VirtualKeyCode.SPACE);

        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_H);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_E);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_L);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_L);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_O);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_1);
        //    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
        //}
        //public static void SendInput(ushort charUnicode)
        //{
        //    // In 32 bit the IntPtr should be 4; it's 8 in 64 bit.
        //    if (Marshal.SizeOf(new IntPtr()) == 8)
        //    {
        //        SendInputExternalCalls.SendInput( SendInput64(charUnicode);
        //    }
        //    else
        //    {
        //        SendInput32(charUnicode);
        //    }
        //}
        internal static class SendInputExternalCalls
        {
            // This SendInput call uses the 32bit input structure.
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "SendInput")]
            public static extern UInt32 SendInput(
                UInt32 numInputs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        SEND_INPUT_FOR_32_BIT[] sendInputsFor,
                Int32 cbSize);

            // This SendInput call uses the 64bit input structure.
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "SendInput")]
            public static extern UInt32 SendInput(
                UInt32 numInputs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        SEND_INPUT_FOR_64_BIT[] sendInputsFor,
                Int32 cbSize);
        }

        // This is the basic structure for 32 bit input.  SendInput allows for other input   
        // types, but I was only concerned with keyboard input, so I harcoded my strucs that way.
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        internal struct SEND_INPUT_FOR_32_BIT
        {
            [FieldOffset(0)]
            public uint InputType;
            [FieldOffset(4)]
            public KEYBOARD_INPUT_FOR_32_BIT KeyboardInputStruct;
        }

        // Here is the structure for keyboard input.  The key code, scan code, and flags
        // are what's important.  The other variables are place holders so that the structure
        // maintains the correct size when compared to the other possible input structure types.  
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct KEYBOARD_INPUT_FOR_32_BIT
        {
            public ushort VirtualKeyCode;
            public ushort ScanCode;
            public uint Flags;
            public uint Time;
            public uint ExtraInfo;
            public uint Padding1;
            public uint Padding2;
        }

        // Here's the corresponding 64 bit structure.  Notice that the field offset are larger. 
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        internal struct SEND_INPUT_FOR_64_BIT
        {
            [FieldOffset(0)]
            public uint InputType;
            [FieldOffset(8)]
            public KEYBOARD_INPUT_FOR_64_BIT KeyboardInputStruct;
        }

        // Here's the keyboard 64 bit structure.  Notice that the field offset are again larger.
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        internal struct KEYBOARD_INPUT_FOR_64_BIT
        {
            [FieldOffset(0)]
            public ushort VirtualKeyCode;
            [FieldOffset(2)]
            public ushort ScanCode;
            [FieldOffset(4)]
            public uint Flags;
            [FieldOffset(12)]
            public uint Time;
            [FieldOffset(20)]
            public uint Padding1;
            [FieldOffset(28)]
            public uint Padding2;
        }


    }
}
