using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginVLTK_2
{
    public partial class frm_nhiemvu : Form
    {
        // private int counter = 60;
        public frm_nhiemvu()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        private void btLogin_Click(object sender, EventArgs e)
        {
            int value = 2;
            backgroundWorker1.RunWorkerAsync(argument: value);
        }
        private void Login()
        {
            Process[] procs = Process.GetProcessesByName("vggame");

            IntPtr hWnd = IntPtr.Zero;
            DateTime last = DateTime.MinValue;
            foreach (Process item in procs)
            {
                if (item.StartTime > last)
                {
                    hWnd = item.MainWindowHandle;
                    last = item.StartTime;
                }
            }

            //= procs.First().MainWindowHandle;
            AutoControl.BringToFront(hWnd);
            Thread.Sleep(1000);
            //  Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
            AutoControl.MouseClick(405, 405, EMouseKey.LEFT);
            //AutoControl.SendKeyPress(KeyCode.ENTER);
            // AutoControl.MouseClick(400, 400, EMouseKey.LEFT);
            Thread.Sleep(500);
            AutoControl.SendKeyPress(KeyCode.ENTER);

            Thread.Sleep(500);
            AutoControl.MouseClick(200, 225, EMouseKey.LEFT);
            Thread.Sleep(500);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            Thread.Sleep(500);
            AutoControl.HuySendText(tbUser.Text);
            Thread.Sleep(500);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            Thread.Sleep(500);
            AutoControl.HuySendText(tbPassword.Text);
            Thread.Sleep(500);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            Thread.Sleep(5000);

            AutoControl.SendKeyPress(KeyCode.ENTER);
            Thread.Sleep(5000);
            AutoControl.SendKeyDown(KeyCode.ALT);
            AutoControl.SendKeyDown(KeyCode.KEY_D);
            AutoControl.SendKeyUp(KeyCode.KEY_D);
            AutoControl.SendKeyUp(KeyCode.ALT);
            AutoControl.SendKeyDown(KeyCode.ALT);
            AutoControl.SendKeyDown(KeyCode.KEY_F);
            AutoControl.SendKeyUp(KeyCode.KEY_F);
            AutoControl.SendKeyUp(KeyCode.ALT);

            hWnd = AutoControl.FindWindowHandle(null, "KY Train 2.03");
            if (hWnd != IntPtr.Zero)
            {
                try
                {
                    //Click kim yen
                    hWnd = AutoControl.FindWindowHandle(null, "KY Train 2.03");
                    //  MessageBox.Show(hWnd.ToString("X8"));
                    IntPtr child_hWnd = AutoControl.FindHandle(hWnd, "", "A");
                    // MessageBox.Show(child_hWnd.ToString("X8"));
                    AutoControl.SendClickOnControlByHandle(child_hWnd);
                    // Thread.Sleep(2000);
                    hWnd = AutoControl.FindWindowHandle(null, "DANH SACH");
                    List<IntPtr> l_child_hWnd = AutoControl.GetChildHandle(hWnd);
                    //IntPtr c_child_hWnd = AutoControl.GetChildHandle(child_hWnd).First();
                    //IntPtr c_c_child_hWnd = AutoControl.GetChildHandle(c_child_hWnd)[2];
                    //AutoControl.SendClickOnControlByHandle(c_c_child_hWnd);
                    AutoControl.SendClickOnControlByHandle(l_child_hWnd[4]);
                    //c_c_child_hWnd = AutoControl.GetChildHandle(c_child_hWnd)[0];
                    //AutoControl.SendClickOnControlByHandle(c_c_child_hWnd);
                    AutoControl.SendClickOnControlByHandle(l_child_hWnd[2]);
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.ToString());
                }
            }

            //Click Auto Vulan
            Process vulan = Process.GetProcessesByName("AutoVLBS").First();
            IntPtr hWnd_main = vulan.MainWindowHandle;
            if (hWnd_main != IntPtr.Zero)
            {
                int sleep = 1000;
                Rect location = new Rect();
                GetWindowRect(hWnd_main, ref location);

                AutoControl.BringToFront(hWnd_main);
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 15, location.Top + 29 + 35, EMouseKey.LEFT);//click bat auto
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 100, location.Top + 29 + 35, EMouseKey.DOUBLE_LEFT);//click hide cua so vl
            }

            Thread.Sleep(2000);
            //Click VLAuto
            hWnd = AutoControl.FindWindowHandle(null, "VLAuto 8.4");
            if (hWnd != IntPtr.Zero)
            {
                AutoControl.BringToFront(hWnd);
                Thread.Sleep(2000);
                Rect location = new Rect();
                GetWindowRect(hWnd, ref location);

                hWnd = AutoControl.GetChildHandle(hWnd).First();
                AutoControl.SendClickOnPositionByPost(hWnd, 5, 35, EMouseKey.LEFT);

                //   AutoControl.MouseClick(location.Left + 55, location.Top + 35 + 39, EMouseKey.DOUBLE_LEFT);
            }
            this.Close();
        }
        private bool Checkthoigian(DateTime start, int thoigian)
        {
            double diff = (DateTime.Now - start).TotalSeconds;
            if (diff > thoigian)
            {
                return true;
            }
            return false;
        }
        private bool TraNhiemVu(string input, ref DateTime startluyencong)
        {
            DateTime startloop = DateTime.Now;
            int thoigianchophep = 60;//cho phep vong lap chay 60s thoi
            if (input == "BoPT")
            {
                AutoControl.MouseClick(660, 600, EMouseKey.LEFT);//click bat bang pt
                Thread.Sleep(500);
                AutoControl.MouseClick(400, 340, EMouseKey.LEFT);//click roi nhom
                Thread.Sleep(500);
                AutoControl.MouseClick(660, 600, EMouseKey.LEFT);//click tat bang pt
                Thread.Sleep(500);
                this.Close();
            }
            // Thread.Sleep(2000);
            Point? point = null;
            Bitmap volam;
            Bitmap box;
            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\HanhTrangDaMo.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            if (point == null)//check co mo ruong chua=>Chua mo
            {
                //mo ruong len
                AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                Thread.Sleep(500);
            }
            ThoatKetThanh:;
            if (input == "ThoatKetThanh")
            {
                chaylaiketthanh:;
                AutoControl.MouseClick(400, 250, EMouseKey.LEFT);//click bo bang ket thanh
                Thread.Sleep(500);
                AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//tam dung auto
                Thread.Sleep(500);
                AutoControl.MouseClick(760, 380, EMouseKey.RIGHT);//click THP
                Thread.Sleep(500);
                AutoControl.MouseClick(350, 330, EMouseKey.LEFT);//click dong 2 THP
                Thread.Sleep(500);
                AutoControl.MouseClick(300, 360, EMouseKey.LEFT);//click ban do 8x
                Thread.Sleep(500);
                AutoControl.MouseClick(290, 300, EMouseKey.LEFT);//click chan nui
                Thread.Sleep(5000);
                AutoControl.MouseClick(760, 355, EMouseKey.RIGHT);//click TDP
                Thread.Sleep(3000);

                DateTime start = DateTime.Now;
                do
                {
                    double diff = (DateTime.Now - start).TotalSeconds;
                    if (diff > 3)
                    {
                        break;
                    }
                    volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KetThanh1000.PNG");
                    point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);

                } while (point == null);

                if (point != null)//check co bi ket ko=>Co
                {
                    goto chaylaiketthanh;
                }

                AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//khoi dong lai auto
                Thread.Sleep(500);
                return false;
            }


            List<Bitmap> l_vatpham_sai = new List<Bitmap>();
        again:;
            if (Checkthoigian(startloop, thoigianchophep))
            {
                return false;
            }
            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);

            List<Point> l_p = new List<Point>();
            List<Bitmap> l_anh = new List<Bitmap>();
            int stt = 0;
            if (input == "vatpham")
            {
                if (point != null)
                {
                    goto again;
                }
                else
                {
                    l_p = TinhToanDiemKhaNang(out l_anh);
                }
            }
        chaylai:;
            if (Checkthoigian(startloop, thoigianchophep))
            {
                return false;
            }
            point = null;
            do
            {
                if (Checkthoigian(startloop, thoigianchophep))
                {
                    return false;
                }
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));

                #region KetThanh,LuyenCong
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KetThanh1000.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point != null)
                {
                    input = "ThoatKetThanh";
                    goto ThoatKetThanh;
                }
                point = null;

                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\LuyenCong.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point==null)
                {
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Xong40LuyenCong.PNG");
                    point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                }
                if (point != null)
                {
                    double diff = DateTime.Now.Subtract(startluyencong).TotalSeconds;
                    if (diff > 60)
                    {
                        input = "ThoatKetThanh";
                        goto ThoatKetThanh;
                    }
                }
                point = null;

                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KetTapHoa.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point != null)
                {
                    AutoControl.MouseClick(230, 450, EMouseKey.LEFT);//click tat tap hoa
                    Thread.Sleep(500);
                }
                point = null;
                #endregion

                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            } while (point == null);
            Thread.Sleep(1000);// CHO AUTO ACTION
            //KIEM TRA LAI CON DOI THOAI KO
            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            if (point != null)
            {
                AutoControl.MouseClick(290, 360, EMouseKey.LEFT);
            }
            else
            {
                return false;
            }
            point = null;

            Thread.Sleep(500);
            #region check fullruong
            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\FullRuong1.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            if (point != null)//check co bi fullruong ko=>Co
            {
                AutoControl.MouseClick(270, 300, EMouseKey.LEFT);// tat doi thoai
                point = null;
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\HanhTrangDaMo.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point == null)//check co mo ruong chua=>Chua mo
                {
                    //mo ruong len
                    AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                    Thread.Sleep(500);
                }
                //mo ruong len
                //AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                point = null;
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\6binhmau.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                AutoControl.MouseClick(point.Value.X + 15, point.Value.Y + 15, EMouseKey.RIGHT);
                Thread.Sleep(500);
                AutoControl.MouseClick(point.Value.X + 45, point.Value.Y + 15, EMouseKey.RIGHT);
                Thread.Sleep(500);
                AutoControl.MouseClick(point.Value.X + 15, point.Value.Y + 45, EMouseKey.RIGHT);
                Thread.Sleep(500);
                AutoControl.MouseClick(point.Value.X + 45, point.Value.Y + 45, EMouseKey.RIGHT);
                Thread.Sleep(500);
                AutoControl.MouseClick(point.Value.X + 15, point.Value.Y + 75, EMouseKey.RIGHT);
                Thread.Sleep(500);
                AutoControl.MouseClick(point.Value.X + 45, point.Value.Y + 75, EMouseKey.RIGHT);
                Thread.Sleep(500);
                //dong ruong
                AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                goto chaylai;
            }
            #endregion

            if (input == "vatpham")
            {
                if (stt < l_p.Count)
                {
                nextpoint:;
                    Point try_p = l_p[stt];
                    Bitmap try_anh = l_anh[stt];
                    foreach (Bitmap item in l_vatpham_sai)
                    {
                        point = ImageScanOpenCV.FindOutPoint(item, try_anh, 0.5);
                        if (point != null)
                        {
                            stt++;
                            if (stt < l_p.Count)
                            {
                                goto nextpoint;
                            }
                        }
                    }
                    AutoControl.MouseClick(try_p.X, try_p.Y, EMouseKey.LEFT);//click vat pham
                    Thread.Sleep(500);
                    AutoControl.MouseClick(390, 350, EMouseKey.LEFT);//dat vat pham vao o tra nhiem vu
                    Thread.Sleep(500);
                    AutoControl.MouseClick(450, 300, EMouseKey.LEFT);
                    Thread.Sleep(500);
                    Bitmap vatpham = CaptureHelper.CaptureImage(new Size(180, 120), new Point(315, 330));// chup anh vat pham
                    AutoControl.MouseClick(360, 460, EMouseKey.LEFT);//click OK
                    Thread.Sleep(500);
                    point = null;
                    DateTime start = DateTime.Now;
                    do
                    {
                        double diff = (DateTime.Now - start).TotalSeconds;
                        if (diff > 3)
                        {
                            break;
                        }
                        volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                        box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\SaiVatPham.PNG");
                        point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                        if (point == null)
                        {
                            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KhongVatPhamNam.PNG");
                            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                            if (point == null)
                            {
                                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KhongVatPhamNu.PNG");
                                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                            }
                        }

                    } while (point == null);

                    if (point != null)//check co bi sai vat pham ko=>Co
                    {
                        l_vatpham_sai.Add(vatpham);
                        vatpham.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\VatPhamSai_" /*+ (x + 1).ToString() + "-" + (y + 1).ToString()*/ + l_vatpham_sai.Count.ToString() + ".PNG");
                        stt++;
                        goto chaylai;
                    }
                }
            }
            else
            {
                #region check nhiem vu vat pham
                DateTime start = DateTime.Now;
                do
                {
                    double diff = (DateTime.Now - start).TotalSeconds;
                    if (diff > 3)
                    {
                        break;
                    }
                    volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\NhiemVuVatPham.PNG");
                    point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                    //if (point == null)
                    //{
                    //    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KhongVatPhamNam.PNG");
                    //    point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                    //    if (point == null)
                    //    {
                    //        box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KhongVatPhamNu.PNG");
                    //        point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                    //    }
                    //}

                } while (point == null);

                if (point != null)//check co phai nhiem vu vat pham ko=>Co
                {
                    return true;
                }
                #endregion
            }
            return false;
        }

        private List<Point> TinhToanDiemKhaNang(out List<Bitmap> hinhanh)
        {
            List<Point> ketqua = new List<Point>();
            hinhanh = new List<Bitmap>();
            List<Bitmap> l_bitmap_check = new List<Bitmap>();
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\BinhMau.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\PDT.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\HP.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Mana.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\HoaHong.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\PhaoHoa.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TTL.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TDP.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\THP.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TuiMau.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TSBL.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\ThuocLac.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\BachQuaLo.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\BaoRuong.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\PhiPhong.png"));

            Bitmap oTrong = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\OTrong.PNG");
            Point goc = new Point(623, 133);
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 10; y++)
                {

                    bool timduoc = false;
                    Point check_point = new Point(goc.X + x * 28, goc.Y + y * 28);
                    Bitmap check = CaptureHelper.CaptureImage(new Size(30, 30), new Point(check_point.X - 15, check_point.Y - 15));
                    Point? point = ImageScanOpenCV.FindOutPoint(check, oTrong, 0.3);//check bo qua o Trong
                    if (point != null)
                    {
                        continue;
                    }
                    foreach (Bitmap item in l_bitmap_check)
                    {
                        point = ImageScanOpenCV.FindOutPoint(check, item, 0.7);
                        if (point != null)
                        {
                            timduoc = true;
                            break;
                        }
                    }
                    if (!timduoc)
                    {
                        //  check.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\BinhMau_" + (x + 1).ToString() + "-" + (y + 1).ToString() + ".PNG");
                        ketqua.Add(check_point);
                        hinhanh.Add(check);
                    }
                }
            }
            return ketqua;
        }

        private void btTraNhiemVu_Click(object sender, EventArgs e)
        {
            int value = 1;
            backgroundWorker1.RunWorkerAsync(argument: value);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //int value = (int)e.Argument;
            //switch (value)
            //{
            //    case 1:
            //        TraNhiemVu(this.tbUser.Text);
            //        break;
            //    case 2:
            //        Login();
            //        break;
            //    default:
            //        break;
            //}
            int sovonglap = 0;
            DateTime start = DateTime.MinValue;
        laplai:;
            sovonglap++;
            backgroundWorker1.ReportProgress(sovonglap);
            if (TraNhiemVu("",ref start))
            {
                sovonglap++;
                backgroundWorker1.ReportProgress(sovonglap);
                TraNhiemVu("vatpham",ref start);
            }
            goto laplai;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (e.UserState != null)
            //{
            //    this.Text = "Finishing Job!";
            //}
            //else
            //{
            //    this.Text = "(Closing after " + e.ProgressPercentage + "s)";
            //}
            this.lb_txt.Text = "WORKING(" + e.ProgressPercentage + @")";
            // MessageBox.Show("(Form will close after " + e.ProgressPercentage + "s)");
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            int value = 1;
            backgroundWorker1.RunWorkerAsync(argument: value);
            // timer1.Interval = 60000;
            //timer1.Start();
            //this.Text = "(Closing after " + counter + "s)";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //counter--;
            //this.Text = "(Closing after " + counter + "s)";
            //if (counter == 0)
            //{
            //    timer1.Stop();
            //    this.Close();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Bitmap> l_anh = new List<Bitmap>();
            List<Point> test = TinhToanDiemKhaNang(out l_anh);
            MessageBox.Show(test.Count.ToString());
        }
    }
}
