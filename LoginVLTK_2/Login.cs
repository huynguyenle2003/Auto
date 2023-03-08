using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
    public partial class frm_login : Form
    {
        private int counter = 60;
        public frm_login()
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
            Thread.Sleep(2000);
            AutoControl.MouseClick(330, 330, EMouseKey.LEFT);//cick box ten tai khoan
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

            Thread.Sleep(500);
            AutoControl.MouseClick(333, 483, EMouseKey.LEFT);//bam qua khac

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
        private void TraNhiemVu(string input)
        {

            Point? point = null;
            Bitmap volam;
            Bitmap box;
            if (input == "Reset")
            {
                //Thoat Auto Cu
                Process[] auto = Process.GetProcessesByName("WindowsFormsApp1");
                foreach (Process item in auto)
                {
                    item.Kill();
                }
                Thread.Sleep(2000);

                //Start Auto
                Process myProcess = new Process();
                try
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.Verb = "runas";
                    myProcess.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\WindowsFormsApp1.exe";
                    myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(myProcess.StartInfo.FileName);
                    myProcess.Start();
                }
                catch (Exception eX)
                {
                    Console.WriteLine(eX.Message);
                }
                Thread.Sleep(2000);
                //Click start

                //IntPtr hwnd = AutoControl.FindHandle(myProcess.MainWindowHandle, "", "Start");
                AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(myProcess.MainWindowHandle)[8]);
                //AutoControl.SendClickOnControlByHandle(hwnd);
                Thread.Sleep(500);
                this.Close();
            }
            if (input == "QuitGame")
            {
                AutoControl.SendKeyDown(KeyCode.ALT);
                AutoControl.SendKeyDown(KeyCode.KEY_X);
                AutoControl.SendKeyUp(KeyCode.KEY_X);
                AutoControl.SendKeyUp(KeyCode.ALT);
                Thread.Sleep(2000);
                AutoControl.MouseClick(400, 560, EMouseKey.LEFT);//click thoat game
                Thread.Sleep(500);
                this.Close();
            }
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
            if (input == "ThoatKetHSP")
            {
                AutoControl.MouseClick(220, 420, EMouseKey.LEFT);//click bat bang pt
                Thread.Sleep(1000);
                AutoControl.MouseClick(220, 420, EMouseKey.LEFT);//click bat bang pt
                Thread.Sleep(1000);
                this.Close();
            }
            // Thread.Sleep(2000);

            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\HanhTrangDaMo.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            if (point == null)//check co mo ruong chua=>Chua mo
            {
                //mo ruong len
                AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                Thread.Sleep(500);
            }
            point = null;
            if (input == "KetNamNhac")
            {
                AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//tam dung auto
                Thread.Sleep(1000);
                AutoControl.MouseClick(400, 250, EMouseKey.LEFT);//click bo bang ket thanh
                Thread.Sleep(1000);
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\XaPhuNamNhac.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                AutoControl.MouseClick(point.Value.X, point.Value.Y, EMouseKey.LEFT);//click xa phu
                point = null;
                AutoControl.MouseClick(300, 315, EMouseKey.LEFT);//click thanh thi da di qua
                Thread.Sleep(1000);
                AutoControl.MouseClick(300, 300, EMouseKey.LEFT);//click ba lang huyen
                Thread.Sleep(5000);
                AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//khoi dong lai auto
                Thread.Sleep(500);
                this.Close();
            }
            if (input == "ThoatKetThanh")
            {
                AutoControl.MouseClick(400, 250, EMouseKey.LEFT);//click bo bang ket thanh
                Thread.Sleep(1000);
                bool chaylai = false;
                chaylaiketthanh:;
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point != null)
                {
                    goto chaylaiketthanh;
                }
                point = null;
                AutoControl.MouseClick(400, 250, EMouseKey.LEFT);//click bo bang ket thanh
                Thread.Sleep(1000);
                if (!chaylai)
                {
                    AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//tam dung auto
                    Thread.Sleep(1000);
                }


                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point != null)
                {
                    //   Thread.Sleep(1000);
                    AutoControl.MouseClick(215, 412, EMouseKey.LEFT);//Tat bang doi thoai
                    Thread.Sleep(500);
                }
                point = null;

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
                    chaylai = true;
                    goto chaylaiketthanh;
                }

                AutoControl.MouseClick(400, 250, EMouseKey.DOUBLE_RIGHT);//khoi dong lai auto
                Thread.Sleep(500);
                this.Close();
            }
            List<Bitmap> l_vatpham_sai = new List<Bitmap>();
            l_vatpham_sai.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\VatPhamSai.PNG"));
            again:;
            point = null;
            volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
            box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
            point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);

            List<Point> l_p = new List<Point>();
            List<Bitmap> l_anh = new List<Bitmap>();
            //int stt = 0;
            if (input == "vatpham")
            {
                if (point != null)
                {
                    // goto again;
                    AutoControl.MouseClick(290, 360, EMouseKey.LEFT);//click tra nhiem vu
                    Thread.Sleep(500);
                    volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                    AutoControl.MouseClick(435, 464, EMouseKey.LEFT);
                }
                //  else
                {
                    point = null;
                    //  volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau_VatPham.PNG");
                    point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                    if (point != null)
                    {
                        goto again;
                    }
                    else
                    {
                        l_p = TinhToanDiemKhaNang(volam, out l_anh);
                        volam.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\VatPham\VoLam.PNG");
                    }

                }
            }
            int stt = l_p.Count - 1;
            chaylai:;
            point = null;
            do
            {
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            } while (point == null);

            point = null;
            AutoControl.MouseClick(290, 360, EMouseKey.LEFT);
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
                // if (stt < l_p.Count)
                if (stt >= 0)
                {
                    nextpoint:;
                    Point try_p = l_p[stt];
                    Bitmap try_anh = l_anh[stt];
                    foreach (Bitmap item in l_vatpham_sai)
                    {
                        point = ImageScanOpenCV.FindOutPoint(item, try_anh, 0.7);
                        if (point != null)
                        {
                            // stt++;
                            stt--;
                            //if (stt < l_p.Count)
                            if (stt >= 0)
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
                        AutoControl.MouseClick(300, 400, EMouseKey.LEFT);//click tat bang thong bao
                        l_vatpham_sai.Add(vatpham);
                        vatpham.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\VatPhamSai_" /*+ (x + 1).ToString() + "-" + (y + 1).ToString()*/ + l_vatpham_sai.Count.ToString() + ".PNG");
                        //  stt++;
                        stt--;
                        goto chaylai;
                    }
                }
            }
            this.Close();
        }

        private List<Point> TinhToanDiemKhaNang(Bitmap chuan, out List<Bitmap> hinhanh)
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
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\THPfree.PNG"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TuiMau.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\TSBL.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\ThuocLac.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\BachQuaLo.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\BaoRuong.png"));
            l_bitmap_check.Add(ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\PhiPhong.png"));

            Bitmap oTrong = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\OTrong.PNG");
            Point goc = new Point(623, 133);
            for (int y = 0; y < 10; y++)
            // for (int x = 0; x < 6; x++)
            {
                for (int x = 0; x < 6; x++)
                //  for (int y = 0; y < 10; y++)
                {

                    bool timduoc = false;
                    Point check_point = new Point(goc.X + x * 28, goc.Y + y * 28);
                    // Bitmap check = CaptureHelper.CaptureImage(new Size(30, 30), new Point(check_point.X - 15, check_point.Y - 15));
                    Rectangle rec = new Rectangle(new Point(check_point.X - 15, check_point.Y - 15), new Size(30, 30));
                    Bitmap check = CaptureHelper.CropImage(chuan, rec);
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
                        check.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\Check\Xuat\VatPham\VatPham_" + (x + 1).ToString() + "-" + (y + 1).ToString() + ".PNG");
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
            int value = (int)e.Argument;
            switch (value)
            {
                case 1:
                    TraNhiemVu(this.tbUser.Text);
                    break;
                case 2:
                    Login();
                    break;
                default:
                    break;
            }
        }
        private void backgroundWorker1_DoWork_backup(object sender, DoWorkEventArgs e)
        {
            Point? point = null;
            Bitmap volam;
            Bitmap box;
            DateTime start = DateTime.Now;
            do
            {
                double demgio = 40 - (DateTime.Now - start).TotalSeconds;
                backgroundWorker1.ReportProgress((int)demgio);
                volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (demgio < 0)
                {
                    this.Close();
                }
            } while (point == null);
            backgroundWorker1.ReportProgress(99, "OK");
            point = null;
            AutoControl.MouseClick(290, 360, EMouseKey.LEFT);
            Thread.Sleep(500);
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
            }
            this.Close();
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                this.Text = "Finishing Job!";
            }
            else
            {
                this.Text = "(Closing after " + e.ProgressPercentage + "s)";
            }

            // MessageBox.Show("(Form will close after " + e.ProgressPercentage + "s)");
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            // timer1.Interval = 60000;
            timer1.Start();
            this.Text = "(Closing after " + counter + "s)";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            this.Text = "(Closing after " + counter + "s)";
            if (counter == 0)
            {
                timer1.Stop();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // List<Bitmap> l_anh = new List<Bitmap>();
            //List<Point> test = TinhToanDiemKhaNang(out l_anh);
            // MessageBox.Show(test.Count.ToString());
        }

        private void bt_Test_Click(object sender, EventArgs e)
        {
            Bitmap manHinh = CaptureHelper.CaptureImage(new Size(1920, 1080), new Point(0, 0));
            //Bitmap manHinh = ImageScanOpenCV.GetImage(System.IO.Directory.GetCurrentDirectory() + @"\Dota\Img\cusor_offset_failed.PNG");
            Bitmap thanhMau = ImageScanOpenCV.GetImage(System.IO.Directory.GetCurrentDirectory() + @"\Dota\Img\KhungThanhMau_Botv2.png");
            //double percent = 0;
            Point? find = FindOutPoint(manHinh, thanhMau);
            // MessageBox.Show(find.ToString());
        }
        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap)
        {
            Image<Bgr, byte> arg_17_0 = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            Point? result = null;
            using (Image<Gray, float> image = arg_17_0.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                image.Save(System.IO.Directory.GetCurrentDirectory() + @"\Dota\Img\Result.png");
                double[] array;
                double[] array2;
                Point[] array3;
                Point[] array4;

                image.MinMax(out array, out array2, out array3, out array4);
                if (array2[0] == 1)
                {
                    result = new Point?(array4[0]);
                }
            }
            return result;
        }
    }
}
