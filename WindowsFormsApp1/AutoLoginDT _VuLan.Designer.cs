
namespace WindowsFormsApp1
{
    partial class AutoLoginDT_VuLan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btPathVLTK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_Sound = new System.Windows.Forms.CheckBox();
            this.lb_path_auto = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btPathAuto = new System.Windows.Forms.Button();
            this.btTest = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btLogin = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lb_status = new System.Windows.Forms.Label();
            this.lb_path = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đường dẫn VLTK :";
            // 
            // btPathVLTK
            // 
            this.btPathVLTK.Location = new System.Drawing.Point(144, 14);
            this.btPathVLTK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPathVLTK.Name = "btPathVLTK";
            this.btPathVLTK.Size = new System.Drawing.Size(39, 28);
            this.btPathVLTK.TabIndex = 1;
            this.btPathVLTK.Text = "...";
            this.btPathVLTK.UseVisualStyleBackColor = true;
            this.btPathVLTK.Click += new System.EventHandler(this.btPathVLTK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chb_Sound);
            this.groupBox1.Controls.Add(this.lb_path_auto);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btPathAuto);
            this.groupBox1.Controls.Add(this.btTest);
            this.groupBox1.Controls.Add(this.btStop);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btLogin);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.lb_status);
            this.groupBox1.Controls.Add(this.lb_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btPathVLTK);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1035, 135);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cài đặt";
            // 
            // chb_Sound
            // 
            this.chb_Sound.AutoSize = true;
            this.chb_Sound.Location = new System.Drawing.Point(159, 106);
            this.chb_Sound.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chb_Sound.Name = "chb_Sound";
            this.chb_Sound.Size = new System.Drawing.Size(190, 20);
            this.chb_Sound.TabIndex = 10;
            this.chb_Sound.Text = "Rung chuông khi xong 1acc";
            this.chb_Sound.UseVisualStyleBackColor = true;
            // 
            // lb_path_auto
            // 
            this.lb_path_auto.AutoSize = true;
            this.lb_path_auto.Location = new System.Drawing.Point(183, 49);
            this.lb_path_auto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_path_auto.Name = "lb_path_auto";
            this.lb_path_auto.Size = new System.Drawing.Size(0, 16);
            this.lb_path_auto.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Đường dẫn Auto :";
            // 
            // btPathAuto
            // 
            this.btPathAuto.Location = new System.Drawing.Point(144, 43);
            this.btPathAuto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPathAuto.Name = "btPathAuto";
            this.btPathAuto.Size = new System.Drawing.Size(39, 28);
            this.btPathAuto.TabIndex = 8;
            this.btPathAuto.Text = "...";
            this.btPathAuto.UseVisualStyleBackColor = true;
            this.btPathAuto.Click += new System.EventHandler(this.btPathAuto_Click);
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(711, 97);
            this.btTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(100, 28);
            this.btTest.TabIndex = 6;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(927, 97);
            this.btStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(100, 28);
            this.btStop.TabIndex = 4;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 107);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max Acc";
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(819, 97);
            this.btLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(100, 28);
            this.btLogin.TabIndex = 3;
            this.btLogin.Text = "Start";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(84, 105);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(67, 22);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Location = new System.Drawing.Point(11, 79);
            this.lb_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(45, 16);
            this.lb_status.TabIndex = 3;
            this.lb_status.Text = "Status";
            // 
            // lb_path
            // 
            this.lb_path.AutoSize = true;
            this.lb_path.Location = new System.Drawing.Point(183, 20);
            this.lb_path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_path.Name = "lb_path";
            this.lb_path.Size = new System.Drawing.Size(0, 16);
            this.lb_path.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // AutoLoginDT_VuLan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 165);
            this.Controls.Add(this.groupBox1);
            this.Location = new System.Drawing.Point(-10, 875);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AutoLoginDT_VuLan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutoLoginDT_VuLan v1.2";
            this.Load += new System.EventHandler(this.AutoLoginDT_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btPathVLTK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_path;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Label lb_status;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btPathAuto;
        private System.Windows.Forms.Label lb_path_auto;
        private System.Windows.Forms.CheckBox chb_Sound;
    }
}