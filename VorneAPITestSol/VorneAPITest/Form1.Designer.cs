
namespace VorneAPITest
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblWC = new System.Windows.Forms.Label();
            this.cbWC = new System.Windows.Forms.ComboBox();
            this.lblClock = new System.Windows.Forms.Label();
            this.lblPS = new System.Windows.Forms.Label();
            this.lblPartID = new System.Windows.Forms.Label();
            this.lblHr = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblSec = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblWC
            // 
            this.lblWC.AutoSize = true;
            this.lblWC.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWC.Location = new System.Drawing.Point(476, 366);
            this.lblWC.Name = "lblWC";
            this.lblWC.Size = new System.Drawing.Size(90, 42);
            this.lblWC.TabIndex = 0;
            this.lblWC.Text = "WC:";
            // 
            // cbWC
            // 
            this.cbWC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWC.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWC.FormattingEnabled = true;
            this.cbWC.Location = new System.Drawing.Point(572, 363);
            this.cbWC.Name = "cbWC";
            this.cbWC.Size = new System.Drawing.Size(150, 50);
            this.cbWC.TabIndex = 1;
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 135.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.Location = new System.Drawing.Point(-25, 105);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(793, 205);
            this.lblClock.TabIndex = 2;
            this.lblClock.Text = "00:00:00";
            // 
            // lblPS
            // 
            this.lblPS.AutoSize = true;
            this.lblPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPS.Location = new System.Drawing.Point(12, 9);
            this.lblPS.Name = "lblPS";
            this.lblPS.Size = new System.Drawing.Size(399, 42);
            this.lblPS.TabIndex = 8;
            this.lblPS.Text = "PRODUCTION STATE";
            // 
            // lblPartID
            // 
            this.lblPartID.AutoSize = true;
            this.lblPartID.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartID.Location = new System.Drawing.Point(537, 9);
            this.lblPartID.Name = "lblPartID";
            this.lblPartID.Size = new System.Drawing.Size(185, 42);
            this.lblPartID.TabIndex = 9;
            this.lblPartID.Text = "PART ID#";
            // 
            // lblHr
            // 
            this.lblHr.AutoSize = true;
            this.lblHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHr.Location = new System.Drawing.Point(90, 105);
            this.lblHr.Name = "lblHr";
            this.lblHr.Size = new System.Drawing.Size(47, 29);
            this.lblHr.TabIndex = 10;
            this.lblHr.Text = "HR";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.Location = new System.Drawing.Point(339, 105);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(57, 29);
            this.lblMin.TabIndex = 11;
            this.lblMin.Text = "MIN";
            // 
            // lblSec
            // 
            this.lblSec.AutoSize = true;
            this.lblSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSec.Location = new System.Drawing.Point(587, 105);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new System.Drawing.Size(62, 29);
            this.lblSec.TabIndex = 12;
            this.lblSec.Text = "SEC";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(12, 366);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(106, 42);
            this.lblTime.TabIndex = 13;
            this.lblTime.Text = "TIME";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 425);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblSec);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblHr);
            this.Controls.Add(this.lblPartID);
            this.Controls.Add(this.lblPS);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.cbWC);
            this.Controls.Add(this.lblWC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblWC;
        private System.Windows.Forms.ComboBox cbWC;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblPS;
        private System.Windows.Forms.Label lblPartID;
        private System.Windows.Forms.Label lblHr;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblSec;
        private System.Windows.Forms.Label lblTime;
    }
}

