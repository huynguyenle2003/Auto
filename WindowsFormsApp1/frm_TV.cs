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
        List<Point> toadoX2;
        Point myTVB;
        private void frm_TV_Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManager_KeyDown;
            toadoX2 = new List<Point>();

            toadoX2.Add(new Point(145, 62));
            // toadoX2.Add(new Point(48, 122));
            toadoX2.Add(new Point(48, 215));
            toadoX2.Add(new Point(52, 372));
            // toadoX2.Add(new Point(33, 463));
            toadoX2.Add(new Point(180, 558));

            toadoX2.Add(new Point(630, 62)); // 642,62
            // toadoX2.Add(new Point(719, 152));
            toadoX2.Add(new Point(693, 243));
            toadoX2.Add(new Point(190, 370));
            // toadoX2.Add(new Point(681, 406));
            toadoX2.Add(new Point(616, 556));
            myTVB = new Point(402, 313);
        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (chbSmoothLHT.Checked && e.KeyCode == Keys.D5)
            {
                int timeLength = int.Parse(tbSpamLHT.Text);
                Point current = GetCursorPoint();

                HookManager.KeyDown -= HookManager_KeyDown;
                HuyKeyPress(KeyDirectX.D3);
                DateTime now = DateTime.Now;
                Thread.Sleep(200);
                if (chbGocX2.Checked)
                {
                    Point toado = FindToaDoX2(current);
                    SetCursorPos(toado.X, toado.Y);
                }
                do
                {
                    Thread.Sleep(10);
                    HuyKeyPress(KeyDirectX.D4);
                }
                while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbSmoothLHT.Checked && e.KeyCode == Keys.D4)
            {
                HookManager.KeyDown -= HookManager_KeyDown;
                Point current = GetCursorPoint();
                this.Text = current.X + "-" + current.Y;
                HookManager.KeyDown += HookManager_KeyDown;

            }
            if (chbBackLHT.Checked && e.KeyCode == Keys.D3)
            {
                int timeLength = int.Parse(tbSpamLHTBack.Text);
                Point current = GetCursorPoint();

                HookManager.KeyDown -= HookManager_KeyDown;
                HuyKeyPress(KeyDirectX.D3);
                DateTime now = DateTime.Now;
                Thread.Sleep(200);
                //HuyKeyPress(KeyDirectX.M);
                //{
                //    Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                //    SetCursorPos(toado.X, toado.Y);
                //}
                do
                {
                    Thread.Sleep(10);
                    HuyKeyPress(KeyDirectX.D4);
                }
                while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbLHT.Checked && e.KeyCode == Keys.Space)
            {
                int timeLength = int.Parse(tbSpamLHTBack.Text);
                Point current = GetCursorPoint();

                HookManager.KeyDown -= HookManager_KeyDown;
                Point spamLHT = ModifyPoint(current, 30);
                SetCursorPos(spamLHT.X, spamLHT.Y);
                Thread.Sleep(100);
                HuyKeyPress(KeyDirectX.D3);
                SetCursorPos(current.X, current.Y);
                DateTime now = DateTime.Now;
                Thread.Sleep(200);
                //{
                //    Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                //    SetCursorPos(toado.X, toado.Y);
                //}
                do
                {
                    Thread.Sleep(10);
                    HuyKeyPress(KeyDirectX.D4);
                }
                while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbTocBien.Checked && e.KeyCode == Keys.F)
            {
                int timeLength = int.Parse(tbSpamLHTBack.Text);

                HookManager.KeyDown -= HookManager_KeyDown;
                HuyKeyPress(KeyDirectX.V);
                Thread.Sleep(100);
                HuyKeyPress(KeyDirectX.D3);

                DateTime now = DateTime.Now;
                Thread.Sleep(200);
                //{
                //    Point toado = new Point(2 * myTVB.X - current.X, 2 * myTVB.Y - current.Y);
                //    SetCursorPos(toado.X, toado.Y);
                //}
                do
                {
                    Thread.Sleep(10);
                    HuyKeyPress(KeyDirectX.D4);
                }
                while ((DateTime.Now - now).TotalMilliseconds < timeLength);
                HookManager.KeyDown += HookManager_KeyDown;
            }
        }
        private Point FindToaDoX2(Point current)
        {
            Point select = toadoX2[0];
            double tichVoHuong = TinhToanTichVoHuong(current, select);
            for (int i = 1; i < toadoX2.Count; i++)
            {
                Point candidate = toadoX2[i];
                double tichVoHuong_new = TinhToanTichVoHuong(current, candidate);
                if (tichVoHuong_new > tichVoHuong)
                {
                    select = candidate;
                    tichVoHuong = tichVoHuong_new;
                }
            }
            if (tbLengthx2.Text == "0")
            {
                return select;
            }
            return ModifyPoint(select, int.Parse(tbLengthx2.Text));
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
