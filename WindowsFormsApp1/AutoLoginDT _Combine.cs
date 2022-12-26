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

    public partial class AutoLoginDT_Combine : Form
    {
        List<Point> l_point = new List<Point>();
        Point p1 = new Point(55, 35);
        Point p2 = new Point(55, 55);
        Point p3 = new Point(55, 70);
        Point p4 = new Point(55, 85);
        Point p5 = new Point(55, 100);
        bool stop = false;
        public AutoLoginDT_Combine()
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
            stop = false;
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
                IntPtr hWnd = AutoControl.FindWindowHandle(null, "VLAuto 8.4");
                if (hWnd == IntPtr.Zero)
                {
                    Process proc_vlauto = Process.Start(@"D:\Games\VoLam_Update\_VLAutoDaTau-03-06-2021\VLAutoPr.exe");
                    Thread.Sleep(5000);
                    try
                    {
                        hWnd = proc_vlauto.MainWindowHandle;
                        AutoControl.BringToFront(hWnd);
                        Thread.Sleep(1000);
                        hWnd = AutoControl.GetChildHandle(hWnd).First();
                        AutoControl.SendClickOnPositionByPost(hWnd, 5, 35, EMouseKey.LEFT);
                        Thread.Sleep(1000);
                        AutoControl.SendClickOnPositionByPost(hWnd, 5, 55, EMouseKey.LEFT);
                        Thread.Sleep(1000);
                        AutoControl.SendClickOnPositionByPost(hWnd, 5, 70, EMouseKey.LEFT);
                        Thread.Sleep(1000);
                        AutoControl.SendClickOnPositionByPost(hWnd, 5, 85, EMouseKey.LEFT);
                    }
                    catch (Exception)
                    {
                    }

                }
                backgroundWorker1.ReportProgress(1, "Calculate Acc logged");
                try
                {
                    LoadListNPCOnline(ref online);
                }
                catch (Exception)
                {
                }

                UpdateWishList(online, ref wishlist, oSheet);

                List<NPC> l_remove = new List<NPC>();
                foreach (NPC item in wishlist)
                {
                    backgroundWorker1.ReportProgress(1, "Consider acc " + item.Name);
                    if (!item.CheckOnline(online, oSheet))
                    {
                        if (online.Count < this.numericUpDown1.Value)
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
                            break;
                        }
                    }
                    else
                    {
                        bool quitgame = false;
                        switch (item.CheckStatusVulan(oSheet))
                        {
                            case NPC.Status.DangLamNhiemVu:
                                // backgroundWorker1.ReportProgress(1, "Everything is OK!");
                                backgroundWorker1.ReportProgress(1, item.Report());
                                Thread.Sleep(1000);
                                break;
                            case NPC.Status.DungIm:
                                // Refresh game();
                                backgroundWorker1.ReportProgress(1, item.Name + " is Idle!");
                                //  item.QuitGame();
                                Thread.Sleep(1000);
                                break;
                            case NPC.Status.XongNhiemVu:
                                // backgroundWorker1.ReportProgress(1, item.Name + " is Finish");
                                oSheet.Cells[item.RowExcel, 5] = DateTime.Now.ToString();
                                backgroundWorker1.ReportProgress(1, item.Name + " is quitting game");

                                if (!item.CheckToaDo())
                                {
                                    oSheet.Cells[item.RowExcel, 7] = "KIEM TRA LAI SAI TOA DO";
                                }
                                else
                                {
                                    oSheet.Cells[item.RowExcel, 8] = item.Map + " - " + item.Toado;
                                    oSheet.Cells[item.RowExcel, 10] = item.Datau;
                                    oSheet.Cells[item.RowExcel, 11] = item.Nhiemvu;
                                }
                                oSheet.Cells[item.RowExcel, 7] = item.Test_note;
                                oSheet.Cells[item.RowExcel, 12] = item.Kinhnghiem;
                                DateTime date = DateTime.MinValue;
                                string date_text = Convert.ToString(oSheet.Cells[item.RowExcel, 4].Value2);
                                if (date_text != null)
                                {
                                    if (date_text.Contains('\n'))
                                    {
                                        DateTime.TryParse(date_text.Split('\n').First(), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                                    }
                                    else
                                    {
                                        DateTime.TryParse(date_text, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                                    }

                                    if (date > DateTime.MinValue)
                                    {
                                        oSheet.Cells[item.RowExcel, 13] = (DateTime.Now - date).TotalMinutes;
                                    }
                                }
                                item.QuitGame();
                                Thread.Sleep(5000);
                                quitgame = true;
                                l_remove.Add(item);
                                break;
                            case NPC.Status.DangLamMatChi:
                                backgroundWorker1.ReportProgress(1, item.Name + " dang lam mat chi!");
                                continue;
                                if (item.IsCurrentInTown())
                                {
                                    continue;
                                }
                                DateTime datethoat = DateTime.MinValue;
                                date_text = Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2);
                                if (date_text != null)
                                {
                                    if (date_text.Contains('\n'))
                                    {
                                        DateTime.TryParse(date_text.Split('\n').Last(), CultureInfo.CurrentCulture, DateTimeStyles.None, out datethoat);
                                    }
                                    else
                                    {
                                        DateTime.TryParse(date_text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datethoat);
                                    }

                                    if (datethoat > DateTime.MinValue)
                                    {
                                        if ((DateTime.Now - datethoat).TotalSeconds < 90)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                string matchi = "";
                                matchi = Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2);
                                if (matchi == "" || matchi == null)
                                {
                                    oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                }
                                else
                                {
                                    oSheet.Cells[item.RowExcel, 6] = matchi + "\n" + DateTime.Now.ToString();
                                }
                                matchi = Convert.ToString(oSheet.Cells[item.RowExcel, 10].Value2);
                                oSheet.Cells[item.RowExcel, 10] = matchi + "\n" + item.Datau;
                                item.BoMatChiVAuto();
                                break;
                            case NPC.Status.KetTrongThanh:
                                oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                oSheet.Cells[item.RowExcel, 7] = "KET TRONG THANH";
                                //     item.QuitGame();
                                // l_remove.Add(item);
                                break;
                            case NPC.Status.TraNhiemVu:
                                //backgroundWorker1.ReportProgress(1, item.Name + " Tra Nhiem Vu");
                                //item.TraNhiemVu();
                                break;
                            default:
                                break;
                        }
                        if (quitgame)
                        {
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
                if (online.Count < this.numericUpDown1.Value)
                {
                    LoadNPCWishList(oSheet, ref wishlist);
                }
                //  Thread.Sleep(5000);
            } while (!stop);
            backgroundWorker1.ReportProgress(1, "Stop!");
        }

        private void UpdateWishList(List<NPC> online, ref List<NPC> wishlist, Excel.Worksheet sheet)
        {
            foreach (NPC item in online)
            {
                try
                {
                    var itemtoremove = (wishlist.Single(r => r.Name == item.Name));
                }
                catch (Exception)
                {
                    int row = FindRowExcel(sheet, item.Name);
                    item.RowExcel = row;
                    wishlist.Add(item);
                }
            }
        }

        private int FindRowExcel(Excel.Worksheet worksheet, string name)
        {
            //int rowStart = worksheet.Dimension.Start.Row;
            //int rowEnd = worksheet.Dimension.End.Row;

            //string cellRange = rowStart.ToString() + ":" + rowEnd.ToString();

            //var searchCell = from cell in worksheet.Cells[cellRange] //you can define your own range of cells for lookup
            //                 where cell.Value.ToString() == "Total"
            //                 select cell.Start.Row;
            //var searchCell = from cell in worksheet.UsedRange //you can define your own range of cells for lookup
            //                 where cell.Value.ToString() == "Total"
            //                 select cell.Start.Row;
            //int rowNum = searchCell.First();

            foreach (Excel.Range item in worksheet.UsedRange.Cells)
            {
                if (Convert.ToString(item.Value2) == name)
                {
                    string row = item.Address;
                    return int.Parse(row.Split('$').Last());
                }
            }
            return 30;
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
                //string tamthoat = Convert.ToString(row.Cells[1, 6].Value2);
                //if (tamthoat != null)
                //{
                //    continue;
                //}
                string name = Convert.ToString(row.Cells[1, 3].Value2);
                if (name == "")
                {
                    continue;
                }
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
                    string user = Convert.ToString(row.Cells[1, 1].Value2);
                    if (user == "")
                    {
                        continue;
                    }
                    NPC npc = new NPC(Convert.ToString(row.Cells[1, 3].Value2), user, Convert.ToString(row.Cells[1, 2].Value2), "", "");
                    npc.RowExcel = row.Row;
                    DateTime.TryParse(Convert.ToString(row.Cells[1, 4].Value2), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                    if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {
                        osheet.Cells[npc.RowExcel, 4] = "";
                        osheet.Cells[npc.RowExcel, 9] = "";
                    }
                    wishlist.Add(npc);
                }
                nextrow:;
            }
            //wishlist.Add(new NPC("huydb20", "huydaibang20", "1233041990", "", ""));
        }
        private void LoadListNPCOnline(ref List<NPC> l_NPC_online)
        {
            int sleep = 2000;
            l_NPC_online.Clear();

            Process proc_vlauto = GetVLAuto();

            //  Process proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            IntPtr hWnd_main = proc_vlauto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

            List<string> name_online = LoadListName();
            // IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, proc_vlauto.Id);

            foreach (string name in name_online)
            {
                int i = name_online.IndexOf(name);
                AutoControl.SendClickOnPositionByPost(hWnd, l_point[i].X, l_point[i].Y, EMouseKey.LEFT);
                Thread.Sleep(sleep);
                NPC npc = new NPC(hWnd_main, name);
                npc.ClickPoint = l_point[i];
                npc.ProcessAuto = proc_vlauto;
                if (npc.Name != "")
                {
                    l_NPC_online.Add(npc);
                }
            }

            #region old methold
            //AutoControl.SendClickOnPositionByPost(hWnd, p1.X, p1.Y, EMouseKey.LEFT);
            //Thread.Sleep(sleep);
            //NPC npc = new NPC(hWnd_main, "1");
            //npc.ClickPoint = p1;
            //npc.Process = proc_vlauto;
            //if (npc.Name != "")
            //{
            //    l_NPC_online.Add(npc);
            //}


            //AutoControl.SendClickOnPositionByPost(hWnd, p2.X, p2.Y, EMouseKey.LEFT);
            //Thread.Sleep(sleep);
            //npc = new NPC(hWnd_main, "2");
            //npc.ClickPoint = p2;
            //npc.Process = proc_vlauto;
            //if (npc.Name != "")
            //{
            //    l_NPC_online.Add(npc);
            //}

            //AutoControl.SendClickOnPositionByPost(hWnd, p3.X, p3.Y, EMouseKey.LEFT);
            //Thread.Sleep(sleep);
            //npc = new NPC(hWnd_main, "3");
            //npc.ClickPoint = p3;
            //npc.Process = proc_vlauto;
            //if (npc.Name != "")
            //{
            //    l_NPC_online.Add(npc);
            //}

            //AutoControl.SendClickOnPositionByPost(hWnd, p4.X, p4.Y, EMouseKey.LEFT);
            //Thread.Sleep(sleep);
            //npc = new NPC(hWnd_main, "4");
            //npc.ClickPoint = p4;
            //npc.Process = proc_vlauto;
            //if (npc.Name != "")
            //{
            //    l_NPC_online.Add(npc);
            //}

            //AutoControl.SendClickOnPositionByPost(hWnd, p5.X, p5.Y, EMouseKey.LEFT);
            //Thread.Sleep(sleep);
            //npc = new NPC(hWnd_main, "5");
            //npc.ClickPoint = p5;
            //npc.Process = proc_vlauto;
            //if (npc.Name != "")
            //{
            //    l_NPC_online.Add(npc);
            //}
            #endregion
        }

        private List<string> LoadListName()
        {
            List<string> kq = new List<string>();
            List<Process> list = Process.GetProcessesByName("vggame").ToList();
            foreach (Process item in list.OrderByDescending(e => e.StartTime))
            {
                IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, item.Id);
                string name = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(item.MainModule.BaseAddress, 0x2DEC44));
                if (name != "")
                {
                    kq.Add(name);
                }
                else
                {
                    item.Kill();
                }
            }
            return kq;
        }

        private Process LoginVLKT(string user, string pass)
        {
            string source = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\huydbchuan.cfg";
            string des = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\huydb" + user.Substring(10) + ".cfg";
            if (user == "ahthieulam")
            {
                // source = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\huydb14_chuan.cfg";
                des = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\B-24-39Gi-75.cfg";
            }
            //if (user == "huydaibang14")
            //{
            //    source = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\huydb14_chuan.cfg";
            //    des = @"D:\Games\VoLam_Update\_Auto Train Moi Nhat 2021\0.AutoVLBS19_Free_VinhVien\vlbs1.9_volampk.net\Auto\Auto\UserData\huydb14.cfg";
            //}
            File.Copy(source, des, true);
            Thread.Sleep(500);
            string chuan_name = "ChuanDT_15";
            string vl_name = "";
            switch (user)
            {
                case "huydaibang01":
                    vl_name = "fdd047edcl758f813bc";
                    break;
                case "huydaibang02":
                    vl_name = "fdd046b1cl758f81043";
                    break;
                case "huydaibang03":
                    vl_name = "fdd05985cl758f810ca";
                    break;
                case "huydaibang04":
                    vl_name = "fdd05949cl758f81151";
                    break;
                case "huydaibang05":
                    vl_name = "fdd0581dcl758f811d8";
                    break;
                case "huydaibang06":
                    vl_name = "fdd05be1cl758f81e6f";
                    break;
                case "huydaibang07":
                    vl_name = "fdd05ab5cl758f81ef6";
                    break;
                case "huydaibang08":
                    vl_name = "fdd05a79cl758f81f7d";
                    break;
                case "huydaibang09":
                    vl_name = "fdd05d4dcl758f81f04";
                    break;
                case "huydaibang10":
                    vl_name = "fdd048b4cl758f868ff";
                    break;
                case "huydaibang11":
                    vl_name = "fdd04878cl758f86886";
                    break;
                case "huydaibang12":
                    vl_name = "fdd04b4ccl758f8690d";
                    break;
                case "huydaibang13":
                    vl_name = "fdd04a10cl758f86994";
                    chuan_name = "ChuanDT_14";
                    break;
                case "huydaibang14":
                    vl_name = "fdd04de4cl758f8161b";
                    chuan_name = "ChuanDT_14";
                    break;
                case "huydaibang15":
                    vl_name = "fdd04ca8cl758f816a2";
                    break;
                case "huydaibang16":
                    vl_name = "fdd04c7ccl758f81729";
                    break;
                case "huydaibang17":
                    vl_name = "fdd04f40cl758f817b0";
                    break;
                case "huydaibang18":
                    vl_name = "fdd04e14cl758f81447";
                    break;
                case "huydaibang19":
                    vl_name = "fdd041d8cl758f814ce";
                    break;
                case "huydaibang20":
                    vl_name = "fdd07d4fcl758f861b9";
                    break;
                case "ahthieulam":
                    vl_name = "cdb83129al6ef62b4c";
                    chuan_name = "ChuanDT_14";
                    break;
                default:
                    break;
            }
            source = @"D:\Games\VoLam_Update\_VLAutoDaTau-03-06-2021\UiConfig\" + chuan_name + ".cfg";
            des = @"D:\Games\VoLam_Update\_VLAutoDaTau-03-06-2021\UiConfig\" + vl_name + ".cfg";
            File.Copy(source, des, true);
            Thread.Sleep(500);
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
            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[3], user);//user
            Thread.Sleep(500);
            AutoControl.SendText(AutoControl.GetChildHandle(proc.MainWindowHandle)[2], pass);//pass
            Thread.Sleep(500);
            AutoControl.SendClickOnControlByHandle(AutoControl.GetChildHandle(proc.MainWindowHandle)[1]);
            proc.WaitForExit();

            IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, kq.Id);
            string name_loged = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(kq.MainModule.BaseAddress, 0x2DEC44));

            if (name_loged == "")
            {
                kq.Kill();
                return null;
            }
            return kq;
        }

        private Process GetVLAuto()
        {
            Process[] list = Process.GetProcessesByName("AutoVLBS");
            Process proc_vlauto = null;
            if (list.Count() == 0)
            {
                //proc_vlauto = Process.Start(@"D:\Games\[DaiLamAn_VoCongTruyenKy]_FULL_MatKhau_123456\1.Auto Luyen Cong\_VL_AutoPRO_88_ChuyenDaTau\VLAutoPr.exe");
            }
            else if (list.Count() == 1)
            {
                proc_vlauto = list.First();
            }
            else
            {
                DateTime last = DateTime.MinValue;
                foreach (Process item in list)
                {
                    if (item.StartTime > last)
                    {
                        if (proc_vlauto != null)
                        {
                            proc_vlauto.Kill();
                        }
                        proc_vlauto = item;
                        last = item.StartTime;
                    }
                    else
                    {
                        item.Kill();
                    }
                }
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
            Process vlauto = GetVLAuto();
            List<IntPtr> handles = AutoControl.GetChildHandle(vlauto.MainWindowHandle);
            for (int i = 0; i < handles.Count; i++)
            {
                if (SystemMethod.GetTextBoxText(handles[i]) == "4.08%")
                {
                    MessageBox.Show(i.ToString());
                    break;
                }
            }
        }
    }
}
