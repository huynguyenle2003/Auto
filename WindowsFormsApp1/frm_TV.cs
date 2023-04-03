using Gma.UserActivityMonitor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frm_TV : Form
    {
        public frm_TV()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetCursorInfo(out CURSORINFO pci);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ClipCursor(ref RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetClipCursor(out RECT lpRect);
        List<Point> toadoX2;
        Point myTVB;
        bool continuedSpame = false;
        bool dht = false;
        Point targetX2;
        Point targetDHT;
        RECT current_rect;
        private void ClipCursorOffset(Point current, int offset)
        {
            RECT rect = new RECT(current.X - offset, current.Y - offset, current.X + offset, current.Y + offset);
            ClipCursor(ref rect);
        }
        private void frm_TV_Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.MouseDown += HookManager_MouseDown;


            GetClipCursor(out current_rect);
            toadoX2 = new List<Point>();

            toadoX2.Add(new Point(145, 62));    //1
            //toadoX2.Add(new Point(48, 122));  //2
            toadoX2.Add(new Point(48, 215));    //3
            //toadoX2.Add(new Point(52, 372));  //4
            toadoX2.Add(new Point(33, 463));    //5
            toadoX2.Add(new Point(180, 558));   //6

            toadoX2.Add(new Point(630, 62));    //7
            // toadoX2.Add(new Point(719, 152)); //8
            toadoX2.Add(new Point(693, 243));   //9
            //toadoX2.Add(new Point(190, 370)); //10
            toadoX2.Add(new Point(681, 406));   //11
            toadoX2.Add(new Point(616, 556));   //12
            myTVB = new Point(402, 313);
        }
        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            HookManager.MouseDown -= HookManager_MouseDown;
            //if (e.Button == MouseButtons.Right)
            //{
            //    continuedSpame = false;
            //    Point current = GetCursorPoint();
            //    int apSat = 50;
            //    Point toado = ModifyPoint(current, apSat);
            //    SetCursorPos(toado.X, toado.Y);
            //    Thread.Sleep(200);
            //    HuyKeyPress(KeyDirectX.F2);
            //    //Point toadox2 = FindToaDoX2(toado);
            //    //SetCursorPos(toadox2.X, toadox2.Y);

            //    continuedSpame = true;
            //    new Thread(() =>
            //    {
            //        spam9x();
            //    }).Start();
            //}
            //else
            {
                continuedSpame = false;
            }
            HookManager.MouseDown += HookManager_MouseDown;
        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (chbSmoothLHT.Checked && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.D2))
            {
                //int timeLength = int.Parse(tbSpamLHT.Text);
                //Point current = GetCursorPoint();

                //HookManager.KeyDown -= HookManager_KeyDown;
                //HuyKeyPress(KeyDirectX.D3);
                //DateTime now = DateTime.Now;
                //Thread.Sleep(100);
                //if (chbGocX2.Checked)
                //{
                //    if (e.KeyCode == Keys.D2)
                //    {
                //        // Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                //        Point toado = FindToaDoX2(current, true);
                //        SetCursorPos(toado.X, toado.Y);
                //    }
                //    else
                //    {
                //        Point toado = FindToaDoX2(current);
                //        SetCursorPos(toado.X, toado.Y);
                //    }
                //}
                //do
                //{
                //    Thread.Sleep(10);
                //    HuyKeyPress(KeyDirectX.D4);
                //}
                //while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                //HookManager.KeyDown += HookManager_KeyDown;

                HookManager.KeyDown -= HookManager_KeyDown;
                if (continuedSpame == true || e.KeyCode == Keys.D2)
                {
                    //targetDHT = GetCursorPoint();
                    //targetX2 = new Point(0, 0);
                    //dht = true;
                    Point current = GetCursorPoint();

                    int diff_y = current.Y - myTVB.Y;
                    int diff_x = current.X - myTVB.X;
                    Point targetTT;
                    if (diff_y > 20)
                    {
                        if (diff_x > 0)
                        {
                            targetTT = toadoX2[4];
                        }
                        else
                        {
                            targetTT = toadoX2[0];
                        }
                    }
                    else
                    {
                        if (diff_x > 0)
                        {
                            targetTT = toadoX2[1];
                        }
                        else
                        {
                            targetTT = toadoX2[5];
                        }
                    }
                    targetTT = ModifyPoint(targetTT, 100);
                    if (continuedSpame == true)
                    {
                        targetX2 = targetTT;
                        targetDHT = current;
                        dht = true;
                    }
                    else
                    {
                        HuyKeyPress(KeyDirectX.F2);
                        Thread.Sleep(200);
                        SetCursorPos(targetTT.X, targetTT.Y);
                        continuedSpame = true;
                        new Thread(() =>
                        {
                            spam9x();
                        }).Start();
                    }
                }
                else
                {
                    HuyKeyPress(KeyDirectX.F2);
                    Thread.Sleep(100);

                    new Thread(() =>
                    {
                        GetX2Point();
                    }).Start();
                    continuedSpame = true;
                    new Thread(() =>
                    {
                        spam9x();
                    }).Start();
                }
                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbSmoothLHT.Checked && e.KeyCode == Keys.D4)
            {
                HookManager.KeyDown -= HookManager_KeyDown;
                Point current = GetCursorPoint();
                Point toado = FindToaDoX2(current);
                SetCursorPos(toado.X, toado.Y);
                //this.Text += "__" + toado.X + "-" + toado.Y;
                HookManager.KeyDown += HookManager_KeyDown;

            }
            if (false && e.KeyCode == Keys.D) //ko xai
            {
                //int timeLength = int.Parse(tbSpamLHTBack.Text);
                //Point current = GetCursorPoint();

                //HookManager.KeyDown -= HookManager_KeyDown;
                //HuyKeyPress(KeyDirectX.D3);
                //DateTime now = DateTime.Now;
                //Thread.Sleep(200);
                ////HuyKeyPress(KeyDirectX.M);
                ////{
                ////    Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                ////    SetCursorPos(toado.X, toado.Y);
                ////}
                //do
                //{
                //    Thread.Sleep(10);
                //    HuyKeyPress(KeyDirectX.D4);
                //}
                //while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                //HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbLHT.Checked && e.KeyCode == Keys.Space)
            {
                //int timeLength = int.Parse(tbSpamLHT.Text);
                //Point current = GetCursorPoint();

                //HookManager.KeyDown -= HookManager_KeyDown;
                //Point toado = toadoX2[0];

                //if (current.X < myTVB.X)
                //{
                //    toado = toadoX2[4];
                //}
                //toado = ModifyPoint(toado, int.Parse(tbLengthx2.Text));
                //SetCursorPos(toado.X, toado.Y);
                ////Point spamLHT = ModifyPoint(current, 1);
                ////SetCursorPos(spamLHT.X, spamLHT.Y);
                ////Thread.Sleep(50);
                ////HuyKeyPress(KeyDirectX.D3);
                ////SetCursorPos(current.X, current.Y);
                //DateTime now = DateTime.Now;
                ////Thread.Sleep(100);
                //////{
                ////Point toado = FindToaDoX2(current);
                ////SetCursorPos(toado.X, toado.Y);
                //////}
                //do
                //{
                //    Thread.Sleep(10);
                //    HuyKeyPress(KeyDirectX.D4);
                //}
                //while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                //HookManager.KeyDown += HookManager_KeyDown;
                HookManager.KeyDown -= HookManager_KeyDown;

                Point current = GetCursorPoint();
                int apSat = 50;
                targetDHT = ModifyPoint(current, apSat);
                new Thread(() =>
                {
                    GetX2Point(false);
                    dht = true;
                }).Start();

                //GetX2Point(false);
                //MessageBox.Show("outsite:" + targetX2.X.ToString() + "-" + targetX2.Y);
                //dht = true;
                //continuedSpame = true;
                //new Thread(() =>
                //{
                //    spam9x();
                //}).Start();

                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbTocBien.Checked && e.KeyCode == Keys.F)
            {
                //int timeLength = int.Parse(tbSpamLHT.Text);

                //HookManager.KeyDown -= HookManager_KeyDown;
                //HuyKeyPress(KeyDirectX.V);
                //Thread.Sleep(100);
                //HuyKeyPress(KeyDirectX.R);
                //Thread.Sleep(100);
                //HuyKeyPress(KeyDirectX.D3);
                //Thread.Sleep(100);
                //HuyKeyPress(KeyDirectX.R);

                //DateTime now = DateTime.Now;


                ////{
                ////    Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                ////    SetCursorPos(toado.X, toado.Y);
                ////}

                //do
                //{
                //    Thread.Sleep(10);
                //    HuyKeyPress(KeyDirectX.D4);
                //}
                //while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                //HookManager.KeyDown += HookManager_KeyDown;

                continuedSpame = false;
                HookManager.KeyDown -= HookManager_KeyDown;
                HuyKeyPress(KeyDirectX.V);
                HuyKeyPress(KeyDirectX.F2);
                continuedSpame = true;
                Thread.Sleep(200);
                new Thread(() =>
                {
                    GetX2Point();
                }).Start();
                new Thread(() =>
                {
                    spam9x();
                }).Start();
                HookManager.KeyDown += HookManager_KeyDown;
                Thread.Sleep(200);
                HuyKeyPress(KeyDirectX.V);
                Thread.Sleep(200);
                HuyKeyPress(KeyDirectX.V);
                //Thread.Sleep(200);
                //HuyKeyPress(KeyDirectX.V);
                //Thread.Sleep(200);
                //HuyKeyPress(KeyDirectX.V);
            }

        }
        private void spam9x()
        {
            do
            {
                if (dht)
                {
                    // MessageBox.Show("DHt:"+targetDHT.X.ToString() +"-"+ targetDHT.Y);
                    SetCursorPos(targetDHT.X, targetDHT.Y);
                    //int offset = 5;
                    //ClipCursorOffset(targetDHT, offset);
                    Thread.Sleep(100);
                    HuyKeyPress(KeyDirectX.F2);
                    Thread.Sleep(100);
                    HuyKeyPress(KeyDirectX.F2);
                    Thread.Sleep(100);
                    HuyKeyPress(KeyDirectX.F2);
                    //Thread.Sleep(100);
                    //HuyKeyPress(KeyDirectX.F2);
                    //Thread.Sleep(100);
                    if (targetX2.X != 0 && targetX2.Y != 0)
                    {
                        SetCursorPos(targetX2.X, targetX2.Y);
                    }
                    //ClipCursor(ref current_rect);
                    dht = false;
                }
                HuyKeyPress(KeyDirectX.F3);
                Thread.Sleep(50);
            }
            while (continuedSpame);
        }
        private Point FindToaDoX2(Point current, bool nguoc = false)
        {
            Point select = toadoX2[0];
            double tichVoHuong = TinhToanTichVoHuong(current, select);
            for (int i = 1; i < toadoX2.Count; i++)
            {
                Point candidate = toadoX2[i];
                double tichVoHuong_new = TinhToanTichVoHuong(current, candidate);
                if (nguoc && tichVoHuong_new < tichVoHuong)
                {
                    select = candidate;
                    tichVoHuong = tichVoHuong_new;
                }
                if (!nguoc && tichVoHuong_new > tichVoHuong)
                {
                    select = candidate;
                    tichVoHuong = tichVoHuong_new;
                }
            }
            //this.Text = select.X + "-" + select.Y;
            if (tbLengthx2.Text == "0")
            {
                return select;
            }
            else
            {
                return ModifyPoint(select, int.Parse(tbLengthx2.Text));
            }
            //if (!nguoc)
            //{


            //}
            //else
            //{
            //    //if (tbBack.Text == "0")
            //    //{
            //    //    return select;
            //    //}
            //    //else
            //    //{
            //    //    return ModifyPoint(select, int.Parse(tbBack.Text));
            //    //}
            //}
        }
        private Point ModifyPoint(Point toado, int length)
        {
            double vt_x = toado.X - myTVB.X;
            double vt_y = toado.Y - myTVB.Y;
            double vt_length = Math.Sqrt(vt_x * vt_x + vt_y * vt_y);
            vt_x = length * vt_x / vt_length;
            vt_y = length * vt_y / vt_length;
            return new Point(myTVB.X + (int)vt_x, myTVB.Y + (int)vt_y);
        }
        private void GetX2Point(bool set = true)
        {
            //Thread.CurrentThread.IsBackground = true;
            Thread.Sleep(100);
            Point current = GetCursorPoint();
            do
            {
                Thread.Sleep(50);
                Point next = GetCursorPoint();
                if (next.X == current.X && next.Y == current.Y)
                {
                    break;
                }
                current = next;
            }
            while (true);
            if (chbGocX2.Checked)
            {
                Point toado = FindToaDoX2(current);
                targetX2 = toado;
                if (set)
                {
                    SetCursorPos(toado.X, toado.Y);
                }
            }
        }
        private double TinhToanTichVoHuong(Point current, Point toado)
        {
            double vt_start_x = current.X - myTVB.X;
            double vt_start_y = current.Y - myTVB.Y;
            double vt_start_length = Math.Sqrt(vt_start_x * vt_start_x + vt_start_y * vt_start_y);
            vt_start_x = vt_start_x / vt_start_length;
            vt_start_y = vt_start_y / vt_start_length;

            double vt_end_x = toado.X - myTVB.X;
            double vt_end_y = toado.Y - myTVB.Y;
            double vt_end_length = Math.Sqrt(vt_end_x * vt_end_x + vt_end_y * vt_end_y);
            vt_end_x = vt_end_x / vt_end_length;
            vt_end_y = vt_end_y / vt_end_length;

            double result = vt_start_x * vt_end_x + vt_start_y * vt_end_y;
            return result;
        }
        public Point GetCursorPoint()
        {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            return new Point(pci.ptScreenPos.x, pci.ptScreenPos.y);
        }
        public static void HuyKeyPress(KeyDirectX keyCode, int milisecondsLoop = 0)
        {
            DateTime now = DateTime.Now;
        loop:
            INPUT iNPUT = new INPUT
            {
                Type = 1u
            };
            iNPUT.Data.Keyboard = new KEYBDINPUT
            {
                Vk = 0,
                Scan = (ushort)keyCode,
                Flags = 8u,
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            INPUT iNPUT2 = new INPUT
            {
                Type = 1u
            };
            iNPUT2.Data.Keyboard = new KEYBDINPUT
            {
                Vk = 0,
                Scan = (ushort)keyCode,
                Flags = 8u | 2u,
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
            if ((DateTime.Now - now).TotalMilliseconds > milisecondsLoop) return;
            goto loop;
        }
    }
}
