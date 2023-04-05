
namespace WindowsFormsApp1
{
    partial class frm_TV
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
            this.chbSmoothLHT = new System.Windows.Forms.CheckBox();
            this.chbGocX2 = new System.Windows.Forms.CheckBox();
            this.chbLHT = new System.Windows.Forms.CheckBox();
            this.chbTocBien = new System.Windows.Forms.CheckBox();
            this.chbDoiGoc = new System.Windows.Forms.CheckBox();
            this.tbLengthx2 = new System.Windows.Forms.TextBox();
            this.chbOnHouse = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chbSmoothLHT
            // 
            this.chbSmoothLHT.AutoSize = true;
            this.chbSmoothLHT.Checked = true;
            this.chbSmoothLHT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSmoothLHT.Location = new System.Drawing.Point(12, 12);
            this.chbSmoothLHT.Name = "chbSmoothLHT";
            this.chbSmoothLHT.Size = new System.Drawing.Size(124, 17);
            this.chbSmoothLHT.TabIndex = 1;
            this.chbSmoothLHT.Text = "Smooth LHT ( 2 / 3 )";
            this.chbSmoothLHT.UseVisualStyleBackColor = true;
            // 
            // chbGocX2
            // 
            this.chbGocX2.AutoSize = true;
            this.chbGocX2.Checked = true;
            this.chbGocX2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbGocX2.Location = new System.Drawing.Point(12, 35);
            this.chbGocX2.Name = "chbGocX2";
            this.chbGocX2.Size = new System.Drawing.Size(95, 17);
            this.chbGocX2.TabIndex = 3;
            this.chbGocX2.Text = "Tìm góc X2 (4)";
            this.chbGocX2.UseVisualStyleBackColor = true;
            // 
            // chbLHT
            // 
            this.chbLHT.AutoSize = true;
            this.chbLHT.Checked = true;
            this.chbLHT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbLHT.Location = new System.Drawing.Point(12, 58);
            this.chbLHT.Name = "chbLHT";
            this.chbLHT.Size = new System.Drawing.Size(127, 17);
            this.chbLHT.TabIndex = 8;
            this.chbLHT.Text = "LHT Tai Cho (Space)";
            this.chbLHT.UseVisualStyleBackColor = true;
            // 
            // chbTocBien
            // 
            this.chbTocBien.AutoSize = true;
            this.chbTocBien.Checked = true;
            this.chbTocBien.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTocBien.Location = new System.Drawing.Point(12, 81);
            this.chbTocBien.Name = "chbTocBien";
            this.chbTocBien.Size = new System.Drawing.Size(80, 17);
            this.chbTocBien.TabIndex = 9;
            this.chbTocBien.Text = "Toc bien (f)";
            this.chbTocBien.UseVisualStyleBackColor = true;
            // 
            // chbDoiGoc
            // 
            this.chbDoiGoc.AutoSize = true;
            this.chbDoiGoc.Checked = true;
            this.chbDoiGoc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbDoiGoc.Location = new System.Drawing.Point(12, 104);
            this.chbDoiGoc.Name = "chbDoiGoc";
            this.chbDoiGoc.Size = new System.Drawing.Size(98, 17);
            this.chbDoiGoc.TabIndex = 10;
            this.chbDoiGoc.Text = "Doi goc (arrow)";
            this.chbDoiGoc.UseVisualStyleBackColor = true;
            // 
            // tbLengthx2
            // 
            this.tbLengthx2.Location = new System.Drawing.Point(134, 33);
            this.tbLengthx2.Name = "tbLengthx2";
            this.tbLengthx2.Size = new System.Drawing.Size(36, 20);
            this.tbLengthx2.TabIndex = 11;
            this.tbLengthx2.Text = "100";
            // 
            // chbOnHouse
            // 
            this.chbOnHouse.AutoSize = true;
            this.chbOnHouse.Location = new System.Drawing.Point(110, 104);
            this.chbOnHouse.Name = "chbOnHouse";
            this.chbOnHouse.Size = new System.Drawing.Size(77, 17);
            this.chbOnHouse.TabIndex = 12;
            this.chbOnHouse.Text = "Tren Ngua";
            this.chbOnHouse.UseVisualStyleBackColor = true;
            // 
            // frm_TV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 132);
            this.Controls.Add(this.chbOnHouse);
            this.Controls.Add(this.tbLengthx2);
            this.Controls.Add(this.chbDoiGoc);
            this.Controls.Add(this.chbTocBien);
            this.Controls.Add(this.chbLHT);
            this.Controls.Add(this.chbGocX2);
            this.Controls.Add(this.chbSmoothLHT);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_TV";
            this.Text = "Auto TVB";
            this.Load += new System.EventHandler(this.frm_TV_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbSmoothLHT;
        private System.Windows.Forms.CheckBox chbGocX2;
        private System.Windows.Forms.CheckBox chbLHT;
        private System.Windows.Forms.CheckBox chbTocBien;
        private System.Windows.Forms.CheckBox chbDoiGoc;
        private System.Windows.Forms.TextBox tbLengthx2;
        private System.Windows.Forms.CheckBox chbOnHouse;
    }
}