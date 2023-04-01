using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Gma.UserActivityMonitor;
using KAutoHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Dota : Form
    {
        string pathSave = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings.txt";
        string pathOption = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Options.txt";
        Dictionary<string, string> dic_saved = new Dictionary<string, string>();
        List<ComboKey> l_keys = new List<ComboKey>();
        List<ComboKey> l_keymulti = new List<ComboKey>();
        public ComboSettings user = new ComboSettings();
        KeysConverter keyConvert;
        ComboOption options;
        string comboOrder = "";
        DateTime lastUpdateOrder;
        DateTime sessionEnd;
        Bitmap enemy;
        Bitmap ally;
        Bitmap r_Invoker;
        Bitmap Invoker_ThienThach_ChuaDung;
        Bitmap Invoker_MiniStun_ChuaDung;
        Bitmap Invoker_TuongBang_ChuaDung;
        Bitmap Invoker_ThaiDuong_ChuaDung;
        private int inner_interval = 1; //milisecond
        private int interval = 50; //milisecond
        private int delayBetweenCombos = 500; //milisecond
        private TimeSpan span_interval = TimeSpan.FromMilliseconds(20);
        // private int clearComboOrder = 1;//second
        bool runHandler = true;
        Keys toogleOnOff;
        bool OnOff = true;
        //bool stopImmediate = false;
        CancellationTokenSource cancelCombo;
        CancellationToken cancelToken;
        public Dota()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetCursorInfo(out CURSORINFO pci);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ClipCursor(ref RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetClipCursor(out RECT lpRect);
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("User32.Dll")]
        public static extern int ShowCursor(bool show);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
        enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
        }
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);
        static int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }
        static int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }
        private void Dota_Load(object sender, EventArgs e)
        {
            #region Load Image Constrain
            enemy = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Img\CursorEnemy.PNG");
            ally = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Img\CursorAlly.PNG");
            r_Invoker = LoadBitMapRBG(System.IO.Directory.GetCurrentDirectory() + @"\Img\Invoker\R_Invoker.PNG");
            Invoker_ThienThach_ChuaDung = LoadBitMapRBG(System.IO.Directory.GetCurrentDirectory() + @"\Img\Invoker\Invoker_ThienThach_ChuaDung.PNG");
            Invoker_MiniStun_ChuaDung = LoadBitMapRBG(System.IO.Directory.GetCurrentDirectory() + @"\Img\Invoker\Invoker_MiniStun_ChuaDung.PNG");
            Invoker_TuongBang_ChuaDung = LoadBitMapRBG(System.IO.Directory.GetCurrentDirectory() + @"\Img\Invoker\Invoker_TuongBang_ChuaDung.PNG");
            Invoker_ThaiDuong_ChuaDung = LoadBitMapRBG(System.IO.Directory.GetCurrentDirectory() + @"\Img\Invoker\Invoker_ThaiDuong_ChuaDung.PNG");
            #endregion
            //bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
            //bool NumLock = (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
            //bool ScrollLock = (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
            keyConvert = new KeysConverter();
            nudTimeSpanIn.Controls.RemoveAt(0);
            nudTimeSpanOut.Controls.RemoveAt(0);
            lastUpdateOrder = DateTime.Now;
            delayBetweenCombos = (int)nudTimeSpanOut.Value;
            interval = (int)nudTimeSpanIn.Value;
            span_interval = TimeSpan.FromMilliseconds(interval);
            //HookManager.KeyPress += HookManager_KeyPress;
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;

            string[] arr_origin = File.ReadAllLines(pathSave);
            foreach (string item in arr_origin)
            {
                string[] arr_item = item.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr_item.Count() == 2)
                {
                    dic_saved.Add(arr_item[0], arr_item[1]);
                    cbSaved.Items.Add(arr_item[0]);
                }
            }
            LoadListKeys();
            options = JsonConvert.DeserializeObject<ComboOption>(File.ReadAllText(pathOption));

            toogleOnOff = ComboSettings.GetKeySelect(options.OnOff[0]);
            sessionEnd = DateTime.Now;

            //stopImmediate = false;
            cancelCombo = new CancellationTokenSource();
            //ComboKeyOption testOptions = new ComboKeyOption();
            //File.WriteAllText(pathOption, JsonConvert.SerializeObject(testOptions));
        }
        private Bitmap LoadBitMapRBG(string v)
        {
            Bitmap bmp = new Bitmap(v);
            Bitmap result = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb);
            return result;
        }
        private void LoadListKeys()
        {
            l_keys = new List<ComboKey>();
            l_keymulti = new List<ComboKey>();
            if (chbCombo1.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey1.Text.ToLower(), tbCombo1.Text, tbCombo1.Tag);
                if (tbKey1.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo2.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey2.Text.ToLower(), tbCombo2.Text, tbCombo2.Tag);
                if (tbKey2.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo3.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey3.Text.ToLower(), tbCombo3.Text, tbCombo3.Tag);
                if (tbKey3.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo4.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey4.Text.ToLower(), tbCombo4.Text, tbCombo4.Tag);
                if (tbKey4.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo5.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey5.Text.ToLower(), tbCombo5.Text, tbCombo5.Tag);
                if (tbKey5.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo6.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey6.Text.ToLower(), tbCombo6.Text, tbCombo6.Tag);
                if (tbKey6.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo7.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey7.Text.ToLower(), tbCombo7.Text, tbCombo7.Tag);
                if (tbKey7.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo8.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey8.Text.ToLower(), tbCombo8.Text, tbCombo8.Tag);
                if (tbKey8.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo9.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey9.Text.ToLower(), tbCombo9.Text, tbCombo9.Tag);
                if (tbKey9.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
            if (chbCombo10.Checked)
            {
                ComboKey cbkey = new ComboKey(tbKey10.Text.ToLower(), tbCombo10.Text, tbCombo10.Tag);
                if (tbKey10.Text.Length > 1)
                {
                    l_keymulti.Add(cbkey);
                }
                else
                {
                    l_keys.Add(cbkey);
                }
            }
        }
        private void SetSetting(string json_text)
        {
            try
            {
                user = JsonConvert.DeserializeObject<ComboSettings>(json_text);
                if (user != null)
                {
                    LoadSetting(user);
                }
            }
            catch (Exception)
            {
            }
        }
        private void LoadSetting(ComboSettings user)
        {
            runHandler = true;
            nudTimeSpanIn.Value = user.DelayIn;
            nudTimeSpanOut.Value = user.DelayOut;

            string[] key1 = user.Key1.Split('_');
            tbKey1.Text = key1[0];
            tbCombo1.Text = key1[2];
            chbCombo1.Checked = key1[1] == "True";
            tbCombo1.Tag = null;
            if (key1.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key1[3]);
                tbCombo1.Tag = option;
            }

            string[] key2 = user.Key2.Split('_');
            tbKey2.Text = key2[0];
            tbCombo2.Text = key2[2];
            chbCombo2.Checked = key2[1] == "True";
            tbCombo2.Tag = null;
            if (key2.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key2[3]);
                tbCombo2.Tag = option;
            }

            string[] key3 = user.Key3.Split('_');
            tbKey3.Text = key3[0];
            tbCombo3.Text = key3[2];
            chbCombo3.Checked = key3[1] == "True";
            tbCombo3.Tag = null;
            if (key3.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key3[3]);
                tbCombo3.Tag = option;
            }

            string[] key4 = user.Key4.Split('_');
            tbKey4.Text = key4[0];
            tbCombo4.Text = key4[2];
            chbCombo4.Checked = key4[1] == "True";
            tbCombo4.Tag = null;
            if (key4.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key4[3]);
                tbCombo4.Tag = option;
            }

            string[] key5 = user.Key5.Split('_');
            tbKey5.Text = key5[0];
            tbCombo5.Text = key5[2];
            chbCombo5.Checked = key5[1] == "True";
            tbCombo5.Tag = null;
            if (key5.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key5[3]);
                tbCombo5.Tag = option;
            }

            string[] key6 = user.Key6.Split('_');
            tbKey6.Text = key6[0];
            tbCombo6.Text = key6[2];
            chbCombo6.Checked = key6[1] == "True";
            tbCombo6.Tag = null;
            if (key6.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key6[3]);
                tbCombo6.Tag = option;
            }

            string[] key7 = user.Key7.Split('_');
            tbKey7.Text = key7[0];
            tbCombo7.Text = key7[2];
            chbCombo7.Checked = key7[1] == "True";
            tbCombo7.Tag = null;
            if (key7.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key7[3]);
                tbCombo7.Tag = option;
            }

            string[] key8 = user.Key8.Split('_');
            tbKey8.Text = key8[0];
            tbCombo8.Text = key8[2];
            chbCombo8.Checked = key8[1] == "True";
            tbCombo8.Tag = null;
            if (key8.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key8[3]);
                tbCombo8.Tag = option;
            }

            string[] key9 = user.Key9.Split('_');
            tbKey9.Text = key9[0];
            tbCombo9.Text = key9[2];
            chbCombo9.Checked = key9[1] == "True";
            tbCombo9.Tag = null;
            if (key9.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key9[3]);
                tbCombo9.Tag = option;
            }

            string[] key10 = user.Key10.Split('_');
            tbKey10.Text = key10[0];
            tbCombo10.Text = key10[2];
            chbCombo10.Checked = key10[1] == "True";
            tbCombo10.Tag = null;
            if (key10.Length == 4)
            {
                ComboKeyOption option = JsonConvert.DeserializeObject<ComboKeyOption>(key10[3]);
                tbCombo10.Tag = option;
            }

            runHandler = false;
        }
        private void SaveSetting(ComboSettings user)
        {
            user.DelayIn = (int)nudTimeSpanIn.Value;
            user.DelayOut = (int)nudTimeSpanOut.Value;
            user.Key1 = tbKey1.Text + "_" + chbCombo1.Checked.ToString() + "_" + tbCombo1.Text;
            user.Key2 = tbKey2.Text + "_" + chbCombo2.Checked.ToString() + "_" + tbCombo2.Text;
            user.Key3 = tbKey3.Text + "_" + chbCombo3.Checked.ToString() + "_" + tbCombo3.Text;
            user.Key4 = tbKey4.Text + "_" + chbCombo4.Checked.ToString() + "_" + tbCombo4.Text;
            user.Key5 = tbKey5.Text + "_" + chbCombo5.Checked.ToString() + "_" + tbCombo5.Text;
            user.Key6 = tbKey6.Text + "_" + chbCombo6.Checked.ToString() + "_" + tbCombo6.Text;
            user.Key7 = tbKey7.Text + "_" + chbCombo7.Checked.ToString() + "_" + tbCombo7.Text;
            user.Key8 = tbKey8.Text + "_" + chbCombo8.Checked.ToString() + "_" + tbCombo8.Text;
            user.Key9 = tbKey9.Text + "_" + chbCombo9.Checked.ToString() + "_" + tbCombo9.Text;
            user.Key10 = tbKey10.Text + "_" + chbCombo10.Checked.ToString() + "_" + tbCombo10.Text;
        }
        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }
        private async void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            //DateTime start = DateTime.Now;
            //MessageBox.Show(e.KeyCode.ToString());
            //return;
            if (e.KeyCode == toogleOnOff)
            {
                OnOff = !OnOff;
                if (OnOff)
                {
                    this.Text = "Dota";
                }
                else
                {
                    this.Text = "Dota-Off";
                }
            }
            if (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Divide)
            {
                this.Text = "Dota";
                OnOff = true;
                return;
            }
            if (!OnOff)
            {
                return;
            }
            HookManager.KeyDown -= HookManager_KeyDown;

            foreach (ComboKey comboKey in l_keys)
            {
                if (comboKey.Keys == e.KeyCode)
                {
                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                    {
                        if (IsCorrectCursor(comboKey))
                        {
                            //comboKey.LastCall = DateTime.Now;
                            //stopImmediate = false;
                            //Combo(comboKey, DateTime.Now);
                            //if (stopImmediate)
                            //{
                            //    comboKey.LastCall = DateTime.MinValue;
                            //}
                            cancelCombo = new CancellationTokenSource();
                            comboKey.LastCall = DateTime.Now;
                            cancelToken = cancelCombo.Token;
                            Action actCombo = () =>
                            {
                                Combo(comboKey, DateTime.Now);
                            };
                            try
                            {
                                await Task.Run(actCombo);
                                if (cancelToken.IsCancellationRequested)
                                {
                                    // this.Text = "Stoped Action";
                                    comboKey.LastCall = DateTime.MinValue;
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                this.Text = "Stoped Task";
                                comboKey.LastCall = DateTime.MinValue;
                            }
                            //finally
                            //{
                            //    cancelCombo.Dispose();
                            //}
                            break;
                        }
                    }
                }
            }


            string key = keyConvert.ConvertToString(e.KeyCode).ToLower();
            if ((DateTime.Now - lastUpdateOrder).TotalMilliseconds > delayBetweenCombos)
            {
                comboOrder = key;
                lastUpdateOrder = DateTime.Now;
            }
            else
            {
                comboOrder += key;
                lastUpdateOrder = DateTime.Now;
            }
            foreach (ComboKey comboKey in l_keymulti)
            {
                if (comboOrder.Contains(comboKey.Multikeys))
                {
                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                    {
                        if (IsCorrectCursor(comboKey))
                        {
                            //comboKey.LastCall = DateTime.Now;
                            //stopImmediate = false;
                            //Combo(comboKey, DateTime.Now);
                            //if (stopImmediate)
                            //{
                            //    comboKey.LastCall = DateTime.MinValue;
                            //}
                            //break;

                            comboKey.LastCall = DateTime.Now;
                            cancelCombo = new CancellationTokenSource();
                            cancelToken = cancelCombo.Token;
                            Action actCombo = () =>
                            {
                                Combo(comboKey, DateTime.Now);
                            };
                            try
                            {
                                await Task.Run(actCombo, cancelToken);
                                if (cancelToken.IsCancellationRequested)
                                {
                                    comboKey.LastCall = DateTime.MinValue;
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                this.Text = "Stoped Task";
                                comboKey.LastCall = DateTime.MinValue;
                            }
                            //finally
                            //{
                            //    cancelCombo.Dispose();
                            //    cancelCombo = new CancellationTokenSource();
                            //}
                            break;
                        }
                    }
                }
            }
            //  DateTime end = DateTime.Now;
            HookManager.KeyDown += HookManager_KeyDown;
            // MessageBox.Show((end - start).TotalMilliseconds.ToString());
        }
        public async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            //  SetInterval(action, timeout);
        }
        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (!OnOff)
            {
                return;
            }
            if (Control.ModifierKeys != Keys.None)
            {
                // stopImmediate = true;
                if (cancelCombo != null)
                {
                    cancelCombo.Cancel();
                }
            }
            //else if (e.KeyCode == Keys.S)
            //{
            //    HuyMouseClick(new Point(960, 230), EMouseKey.RIGHT);
            //    //HuyMouseDrag(new Point(960, 540), new Point(1060, 540));
            //}
        }
        private void castw()
        {
            HuyKeyDown(KeyDirectX.LeftAlt);
            HuyKeyDown(KeyDirectX.W);
            HuyKeyUp(KeyDirectX.W);
            HuyKeyUp(KeyDirectX.LeftAlt);

            //Keys select1 = GetKeySelect(tbKey1.Text.ToLower()[0]);
            //Keys select2 = GetKeySelect(tbKey2.Text.ToLower()[0]);
            //Keys select3 = GetKeySelect(tbKey3.Text.ToLower()[0]);
            //Keys select4 = GetKeySelect(tbKey4.Text.ToLower()[0]);
            //Keys select5 = GetKeySelect(tbKey5.Text.ToLower()[0]);
            //Keys select6 = GetKeySelect(tbKey6.Text.ToLower()[0]);
            //Keys select7 = GetKeySelect(tbKey7.Text.ToLower()[0]);
            //Keys select8 = GetKeySelect(tbKey8.Text.ToLower()[0]);
            //Keys select9 = GetKeySelect(tbKey9.Text.ToLower()[0]);
            //Keys select10 = GetKeySelect(tbKey10.Text.ToLower()[0]);
            //if (e.KeyCode == select1 && chbCombo1.Checked)
            //{
            //    Combo(tbCombo1.Text);
            //}
            //else if (e.KeyCode == select2 && chbCombo2.Checked)
            //{
            //    Combo(tbCombo2.Text);
            //}
            //else if (e.KeyCode == select3 && chbCombo3.Checked)
            //{
            //    Combo(tbCombo3.Text);
            //}
            //else if (e.KeyCode == select4 && chbCombo4.Checked)
            //{
            //    Combo(tbCombo4.Text);
            //}
            //else if (e.KeyCode == select5 && chbCombo5.Checked)
            //{
            //    Combo(tbCombo5.Text);
            //}
            //else if (e.KeyCode == select6 && chbCombo6.Checked)
            //{
            //    Combo(tbCombo6.Text);
            //}
            //else if (e.KeyCode == select7 && chbCombo7.Checked)
            //{
            //    Combo(tbCombo7.Text);
            //}
            //else if (e.KeyCode == select8 && chbCombo8.Checked)
            //{
            //    Combo(tbCombo8.Text);
            //}
            //else if (e.KeyCode == select9 && chbCombo9.Checked)
            //{
            //    Combo(tbCombo9.Text);
            //}
            //else if (e.KeyCode == select10 && chbCombo10.Checked)
            //{
            //    Combo(tbCombo10.Text);
            //}
        }
        public async Task ComboOld(List<KeyDirectX> tbCombo)
        {
            foreach (KeyDirectX item in tbCombo)
            {
                await SetInterval(() => HuyKeyPress(item), span_interval);
            }
        }
        public Bitmap GetCursorBitmap()
        {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            Icon ic = Icon.FromHandle(pci.hCursor);
            Bitmap bmp = ic.ToBitmap();
            if (options.SaveImg)
            {
                bmp.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\CursorCurrent.PNG");
            }
            return bmp;
        }
        public Point GetCursorPoint()
        {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            return new Point(pci.ptScreenPos.x, pci.ptScreenPos.y);
        }
        public bool IsCorrectCursor(ComboKey combo)
        {
            switch (combo.Option.CallWithMouse)
            {
                case "enemy":
                    return CompareBitmapsFast(enemy, GetCursorBitmap());
                case "ally":
                    return CompareBitmapsFast(ally, GetCursorBitmap());
                case "notAlly":
                    return !CompareBitmapsFast(ally, GetCursorBitmap());
                case "notEnemy":
                    return !CompareBitmapsFast(enemy, GetCursorBitmap());
                case "findEnemy":
                    if (CompareBitmapsFast(enemy, GetCursorBitmap()))
                    {
                        return true;
                    }
                    else
                    {
                        return TryGetEnemyPosition(200, 300);
                    }
                case "tryFindEnemy":
                    TryGetEnemyPosition(200, 300);
                    return true;
                default:
                    return true;
            }
        }
        public bool TryGetEnemyPosition(int offset_X, int offset_Y, bool moveCursor = true, double precast = 0)
        {
            Point current = GetCursorPoint();
            int offsetFromHPdownToNhanVat = 80;
            int offsetTopBonus = 100;
            Point offset_point = new Point(current.X - offset_X, current.Y - offset_Y - offsetTopBonus);
            Bitmap manHinh = CaptureHelper.CaptureImage(new Size(2 * offset_X, 2 * offset_Y + offsetTopBonus), offset_point);

            //Full man hinh
            if (offset_X < 0 && offset_Y < 0)
            {
                manHinh = CaptureHelper.CaptureImage(new Size(1920, 1080), new Point(0, 0));
            }

            if (options.SaveImg)
            {
                manHinh.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\cusor_offset.PNG");
            }
            //Bitmap manHinh = CaptureHelper.CaptureImage(new Size(1920 / 5, 1080 / 5), new Point(0, 0));
            // Bitmap thanhMau = ImageScanOpenCV.GetImage(System.IO.Directory.GetCurrentDirectory() + @"\Img\KhungThanhMau_Top.png");
            Bitmap thanhMau = ImageScanOpenCV.GetImage(System.IO.Directory.GetCurrentDirectory() + @"\Img\KhungThanhMau_Botv2.png");
            Point? finded = FindOutPoint(manHinh, thanhMau);
            if (finded.HasValue)
            {
                if (moveCursor)
                {
                    int width_mh = 1920;
                    int cast_x = 100 - 100 * current.X / width_mh;
                    //this.Text += cast_x.ToString() + ";";

                    Point enemyPosition = new Point(offset_point.X + finded.Value.X + cast_x, offset_point.Y + finded.Value.Y + offsetFromHPdownToNhanVat);
                    if (precast > 0)
                    {
                        int precast_x = (int)(enemyPosition.X + precast * (enemyPosition.X - current.X));
                        int precast_y = (int)(enemyPosition.Y + precast * (enemyPosition.Y - current.Y));
                        enemyPosition = new Point(precast_x, precast_y);
                    }
                    //Clipboard.SetText(Clipboard.GetText() + finded.Value.X.ToString() + "-" + finded.Value.Y.ToString() + "_" + enemyPosition.X.ToString() + "-" + enemyPosition.Y.ToString() + "; ");
                    SetCursorPos(enemyPosition.X, enemyPosition.Y);
                    //manHinh = CaptureHelper.CaptureImage(new Size(200, 200), new Point(enemyPosition.X - 100, enemyPosition.Y - 100));
                    //manHinh.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\precast.PNG");

                }
                return true;
            }
            else
            {
                //manHinh.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\cusor_offset_failed.PNG");
                return false;
            }
        }
        //public void Combo(List<KeyDirectX> tbCombo)
        public void Combo(ComboKey combo, DateTime overAllStart)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return;
            }
            //if (stopImmediate)
            //{
            //    return;
            //}

            DateTime start_combo = DateTime.Now;
            Point current = GetCursorPoint();
            if (combo.Option.SpecialHero == "ChupAnh")
            {
                int x = 192;
                int y = 0;
                int.TryParse(this.Text, out y);

                if (combo.Keys == Keys.N)
                {
                    y += 108;
                }
                if (combo.Keys == Keys.P)
                {
                    y -= 108;
                }
                this.Text = y.ToString();
                AutoControl.MouseClick(x, y, EMouseKey.RIGHT);//cick box ten tai khoan
                return;
            }

            RECT current_rect;
            GetClipCursor(out current_rect);
            if (combo.Option.LockMouseUntilPressThis != KeyDirectX.Nothing || combo.Option.SpecialHero == "Tinker")
            {
                int offset = 5;
                ClipCursorOffset(current, offset);
            }
            bool exitLoop = false;
            #region Pre Combo
            if (combo.L_code_combo.Count > 0)
            {
                switch (combo.Option.SpecialHero)
                {
                    case "Invoker":
                        if (combo.Option.ComboName == "code1" || combo.Option.ComboName == "code2" || combo.Option.ComboName == "code3")
                        {
                            //this.Text = "Dota";
                            //Chup anh nut D,F
                            Bitmap bmp1 = CaptureHelper.CaptureImage(options.SizeD, options.PointD);
                            Bitmap bmp2 = CaptureHelper.CaptureImage(options.SizeF, options.PointF);
                            Bitmap sosanh = Invoker_ThienThach_ChuaDung;
                            Bitmap sosanh2 = Invoker_MiniStun_ChuaDung;
                            if (combo.Option.ComboName == "code2")
                            {
                                sosanh = Invoker_MiniStun_ChuaDung;
                                sosanh2 = Invoker_TuongBang_ChuaDung;
                            }
                            if (combo.Option.ComboName == "code3")
                            {
                                sosanh = Invoker_TuongBang_ChuaDung;
                                sosanh2 = Invoker_ThaiDuong_ChuaDung;
                            }
                            if (options.SaveImg)
                            {
                                bmp1.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\D_test1.PNG");
                                bmp2.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\F_test1.PNG");
                            }
                            if (CompareBitmapsSlow(bmp1, sosanh, 95))
                            {
                                combo.Combo = combo.L_code_combo[0];
                                // this.Text = "thienthach-chuadung";
                            }
                            //else if (CompareBitmapsSlow(bmp1, Invoker_MiniStun_DaDung,95)) { combo.Combo = combo.L_code_combo[1]; this.Text = "ministun-dadung"; }
                            else
                            {
                                if (CompareBitmapsSlow(bmp1, sosanh2, 95) && CompareBitmapsSlow(bmp2, sosanh, 95))
                                {
                                    //this.Text = "Trung";
                                    goto endCombo;
                                }
                                else
                                {
                                    combo.Combo = combo.L_code_combo[1];
                                }
                                //this.Text = "conlai"; 
                            }
                        }
                        else if (combo.Option.ComboName == "code4") // Tuong Bang
                        {
                            bool castXX = false;
                            Point origin_current = new Point(current.X, current.Y);
                            HuyKeyPress(KeyDirectX.A);
                            int x = (int)(0.5 * (current_rect.Left + current_rect.Right));
                            int y = (int)(0.5 * (current_rect.Top + current_rect.Bottom));
                            if (current.X == x && current.Y == y)
                            {
                                if (!TryGetEnemyPosition(600, 400))//520-330
                                {
                                    combo.Combo = combo.L_code_combo[0];
                                    goto boquaclickchuot;
                                }
                                castXX = true;
                                current = GetCursorPoint();
                            }
                            else
                            {
                                TryGetEnemyPosition(200, 200);
                                current = GetCursorPoint();
                            }
                            AutoControl.MouseClick(current.X, current.Y, EMouseKey.RIGHT);//cick box ten tai khoan
                            //HuyKeyPress(KeyDirectX.S);

                            //Time consumed 250ms

                            //Thoi gian cho quay ve huong enemy
                            //Thread.Sleep(300);

                            //if ((DateTime.Now - sessionEnd).TotalMilliseconds > 0)
                            //{
                            //    goto endCombo;
                            //}

                            //this.Text = "Dota";
                            //Chup anh nut D,F
                            Bitmap bmp1 = CaptureHelper.CaptureImage(options.SizeD, options.PointD);
                            Bitmap bmp2 = CaptureHelper.CaptureImage(options.SizeF, options.PointF);
                            if (options.SaveImg)
                            {
                                bmp1.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\D_test1.PNG");
                                bmp2.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\F_test1.PNG");
                            }
                            KeyDirectX keyActive = KeyDirectX.Nothing;
                            KeyDirectX keySecond = KeyDirectX.Nothing;
                            bool secondIsThaiDuong = false;
                            if (CompareBitmapsSlow(bmp1, Invoker_TuongBang_ChuaDung, 95))
                            {
                                keyActive = KeyDirectX.D;
                                if (CompareBitmapsSlow(bmp2, Invoker_MiniStun_ChuaDung, 95))
                                {
                                    keySecond = KeyDirectX.F;
                                }
                                else if (CompareBitmapsSlow(bmp2, Invoker_ThaiDuong_ChuaDung, 95))
                                {
                                    keySecond = KeyDirectX.F;
                                    secondIsThaiDuong = true;
                                }
                            }
                            else if ((CompareBitmapsSlow(bmp2, Invoker_TuongBang_ChuaDung, 95)))
                            {
                                keyActive = KeyDirectX.F;
                                if (CompareBitmapsSlow(bmp1, Invoker_MiniStun_ChuaDung, 95))
                                {
                                    keySecond = KeyDirectX.D;
                                }
                                else if (CompareBitmapsSlow(bmp1, Invoker_ThaiDuong_ChuaDung, 95))
                                {
                                    keySecond = KeyDirectX.D;
                                    secondIsThaiDuong = true;
                                }
                            }
                            else
                            {
                                combo.Combo = combo.L_code_combo[0];
                                goto boquaclickchuot;
                            }
                            //Time consumed 300ms
                            if (keySecond != KeyDirectX.Nothing)
                            {
                                if (!castXX) // khong tu dong tim doi thu de tha thai duong ma tha ngay vi tri chuot
                                {
                                    SetCursorPos(origin_current.X, origin_current.Y);
                                }
                                else
                                {
                                    TryGetEnemyPosition(200, 200, true, 1);//0.6
                                    Thread.Sleep(20); //wait for get enemy finish.
                                }
                                HuyKeyPress(keySecond);
                                Thread.Sleep(inner_interval);
                                HuyKeyPress(KeyDirectX.D3);
                                Thread.Sleep(inner_interval);
                                //HuyKeyPress(KeyDirectX.A);
                                Thread.Sleep(200); //wait for second cast OK
                                if (castXX)// neu ko la zc (la xx) thi du doan vi tri enymy roi cast tuong bang
                                {
                                    TryGetEnemyPosition(200, 200, true, 4);
                                }
                            }

                            current = GetCursorPoint();
                            //Time consumed 450ms

                            //Time consumed Checking....
                            //goto endCombo;

                            int goc_chon_i = 0;
                            double min_khoangcach = 99;
                            Point current_relate = new Point(current.X - x, y - current.Y - 40);
                            //if (current_relate.Y < 0)
                            //{
                            //    current_relate = new Point(current.X - x, y - current.Y - 40);
                            //}
                            //int sLThoaKhoangCach = 0;
                            //string l_gocChon = "";
                            foreach (LineTuongBang item in user.L_tuongbang)
                            {
                                double khoangcach = 100;
                                if (item.IsApproval(current_relate, out khoangcach))
                                {
                                    //sLThoaKhoangCach++;
                                    if (min_khoangcach > khoangcach)
                                    {
                                        min_khoangcach = khoangcach;
                                        goc_chon_i = item.Goc_i;
                                    }
                                }
                                //if (khoangcach < 25)
                                //{
                                //    l_gocChon += item.Goc_i.ToString() + "---" + khoangcach.ToString() + ";";
                                //    sLThoaKhoangCach++;
                                //}
                            }
                            //MessageBox.Show(l_gocChon);
                            if (min_khoangcach == 99)
                            {
                                HuyKeyPress(KeyDirectX.A);
                                goto endCombo;
                            }
                            //this.Text = current.X.ToString() + "-" + current.Y.ToString() + "_" + goc_chon_i.ToString() + "_" + min_khoangcach.ToString() + "_" + sLThoaKhoangCach.ToString();
                            //this.Text = current.X.ToString() + "-" + current.Y.ToString() + "_" + goc_chon_i.ToString() + "_" + min_khoangcach.ToString();
                            double goc_chon = 2 * goc_chon_i * Math.PI / 36;
                            int cast_x = (int)(x + 200 * Math.Cos(goc_chon));
                            int cast_y = (int)(y + 200 * Math.Sin(goc_chon));
                            //int goc = ((int)(Math.Acos(LineTuongBang.TichVoHuong(new Point(x, y), new Point(cast_x, cast_y), current)) * 180 / Math.PI));
                            //if (goc > 180)
                            //{
                            //    goc = 360 - goc;
                            //}
                            //this.Text = goc.ToString() + ": " + x.ToString() + "," + y.ToString() + "---" + current.X.ToString() + "," + current.Y.ToString() + "---" + cast_x.ToString() + "," + cast_y.ToString();



                            SetCursorPos(cast_x, cast_y);
                            Thread.Sleep(10);
                            //Time consumed Checking....
                            //goto endCombo;

                            AutoControl.MouseClick(cast_x, cast_y, EMouseKey.RIGHT);//cick box ten tai khoan
                            HuyKeyPress(KeyDirectX.S);
                            Thread.Sleep(20);
                            AutoControl.MouseClick(cast_x, cast_y, EMouseKey.RIGHT);//cick box ten tai khoan
                            HuyKeyPress(KeyDirectX.S);
                            Thread.Sleep(20);
                            AutoControl.MouseClick(cast_x, cast_y, EMouseKey.RIGHT);//cick box ten tai khoan
                            HuyKeyPress(KeyDirectX.S);
                            Thread.Sleep(20);

                            //Time consumed Checking....
                            //goto endCombo;

                            HuyKeyPress(keyActive);
                            SetCursorPos(current.X, current.Y);
                            if (keySecond != KeyDirectX.Nothing)
                            {
                                Thread.Sleep(100);
                                if (secondIsThaiDuong)
                                {
                                    // Co kha nang miss Q
                                    HuyKeyPress(KeyDirectX.Q);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.Q);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.D3);
                                    Thread.Sleep(inner_interval);

                                    //try call minitun again
                                    HuyKeyPress(KeyDirectX.Q);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.R);
                                    Thread.Sleep(inner_interval);

                                    TryGetEnemyPosition(200, 200);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.F);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.D3);
                                    Thread.Sleep(100);

                                    HuyKeyPress(KeyDirectX.Decimal);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.Divide);
                                }
                                else
                                {
                                    HuyKeyPress(KeyDirectX.Decimal);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.E);
                                    Thread.Sleep(inner_interval);
                                    HuyKeyPress(KeyDirectX.Divide);
                                    Thread.Sleep(100);
                                    HuyKeyPress(KeyDirectX.R);
                                }
                            }

                            Thread.Sleep(inner_interval);
                            HuyKeyPress(KeyDirectX.A);
                            goto endCombo;
                        boquaclickchuot:;
                        }
                        else if (combo.Option.ComboName == "code5")
                        {
                            //Thread.Sleep(200);
                            //ShowCursor(false);
                            //ClipCursorOffset(new Point(550, 970), 1);

                            AutoControl.MouseClick(550, 970, EMouseKey.DOUBLE_LEFT);//cick box ten tai khoan
                            Thread.Sleep(20);
                            AutoControl.MouseClick(550, 970, EMouseKey.DOUBLE_LEFT);//cick box ten tai khoan
                            Thread.Sleep(20);
                            AutoControl.MouseClick(550, 970, EMouseKey.DOUBLE_LEFT);//cick box ten tai khoan
                            Thread.Sleep(20);
                            //ClipCursor(ref current_rect);
                            sessionEnd = DateTime.Now.AddSeconds(10);
                            //Thread.Sleep(2000);
                            //ShowCursor(true);

                            //SetCursorPos(current.X, current.Y);
                            int x = (int)(0.5 * (current_rect.Left + current_rect.Right));
                            int y = (int)(0.5 * (current_rect.Top + current_rect.Bottom));
                            SetCursorPos(x, y);
                            foreach (ComboKey comboKey in l_keymulti)
                            {
                                if (comboKey.Option.ComboName == "code4")
                                {
                                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                                    {
                                        if (IsCorrectCursor(comboKey))
                                        {
                                            comboKey.LastCall = DateTime.Now;
                                            Combo(comboKey, overAllStart);
                                            break;
                                        }
                                    }
                                }
                            }

                            goto endCombo;
                        }
                        break;
                    case "Tinker-code":
                        if (combo.Option.ComboName == "code1" && CompareBitmapsFast(ally, GetCursorBitmap()))
                        {
                            if (CompareBitmapsFast(ally, GetCursorBitmap()))
                            {
                                HuyKeyPress(KeyDirectX.N);
                                goto endCombo;
                            }
                        }
                        else if (combo.Option.ComboName == "code2")
                        {
                            Keys active = active = Keys.D0;
                            if (TryGetEnemyPosition(225, 100))
                            {
                                active = Keys.D;
                            }
                            else
                            {
                                if (CompareBitmapsFast(enemy, GetCursorBitmap()))
                                {
                                    active = Keys.G;
                                }
                            }
                            foreach (ComboKey comboKey in l_keys)
                            {
                                if (comboKey.Keys == active)
                                {
                                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                                    {
                                        if (IsCorrectCursor(comboKey))
                                        {
                                            comboKey.LastCall = DateTime.Now;
                                            Combo(comboKey, overAllStart);
                                            break;
                                        }
                                    }
                                }
                            }
                            goto endCombo;
                        }
                        else if (combo.Option.ComboName == "code3")
                        {
                            HuyMouseDrag(new Point(1295, 1055), new Point(1295, 965));
                            SetCursorPos(current.X, current.Y);
                            return;
                        }
                        else
                        {
                            bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
                            //this.Text = CapsLock.ToString();
                            if (CapsLock && combo.L_code_combo.Count > 1)
                            {
                                combo.Combo = combo.L_code_combo[1];
                            }
                            else
                            {
                                combo.Combo = combo.L_code_combo[0];
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion
            #region On combo
            if (combo.Option.CallWithMouse == "findEmeny")
            {
                ClipCursor(ref current_rect);
            }
            foreach (KeyDirectX item in combo.Combo)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }
                //if (stopImmediate)
                //{
                //    return;
                //}
                Thread.Sleep(inner_interval);
                if (item == KeyDirectX.NumPadStar)
                {
                    Thread.Sleep(interval);
                }
                //else if (item == KeyDirectX.Equals)
                //{
                //    SetCursorPos(current.X, current.Y);
                //    Thread.Sleep(50);
                //}
                else
                {
                    switch (combo.Option.SpecialHero)
                    {
                        case "Tinker":
                            if (item == KeyDirectX.Q)
                            {

                                //Chup anh nut Q
                                Bitmap bmp1 = CaptureHelper.CaptureImage(options.SizeQ, options.PointQ);
                                // Bitmap bmp1 = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Img\Q_test1_1.PNG");
                                if (options.SaveImg)
                                {
                                    bmp1.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\Q_test1.PNG");
                                }
                                //Bam Q
                                int lanthu = 0;
                            chaylai:
                                lanthu++;
                                //this.Text = lanthu.ToString();
                                //if (!IsCorrectCursor(combo))
                                //{
                                //    ClipCursor(ref current_rect);
                                //    exitLoop = true;
                                //    break;
                                //}

                                //int lanthu = 0;
                                //    chaylai:
                                //     lanthu++;
                                //   if (lanthu > 4)
                                //if (stopImmediate)
                                //{
                                //    exitLoop = true;
                                //    break;
                                //}
                                //cancelToken.ThrowIfCancellationRequested();
                                //if (stopImmediate)
                                //{
                                //    return;
                                //}
                                if (cancelToken.IsCancellationRequested)
                                {
                                    return;
                                }
                                if ((DateTime.Now - start_combo).TotalMilliseconds > 500)
                                {
                                    if ((DateTime.Now - overAllStart).TotalMilliseconds > 2000)
                                    {
                                        HuyKeyPress(KeyDirectX.S);
                                    }
                                    //if (!options.ContinueAfterFailed)
                                    else
                                    {
                                        // HuyKeyPress(KeyDirectX.S);
                                        Combo(combo, overAllStart);
                                    }
                                    exitLoop = true;
                                    break;
                                }
                                IsCorrectCursor(combo);
                                HuyKeyPress(item);
                                //Check anh nut Q
                                Thread.Sleep(inner_interval);
                                ClipCursor(ref current_rect);

                                Thread.Sleep(100);
                                Bitmap bmp2 = CaptureHelper.CaptureImage(options.SizeQ, options.PointQ);
                                if (options.SaveImg)
                                {
                                    bmp2.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\Q_test2.PNG");
                                }
                                if (CompareBitmapsFast(bmp1, bmp2)) { goto chaylai; }
                                Thread.Sleep(400);
                                Bitmap bmp3 = CaptureHelper.CaptureImage(options.SizeQ, options.PointQ);
                                if (options.SaveImg)
                                {
                                    bmp3.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\Q_test3.PNG");
                                }
                                if (CompareBitmapsFast(bmp1, bmp3)) { HuyKeyPress(KeyDirectX.S); exitLoop = true; break; }
                            }
                            else if (item == KeyDirectX.D2)
                            {
                                double khoangcach = Math.Sqrt(Math.Pow(GetCursorPoint().X - current.X, 2) + Math.Pow(GetCursorPoint().Y - current.Y, 2));
                                if (khoangcach > options.BlinkMin)
                                {
                                    //if (options.BlinkDelay > 0)
                                    //{
                                    //    Thread.Sleep(options.BlinkDelay);
                                    //}
                                    HuyKeyPress(item);
                                }
                            }
                            else
                            {
                                if ((combo.Option.CallWithMouse == "findEmeny"))
                                {
                                    IsCorrectCursor(combo);//try focus Enemy
                                }
                                HuyKeyPress(item);
                            }
                            break;
                        case "Tinker-code":
                            if ((item == KeyDirectX.D2 && combo.Option.ComboName == "code1"))
                            {
                                if (CompareBitmapsFast(enemy, GetCursorBitmap()))
                                {
                                    //Lấy vị trí chuột hiện tại
                                    Point rightNow = GetCursorPoint();
                                    //Move chuột về vị trí tính toán
                                    // double length = 0;
                                    Point tinhToan = TinhToanViTri(current_rect, rightNow, 650);

                                    //this.Text = rightNow.X + "," + rightNow.Y + "_" + tinhToan.X + "," + tinhToan.Y + "_" + length.ToString();
                                    //Clipboard.SetText(rightNow.X + "," + rightNow.Y + "_" + tinhToan.X + "," + tinhToan.Y);
                                    SetCursorPos(tinhToan.X, tinhToan.Y);
                                    //Cast 
                                    Thread.Sleep(50);
                                    HuyKeyPress(item);
                                    //Đưa chuột về vị trí ban đầu
                                    Thread.Sleep(50);
                                    SetCursorPos(rightNow.X, rightNow.Y);
                                    break;
                                }
                            }
                            HuyKeyPress(item);
                            break;
                        case "Invoker":
                            if (item == KeyDirectX.NumPadMinus || item == KeyDirectX.NumPadPlus)
                            {
                                int lanthu = 0;
                            chaylai:
                                lanthu++;
                                //this.Text = this.Text + lanthu;
                                //Chup anh nut R
                                Bitmap bmp1 = CaptureHelper.CaptureImage(options.SizeR, options.PointR);
                                if (options.SaveImg)
                                {
                                    bmp1.Save(System.IO.Directory.GetCurrentDirectory() + @"\Img\R_test1.PNG");
                                }
                                if (lanthu > options.LanThu)
                                {
                                    if (!options.ContinueAfterFailed)
                                    {
                                        if (item == KeyDirectX.NumPadMinus && combo.Combo[0] == KeyDirectX.Decimal)
                                        {
                                            //this.Text = "fail";
                                            HuyKeyPress(KeyDirectX.Divide);
                                        }
                                        exitLoop = true;
                                    }
                                    if (item == KeyDirectX.NumPadPlus)
                                    {
                                        //this.Text = "numpadplus";
                                        exitLoop = false;
                                    }
                                    break;
                                }
                                if (CompareBitmapsSlow(bmp1, r_Invoker, 95)) { break; }
                                Thread.Sleep(options.LanThuDelay);
                                goto chaylai;
                            }
                            else
                            {
                                HuyKeyPress(item);
                            }
                            break;
                        default:
                            if (item == combo.Option.LockMouseUntilPressThis)
                            {
                                ClipCursor(ref current_rect);
                            }
                            HuyKeyPress(item);
                            break;
                    }
                }
                if (exitLoop) break;
            }
            #endregion
            #region after Combo
            if (combo.Option.ComboName != "")
            {
                switch (combo.Option.SpecialHero)
                {
                    case "Tinker-code":
                        if (combo.Option.ComboName == "code4")
                        {
                            foreach (ComboKey comboKey in l_keys)
                            {
                                if (comboKey.Keys == Keys.F)
                                {
                                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                                    {
                                        if (IsCorrectCursor(comboKey))
                                        {
                                            comboKey.LastCall = DateTime.Now;
                                            Combo(comboKey, overAllStart);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        #endregion
        endCombo:
            ClipCursor(ref current_rect);
            //this.Text = (DateTime.Now - overAllStart).TotalMilliseconds.ToString();
            //CleaningDecimal();

        }
        private void ClipCursorOffset(Point current, int offset)
        {
            RECT rect = new RECT(current.X - offset, current.Y - offset, current.X + offset, current.Y + offset);
            ClipCursor(ref rect);
        }
        private Point TinhToanViTri(RECT current_rect, Point rightNow, int tamdanh)
        {
            int rawlength = 650;
            int x = (current_rect.Left + current_rect.Right) / 2;
            int y = (current_rect.Top + current_rect.Bottom) / 2;
            double ratioy = 0.5 * (double)rightNow.Y / y;
            if (ratioy < 0.5)
            {
                rightNow = new Point(rightNow.X, rightNow.Y + 40);
                ratioy = 0.5 * (double)rightNow.Y / y;
            }
            double ratiox = -Math.Abs((double)rightNow.X - x) / x;
            // double ratio = -1.2137 * ratioy * ratioy + 1.9655 * ratioy + 0.3558 + 0.2 * ratiox;
            //double ratio = 1.4424 * ratioy + 0.2603 + (-0.5 * ratioy + 0.25) * ratiox;
            double length = 791.42 * ratioy + 139.72 + (ratiox / 0.9) * (398.43 * ratioy * ratioy - 13.328 * ratioy - 0.7575);
            if (ratioy > 0.5)
            {
                length = 461.35 * ratioy + 85.013 + (ratiox / 0.9) * (754.89 * ratioy * ratioy - 951.91 * ratioy + 157.91);
            }
            int vt_x = x - rightNow.X;
            int vt_y = y - rightNow.Y;
            double vt_length = Math.Sqrt(vt_x * vt_x + vt_y * vt_y) * tamdanh / rawlength;
            vt_x = (int)(vt_x * length / vt_length);
            vt_y = (int)(vt_y * length / vt_length);

            return new Point(rightNow.X + vt_x, rightNow.Y + vt_y);
        }
        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap)
        {
            Image<Bgr, byte> arg_17_0 = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            Point? result = null;
            using (Image<Gray, float> image = arg_17_0.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                // image.Save(@"C:\Users\HP\Desktop\tool\HP check\Result.png");
                double[] array;
                double[] array2;
                Point[] array3;
                Point[] array4;

                image.MinMax(out array, out array2, out array3, out array4);
                if (array2[0] >= 0.9999)
                {
                    result = new Point?(array4[0]);
                }
            }
            return result;
        }
        private void CleaningDecimal()
        {
            OnOff = true;
            this.Text = "Dota";
        }
        public void Combo(string tbCombo)
        {
            foreach (char item in tbCombo.ToLower())
            {
                Thread.Sleep(interval);
                HuyKeyPress(ComboSettings.GetKeyDirectXSelect(item));
            }
        }
        public static void HuyKeyPress(KeyDirectX keyCode)
        {
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
        }
        public static void HuyKeyDown(KeyDirectX keyCode)
        {
            INPUT iNPUT = new INPUT
            {
                Type = 1u
            };
            iNPUT.Data.Keyboard = default(KEYBDINPUT);
            iNPUT.Data.Keyboard.Vk = 0;
            iNPUT.Data.Keyboard.Scan = (ushort)keyCode;
            iNPUT.Data.Keyboard.Flags = 8u;
            iNPUT.Data.Keyboard.Time = 0u;
            iNPUT.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[]
            {
                iNPUT
            };
            if (SendInput(1u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
        }
        public static void HuyKeyUp(KeyDirectX keyCode)
        {
            INPUT iNPUT = new INPUT
            {
                Type = 1u
            };
            iNPUT.Data.Keyboard = default(KEYBDINPUT);
            iNPUT.Data.Keyboard.Vk = 0;
            iNPUT.Data.Keyboard.Scan = (ushort)keyCode;
            iNPUT.Data.Keyboard.Flags = 8u | 2u;
            iNPUT.Data.Keyboard.Time = 0u;
            iNPUT.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[]
            {
                iNPUT
            };
            if (SendInput(1u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
        }
        public static void HuyMouseDown(Point point, EMouseKey eMouse = EMouseKey.LEFT)
        {
            uint flag = 2u;
            if (eMouse != EMouseKey.LEFT)
            {
                flag = 8u;
            }
            INPUT iNPUT = new INPUT
            {
                Type = 0u
            };
            iNPUT.Data.Mouse = default(MOUSEINPUT);
            iNPUT.Data.Mouse.X = CalculateAbsoluteCoordinateX(point.X);
            iNPUT.Data.Mouse.Y = CalculateAbsoluteCoordinateY(point.Y);
            iNPUT.Data.Mouse.MouseData = 0;
            iNPUT.Data.Mouse.Flags = 32768u | 1u | flag;
            iNPUT.Data.Mouse.Time = 0u;
            iNPUT.Data.Mouse.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[]
            {
                iNPUT
            };
            if (SendInput(1u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
        }
        public static void HuyMouseUp(Point point, EMouseKey eMouse = EMouseKey.LEFT)
        {
            uint flag = 4u;
            if (eMouse != EMouseKey.LEFT)
            {
                flag = 16u;
            }
            INPUT iNPUT = new INPUT
            {
                Type = 0u
            };
            iNPUT.Data.Mouse = default(MOUSEINPUT);
            iNPUT.Data.Mouse.X = CalculateAbsoluteCoordinateX(point.X);
            iNPUT.Data.Mouse.Y = CalculateAbsoluteCoordinateY(point.Y);
            iNPUT.Data.Mouse.MouseData = 0;
            iNPUT.Data.Mouse.Flags = 32768u | 1u | flag;
            iNPUT.Data.Mouse.Time = 0u;
            iNPUT.Data.Mouse.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[]
            {
                iNPUT
            };
            if (SendInput(1u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
        }
        public static void HuyMouseClick(Point point, EMouseKey eMouse = EMouseKey.LEFT)
        {
            uint flag = 2u;
            if (eMouse != EMouseKey.LEFT)
            {
                flag = 8u;
            }
            INPUT iNPUT = new INPUT
            {
                Type = 0u
            };
            iNPUT.Data.Mouse = default(MOUSEINPUT);
            iNPUT.Data.Mouse.X = CalculateAbsoluteCoordinateX(point.X);
            iNPUT.Data.Mouse.Y = CalculateAbsoluteCoordinateY(point.Y);
            iNPUT.Data.Mouse.MouseData = 0;
            iNPUT.Data.Mouse.Flags = 32768u | 1u | flag;
            iNPUT.Data.Mouse.Time = 0u;
            iNPUT.Data.Mouse.ExtraInfo = IntPtr.Zero;

            uint flag2 = 4u;
            if (eMouse != EMouseKey.LEFT)
            {
                flag2 = 16u;
            }
            INPUT iNPUT2 = new INPUT
            {
                Type = 0u
            };
            iNPUT2.Data.Mouse = default(MOUSEINPUT);
            iNPUT2.Data.Mouse.X = CalculateAbsoluteCoordinateX(point.X);
            iNPUT2.Data.Mouse.Y = CalculateAbsoluteCoordinateY(point.Y);
            iNPUT2.Data.Mouse.MouseData = 0;
            iNPUT2.Data.Mouse.Flags = 32768u | 1u | flag2;
            iNPUT2.Data.Mouse.Time = 0u;
            iNPUT2.Data.Mouse.ExtraInfo = IntPtr.Zero;
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
        public static void HuyMouseDrag(Point start, Point end)
        {
            HuyMouseDown(start);
            Thread.Sleep(50);
            HuyMouseDown(end);
            Thread.Sleep(50);
            HuyMouseUp(end);
        }
        private void Dota_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Properties.Settings.Default.delay1 = tbKey1.Text + "_" + chbCombo1.Checked.ToString() + "_" + tbCombo1.Text;
            //Properties.Settings.Default.key2 = tbKey2.Text + "_" + chbCombo2.Checked.ToString() + "_" + tbCombo2.Text;
            //Properties.Settings.Default.key3 = tbKey3.Text + "_" + chbCombo3.Checked.ToString() + "_" + tbCombo3.Text;
            //Properties.Settings.Default.key4 = tbKey4.Text + "_" + chbCombo4.Checked.ToString() + "_" + tbCombo4.Text;
            //Properties.Settings.Default.key5 = tbKey5.Text + "_" + chbCombo5.Checked.ToString() + "_" + tbCombo5.Text;
            //Properties.Settings.Default.key6 = tbKey6.Text + "_" + chbCombo6.Checked.ToString() + "_" + tbCombo6.Text;
            //Properties.Settings.Default.key7 = tbKey7.Text + "_" + chbCombo7.Checked.ToString() + "_" + tbCombo7.Text;
            //Properties.Settings.Default.key8 = tbKey8.Text + "_" + chbCombo8.Checked.ToString() + "_" + tbCombo8.Text;
            //Properties.Settings.Default.key9 = tbKey9.Text + "_" + chbCombo9.Checked.ToString() + "_" + tbCombo9.Text;
            //Properties.Settings.Default.key10 = tbKey10.Text + "_" + chbCombo10.Checked.ToString() + "_" + tbCombo10.Text;
            //Properties.Settings.Default.Save();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbSaved.Text.Trim() == "")
            {
                MessageBox.Show("Chưa Nhập Tên !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSaved.Text = "                        ";
                cbSaved.Focus();
            }
            else
            {
                SaveSetting(user);
                user.L_tuongbang = null;
                string json_text = JsonConvert.SerializeObject(user);
                string name = cbSaved.Text.Trim();
                // string save = name + "---" + json_text;
                if (dic_saved.ContainsKey(name))
                {
                    dic_saved[name] = json_text;
                }
                else
                {
                    dic_saved.Add(name, json_text);
                    cbSaved.Items.Insert(0, name);
                }
                string toWrite = "";
                foreach (KeyValuePair<string, string> item in dic_saved)
                {
                    toWrite = item.Key + "---" + item.Value + "\n" + toWrite;
                }
                File.WriteAllText(pathSave, toWrite);
                MessageBox.Show("Saved!");
            }
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            dic_saved.Remove(cbSaved.Text);
            cbSaved.Items.Remove(cbSaved.Text);
            string toWrite = "";
            foreach (KeyValuePair<string, string> item in dic_saved)
            {
                toWrite = item.Key + "---" + item.Value + "\n" + toWrite;
            }
            File.WriteAllText(pathSave, toWrite);
            LoadListKeys();
        }
        private void cbSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSaved.Text != "")
            {
                string json_text = dic_saved[cbSaved.Text];
                SetSetting(json_text);
            }
            LoadListKeys();
        }
        #region Event combo change
        private void chbCombo1_CheckedChanged(object sender, EventArgs e)
        {

            if (chbCombo1.Checked)
            {
                tbKey1.Enabled = false;
                tbCombo1.Enabled = false;
            }
            else
            {
                tbKey1.Enabled = true;
                tbCombo1.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo2_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo2.Checked)
            {
                tbKey2.Enabled = false;
                tbCombo2.Enabled = false;
            }
            else
            {
                tbKey2.Enabled = true;
                tbCombo2.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo3_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo3.Checked)
            {
                tbKey3.Enabled = false;
                tbCombo3.Enabled = false;
            }
            else
            {
                tbKey3.Enabled = true;
                tbCombo3.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo4_CheckedChanged(object sender, EventArgs e)
        {
            if (!runHandler) LoadListKeys();
            if (chbCombo4.Checked)
            {
                tbKey4.Enabled = false;
                tbCombo4.Enabled = false;
            }
            else
            {
                tbKey4.Enabled = true;
                tbCombo4.Enabled = true;
            }
        }
        private void chbCombo5_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo5.Checked)
            {
                tbKey5.Enabled = false;
                tbCombo5.Enabled = false;
            }
            else
            {
                tbKey5.Enabled = true;
                tbCombo5.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo6_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo6.Checked)
            {
                tbKey6.Enabled = false;
                tbCombo6.Enabled = false;
            }
            else
            {
                tbKey6.Enabled = true;
                tbCombo6.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo7_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo7.Checked)
            {
                tbKey7.Enabled = false;
                tbCombo7.Enabled = false;
            }
            else
            {
                tbKey7.Enabled = true;
                tbCombo7.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo8_CheckedChanged(object sender, EventArgs e)
        {

            if (chbCombo8.Checked)
            {
                tbKey8.Enabled = false;
                tbCombo8.Enabled = false;
            }
            else
            {
                tbKey8.Enabled = true;
                tbCombo8.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo9_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo9.Checked)
            {
                tbKey9.Enabled = false;
                tbCombo9.Enabled = false;
            }
            else
            {
                tbKey9.Enabled = true;
                tbCombo9.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        private void chbCombo10_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCombo10.Checked)
            {
                tbKey10.Enabled = false;
                tbCombo10.Enabled = false;
            }
            else
            {
                tbKey10.Enabled = true;
                tbCombo10.Enabled = true;
            }
            if (!runHandler) LoadListKeys();
        }
        #endregion
        private void nudTimeSpan_ValueChanged(object sender, EventArgs e)
        {
            interval = (int)nudTimeSpanIn.Value;
            span_interval = TimeSpan.FromMilliseconds(interval);
        }
        private void nudTimeSpanOut_ValueChanged(object sender, EventArgs e)
        {
            delayBetweenCombos = (int)nudTimeSpanOut.Value;
        }
        public static bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
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
        public static bool CompareBitmapsSlow(Bitmap bmp1, Bitmap bmp2, int percentMin)
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

            int bytesDiff = 0;
            for (int n = 0; n <= bytes - 1; n++)
            {
                if (Math.Abs(b1bytes[n] - b2bytes[n]) > 10)
                {
                    bytesDiff++;
                    //result = false;
                    //break;
                }
            }
            int percent = 100 - (int)(100 * bytesDiff / bytes);
            if (percent < percentMin)
            {
                result = false;
            }
            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }
    }
    public enum KeyDirectX : ushort
    {
        Sleep = 223, Next = 209, Stop = 149, Convert = 121, Decimal = 83, X = 45, Y = 21, Escape = 1, Circumflex = 144, PageDown = 209, DownArrow = 208, RightArrow = 205, LeftArrow = 203, PageUp = 201, UpArrow = 200, RightAlt = 184, NumPadSlash = 181, NumPadPeriod = 83, NumPadPlus = 78, NumPadMinus = 74, CapsLock = 58, LeftAlt = 56, NumPadStar = 55, BackSpace = 14, MediaSelect = 237, Mail = 236, MyComputer = 235, WebBack = 234, WebForward = 233, WebStop = 232, WebRefresh = 231, WebFavorites = 230, WebSearch = 229, Wake = 227, Power = 222, Apps = 221, RightWindows = 220, LeftWindows = 219, Down = 208, End = 207, Prior = 201, Up = 200, Home = 199, RightMenu = 184, SysRq = 183, Divide = 181, NumPadComma = 179, WebHome = 178, VolumeUp = 176, VolumeDown = 174, MediaStop = 164, PlayPause = 162, Calculator = 161, Mute = 160, RightControl = 157, NumPadEnter = 156, NextTrack = 153, Unlabeled = 151, AX = 150, Kanji = 148, Underline = 147, Colon = 146, At = 145, PrevTrack = 144, NumPadEquals = 141, AbntC2 = 126, Yen = 125, NoConvert = 123, AbntC1 = 115, Kana = 112, F15 = 102, F14 = 101, F13 = 100, F12 = 88, F11 = 87, OEM102 = 86, NumPad0 = 82, NumPad3 = 81, NumPad2 = 80, NumPad1 = 79, NumPad6 = 77, NumPad5 = 76, NumPad4 = 75, Subtract = 74, NumPad9 = 73, NumPad8 = 72, NumPad7 = 71, Scroll = 70, Numlock = 69, F10 = 68, F9 = 67, F8 = 66, F7 = 65, F6 = 64, F5 = 63, F4 = 62, F3 = 61, F2 = 60, F1 = 59, Capital = 58, Space = 57, LeftMenu = 56, Multiply = 55, RightShift = 54, Slash = 53, Period = 52, Comma = 51, M = 50, N = 49, B = 48, V = 47, C = 46, Z = 44, BackSlash = 43, LeftShift = 42, Grave = 41, Apostrophe = 40, SemiColon = 39, L = 38, K = 37, J = 36, H = 35, G = 34, F = 33, D = 32, S = 31, A = 30, LeftControl = 29, Return = 28, RightBracket = 27, LeftBracket = 26, P = 25, O = 24, I = 23, U = 22, T = 20, R = 19, E = 18, W = 17, Tab = 15, Back = 14, Equals = 13, Minus = 12, D0 = 11, D9 = 10, D8 = 9, D7 = 8, D6 = 7, D5 = 6, D4 = 5, D3 = 4, D2 = 3, D1 = 2, Insert = 210, Right = 205, Left = 203, Pause = 197, Add = 78, Delete = 211, Q = 16, Nothing = 0
    }
}
public struct INPUT
{
    public uint Type;

    public MOUSEKEYBDHARDWAREINPUT Data;
}
public struct KEYBDINPUT
{
    public ushort Vk;

    public ushort Scan;

    public uint Flags;

    public uint Time;

    public IntPtr ExtraInfo;
}
[StructLayout(LayoutKind.Explicit)]
public struct MOUSEKEYBDHARDWAREINPUT
{
    [FieldOffset(0)]
    public HARDWAREINPUT Hardware;

    [FieldOffset(0)]
    public KEYBDINPUT Keyboard;

    [FieldOffset(0)]
    public MOUSEINPUT Mouse;
}
public struct HARDWAREINPUT
{
    public uint Msg;

    public ushort ParamL;

    public ushort ParamH;
}
public struct MOUSEINPUT
{
    //https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput

    public int X;

    public int Y;

    public uint MouseData;

    public uint Flags;

    public uint Time;

    public IntPtr ExtraInfo;
}
[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public Int32 x;
    public Int32 y;
}
[StructLayout(LayoutKind.Sequential)]
public struct CURSORINFO
{
    public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
                                // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
    public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
                                //    0             The cursor is hidden.
                                //    CURSOR_SHOWING    The cursor is showing.
    public IntPtr hCursor;          // Handle to the cursor. 
    public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
}
public struct RECT
{
    #region Variables.
    /// <summary>
    /// Left position of the rectangle.
    /// </summary>
    public int Left;
    /// <summary>
    /// Top position of the rectangle.
    /// </summary>
    public int Top;
    /// <summary>
    /// Right position of the rectangle.
    /// </summary>
    public int Right;
    /// <summary>
    /// Bottom position of the rectangle.
    /// </summary>
    public int Bottom;
    #endregion

    #region Operators.
    /// <summary>
    /// Operator to convert a RECT to Drawing.Rectangle.
    /// </summary>
    /// <param name="rect">Rectangle to convert.</param>
    /// <returns>A Drawing.Rectangle</returns>
    public static implicit operator Rectangle(RECT rect)
    {
        return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }

    /// <summary>
    /// Operator to convert Drawing.Rectangle to a RECT.
    /// </summary>
    /// <param name="rect">Rectangle to convert.</param>
    /// <returns>RECT rectangle.</returns>
    public static implicit operator RECT(Rectangle rect)
    {
        return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }
    #endregion

    #region Constructor.
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="left">Horizontal position.</param>
    /// <param name="top">Vertical position.</param>
    /// <param name="right">Right most side.</param>
    /// <param name="bottom">Bottom most side.</param>
    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    #endregion
}
