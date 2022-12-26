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

    public partial class AutoLoginDT_VuLan : Form
    {
        List<Point> l_point = new List<Point>();
        Point p1 = new Point(55, 35);
        Point p2 = new Point(55, 55);
        Point p3 = new Point(55, 70);
        Point p4 = new Point(55, 85);
        Point p5 = new Point(55, 100);
        bool stop = false;
        public AutoLoginDT_VuLan()
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

            List<NPC> wishlist = new List<NPC>();
            backgroundWorker1.ReportProgress(1, "Load List Acc to login");
            LoadNPCWishList(oSheet, ref wishlist);
            List<NPC> online = new List<NPC>();
            do
            {
                backgroundWorker1.ReportProgress(1, "Calculate Acc logged");
            loadlai:;
                try
                {
                    //LoadListNPCOnline(ref online, oSheet);
                    if (!LoadListNPCOnline(ref online, oSheet))
                    {
                        Thread.Sleep(10000);
                        goto loadlai;
                    }
                }
                catch (Exception)
                {
                    goto loadlai;
                }

                UpdateWishList(online, ref wishlist, oSheet);//=>De chay cac User minh tu load

                List<NPC> l_remove = new List<NPC>();
                foreach (NPC item in wishlist)
                {
                    backgroundWorker1.ReportProgress(1, "Consider acc " + item.Name);
                    if (item.User == "" || item.User == null)
                    {
                        continue;
                    }
                    if (!item.CheckOnline(online, oSheet))
                    {
                        if (online.Count < this.numericUpDown1.Value)
                        {
                            if (item.CheckTypeOnline(online, 2))
                            {
                                backgroundWorker1.ReportProgress(1, "Logging acc " + item.Name);
                                string auto = Convert.ToString(oSheet.Cells[item.RowExcel, 13].Value2);
                                // string tennv = Convert.ToString(oSheet.Cells[item.RowExcel, 3].Value2);
                                Process p = LoginVLKT(item.User, item.Pass, auto, item.Name);
                                //ghi lai thoi diem login
                                if (p == null)
                                {
                                    break;
                                }
                                string current_login = "";
                                current_login = Convert.ToString(oSheet.Cells[item.RowExcel, 4].Value);
                                if (current_login == "" || current_login == null)
                                {
                                    oSheet.Cells[item.RowExcel, 4] = DateTime.Now.ToString();
                                }
                                //else
                                //{
                                //    oSheet.Cells[item.RowExcel, 4] = current_login + "\n" + DateTime.Now.ToString();
                                //}

                                Thread.Sleep(5000);
                                item.Process = p;
                                break;
                            }
                        }
                    }
                    else
                    {
                        bool looping = false;
                        switch (item.CheckStatusVulan(oSheet))
                        {
                            case NPC.Status.HuyNVKinhNghiem:

                                if (item.IsCurrentInTown())
                                {
                                    continue;
                                }

                                double diff = DateTime.Now.Subtract(item.LastHuyMatChi).TotalSeconds;
                                if (diff < 90)
                                {
                                    continue;
                                }

                                item.BoMatChiVuLan(oSheet, "kinhnghiem");
                                looping = true;
                                break;
                            case NPC.Status.DangLamNhiemVu:
                                // backgroundWorker1.ReportProgress(1, "Everything is OK!");
                                backgroundWorker1.ReportProgress(1, item.Report());
                                Thread.Sleep(1000);
                                break;
                            case NPC.Status.DungIm:
                                // Refresh game();
                                backgroundWorker1.ReportProgress(1, item.Name + " is Idle!");
                                // oSheet.Cells[item.RowExcel, 16] = item.ThuNhap;
                                item.TryRefresh(oSheet, true);
                                looping = true;
                                break;
                            case NPC.Status.SaiMapLuyenCong:
                                diff = (DateTime.Now - item.LastKiemTraSaiMap).TotalSeconds;
                                if (diff < 30)
                                {
                                    backgroundWorker1.ReportProgress(1, item.Name + " is Temp Quit !");
                                    item.TryRefresh(oSheet, false);

                                    oSheet.Cells[item.RowExcel, 5] = DateTime.Now.ToString();
                                    oSheet.Cells[item.RowExcel, 6].Value2 = "OK";
                                    oSheet.Cells[item.RowExcel, 8].Value2 = "SAI MAP LUYEN CONG";
                                    oSheet.Cells[item.RowExcel, 8].Interior.Color = System.Drawing.Color.Red;
                                    l_remove.Add(item);

                                    item.QuitGame();

                                    looping = true;
                                }
                                else
                                {
                                    item.LastKiemTraSaiMap = DateTime.Now;
                                }
                                break;
                            case NPC.Status.XongNhiemVu:
                                item.TryRefresh(oSheet, false);
                                Thread.Sleep(2000);
                                backgroundWorker1.ReportProgress(1, item.Name + " is Finish");

                                backgroundWorker1.ReportProgress(1, item.Name + " is quitting game");
                                bool check = Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2) == "OK";
                                //   bool check = false;
                                if (!check)
                                {
                                    check = item.CheckNhanChu("NhanCu", 5);
                                }
                                if (!item.CheckToaDo())
                                {
                                    oSheet.Cells[item.RowExcel, 7] = "SAI TOA DO";
                                }
                                else
                                {
                                    oSheet.Cells[item.RowExcel, 8] = item.Map + " - " + item.Toado;
                                    oSheet.Cells[item.RowExcel, 11] = item.Nhiemvu;
                                }
                                oSheet.Cells[item.RowExcel, 7] = item.Test_note;
                                oSheet.Cells[item.RowExcel, 12] = item.Kinhnghiem;
                                string date_text = Convert.ToString(oSheet.Cells[item.RowExcel, 4].Value);
                                if (check)
                                {
                                    if (chb_Sound.Checked)
                                    {
                                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\TiengChuong.wav");
                                        player.Play();
                                    }
                                    oSheet.Cells[item.RowExcel, 6] = "OK";
                                    oSheet.Cells[item.RowExcel, 5] = DateTime.Now.ToString();

                                    DateTime date = DateTime.MinValue;

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
                                            oSheet.Cells[item.RowExcel, 14] = (DateTime.Now - date).TotalMinutes;
                                        }
                                    }

                                    l_remove.Add(item);
                                }
                                else
                                {
                                    oSheet.Cells[item.RowExcel, 5] = "Tempt_" + DateTime.Now.ToString();
                                    item.LastUpdateToaDo = DateTime.Now;
                                }
                                item.QuitGame();
                                looping = true;

                                break;
                            case NPC.Status.DangLamMatChi:
                                //   BoNhiemVu();
                                // oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                //DateTime datethoat = DateTime.MinValue;
                                //DateTime.TryParse(Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2), CultureInfo.CurrentCulture, DateTimeStyles.None, out datethoat);
                                //if ((DateTime.Now - datethoat).TotalSeconds < 90)
                                //{
                                //    continue;
                                //}

                                if (item.IsCurrentInTown())
                                {
                                    continue;
                                }
                                //DateTime datethoat = DateTime.MinValue;
                                //date_text = Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2);
                                //if (date_text != null)
                                //{
                                //    if (date_text.Contains('\n'))
                                //    {
                                //        DateTime.TryParse(date_text.Split('\n').Last(), CultureInfo.CurrentCulture, DateTimeStyles.None, out datethoat);
                                //    }
                                //    else
                                //    {
                                //        DateTime.TryParse(date_text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datethoat);
                                //    }

                                //    if (datethoat > DateTime.MinValue)
                                //    {
                                //        if ((DateTime.Now - datethoat).TotalSeconds < 90)
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //}
                                diff = DateTime.Now.Subtract(item.LastHuyMatChi).TotalSeconds;
                                if (diff < 90)
                                {
                                    continue;
                                }
                                //string matchi = "";
                                //matchi = Convert.ToString(oSheet.Cells[item.RowExcel, 6].Value2);
                                //if (matchi == "" || matchi == null)
                                //{
                                //    oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                //}
                                //else
                                //{
                                //    oSheet.Cells[item.RowExcel, 6] = matchi + "\n" + DateTime.Now.ToString();
                                //}

                                //    oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                //matchi = Convert.ToString(oSheet.Cells[item.RowExcel, 9].Value2);
                                //oSheet.Cells[item.RowExcel, 9] = matchi + "\n" + item.Datau;
                                //matchi = Convert.ToString(oSheet.Cells[item.RowExcel, 10].Value2);
                                //oSheet.Cells[item.RowExcel, 10] = matchi + "\n" + item.Datau;

                                item.BoMatChiVuLan(oSheet, "matchi");
                                //l_remove.Add(item);
                                looping = true;
                                break;
                            case NPC.Status.KetTrongThanh:
                                oSheet.Cells[item.RowExcel, 6] = DateTime.Now.ToString();
                                oSheet.Cells[item.RowExcel, 7] = "KET TRONG THANH";
                                //     item.QuitGame();
                                // l_remove.Add(item);
                                break;
                            case NPC.Status.TraNhiemVu:
                                backgroundWorker1.ReportProgress(1, item.Name + " Tra Nhiem Vu");
                                if (item.TraNhiemVu(""))
                                {
                                    item.LastUpdateToaDo = DateTime.Now;
                                }
                                looping = true;
                                break;
                            case NPC.Status.TraNhiemVuTrangBi:
                                item.LanChanTraNhiemVuTrangBi = !item.LanChanTraNhiemVuTrangBi;
                                if (item.LanChanTraNhiemVuTrangBi)
                                {
                                    Thread.Sleep(5000);
                                    backgroundWorker1.ReportProgress(1, item.Name + " Tra Nhiem Vu");
                                    //item.LastUpdateToaDo = DateTime.Now;
                                    if (item.isCurrentGanDaTau(1))
                                    {
                                        if (item.TraNhiemVu("vatpham"))
                                        {
                                            item.LastUpdateToaDo = DateTime.Now;
                                        }
                                        looping = true;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        if (looping)
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
                    wishlist = new List<NPC>();
                    online = new List<NPC>();
                    LoadNPCWishList(oSheet, ref wishlist);
                }
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                {
                    wishlist = new List<NPC>();
                    online = new List<NPC>();
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
                    string rowData = Convert.ToString(sheet.Cells[row, 5].Value);
                    if (rowData == "Logout")
                    {
                        continue;
                    }
                    DateTime date = DateTime.MinValue;
                    DateTime.TryParse(rowData, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                    if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {
                        wishlist.Add(item);
                    }
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
            return 99;
        }

        private void AutoLoginDT_Load(object sender, EventArgs e)
        {
            LoadDefautSetting();
        }
        private void LoadDefautSetting()
        {
            lb_path.Text = Properties.Settings.Default.VLTK_path;
            lb_path_auto.Text = Properties.Settings.Default.Auto_path;
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
        private void LoadNPCWishList(Excel.Worksheet osheet, ref List<NPC> wishlist)
        {
            Excel.Range usedRange = osheet.UsedRange;
            foreach (Excel.Range row in usedRange.Rows)
            {
                //if (wishlist.Count >= (int)this.numericUpDown1.Value)
                //{
                //    return;
                //}
                string rowData = Convert.ToString(row.Cells[1, 5].Value);
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
                if (name == null || name == "")
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
                //if (rowData.Contains("Temp"))
                //{
                //    MessageBox.Show(date.ToString());
                //}
                //MessageBox.Show(row.Cells[1, 5].Value2 + "\n" + row.Cells[1, 5].Value + "\n" + row.Cells[1, 5].Text + "\n" + date.ToString());
                //   MessageBox.Show(CultureInfo.CurrentCulture.Name+"\n"+DateTime.Now.ToString()+"\n"+DateTime.Now.ToString(CultureInfo.CurrentCulture)+"\n" + DateTime.Now.ToString(CultureInfo.CurrentUICulture));
                if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    string user = Convert.ToString(row.Cells[1, 1].Value2);
                    if (user == "")
                    {
                        continue;
                    }
                    NPC npc = new NPC(Convert.ToString(row.Cells[1, 3].Value2), user, Convert.ToString(row.Cells[1, 2].Value2), "", "");
                    npc.RowExcel = row.Row;
                    DateTime.TryParse(Convert.ToString(row.Cells[1, 4].Value), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                    if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {
                        osheet.Cells[npc.RowExcel, 4] = "";
                        osheet.Cells[npc.RowExcel, 6] = "";
                        osheet.Cells[npc.RowExcel, 7] = "";
                        osheet.Cells[npc.RowExcel, 8] = "";
                        osheet.Cells[npc.RowExcel, 9] = "";
                        osheet.Cells[npc.RowExcel, 10] = "";
                        osheet.Cells[npc.RowExcel, 11] = "";
                        osheet.Cells[npc.RowExcel, 12] = "";
                        osheet.Cells[npc.RowExcel, 14] = "";
                        osheet.Cells[npc.RowExcel, 15] = "";
                        osheet.Cells[npc.RowExcel, 16] = "";
                        osheet.Cells[npc.RowExcel, 17] = "";
                        osheet.Cells[npc.RowExcel, 18] = "";
                    }
                    npc.TypeNPC = Convert.ToString(row.Cells[1, 13].Value2);
                    wishlist.Add(npc);
                }
            nextrow:;
            }
            //wishlist.Add(new NPC("huydb20", "huydaibang20", "1233041990", "", ""));
            //MessageBox.Show(wishlist.First().Name);
        }
        private bool LoadListNPCOnline(ref List<NPC> l_NPC_online, Excel.Worksheet worksheet)
        {
            int sleep = 2000;
            l_NPC_online.Clear();

            Process proc_vlauto = GetVLAuto();

            //  Process proc_vlauto = Process.GetProcessesByName("VLAutoPr").First();
            IntPtr hWnd_main = proc_vlauto.MainWindowHandle;
            IntPtr hWnd = AutoControl.FindWindowExFromParent(hWnd_main, "List1", "SysListView32");

            //List<string> name_online = LoadListName();
            // IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, proc_vlauto.Id);
            Dictionary<string, Process> name_online = LoadListName();
            //  foreach (string name in name_online)
            int i = 0;
            foreach (KeyValuePair<string, Process> item in name_online)
            {
                // int i = name_online.IndexOf(name);
                AutoControl.SendClickOnPositionByPost(hWnd, l_point[i].X, l_point[i].Y, EMouseKey.LEFT);
                Thread.Sleep(sleep);
                NPC npc = null;
                try
                {
                    npc = new NPC(hWnd_main, item.Key);
                }
                catch (Exception)
                {
                    //npc = new NPC("Null", "null", "null");
                    //l_NPC_online.Add(npc);
                    Process p = Process.GetProcessesByName("vggame").ToList().OrderBy(e => e.StartTime).Last();
                    p.Kill();
                    return false;
                    // npc = new NPC("Null", "null", "null");
                }
                npc.ClickPoint = l_point[i];
                npc.ProcessAuto = proc_vlauto;
                string user = "";
                npc.TypeNPC = GetTypeNPC(worksheet, item.Key, ref user);
                npc.User = user;
                npc.Process = item.Value;
                if (npc.Name != "")
                {
                    l_NPC_online.Add(npc);
                }
                i++;
            }
            return true;
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
        private string GetTypeNPC(Excel.Worksheet worksheet, string name, ref string user)
        {
            foreach (Excel.Range item in worksheet.UsedRange.Cells)
            {
                if (Convert.ToString(item.Value2) == name)
                {
                    string row = item.Address;
                    int i = int.Parse(row.Split('$').Last());
                    user = Convert.ToString(worksheet.Cells[i, 1].Value2);
                    return Convert.ToString(worksheet.Cells[i, 13].Value2);
                }
            }
            return "None";
        }
        private Dictionary<string, Process> LoadListName()
        {
            Dictionary<string, Process> kq = new Dictionary<string, Process>();
            List<Process> list = Process.GetProcessesByName("vggame").ToList();
            foreach (Process item in list.OrderByDescending(e => e.StartTime))
            {
                IntPtr processHandle = SystemMethod.OpenProcess(SystemMethod.PROCESS_WM_READ, false, item.Id);
                string name = SystemMethod.Instance.ReadMemoryToString(processHandle, IntPtr.Add(item.MainModule.BaseAddress, 0x2DEC44));
                if (name != "")
                {
                    kq.Add(name, item);
                }
                else
                {
                    item.Kill();
                }
            }
            return kq;
        }

        private Process LoginVLKT(string user, string pass, string auto, string tennv)
        {
            try
            {
                try
                {
                    string source = Properties.Settings.Default.Auto_path + @"\UserData\Chuan\" + auto + ".cfg";
                    string des = Properties.Settings.Default.Auto_path + @"\UserData\" + tennv + ".cfg";
                    File.Copy(source, des, true);
                }
                catch (Exception)
                {
                }

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
            catch (Exception)
            {
                return null;
            }
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
            // MessageBox.Show((SystemMethod.IsWindowVisible(new IntPtr(0x00100a96))).ToString());

            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DaTau\TiengChuong.wav");
            //player.Play();

            // Process vlauto = GetVLAuto();
            Process vlauto = Process.GetProcessesByName("WindowsFormsApp1").First();
            List<IntPtr> handles = AutoControl.GetChildHandle(vlauto.MainWindowHandle);
            for (int i = 0; i < handles.Count; i++)
            {
                string text = SystemMethod.GetTextBoxText(handles[i]);
                if (text == null)
                {
                    continue;
                }
                if (text.Contains("Start"))
                {
                    MessageBox.Show(i.ToString());
                    break;
                }
            }


            string kq = "";
            Dictionary<string, Process> name = LoadListName();
            foreach (KeyValuePair<string, Process> item in name)
            {
                kq += item.Key + "\n";
            }
            Clipboard.SetText(kq);
        }

        private void btPathAuto_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPEN = new OpenFileDialog();
            if (DialogResult.OK == OPEN.ShowDialog())
            {
                string duongdan = Path.GetDirectoryName(OPEN.FileName);
                Properties.Settings.Default.Auto_path = duongdan;
                Properties.Settings.Default.Save();
                lb_path_auto.Text = duongdan;
            }
        }
    }
}
