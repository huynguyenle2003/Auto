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
        Bitmap enemy;
        Bitmap ally;
        private int inner_interval = 1; //milisecond
        private int interval = 50; //milisecond
        private int delayBetweenCombos = 500; //milisecond
        private TimeSpan span_interval = TimeSpan.FromMilliseconds(20);
        // private int clearComboOrder = 1;//second
        bool runHandler = true;
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
        private void Dota_Load(object sender, EventArgs e)
        {
            enemy = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Img\CursorEnemy.PNG");
            ally = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\Img\CursorAlly.PNG");
            keyConvert = new KeysConverter();
            nudTimeSpanIn.Controls.RemoveAt(0);
            nudTimeSpanOut.Controls.RemoveAt(0);
            lastUpdateOrder = DateTime.Now;
            delayBetweenCombos = (int)nudTimeSpanOut.Value;
            interval = (int)nudTimeSpanIn.Value;
            span_interval = TimeSpan.FromMilliseconds(interval);
            //HookManager.KeyPress += HookManager_KeyPress;
            HookManager.KeyDown += HookManager_KeyDown;
            // HookManager.KeyUp += HookManager_KeyUp;

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

            //ComboKeyOption testOptions = new ComboKeyOption();
            //File.WriteAllText(pathOption, JsonConvert.SerializeObject(testOptions));

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
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            //  DateTime start = DateTime.Now;
            HookManager.KeyDown -= HookManager_KeyDown;

            foreach (ComboKey comboKey in l_keys)
            {
                if (comboKey.Keys == e.KeyCode)
                {
                    if ((DateTime.Now - comboKey.LastCall).TotalMilliseconds > delayBetweenCombos)
                    {
                        if (IsCorrectCursor(comboKey))
                        {
                            comboKey.LastCall = DateTime.Now;
                            //await Combo(comboKey.Combo);
                            Combo(comboKey);
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
                            comboKey.LastCall = DateTime.Now;
                            Combo(comboKey);
                            //comboOrder = "";
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
                default:
                    return true;
            }
        }
        //public void Combo(List<KeyDirectX> tbCombo)
        public void Combo(ComboKey combo)
        {
            Point current = GetCursorPoint();
            RECT current_rect;
            GetClipCursor(out current_rect);
            if (combo.Option.LockMouseUntilPressThis != KeyDirectX.Nothing || combo.Option.SpecialHero == "Tinker")
            {
                int offset = 5;
                RECT rect = new RECT(current.X - offset, current.Y - offset, current.X + offset, current.Y + offset);
                ClipCursor(ref rect);
            }
            bool exitLoop = false;
            foreach (KeyDirectX item in combo.Combo)
            {
                Thread.Sleep(inner_interval);
                if (item == KeyDirectX.NumPadStar)
                {
                    Thread.Sleep(interval);
                }
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
                                if (!IsCorrectCursor(combo))
                                {
                                    ClipCursor(ref current_rect);
                                    exitLoop = true;
                                    break;
                                }
                                HuyKeyPress(item);
                                //Check anh nut Q
                                Thread.Sleep(inner_interval);
                                ClipCursor(ref current_rect);
                                int lanthu = 0;
                                chaylai:
                                lanthu++;
                                if (lanthu > 4)
                                {
                                    HuyKeyPress(KeyDirectX.S);
                                    exitLoop = true;
                                    break;
                                }
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
                                    if (options.BlinkDelay > 0)
                                    {
                                        Thread.Sleep(options.BlinkDelay);
                                    }
                                    HuyKeyPress(item);
                                }
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
            ClipCursor(ref current_rect);
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
        public static void HuyAltKeyPress(KeyDirectX keyCode)
        {
            INPUT iNPUT0 = new INPUT
            {
                Type = 1u
            };
            iNPUT0.Data.Keyboard = new KEYBDINPUT
            {
                Vk = 0,
                Scan = 56,
                Flags = 8u,
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            INPUT iNPUT1 = new INPUT
            {
                Type = 1u
            };
            iNPUT1.Data.Keyboard = new KEYBDINPUT
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
            INPUT iNPUT3 = new INPUT
            {
                Type = 1u
            };
            iNPUT2.Data.Keyboard = new KEYBDINPUT
            {
                Vk = 0,
                Scan = 56,
                Flags = 8u | 2u,
                Time = 0u,
                ExtraInfo = IntPtr.Zero
            };
            INPUT[] inputs = new INPUT[]
            {
                iNPUT0,
                iNPUT1,
                iNPUT2,
                iNPUT3,
            };
            if (SendInput(4u, inputs, Marshal.SizeOf(typeof(INPUT))) == 0u)
            {
                throw new Exception();
            }
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