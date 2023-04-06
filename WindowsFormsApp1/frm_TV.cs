using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Gma.UserActivityMonitor;
using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        bool dht_tocbien_doigoc = false;
        Point targetX2;
        Point targetDHT;
        private void ClipCursorOffset(Point current, int offset)
        {
            RECT rect = new RECT(current.X - offset, current.Y - offset, current.X + offset, current.Y + offset);
            ClipCursor(ref rect);
        }
        private void frm_TV_Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.MouseDown += HookManager_MouseDown;
            //HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
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
            continuedSpame = false;
            HookManager.MouseDown += HookManager_MouseDown;
        }
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (chbSmoothLHT.Checked && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.D2))
            {
                HookManager.KeyDown -= HookManager_KeyDown;
                Point current = GetCursorPoint();
                if (e.KeyCode == Keys.D2)
                {
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
                            targetTT = toadoX2[2];
                        }
                        else
                        {
                            targetTT = toadoX2[6];
                        }
                    }
                    targetTT = ModifyPoint(targetTT, int.Parse(tbLengthx2.Text));
                    if (continuedSpame == true)
                    {
                        targetX2 = targetTT;
                        targetDHT = current;
                        dht = true;
                    }
                    else
                    {
                        HuyKeyPress(KeyDirectX.F2);
                        Thread.Sleep(100);
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
                    if (continuedSpame == true)
                    {
                        targetX2 = new Point(0, 0);
                        targetDHT = current;
                        dht = true;
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
            if (chbDoiGoc.Checked && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                if (!continuedSpame)
                {
                    targetX2 = new Point(0, 0);
                    continuedSpame = true;
                    dht_tocbien_doigoc = true;
                    new Thread(() =>
                    {
                        spam9x();
                    }).Start();
                }
                Point enemy = TryFindEnemy(chbOnHouse.Checked, 300, 300);
                int hor_x = 20;
                int hor_y = 1;
                int vert_x = 1;
                int vert_y = 20;
                if (chbOnHouse.Checked)
                {
                    hor_x = 25;
                    hor_y = 0;
                    //vert_x = 1;
                    vert_y = 10;
                }
                if (enemy.X != 0 && enemy.Y != 0)
                {
                    HookManager.KeyDown -= HookManager_KeyDown;
                    if (e.KeyCode == Keys.Left)
                    {
                        if (chbOnHouse.Checked)
                        {
                            targetX2 = ModifyPoint(toadoX2[5], int.Parse(tbLengthx2.Text));
                        }
                        else
                        {
                            targetX2 = ModifyPoint(toadoX2[6], int.Parse(tbLengthx2.Text));
                        }
                        targetDHT = new Point(enemy.X - hor_x, enemy.Y + hor_y);
                        dht = true;
                    }
                    if (e.KeyCode == Keys.Right)
                    {
                        if (chbOnHouse.Checked)
                        {
                            targetX2 = ModifyPoint(toadoX2[1], int.Parse(tbLengthx2.Text));
                        }
                        else
                        {
                            targetX2 = ModifyPoint(toadoX2[2], int.Parse(tbLengthx2.Text));
                        }
                        targetDHT = new Point(enemy.X + hor_x, enemy.Y + hor_y);
                        dht = true;
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (myTVB.X > enemy.X)
                        {
                            targetX2 = ModifyPoint(toadoX2[0], int.Parse(tbLengthx2.Text));
                            targetDHT = new Point(enemy.X + vert_x, enemy.Y + vert_y);
                            dht = true;
                        }
                        else
                        {
                            targetX2 = ModifyPoint(toadoX2[4], int.Parse(tbLengthx2.Text));
                            targetDHT = new Point(enemy.X - vert_x, enemy.Y + vert_y);
                            dht = true;
                        }
                    }
                    if (e.KeyCode == Keys.Up)
                    {
                        if (myTVB.X > enemy.X)
                        {
                            if (chbOnHouse.Checked)
                            {
                                targetX2 = ModifyPoint(toadoX2[1], int.Parse(tbLengthx2.Text));
                            }
                            else
                            {
                                targetX2 = ModifyPoint(toadoX2[2], int.Parse(tbLengthx2.Text));
                            }
                            targetDHT = new Point(enemy.X + hor_x, enemy.Y + hor_y);
                            dht = true;
                        }
                        else
                        {
                            if (chbOnHouse.Checked)
                            {
                                targetX2 = ModifyPoint(toadoX2[5], int.Parse(tbLengthx2.Text));
                            }
                            else
                            {
                                targetX2 = ModifyPoint(toadoX2[6], int.Parse(tbLengthx2.Text));
                            }
                            targetDHT = new Point(enemy.X - hor_x, enemy.Y + hor_y);
                            dht = true;
                        }
                    }
                    //test
                    //SetCursorPos(targetDHT.X, targetDHT.Y);
                    //Thread.Sleep(100);
                    //HuyKeyPress(KeyDirectX.F2);
                    //Thread.Sleep(200);
                    //SetCursorPos(targetX2.X, targetX2.Y);
                    //HuyKeyPress(KeyDirectX.F3);
                    HookManager.KeyDown += HookManager_KeyDown;
                }
            }
            if (chbLHT.Checked && e.KeyCode == Keys.Space)
            {
                HookManager.KeyDown -= HookManager_KeyDown;
                Point current = GetCursorPoint();
                int apSat = 50;
                targetDHT = ModifyPoint(current, apSat);
                new Thread(() =>
                {
                    GetX2Point(false);
                    dht = true;
                }).Start();
                HookManager.KeyDown += HookManager_KeyDown;
            }
            if (chbTocBien.Checked && e.KeyCode == Keys.F)
            {
                targetX2 = new Point(0, 0);
                continuedSpame = false;
                HookManager.KeyDown -= HookManager_KeyDown;
                HuyKeyPress(KeyDirectX.F2);
                HuyKeyPress(KeyDirectX.V);

                continuedSpame = true;
                Thread.Sleep(100);
                new Thread(() =>
                {
                    GetX2Point(false);
                }).Start();
                new Thread(() =>
                {
                    spam9x();
                }).Start();
                for (int i = 0; i < 4; i++)
                {
                    HuyKeyPress(KeyDirectX.V);
                    Thread.Sleep(50);
                    HuyKeyPress(KeyDirectX.V);
                    Thread.Sleep(50);
                    HuyKeyPress(KeyDirectX.F3);
                    Thread.Sleep(50);
                }
                SetCursorPos(targetX2.X, targetX2.Y);
                HookManager.KeyDown += HookManager_KeyDown;
            }
        }

        private Point TryFindEnemy(bool trenNgua, int offset_X, int offset_Y)
        {
            Point current = GetCursorPoint();
            int offsetHorse = 0;
            if (trenNgua)
            {
                offsetHorse = 40;
            }
            int offsetTopBonus = 50;
            Point offset_point = new Point(current.X - offset_X, current.Y - offset_Y - offsetTopBonus);
            Bitmap manHinh = CaptureHelper.CaptureImage(new Size(2 * offset_X, 2 * offset_Y + offsetTopBonus), offset_point);
            //manHinh.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\cusor_offset.PNG"); //save image
            Point? finded = FindOutPoint(manHinh);
            Point result = new Point(0, 0);
            if (finded.HasValue)
            {
                result = new Point(offset_point.X + finded.Value.X + 20, offset_point.Y + finded.Value.Y + 75 + offsetHorse);
            }
            return result;
        }
        public static Point? FindOutPoint(Bitmap mainBitmap)
        {
            Image<Bgr, byte> arg_17_0 = new Image<Bgr, byte>(mainBitmap);
            //Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            Point? result = null;
            using (Image<Gray, byte> image = arg_17_0.InRange(new Bgr(0, 190, 229), new Bgr(0, 190, 231)))
            {
                //image.Save(@"D:\4. Lap Trinh\Project\Auto\WindowsFormsApp1\WindowsFormsApp1\bin\Debug\Dota\Img\Result.png");
                double[] array;
                double[] array2;
                Point[] array3;
                Point[] array4;

                image.MinMax(out array, out array2, out array3, out array4);
                if (array2[0] == 255)
                {
                    result = new Point?(array4[0]);
                }
            }
            return result;
        }
        private void spam9x()
        {
            do
            {
                if (dht)
                {
                    Point current = GetCursorPoint();
                    SetCursorPos(targetDHT.X, targetDHT.Y);
                    Thread.Sleep(100);
                    //Bam DHT
                    int lanthu = 0;
                    Bitmap bmp2 = CaptureHelper.CaptureImage(new Size(10, 10), new Point(110, 580));
                    chaylai:
                    lanthu++;
                    if (lanthu > 10)
                    {
                        SetCursorPos(current.X, current.Y);
                        goto boqua;
                    }
                    if (chbVwhenchange.Checked || dht_tocbien_doigoc)
                    {
                        HuyKeyPress(KeyDirectX.V);
                    }
                    HuyKeyPress(KeyDirectX.F2);
                    Thread.Sleep(50);
                    Bitmap bmp1 = CaptureHelper.CaptureImage(new Size(10, 10), new Point(110, 580));
                    if (CompareBitmapsFast(bmp1, bmp2)) { goto chaylai; }
                    if (targetX2.X != 0 && targetX2.Y != 0)
                    {
                        SetCursorPos(targetX2.X, targetX2.Y);
                    }
                    if (chbVwhenchange.Checked || dht_tocbien_doigoc)
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            HuyKeyPress(KeyDirectX.V);
                            Thread.Sleep(50);
                            HuyKeyPress(KeyDirectX.V);
                            Thread.Sleep(50);
                            HuyKeyPress(KeyDirectX.F3);
                            Thread.Sleep(50);
                        }
                    }
                    boqua:
                    dht = false;
                    dht_tocbien_doigoc = false;
                }
                HuyKeyPress(KeyDirectX.F3);
                Thread.Sleep(50);
            }
            while (continuedSpame);
        }
        private bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
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
