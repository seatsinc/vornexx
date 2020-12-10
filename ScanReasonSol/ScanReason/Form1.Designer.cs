
namespace ScanReason
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.lblScan = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblDownReason = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 399.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScan.Location = new System.Drawing.Point(2, 38);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(1734, 603);
            this.lblScan.TabIndex = 2;
            this.lblScan.Text = "SCAN";
            this.lblScan.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer
            // 
            this.timer.Interval = 2100;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblDownReason
            // 
            this.lblDownReason.AutoSize = true;
            this.lblDownReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 174.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownReason.Location = new System.Drawing.Point(-452, 340);
            this.lblDownReason.Name = "lblDownReason";
            this.lblDownReason.Size = new System.Drawing.Size(1953, 264);
            this.lblDownReason.TabIndex = 3;
            this.lblDownReason.Text = "DOWN  REASON";
            this.lblDownReason.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 600);
            this.Controls.Add(this.lblDownReason);
            this.Controls.Add(this.lblScan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblDownReason;
    }
}

