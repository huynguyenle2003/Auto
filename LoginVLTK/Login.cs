using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginVLTK
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            Process[] procs = Process.GetProcessesByName("vggame");

            IntPtr hWnd = procs.First().MainWindowHandle;
            AutoControl.BringToFront(hWnd);
            //  Form1.HuyMouseClick(sv_point, EMouseKey.LEFT);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            AutoControl.SendKeyPress(KeyCode.ENTER);
            this.Close();
        }
    }
}
