using KAutoHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.SystemMethod;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp1
{

    public partial class AutoLoginDT : Form
    {
        List<Point> l_point = new List<Point>();
        Point p1 = new Point(55, 35);
        Point p2 = new Point(55, 55);
        Point p3 = new Point(55, 70);
        Point p4 = new Point(55, 85);
        Point p5 = new Point(55, 100);

        bool stop = false;
        public AutoLoginDT()
        {
            InitializeComponent();
            l_point.Add(p1);
            l_point.Add(p2);
            l_point.Add(p3);
            l_point.Add(p4);
            l_point.Add(p5);
        }


        private void btLogin_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            stop = true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            //   Excel.Range oCell;
            oXL = (Excel.Application)Marshal.GetActiveObject("Excel.Application");
            oXL.Visible = true;
            oWB = (Excel.Workbook)oXL.ActiveWorkbook;
            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
            //oCell = oXL.ActiveCell;
            //int i = oCell.Row;

            List<NPC> wishlist = new List<NPC>(5);
            backgroundWorker1.ReportProgress(1, "Load List Acc to login");
            LoadNPCWishList(oSheet, ref wishlist);
            List<NPC> online = new List<NPC>(5);

            do
            {
                backgroundWorker1.ReportProgress(1, "Calculate Acc logged");
                try
                {
                    LoadListNPCOnline(ref online);
                }
                catch (Exception)
                {
                }

                List<NPC> l_remove = new List<NPC>();
                foreach (NPC item in wishlist)
                {

                    backgroundWorker1.ReportProgress(1, "Consider acc " + item.Name);
                    if (!item.CheckOnline(online, oSheet))
                    {
                        if (online.Count < 5)
                        {
                            backgroundWorker1.ReportProgress(1, "Logging acc " + item.Name);
                            Process p = LoginVLKT(item.User, item.Pass);
                            string current_login = "";
                            current_login = Convert.ToString(oSheet.Cells[item.RowExcel, 4].Value2);
                            if (current_login == "" || current_login == null)
                            {
                                oSheet.Cells[item.RowExcel, 4] = DateTime.Now.ToString();
                            }
                            else
                            {
                                oSheet.Cells[item.RowExcel, 4] = current_login + "\n" + DateTime.Now.ToString();
                            }
                            Thread.Sleep(5000);
                            item.Process = p;
                        }
                    }
                    else
                    {
                        switch (item.CheckStatus())
                        {
                            case NPC.Status.DangLamNhiemVu:
                                // backgroundWorker1.ReportProgress(1, "Everything is OK!");
                                backgroundWorker1.ReportProgress(1, item.Report());
                                Thread.Sleep(1000);
                                break;
                            case NPC.Status.DungIm:
                                // Refresh game();
                                backgroundWorker1.ReportProgress(1, item.Name + " is Idle!");
                                break;
                            case NPC.Status.XongNhiemVu:
                                backgroundWorker1.ReportProgress(1, item.Name + " is Finish");
                                oSheet.Cells[item.RowExcel, 5] = DateTime.Now.ToString();
                                backgroundWorker1.ReportProgress(1, item.Name + " is quitting game");
                                item.QuitGame();
                                l_remove.Add(item);
                                break;
                            case NPC.Status.DangLamMatChi:
                                //   BoNhiemVu();
                                oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                oSheet.Cells[item.RowExcel, 7] = "LAM MAT CHI";
                                item.QuitGame();
                                l_remove.Add(item);
                                break;
                            case NPC.Status.KetTrongThanh:
                                oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                oSheet.Cells[item.RowExcel, 7] = "KET TRONG THANH";
                                item.QuitGame();
                                l_remove.Add(item);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (l_remove.Count > 0)
                {
                    foreach (NPC item in l_remove)
                    {
                        var itemtoremove = (wishlist.Single(r => r.Name == item.Name));
                        wishlist.Remove(itemtoremove);
                    }
                    LoadNPCWishList(oSheet, ref wishlist);
                }
                if (wishlist.Count == 0)
                {
                    LoadNPCWishList(oSheet, ref wishlist);
                }
                Thread.Sleep(5000);
            } while (!stop);
            backgroundWorker1.ReportProgress(1, "Stop!");
        }
        private void AutoLoginDT_Load(object sender, EventArgs e)
        {
            LoadDefautSetting();
        }
        private void LoadDefautSetting()
        {
            lb_path.Text = Properties.Settings.Default.VLTK_path;
        }
        private void btPathVLTK_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPEN = new OpenFileDialog();
            if (DialogResult.OK == OPEN.ShowDialog())
            {
                string duongdan = OPEN.FileName;
                Properties.Settings.Default.VLTK_path = duongdan;
                Properties.Settings.Default.Save();
                lb_path.Text = duongdan;
            }
        }
        private void UpdateWishList(Excel.Worksheet oSheet, ref List<NPC> wishlist)
        {
            LoadNPCWishList(oSheet, ref wishlist);
        }
        private void LoadNPCWishList(Excel.Worksheet osheet, ref List<NPC> wishlist)
        {
            Excel.Range usedRange = osheet.UsedRange;
            foreach (Excel.Range row in usedRange.Rows)
            {
                if (wishlist.Count == (int)this.numericUpDown1.Value)
                {
                    return;
                }
                string rowData = Convert.ToString(row.Cells[1, 5].Value2);
                if (rowData == "Logout")
                {
                    continue;
                }
                string tamthoat = Convert.ToString(row.Cells[1, 6].Value2);
                if (tamthoat != null)
                {
                    continue;
                }
                string name = Convert.ToString(row.Cells[1, 3].Value2);
                foreach (NPC item in wishlist)
                {
                    if (item.Name == name)
                    {
                        goto nextrow;
                    }
                }
                DateTime date = DateTime.MinValue;
                DateTime.TryParse(rowData, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    NPC npc = new NPC(Convert.ToString(row.Cells[1, 3].Value2), Convert.ToString(row.Cells[1, 1].Value2), Convert.ToString(row.Cells[1, 2].Value2), "", "");
                    npc.RowExcel = row.Row;
                    osheet.Cells[npc.RowExcel, 4] = "";
                    wishlist.Add(npc);
                }
            nextrow:;
            }
            //wishlist.Add(new NPC("huydb20", "huydaibang20", "1233041990", "", ""));
        }
        private void LoadListNPCOnline(ref List<NPC> l_NPC_online)
        {
            int sleep = 1000;
            l_NPC_online.Clear();

            Process proc_vlauto = GetVLAuto();

            //  Process proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            IntPtr hWnd = AutoControl.GetChildHandle(proc_vlauto.MainWindowHandle).First();
            IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, proc_vlauto.Id);

            AutoControl.SendClickOnPositionByPost(hWnd, p1.X, p1.Y, EMouseKey.LEFT);
            Thread.Sleep(sleep);
            NPC npc = new NPC(proc_vlauto, processHandle);
            npc.ClickPoint = p1;
            if (npc.Name != "")
            {
                l_NPC_online.Add(npc);
            }


            AutoControl.SendClickOnPositionByPost(hWnd, p2.X, p2.Y, EMouseKey.LEFT);
            Thread.Sleep(sleep);
            npc = new NPC(proc_vlauto, processHandle);
            npc.ClickPoint = p2;
            if (npc.Name != "")
            {
                l_NPC_online.Add(npc);
            }

            AutoControl.SendClickOnPositionByPost(hWnd, p3.X, p3.Y, EMouseKey.LEFT);
            Thread.Sleep(sleep);
            npc = new NPC(proc_vlauto, processHandle);
            npc.ClickPoint = p3;
            if (npc.Name != "")
            {
                l_NPC_online.Add(npc);
            }

            AutoControl.SendClickOnPositionByPost(hWnd, p4.X, p4.Y, EMouseKey.LEFT);
            Thread.Sleep(sleep);
            npc = new NPC(proc_vlauto, processHandle);
            npc.ClickPoint = p4;
            if (npc.Name != "")
            {
                l_NPC_online.Add(npc);
            }

            AutoControl.SendClickOnPositionByPost(hWnd, p5.X, p5.Y, EMouseKey.LEFT);
            Thread.Sleep(sleep);
            npc = new NPC(proc_vlauto, processHandle);
            npc.ClickPoint = p5;
            if (npc.Name != "")
            {
                l_NPC_online.Add(npc);
            }
        }
        private Process LoginVLKT(string user, string pass)
        {
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = Properties.Settings.Default.VLTK_path;
                myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(myProcess.StartInfo.FileName);
                myProcess.Start();
            }
            catch (Exception eX)
            {
                Console.WriteLine(eX.Message);
            }
            //IntPtr hWnd = myProcess.MainWindowHandle;
            //AutoControl.BringToFront(hWnd);

            Thread.Sleep(5000);
            Process[] procs = Process.GetProcessesByName("vggame");

            Process kq = null;
            DateTime last = DateTime.MinValue;
            foreach (Process item in procs)
            {
                if (item.StartTime > last)
                {
                    kq = item;
                    last = item.StartTime;
                }
            }

            Process proc = Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LoginVLTK_2.exe");

            Thread.Sleep(2000);
            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[2], user);//user
            Thread.Sleep(500);
            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[1], pass);//pass
            Thread.Sleep(500);
            AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc.MainWindowHandle)[0]);
            proc.WaitForExit();

            Process proc_vlauto = GetVLAuto();
            IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, proc_vlauto.Id);
            IntPtr hWnd = AutoControl.GetChildHandle(proc_vlauto.MainWindowHandle).First();
            AutoControl.SendClickOnPositionByPost(hWnd, p1.X, p1.Y, EMouseKey.LEFT);
            byte[] buffer = new byte[4];
            int bytesread;
            SystemMethod.ReadProcessMemory(processHandle, IntPtr.Add(proc_vlauto.MainModule.BaseAddress, 0x1F0F50), buffer, 4, out bytesread);
            IntPtr P_main = new IntPtr(BitConverter.ToInt32(buffer, 0));
            string name_loged = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(P_main, 24));
            // MessageBox.Show("Loged: " + name_loged);
            if (name_loged.StartsWith("<GAME"))
            {
                kq.Kill();
                return null;
            }
            return kq;
        }

        public Process GetVLAuto()
        {
            Process[] list = Process.GetProcessesByName("VLAutoPr");
            Process proc_vlauto;
            if (list.Count() == 0)
            {
                proc_vlauto = Process.Start(@"D:\Games\[DaiLamAn_VoCongTruyenKy]_FULL_MatKhau_123456\1.Auto Luyen Cong\_VL_AutoPRO_88_ChuyenDaTau\VLAutoPr.exe");
            }
            else
            {
                proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            }
            return proc_vlauto;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                this.lb_status.Text = "Status: " + e.UserState.ToString();
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            //Process proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            //// MessageBox.Show(SystemMethod.Instance.FindLocationChildHandle(proc_vlauto.MainWindowHandle, "Mở rộng").ToString());
            //AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc_vlauto.MainWindowHandle)[33]);
            //Thread.Sleep(2000);
            IntPtr morong = AutoControl.FindWindowHandle("#32770 (Dialog)", "Nhiem vu - Options");
            AutoControl.SendClickOnPosition(morong, 100, 50, EMouseKey.LEFT, 1);
            //IntPtr huynv = AutoControl.GetChildHandle("", "Nhiem vu - Options");
            //AutoControl.SendClickOnPosition(morong, 20, 40, EMouseKey.LEFT, 1);
            //AutoControl.SendClickOnPosition(morong, 20, 40, EMouseKey.LEFT, 1);
        }
    }
    public class NPC
    {
        public enum Status
        {
            DangLamNhiemVu,
            DungIm,
            XongNhiemVu,
            HuyNVKinhNghiem,
            DangLamMatChi,
            KetTrongThanh,
            TraNhiemVu,//vulan
            TraNhiemVuTrangBi,//vulan
            SaiMapLuyenCong,//vulan
        }

        private string name;
        private string map;
        private string toado;
        private string user;
        private string pass;
        private DateTime lastUpdateMap;
        private DateTime lastUpdateToaDo;
        private DateTime lastTryRefresh;
        private Point clickPoint;
        private int rowExcel;
        private Process process;
        private string mapDoChi_L1;
        private string datau;//vulan
        private string nhiemvu;//vulan
        private Process processAuto;
        private string test_note;
        private DateTime lastKiemTraSaiMap;
        private string kinhnghiem;
        private string typeNPC;
        // bool forceQuit = false;
        private DateTime lastBoPT;
        private DateTime lastHuyMatChi;
        private DateTime lastKetThanh;
        private int thuNhap;
        private int tuVong;
        private bool lanChanTraNhiemVuTrangBi;
        public string Name { get => name; set => name = value; }
        public string Map { get => map; set => map = value; }
        public string Toado { get => toado; set => toado = value; }
        public string User { get => user; set => user = value; }
        public string Pass { get => pass; set => pass = value; }
        public DateTime LastUpdateMap { get => lastUpdateMap; set => lastUpdateMap = value; }
        public DateTime LastUpdateToaDo { get => lastUpdateToaDo; set => lastUpdateToaDo = value; }
        public Point ClickPoint { get => clickPoint; set => clickPoint = value; }
        public int RowExcel { get => rowExcel; set => rowExcel = value; }
        public Process Process { get => process; set => process = value; }//process vo lam
        public string MapDoChi_L1 { get => mapDoChi_L1; set => mapDoChi_L1 = value; }
        public string Datau { get => datau; set => datau = value; }
        public string Nhiemvu { get => nhiemvu; set => nhiemvu = value; }
        public Process ProcessAuto { get => processAuto; set => processAuto = value; }
        public string Test_note { get => test_note; set => test_note = value; }
        public string Kinhnghiem { get => kinhnghiem; set => kinhnghiem = value; }
        //public bool ForceQuit { get => forceQuit; set => forceQuit = value; }
        public DateTime LastBoPT { get => lastBoPT; set => lastBoPT = value; }
        public string TypeNPC { get => typeNPC; set => typeNPC = value; }
        public DateTime LastHuyMatChi { get => lastHuyMatChi; set => lastHuyMatChi = value; }
        public DateTime LastKetThanh { get => lastKetThanh; set => lastKetThanh = value; }
        public DateTime LastTryRefresh { get => lastTryRefresh; set => lastTryRefresh = value; }
        public int ThuNhap { get => thuNhap; set => thuNhap = value; }
        public int TuVong { get => tuVong; set => tuVong = value; }
        public bool LanChanTraNhiemVuTrangBi { get => lanChanTraNhiemVuTrangBi; set => lanChanTraNhiemVuTrangBi = value; }
        public DateTime LastKiemTraSaiMap { get => lastKiemTraSaiMap; set => lastKiemTraSaiMap = value; }

        public NPC(string _name, string _map, string _toado)
        {
            Name = _name;
            Map = _map;
            Toado = _toado;
        }
        public NPC(string _name, string _user, string _pass, string _map, string _toado)
        {
            LanChanTraNhiemVuTrangBi = false;
            Name = _name;
            User = _user;
            Pass = _pass;
        }
        public NPC(Process proc_vlauto, IntPtr processHandle)
        {
            IntPtr hWnd = proc_vlauto.MainWindowHandle;
            //   MessageBox.Show(GetWindowCaption(hWnd));
            // IntPtr hwnd_map = AutoControl.GetChildHandle(AutoControl.GetChildHandle(AutoControl.GetChildHandle(hwnd_vlauto)[1])[8])[7];
            //for (int i = 0; i < AutoControl.GetChildHandle(hWnd).Count; i++)
            //{
            //   // MessageBox.Show(GetWindowCaption(AutoControl.GetChildHandle(hWnd)[i]));
            //    IntPtr item = AutoControl.GetChildHandle(hWnd)[i];
            //    if (GetWindowCaption(item) == "Bản đồ")
            //    {
            //        MessageBox.Show(i.ToString());
            //    }
            //}
            byte[] buffer = new byte[4];
            int bytesread;
            SystemMethod.ReadProcessMemory(processHandle, IntPtr.Add(proc_vlauto.MainModule.BaseAddress, 0x1F0F50), buffer, 4, out bytesread);
            IntPtr P_main = new IntPtr(BitConverter.ToInt32(buffer, 0));
            Name = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(P_main, 24));
            Map = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[199]);
            Toado = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[200]);
        }
        public NPC(IntPtr hWnd, string name)
        {
            //IntPtr hWnd = proc_vlauto.MainWindowHandle;
            ////   MessageBox.Show(GetWindowCaption(hWnd));
            //// IntPtr hwnd_map = AutoControl.GetChildHandle(AutoControl.GetChildHandle(AutoControl.GetChildHandle(hwnd_vlauto)[1])[8])[7];
            ////for (int i = 0; i < AutoControl.GetChildHandle(hWnd).Count; i++)
            ////{
            ////   // MessageBox.Show(GetWindowCaption(AutoControl.GetChildHandle(hWnd)[i]));
            ////    IntPtr item = AutoControl.GetChildHandle(hWnd)[i];
            ////    if (GetWindowCaption(item) == "Bản đồ")
            ////    {
            ////        MessageBox.Show(i.ToString());
            ////    }
            ////}
            //byte[] buffer = new byte[4];
            //int bytesread;
            //SystemMethod.ReadProcessMemory(processHandle, IntPtr.Add(proc_vlauto.MainModule.BaseAddress, 0x1F0F50), buffer, 4, out bytesread);
            //IntPtr P_main = new IntPtr(BitConverter.ToInt32(buffer, 0));
            Name = name;
            string text = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[14]);
            Map = text.Split('(').First();
            Toado = text.Split('(').Last().Split(')').First();
            string text_datau = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[24]);
            Datau = text_datau.Split(':')[1];
            string text_nhiemvu = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[27]);
            Nhiemvu = text_nhiemvu.Split(':')[1];
            string text_kinhnghiem = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[219]);
            Kinhnghiem = text_kinhnghiem;
            string text_thunhap = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[15]);
            ThuNhap = TinhThuNhap(text_thunhap);
            string text_tuvong = GetTextBoxText(AutoControl.GetChildHandle(hWnd)[13]);
            TuVong = int.Parse(text_tuvong.Split(':')[1]);
        }

        private int TinhThuNhap(string text_thunhap)
        {
            //"Thu nhËp: 0 v¹n -37 l­îng"
            int thunhap = 0;
            string van = text_thunhap.Split(':')[1].Split('v')[0];
            string luong = text_thunhap.Split('l')[0].Split('n').Last();
            try
            {
                thunhap += int.Parse(van) * 10000;
                thunhap += int.Parse(luong);
            }
            catch (Exception)
            {
            }
            return thunhap;
        }
        public bool CheckOnline(List<NPC> online, Excel.Worksheet oSheet)
        {
            foreach (NPC item in online)
            {
                if (item.Name == this.Name)
                {
                    if (this.Map != item.Map)
                    {
                        this.Map = item.Map;
                        this.LastUpdateMap = DateTime.Now;
                    }
                    if (this.Toado != item.Toado)
                    {
                        this.Toado = item.Toado;
                        this.lastUpdateToaDo = DateTime.Now;
                    }
                    if (this.Datau == null)
                    {
                        //ghi lai bat dau dt
                        //string current_login = "";
                        //current_login = Convert.ToString(oSheet.Cells[this.RowExcel, 9].Value2);
                        //if (current_login == "" || current_login == null)
                        //{
                        //    oSheet.Cells[this.RowExcel, 9] = item.Datau;
                        //}
                        //else
                        //{
                        //    oSheet.Cells[this.RowExcel, 9] = current_login + "\n" + item.Datau;
                        //}

                        int shxt = 0;
                        string string_shxt = Convert.ToString(oSheet.Cells[this.RowExcel, 19].Value2);
                        if (string_shxt != null)
                        {
                            int.TryParse(string_shxt, out shxt);
                        }


                        string rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 4].Value);
                        DateTime date = DateTime.MinValue;
                        if (rowData != null)
                        {
                            DateTime.TryParse(rowData.Split('\n').Last(), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                        }
                        else
                        {
                            oSheet.Cells[this.RowExcel, 4] = DateTime.Now.ToString();
                            oSheet.Cells[this.RowExcel, 9] = item.Datau;
                            oSheet.Cells[this.RowExcel, 19] = shxt - int.Parse(item.Datau.Split('/')[1]);
                        }


                        //DateTime.TryParse(rowData, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                        if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                        {
                            oSheet.Cells[this.RowExcel, 9] = item.Datau;
                            oSheet.Cells[this.RowExcel, 19] = shxt - int.Parse(item.Datau.Split('/')[1]);
                        }
                        else
                        {
                            rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 9].Value2);
                            if (rowData == null || rowData == "")
                            {
                                oSheet.Cells[this.RowExcel, 9] = item.Datau;
                                oSheet.Cells[this.RowExcel, 19] = shxt - int.Parse(item.Datau.Split('/')[1]);
                            }
                        }


                    }
                    this.Datau = item.Datau;
                    this.Nhiemvu = item.Nhiemvu;
                    this.ClickPoint = item.ClickPoint;
                    this.ProcessAuto = item.ProcessAuto;
                    this.Kinhnghiem = item.Kinhnghiem;
                    this.ThuNhap = item.ThuNhap;
                    this.TuVong = item.TuVong;
                    this.Process = item.Process;
                    //  oSheet.Cells[this.RowExcel, 15] = item.ThuNhap;
                    return true;
                }
            }
            return false;
        }
        public Status CheckStatusVulan(Excel.Worksheet oSheet)
        {

            //if (this.ForceQuit)
            //{
            //    return Status.XongNhiemVu;
            //}
            double diff_Map = DateTime.Now.Subtract(this.LastUpdateMap).TotalSeconds;
            double diff_toado = DateTime.Now.Subtract(this.LastUpdateToaDo).TotalSeconds;
            bool tranv = false;
            bool flat1 = this.Nhiemvu.Contains("D©y chuyÒn");
            bool flat2 = this.Nhiemvu.Contains("NhÉn");
            bool flat3 = this.Nhiemvu.Contains("Ngäc béi");
            bool flat4 = this.Nhiemvu.Contains("Mua ®å trong shop");
            bool flat5 = this.Nhiemvu.Contains("Trang bÞ");
            tranv = flat1 || flat2 || flat3 || flat4 || flat5;
            if (this.IsInTown())
            {
                if (this.LastUpdateMap == DateTime.MinValue || this.LastUpdateToaDo == DateTime.MinValue)
                {
                    goto boqua;
                }
                //if (Nhiemvu == "")
                //{
                //    return Status.XongNhiemVu;
                //}
                if ((diff_Map > 300 && diff_toado > 120) || (diff_toado > 60))
                {
                    if (diff_toado > 300)//quitgame
                    {
                        //this.TryRefresh(oSheet);
                        return Status.XongNhiemVu;
                    }
                    bool check = this.CheckNhanChu("NhanCu", 5);
                    if (check)
                    {
                        //  this.TryRefresh(oSheet);
                        oSheet.Cells[this.RowExcel, 6] = "OK";
                        return Status.XongNhiemVu;
                    }
                    // this.Test_note = "diff_Map > 300 va diff_toado > 120";
                    if ((DateTime.Now - this.LastTryRefresh).TotalSeconds > 180)
                    {
                        return Status.DungIm;
                    }
                    if ((DateTime.Now - this.lastTryRefresh).TotalSeconds < 60)
                    {
                        goto boqua;
                    }
                    if ((DateTime.Now - this.lastTryRefresh).TotalSeconds < 120 && tranv)
                    {
                        goto boqua;
                    }
                    if (this.CheckNhanChu("DoiThoaiDaTau", 5))
                    {
                        goto boqua;
                    }
                    if (CheckViTriGanDaTau(this.Map, this.Toado, 1))
                    {
                        //this.TryRefresh(oSheet);
                        return Status.XongNhiemVu;
                    }

                }
                //D©y chuyÒn D©y chuyÒn NhÉn Ngäc béi
                //if (diff_toado > 60 && this.Map == "Thµnh §« " && this.Toado == "393/317")
                //{
                //    this.LastUpdateToaDo = DateTime.MinValue;
                //   // this.QuitGame();
                //    return Status.DungIm;
                //}
                //if (diff_toado > 180)
                //{
                //    this.Test_note = "diff_toado > 180";
                //    return Status.DungIm;
                //}
            }
            else
            {
                if (diff_toado > 120)
                {
                    if (this.Map.Contains("Hoµnh S¬n"))
                    {
                        this.Action("ThoatKetHSP");
                        return Status.DangLamNhiemVu;
                    }
                    else
                    {
                        if (!this.CheckNhanChu("CheckLag", 5))
                        {
                            // this.TryRefresh(oSheet);
                            return Status.XongNhiemVu;
                        }
                        if ((DateTime.Now - this.LastKetThanh).TotalSeconds > 300)
                        {
                            this.Action("ThoatKetThanh");
                            this.LastKetThanh = DateTime.Now;
                            return Status.DangLamNhiemVu;
                        }
                        if ((DateTime.Now - this.LastKetThanh).TotalSeconds < 60)
                        {
                            goto boqua;
                        }
                        else
                        {
                            if ((DateTime.Now - this.LastTryRefresh).TotalSeconds > 300)
                            {
                                this.TryRefresh(oSheet,true);
                                this.LastTryRefresh = DateTime.Now;
                                return Status.DangLamNhiemVu;
                            }
                            if ((DateTime.Now - this.LastTryRefresh).TotalSeconds < 60)
                            {
                                goto boqua;
                            }
                            else
                            {
                                // this.TryRefresh(oSheet);
                                return Status.XongNhiemVu;
                            }
                        }
                    }
                }
            }
        boqua:;
            if (this.Map == "S¬n B¶o ®éng ")
            {
                if (diff_Map < 120 && diff_Map > 60)
                {
                    if ((DateTime.Now - this.lastBoPT).TotalSeconds > 90)
                    {
                        //this.BoPt();
                        this.Action("BoPT");
                        this.LastBoPT = DateTime.Now;
                    }
                }
                //return Status.DangLamNhiemVu;
            }
            bool phuonghoang = (this.Map.Contains("Ph­îng Hoµng"));
            bool thanh = (this.Map.Contains(" Thµnh"));
            bool namnhac = (this.Map.Contains("Nam Nh¹c trÊn"));
            bool ketthanh1000 = (phuonghoang || thanh || namnhac) && diff_toado > 15;
            if (namnhac && this.Toado == "198/193")
            {
                this.Action("KetNamNhac");
            }
            if (ketthanh1000)
            {
                if ((DateTime.Now - this.LastKetThanh).TotalSeconds > 60)
                {
                    this.Action("ThoatKetThanh");
                    this.LastKetThanh = DateTime.Now;
                    //Thread.Sleep(5000);
                    if (this.Nhiemvu.Contains("Mua ®å trong shop"))
                    {
                        this.TryRefresh(oSheet,true);
                    }
                }
                return Status.DangLamNhiemVu;
            }

            bool ganDatau = CheckViTriGanDaTau(this.Map, this.Toado, 1);

            if (this.Datau.Split('(').Count() > 1)
            {
                string text = this.Datau.Split('(')[1];
                int dalam = int.Parse(text.Split('/').First());
                int nhiemvu = int.Parse(text.Split('/').Last().Split(')').First());
                if (nhiemvu < 5)
                {
                    return Status.DangLamMatChi;
                }
                if (dalam >= nhiemvu)
                {
                    if (diff_Map > 1000)
                    {
                        this.Test_note = "diffmap > 1000";
                        return Status.XongNhiemVu;
                    }
                    return Status.TraNhiemVu;
                }
                else
                {
                    return Status.DangLamNhiemVu;
                }
            }
            else
            {
                if (this.Nhiemvu.Contains("PD"))
                {
                    string text = this.Nhiemvu.Split('(')[1];
                    int dalam = int.Parse(text.Split('/').First());
                    int nhiemvu = int.Parse(text.Split('/').Last().Split(')').First());
                    if (dalam >= nhiemvu)
                    {
                        return Status.TraNhiemVu;
                    }
                    else
                    {
                        return Status.DangLamNhiemVu;
                    }
                }
                if (this.Nhiemvu.Contains("B¶n ®å SHXT"))
                {
                    return Status.TraNhiemVu;
                }
                if (tranv)
                {
                    //if (this.IsInTown())
                    if (ganDatau)
                    {
                        return Status.TraNhiemVuTrangBi;

                    }
                }
                if (this.Nhiemvu.Contains("KN"))
                {
                    string text = this.Nhiemvu.Split('(')[1];
                    double dalam = double.Parse(text.Split('/').First());
                    double nhiemvu = double.Parse(text.Split('/').Last().Split(')').First());
                    //  MessageBox.Show("Kinh Nghiem ---" + nhiemvu.ToString() + "---" + (nhiemvu > 0.5).ToString());
                    if (!this.IsInTown() && this.Map != "S¬n B¶o ®éng ")
                    {
                        return Status.SaiMapLuyenCong;
                    }
                    if (dalam < 0)
                    {
                        return Status.HuyNVKinhNghiem;
                    }
                    if (nhiemvu >= 0.5)
                    {
                        return Status.HuyNVKinhNghiem;
                    }
                    if (dalam >= nhiemvu)
                    {
                        return Status.TraNhiemVu;
                    }
                }
            }

            return Status.DangLamNhiemVu;
        }
        private bool CheckViTriGanDaTau(string map, string toado, int v)
        {
            switch (map)
            {
                case "BiÖn Kinh ":
                    if (SoSanhToaDo(toado, "216,193", v))
                    {
                        return true;
                    }
                    break;
                case "Ph­îng T­êng ":
                    if (SoSanhToaDo(toado, "202,193", v))
                    {
                        return true;
                    }
                    break;
                case "§¹i Lý phñ ":
                    if (SoSanhToaDo(toado, "206,201", v))
                    {
                        return true;
                    }
                    break;
                case "Thµnh §« ":
                    if (SoSanhToaDo(this.toado, "394,316", v))
                    {
                        return true;
                    }
                    break;
                case "T­¬ng D­¬ng ":
                    if (SoSanhToaDo(this.toado, "199,205", v))
                    {
                        return true;
                    }
                    break;
                case "D­¬ng Ch©u ":
                    if (SoSanhToaDo(this.toado, "218,185", v))
                    {
                        return true;
                    }
                    break;
                case "L©m An ":
                    if (SoSanhToaDo(this.toado, "195,186", v))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        public bool isCurrentGanDaTau(int v)
        {
            IntPtr hWnd = AutoControl.FindWindowExFromParent(this.ProcessAuto.MainWindowHandle, "List1", "SysListView32");
            AutoControl.SendClickOnPositionByPost(hWnd, this.ClickPoint.X, this.ClickPoint.Y, EMouseKey.LEFT);
            Thread.Sleep(2000);
            string text = GetTextBoxText(AutoControl.GetChildHandle(this.ProcessAuto.MainWindowHandle)[14]);
            string map = text.Split('(').First();
            string toado = text.Split('(').Last().Split(')').First();
            switch (map)
            {
                case "BiÖn Kinh ":
                    if (SoSanhToaDo(toado, "216,193", v))
                    {
                        return true;
                    }
                    break;
                case "Ph­îng T­êng ":
                    if (SoSanhToaDo(toado, "202,193", v))
                    {
                        return true;
                    }
                    break;
                case "§¹i Lý phñ ":
                    if (SoSanhToaDo(toado, "206,201", v))
                    {
                        return true;
                    }
                    break;
                case "Thµnh §« ":
                    if (SoSanhToaDo(this.toado, "394,316", v))
                    {
                        return true;
                    }
                    break;
                case "T­¬ng D­¬ng ":
                    if (SoSanhToaDo(this.toado, "199,205", v))
                    {
                        return true;
                    }
                    break;
                case "D­¬ng Ch©u ":
                    if (SoSanhToaDo(this.toado, "218,185", v))
                    {
                        return true;
                    }
                    break;
                case "L©m An ":
                    if (SoSanhToaDo(this.toado, "195,186", v))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        private void Action(string v)
        {
            IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

            Rect location = new Rect();
            GetWindowRect(hWnd, ref location);

            //   bool batthanhcong = false;
            Process proc = Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LoginVLTK_2.exe");
            DateTime start = DateTime.Now;
            do
            {
                double diff = (DateTime.Now - start).TotalSeconds;
                if (diff > 15)
                {
                    //proc.Kill();
                    v = "Reset";
                    break;
                    //AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[3], v);//user
                    //return;
                }
                AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(2000);
                if (this.Process == null)
                {
                    Process[] procs = Process.GetProcessesByName("vggame");
                    foreach (Process item in procs)
                    {
                        if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                        {
                            //   batthanhcong = true;
                            this.Process = item;
                            break;
                        }
                    }
                }
                else
                {
                    if (SystemMethod.IsWindowVisible(this.Process.MainWindowHandle))
                    {
                        break;
                    }
                }
            } while (true);
            // } while (!batthanhcong);
            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[3], v);//user

            Thread.Sleep(1000);


            AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc.MainWindowHandle)[0]);

            proc.WaitForExit();

            if (v == "QuitGame")
            {
                return;
            }

            AutoControl.BringToFront(hWnd_main);
            Thread.Sleep(4000);
            AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
            Thread.Sleep(1000);
        }
        private void BoPt()
        {
            IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

            Rect location = new Rect();
            GetWindowRect(hWnd, ref location);

            bool batthanhcong = false;
            Process proc = Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LoginVLTK_2.exe");
            do
            {
                AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(2000);
                Process[] procs = Process.GetProcessesByName("vggame");
                foreach (Process item in procs)
                {
                    if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                    {
                        batthanhcong = true;
                        if (this.Process == null)
                        {
                            this.Process = item;
                        }
                        break;
                    }
                }
            } while (!batthanhcong);

            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[3], "BoPT");//user

            Thread.Sleep(1000);


            AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc.MainWindowHandle)[0]);

            proc.WaitForExit();


            AutoControl.BringToFront(hWnd_main);
            Thread.Sleep(4000);
            AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
            Thread.Sleep(1000);
        }
        public Status CheckStatus()
        {
            //bool isInTown = (this.Map == "Đại Lý") || (this.Map == "Dương Châu") || (this.Map == "Phượng Tường") || (this.Map == "Thành Đô") || (this.Map == "Tương Dương") || (this.Map == "Biện Kinh") || (this.Map == "Biện Kinh");
            double diff_Map = DateTime.Now.Subtract(this.LastUpdateMap).TotalSeconds;
            double diff_ToaDo = DateTime.Now.Subtract(this.LastUpdateToaDo).TotalSeconds;
            if (!this.IsInTown())//ko o thanh
            {
                if (this.Map != this.MapDoChi_L1)
                {
                    this.MapDoChi_L1 = "";
                }
            }
            if (diff_Map > 1000)
            {
                if (this.MapDoChi_L1 == "")
                {
                    this.MapDoChi_L1 = this.Map;
                    this.LastUpdateMap = DateTime.Now;
                }
                else
                {
                    return Status.DangLamMatChi;
                }
                if (this.IsInTown())
                {
                    return Status.KetTrongThanh;
                }
            }
            else if (diff_Map > 300 && diff_ToaDo > 60)
            {
                if (this.IsInTown())
                {
                    return Status.DungIm;
                }
            }
            else if (diff_Map > 120 && diff_ToaDo > 90)
            {
                switch (this.Map)
                {
                    case "Biện Kinh":
                        if (SoSanhToaDo(this.Toado, "216,193", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Phượng Tường":
                        if (SoSanhToaDo(this.Toado, "202,193", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Đại Lý":
                        if (SoSanhToaDo(this.Toado, "206,201", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Thành Đô":
                        if (SoSanhToaDo(this.Toado, "394,316", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Tương Dương":
                        if (SoSanhToaDo(this.Toado, "199,205", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Dương Châu":
                        if (SoSanhToaDo(this.Toado, "218,185", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    case "Lâm An":
                        if (SoSanhToaDo(this.Toado, "195,186", 0))
                        {
                            return Status.XongNhiemVu;
                        }
                        break;
                    default:
                        break;
                }
            }
            return Status.DangLamNhiemVu;
        }
        private bool IsInTown()
        {
            switch (this.Map)
            {
                case "Biện Kinh":
                    return true;
                case "Phượng Tường":
                    return true;
                case "Đại Lý":
                    return true;
                case "Thành Đô":
                    return true;
                case "Tương Dương":
                    return true;
                case "Dương Châu":
                    return true;
                case "Lâm An":
                    return true;
                case "BiÖn Kinh ":
                    return true;
                case "Ph­îng T­êng ":
                    return true;
                case "§¹i Lý phñ ":
                    return true;
                case "Thµnh §« ":
                    return true;
                case "T­¬ng D­¬ng ":
                    return true;
                case "D­¬ng Ch©u ":
                    return true;
                case "L©m An ":
                    return true;
                default:
                    break;
            }
            return false;
        }
        public bool IsCurrentInTown()
        {
            IntPtr hWnd = AutoControl.FindWindowExFromParent(this.ProcessAuto.MainWindowHandle, "List1", "SysListView32");
            AutoControl.SendClickOnPositionByPost(hWnd, this.ClickPoint.X, this.ClickPoint.Y, EMouseKey.LEFT);
            Thread.Sleep(2000);
            string text = GetTextBoxText(AutoControl.GetChildHandle(this.ProcessAuto.MainWindowHandle)[14]);
            string map = text.Split('(').First();
            switch (map)
            {
                case "Biện Kinh":
                    return true;
                case "Phượng Tường":
                    return true;
                case "Đại Lý":
                    return true;
                case "Thành Đô":
                    return true;
                case "Tương Dương":
                    return true;
                case "Dương Châu":
                    return true;
                case "Lâm An":
                    return true;
                case "BiÖn Kinh ":
                    return true;
                case "Ph­îng T­êng ":
                    return true;
                case "§¹i Lý phñ ":
                    return true;
                case "Thµnh §« ":
                    return true;
                case "T­¬ng D­¬ng ":
                    return true;
                case "D­¬ng Ch©u ":
                    return true;
                case "L©m An ":
                    return true;
                default:
                    break;
            }
            return false;
        }
        public bool SoSanhToaDo(string toado1, string toado2, int lamtron)
        {
            char split = ',';
            if (toado1.Contains('/'))
            {
                split = '/';
            }
            int x1 = int.Parse(toado1.Split(split)[0]);
            int y1 = int.Parse(toado1.Split(split)[1]);
            int x2 = int.Parse(toado2.Split(',')[0]);
            int y2 = int.Parse(toado2.Split(',')[1]);
            if ((x1 + lamtron) >= x2 && (x1 - lamtron) <= x2)
            {
                if ((y1 + lamtron) >= y2 && (y1 - lamtron) <= y2)
                {
                    return true;
                }
            }
            return false;
        }
        public void QuitGame()
        {
            this.Action("QuitGame");
            Thread.Sleep(5000);
            try
            {
                this.process.Kill();
            }
            catch (Exception)
            {
            }
            //if (this.process != null)
            //{
            //    this.process.Kill();
            //}
            //else
            //{
            //    Process gameprocess = GetGameProcess();
            //    if (gameprocess != null)
            //    {
            //        gameprocess.Kill();
            //    }
            //}
            Thread.Sleep(5000);
        }
        public string Report()
        {
            string space = " - ";
            string rp = this.Name + space + this.Map + space + Math.Round(DateTime.Now.Subtract(this.LastUpdateMap).TotalSeconds) + "s" + space + "(" + this.Toado + ")" + space + Math.Round(DateTime.Now.Subtract(this.LastUpdateToaDo).TotalSeconds) + "s";
            return rp;
        }
        private Process GetGameProcess()
        {
            IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");
            Rect location = new Rect();
            GetWindowRect(hWnd, ref location);
            AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
            Thread.Sleep(1000);
            Process[] procs = Process.GetProcessesByName("vggame");
            foreach (Process item in procs)
            {
                if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                {
                    return item;
                }
            }
            return null;
        }
        public bool TraNhiemVu(string input)
        {
            bool ketqua = false;
            try
            {
                IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
                IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

                Rect location = new Rect();
                GetWindowRect(hWnd, ref location);

                bool batthanhcong = false;
                Process proc = Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LoginVLTK_2.exe");
                DateTime start = DateTime.Now;
                do
                {
                    double diff = (DateTime.Now - start).TotalSeconds;
                    if (diff > 15)
                    {
                        proc.Kill();
                        return false;
                    }
                    AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                    Thread.Sleep(2000);
                    Process[] procs = Process.GetProcessesByName("vggame");
                    if (this.Process != null)
                    {
                        if (SystemMethod.IsWindowVisible(this.Process.MainWindowHandle))
                        {
                            batthanhcong = true;
                            break;
                        }
                    }
                    //else
                    //{
                    //    foreach (Process item in procs)
                    //    {
                    //        if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                    //        {
                    //            batthanhcong = true;
                    //            if (this.Process == null)
                    //            {
                    //                this.Process = item;
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}

                } while (!batthanhcong);

                AutoControl.BringToFront(proc.MainWindowHandle);
                Thread.Sleep(1000);

                AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[3], input);//user

                Thread.Sleep(1000);

                AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc.MainWindowHandle)[0]);

                proc.WaitForExit();
                if ((DateTime.Now - start).TotalSeconds < 60)
                {
                    ketqua = true;
                }
                AutoControl.BringToFront(hWnd_main);
                Thread.Sleep(4000);

                bool tatthanhcong = false;
                start = DateTime.Now;
                do
                {
                    double diff = (DateTime.Now - start).TotalSeconds;
                    if (diff > 15)
                    {
                        return false;
                    }
                    AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                    Thread.Sleep(2000);
                    if (!SystemMethod.IsWindowVisible(this.Process.MainWindowHandle))
                    {
                        tatthanhcong = true;
                        break;
                    }
                } while (!tatthanhcong);
                //AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
            }
            return ketqua;
        }
        public void BoMatChiVuLan(Excel.Worksheet oSheet, string input)
        {
            this.LastHuyMatChi = DateTime.Now;
            int sleep = 1000;
            IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
            Rect location = new Rect();
            GetWindowRect(hWnd_main, ref location);

            AutoControl.BringToFront(hWnd_main);
            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 6 + this.clickPoint.X, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click vo ten nv
            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 80, location.Top + 180, EMouseKey.LEFT);//click vao tab da tau
            Thread.Sleep(sleep);
            if (input == "kinhnghiem")
            {
                AutoControl.MouseClick(location.Left + 80, location.Top + 305, EMouseKey.LEFT);//click chon nang diem kinh nghiem
            }
            if (input == "matchi")
            {
                AutoControl.MouseClick(location.Left + 80, location.Top + 290, EMouseKey.LEFT);//click chon tim do chi
            }

            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 165, location.Top + 230, EMouseKey.LEFT);//click drop box
            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 165, location.Top + 260, EMouseKey.LEFT);//click chon huy
            Thread.Sleep(sleep);
            //AutoControl.MouseClick(location.Left + 15, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click tat auto
            //Thread.Sleep(sleep * 5);
            //AutoControl.MouseClick(location.Left + 15, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click bat auto
            TryRefresh(oSheet,true);
            Thread.Sleep(5 * sleep);
            AutoControl.MouseClick(location.Left + 165, location.Top + 230, EMouseKey.LEFT);//click drop box
            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 165, location.Top + 245, EMouseKey.LEFT);//click chon auto
            Thread.Sleep(sleep);
            AutoControl.MouseClick(location.Left + 30, location.Top + 180, EMouseKey.LEFT);//click dieu khien
        }
        public void BoMatChiVAuto()
        {
            AutoControl.BringToFront(this.ProcessAuto.MainWindowHandle);
            Rect location_vulan = new Rect();
            GetWindowRect(this.ProcessAuto.MainWindowHandle, ref location_vulan);
            bool batthanhcong = false;
            do
            {
                AutoControl.MouseClick(location_vulan.Left + 15 + this.clickPoint.X, location_vulan.Top + 29 + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(2000);
                Process[] procs = Process.GetProcessesByName("vggame");
                foreach (Process item in procs)
                {
                    if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                    {
                        batthanhcong = true;
                        if (this.Process == null)
                        {
                            this.Process = item;
                        }
                        break;
                    }
                }
            } while (!batthanhcong);

            int sleep = 1000;

            #region getvlauto
            //Process[] list = Process.GetProcessesByName("VLAutoPr");
            //Process proc_vlauto;
            //if (list.Count() == 0)
            //{
            //    proc_vlauto = Process.Start(@"D:\Games\VoLam_Update\_VLAutoDaTau-03-06-2021\VLAutoPr.exe");
            //}
            //else
            //{
            //    proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            //}
            //IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, proc_vlauto.Id);
            #endregion
            IntPtr hWnd_main = AutoControl.FindWindowHandle(null, "VLAuto 8.4");

            if (hWnd_main != IntPtr.Zero)
            {
                //List<Point> l_point = new List<Point>();
                //Point p1 = new Point(55, 35);
                //Point p2 = new Point(55, 55);
                //Point p3 = new Point(55, 70);
                //Point p4 = new Point(55, 85);
                //Point p5 = new Point(55, 100);
                //l_point.Add(p1);
                //l_point.Add(p2);
                //l_point.Add(p3);
                //l_point.Add(p4);
                //l_point.Add(p5);

                Rect location = new Rect();
                GetWindowRect(hWnd_main, ref location);

                AutoControl.BringToFront(hWnd_main);
                //Thread.Sleep(sleep);
                //bool timduocpoint = false;
                //foreach (Point p_item in l_point)//click vo ten nv
                //{
                //    AutoControl.MouseClick(location.Left + 16 + p_item.X, location.Top + 39 + p_item.Y, EMouseKey.LEFT);//click vo ten nv
                //    Thread.Sleep(sleep);

                //    byte[] buffer = new byte[4];
                //    int bytesread;
                //    SystemMethod.ReadProcessMemory(processHandle, IntPtr.Add(proc_vlauto.MainModule.BaseAddress, 0x1F0F50), buffer, 4, out bytesread);
                //    IntPtr P_main = new IntPtr(BitConverter.ToInt32(buffer, 0));
                //    string name = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(P_main, 24));
                //    if (name==this.Name)
                //    {
                //        timduocpoint = true;
                //        break;
                //    }
                //    if (this.Name== "B��Gi�"&&name== "BèÙGiµ")
                //    {
                //        timduocpoint = true;
                //        break;
                //    }
                //}
                //if (!timduocpoint)
                //{
                //    this.QuitGame();
                //    return;
                //}
                AutoControl.MouseClick(location.Left + 16 + this.clickPoint.X, location.Top + 39 + this.ClickPoint.Y, EMouseKey.LEFT);//click vo ten nv
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 125, location.Top + 190, EMouseKey.LEFT);//click vao tab nhiem vu
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 175, location.Top + 475, EMouseKey.LEFT);//click mo rong
                Thread.Sleep(5 * sleep);
                hWnd_main = AutoControl.FindWindowHandle(null, "VLAuto 8.4");
                if (hWnd_main == IntPtr.Zero)
                {
                    foreach (Process process in Process.GetProcessesByName("vggame"))//thoat het game
                    {
                        process.Kill();
                    }
                    //AutoControl.MouseClick(location_vulan.Left + 15 + this.clickPoint.X, location_vulan.Top + 29 + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);// hide cua so
                    return;
                }
                AutoControl.MouseClick(location.Left + 28, location.Top + 128, EMouseKey.LEFT);//click do/matchi
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 190, location.Top + 375, EMouseKey.LEFT);//click o thoi gian
                Thread.Sleep(sleep);
                AutoControl.SendKeyPress(KeyCode.BACKSPACE);//xoa so 5
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 105, location.Top + 448, EMouseKey.LEFT);//click OK
                Thread.Sleep(3 * sleep);

                AutoControl.MouseClick(location.Left + 175, location.Top + 475, EMouseKey.LEFT);//click mo rong
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 28, location.Top + 128, EMouseKey.LEFT);//click do/matchi
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 190, location.Top + 375, EMouseKey.LEFT);//click o thoi gian
                Thread.Sleep(sleep);
                AutoControl.HuySendText("5");//dien so 5
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 105, location.Top + 448, EMouseKey.LEFT);//click OK
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 50, location.Top + 193, EMouseKey.LEFT);//click co ban
                                                                                               //   do
                                                                                               //  {
                if (!this.IsCurrentInTown())
                {
                    AutoControl.MouseClick(location_vulan.Left + 19, location_vulan.Top + 244, EMouseKey.LEFT);//click bat nhiemvu datau
                    Thread.Sleep(sleep);
                    AutoControl.MouseClick(location_vulan.Left + 15, location_vulan.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//tat auto
                    Thread.Sleep(sleep);
                    AutoControl.MouseClick(location_vulan.Left + 15, location_vulan.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//bat auto
                    Thread.Sleep(sleep);
                    AutoControl.MouseClick(location_vulan.Left + 19, location_vulan.Top + 244, EMouseKey.LEFT);//click tat nhiemvu datau
                    Thread.Sleep(5 * sleep);
                    //tat bat lai vauto
                    //AutoControl.MouseClick(location.Left + 25, location.Top + 39 + this.ClickPoint.Y, EMouseKey.LEFT);//tat vauto
                    //Thread.Sleep(sleep);
                    //AutoControl.MouseClick(location.Left + 25, location.Top + 39 + this.ClickPoint.Y, EMouseKey.LEFT);//bat vauto
                    //            
                    // return;
                    this.QuitGame();
                }
                else//check end
                {
                    DateTime start = DateTime.Now;
                    double remain = 40;
                    Point? point = null;
                    Bitmap box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\KetThucNhiemVu.PNG");
                    do
                    {
                        remain = 40 - (DateTime.Now - start).TotalSeconds;
                        Bitmap volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                        point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                        if (remain < 0)
                        {
                            break;
                        }
                    } while (point == null);

                    if (point != null)
                    {
                        //  this.ForceQuit = true;
                    }
                }
                AutoControl.MouseClick(location_vulan.Left + 15 + this.clickPoint.X, location_vulan.Top + 29 + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                //  } while (!this.IsCurrentInTown());
            }
        }
        public bool CheckToaDo()
        {
            switch (this.Map)
            {
                case "BiÖn Kinh ":
                    if (SoSanhToaDo(this.Toado, "216,193", 1))
                    {
                        return true;
                    }
                    break;
                case "Ph­îng T­êng ":
                    if (SoSanhToaDo(this.Toado, "202,193", 1))
                    {
                        return true;
                    }
                    break;
                case "§¹i Lý phñ ":
                    if (SoSanhToaDo(this.Toado, "206,201", 1))
                    {
                        return true;
                    }
                    break;
                case "Thµnh §« ":
                    if (SoSanhToaDo(this.Toado, "394,316", 1))
                    {
                        return true;
                    }
                    break;
                case "T­¬ng D­¬ng ":
                    if (SoSanhToaDo(this.Toado, "199,205", 1))
                    {
                        return true;
                    }
                    break;
                case "D­¬ng Ch©u ":
                    if (SoSanhToaDo(this.Toado, "218,185", 1))
                    {
                        return true;
                    }
                    break;
                case "L©m An ":
                    if (SoSanhToaDo(this.Toado, "195,186", 1))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        internal void TryRefresh(Excel.Worksheet oSheet, bool refesh)
        {
            this.lastTryRefresh = DateTime.Now;

            //string rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 4].Value2);
            //DateTime date = DateTime.MinValue;
            //if (rowData != null)
            //{
            //    DateTime.TryParse(rowData.Split('\n').Last(), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
            //}

            int tuvong = 0;
            string rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 18].Value2);
            if (rowData != null)
            {
                int.TryParse(rowData, out tuvong);
            }
            oSheet.Cells[this.RowExcel, 18] = this.TuVong + tuvong;


            int thunhap = 0;
            rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 16].Value2);
            if (rowData != null)
            {
                int.TryParse(rowData, out thunhap);
            }
            oSheet.Cells[this.RowExcel, 16] = this.ThuNhap + thunhap;

            double tongthu = 0;
            rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 20].Value2);
            if (rowData != null)
            {
                double.TryParse(rowData, out tongthu);
            }
            oSheet.Cells[this.RowExcel, 20] = (double)this.ThuNhap / 10000 + tongthu;

            int shxt = 0;
            rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 19].Value2);
            if (rowData != null)
            {
                int.TryParse(rowData, out shxt);
            }
            shxt = shxt - int.Parse(this.Datau.Split('/')[1]);
            oSheet.Cells[this.RowExcel, 19] = shxt;
            if (shxt < 50)
            {
                //to mau do
                //  oSheet.Range[this.RowExcel].EntireRow.Interior.Color = System.Drawing.Color.Red;
                oSheet.Rows[this.RowExcel].Interior.Color = System.Drawing.Color.OrangeRed;
            }
            else if (shxt < 100)
            {
                //to mau cam
                // oSheet.Range[this.RowExcel].EntireRow.Interior.Color = System.Drawing.Color.Orange;
                oSheet.Rows[this.RowExcel].Interior.Color = System.Drawing.Color.Orange;
            }



            int nhiemvu = 0;
            int nhiemvu2 = 0;
            rowData = Convert.ToString(oSheet.Cells[this.RowExcel, 17].Value2);
            if (rowData != null)
            {
                int.TryParse(rowData.Split('=').First(), out nhiemvu);
                int.TryParse(rowData.Split('=').Last(), out nhiemvu2);
            }
            oSheet.Cells[this.RowExcel, 17] = (int.Parse(this.Datau.Split('/')[1]) + nhiemvu).ToString() + "=" + (int.Parse(this.Datau.Split('/')[2]) + nhiemvu2).ToString();
            try
            {
                string dt_begin = (Convert.ToString(oSheet.Cells[this.RowExcel, 9].Value2));
                int bg = int.Parse(dt_begin.Split('/')[3].Split(' ').First());
                int end = int.Parse(this.Datau.Split('/')[3].Split(' ').First());
                oSheet.Cells[this.RowExcel, 15] = (end - bg) + int.Parse(this.Datau.Split('/')[1]) + nhiemvu + int.Parse(this.Datau.Split('/')[2]) + nhiemvu2 + int.Parse(dt_begin.Split('/')[0]) + int.Parse(dt_begin.Split('/')[1]) + int.Parse(dt_begin.Split('/')[2]);
            }
            catch (Exception)
            { }
            oSheet.Cells[this.RowExcel, 10] = this.Datau;
            //DateTime.TryParse(rowData, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
            //if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            //{
            //    oSheet.Cells[this.RowExcel, 16] = this.ThuNhap + thunhap;
            //}

            if (refesh)
            {
                int sleep = 1000;
                IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
                Rect location = new Rect();
                GetWindowRect(hWnd_main, ref location);

                AutoControl.BringToFront(hWnd_main);
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 6 + this.clickPoint.X, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click vo ten nv
                Thread.Sleep(sleep);
                AutoControl.MouseClick(location.Left + 15, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click tat auto
                Thread.Sleep(sleep * 5);
                AutoControl.MouseClick(location.Left + 15, location.Top + 29 + this.ClickPoint.Y, EMouseKey.LEFT);//click bat auto
                Thread.Sleep(sleep);
            }
        }
        internal bool CheckTypeOnline(List<NPC> online, int chophep)
        {
            //Get sl da online
            int daOnline = 0;
            foreach (NPC item in online)
            {
                if (this.TypeNPC == item.TypeNPC)
                {
                    daOnline++;
                }
            }
            //Check Sl<soluongchophep
            if (daOnline < chophep)
            {
                return true;
            }
            return false;
        }
        internal bool CheckNhanChu(string input, int thoigian)
        {
            IntPtr hWnd_main = this.ProcessAuto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

            Rect location = new Rect();
            GetWindowRect(hWnd, ref location);

            bool batthanhcong = false;
            DateTime start = DateTime.Now;
            do
            {
                double diff = (DateTime.Now - start).TotalSeconds;
                if (diff > 15)
                {
                    return false;
                }

                AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(2000);
                Process[] procs = Process.GetProcessesByName("vggame");
                if (this.Process != null)
                {
                    if (SystemMethod.IsWindowVisible(this.Process.MainWindowHandle))
                    {
                        batthanhcong = true;
                        break;
                    }
                }
                //else
                //{
                //    foreach (Process item in procs)
                //    {
                //        if (SystemMethod.IsWindowVisible(item.MainWindowHandle))
                //        {
                //            batthanhcong = true;
                //            if (this.Process == null)
                //            {
                //                this.Process = item;
                //            }
                //            break;
                //        }
                //    }
                //}
            } while (!batthanhcong);

            if (input == "CheckLag")
            {
                Bitmap volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                Bitmap box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\HanhTrangDaMo.PNG");
                Point? point_check = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
                if (point_check == null)//check co mo ruong chua=>Chua mo
                {
                    //mo ruong len
                    AutoControl.MouseClick(500, 600, EMouseKey.LEFT);
                    Thread.Sleep(500);
                }
                AutoControl.MouseClick(760, 380, EMouseKey.RIGHT);//click THP
                Thread.Sleep(1000);
            }

            start = DateTime.Now;
            Point? point = null;
            do
            {
                double diff = (DateTime.Now - start).TotalSeconds;
                if (diff > thoigian)
                {
                    break;
                }
                Bitmap volam = CaptureHelper.CaptureImage(new Size(820, 640), new Point(0, 0));
                Bitmap box = null;
                if (input == "NhanCu")
                {
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\NhanCu.PNG");
                }
                if (input == "DoiThoaiDaTau")
                {
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\DoiThoaiDaTau1.PNG");
                }
                if (input == "CheckLag")
                {
                    box = ImageScanOpenCV.GetImage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\CheckTHPclicked.PNG");
                }
                point = ImageScanOpenCV.FindOutPoint(volam, box, 0.8);
            } while (point == null);

            if (point != null && input == "CheckLag")
            {
                Thread.Sleep(1000);
                AutoControl.MouseClick(250, 360, EMouseKey.RIGHT);//click tat THP
            }
            bool tatthanhcong = false;
            start = DateTime.Now;
            do
            {
                double diff = (DateTime.Now - start).TotalSeconds;
                if (diff > 15)
                {
                    return false;
                }
                AutoControl.MouseClick(location.Left + this.clickPoint.X, location.Top + this.ClickPoint.Y, EMouseKey.DOUBLE_LEFT);
                Thread.Sleep(2000);
                if (!SystemMethod.IsWindowVisible(this.Process.MainWindowHandle))
                {
                    tatthanhcong = true;
                    break;
                }
            } while (!tatthanhcong);

            if (point != null)
            {
                return true;
            }
            return false;
        }


        //[DllImport("kernel32.dll")]
        //public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        //[DllImport("kernel32.dll")]
        //static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);


    }
    public class SystemMethod
    {
        private static SystemMethod instance;

        public static SystemMethod Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemMethod();
                }
                return SystemMethod.instance;
            }
            private set => instance = value;
        }

        private SystemMethod() { }
        #region System method
        public const int PROCESS_WM_READ = 0x0010;
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        #endregion

        #region My Method
        public string ReadMemoryToString(IntPtr processHandle, IntPtr address)
        {
            int bytesRead = 0;

            byte[] buffer = new byte[24]; //To read a 24 byte unicode string

            SystemMethod.ReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead);

            //MessageBox.Show(Encoding.Unicode.GetString(buffer) +
            //      " (" + bytesRead.ToString() + "bytes)");
            return Encoding.UTF8.GetString(buffer).Split('\0').First();
        }
        public static int GetTextBoxTextLength(IntPtr hTextBox)
        {
            // helper for GetTextBoxText
            uint WM_GETTEXTLENGTH = 0x000E;
            int result = SendMessage4(hTextBox, WM_GETTEXTLENGTH,
              0, 0);
            return result;
        }

        public static string GetTextBoxText(IntPtr hTextBox)
        {
            uint WM_GETTEXT = 0x000D;
            int len = GetTextBoxTextLength(hTextBox);
            if (len <= 0) return null;  // no text
            StringBuilder sb = new StringBuilder(len + 1);
            SendMessage3(hTextBox, WM_GETTEXT, len + 1, sb);
            return sb.ToString();
        }
        public string GetWindowCaption(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowCaption(hwnd, sb, 256);
            return sb.ToString();
        }
        #endregion

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowCaption(IntPtr hwnd, StringBuilder lpString, int maxCount);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage3(IntPtr hwndControl, uint Msg, int wParam, StringBuilder strBuffer); // get text

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage4(IntPtr hwndControl, uint Msg, int wParam, int lParam);  // text length

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        public int FindLocationChildHandle(IntPtr hwnd, string nameChild)
        {
            List<IntPtr> children = AutoControl.GetChildHandle(hwnd);
            for (int i = 0; i < children.Count; i++)
            {
                if (GetTextBoxText(children[i]) == nameChild)
                {
                    return i;
                }
            }
            return 0;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
    }
}
