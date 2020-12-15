
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
            this.lblClock = new System.Windows.Forms.Label();
            this.lblPS = new System.Windows.Forms.Label();
            this.lblPartID = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnHrInc = new System.Windows.Forms.Button();
            this.btnHrDec = new System.Windows.Forms.Button();
            this.btnMinDec = new System.Windows.Forms.Button();
            this.btnMinInc = new System.Windows.Forms.Button();
            this.btnSecDec = new System.Windows.Forms.Button();
            this.btnSecInc = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.taktTimer = new System.Windows.Forms.Timer(this.components);
            this.cbPorts = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lightTimer = new System.Windows.Forms.Timer(this.components);
            this.lblClients = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblWC
            // 
            this.lblWC.AutoSize = true;
            this.lblWC.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWC.Location = new System.Drawing.Point(1294, 816);
            this.lblWC.Name = "lblWC";
            this.lblWC.Size = new System.Drawing.Size(205, 108);
            this.lblWC.TabIndex = 0;
            this.lblWC.Text = "WC";
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 300F);
            this.lblClock.Location = new System.Drawing.Point(66, 173);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(1744, 453);
            this.lblClock.TabIndex = 2;
            this.lblClock.Text = "00:00:00";
            // 
            // lblPS
            // 
            this.lblPS.AutoSize = true;
            this.lblPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPS.Location = new System.Drawing.Point(12, 28);
            this.lblPS.Name = "lblPS";
            this.lblPS.Size = new System.Drawing.Size(1026, 108);
            this.lblPS.TabIndex = 8;
            this.lblPS.Text = "PRODUCTION STATE";
            // 
            // lblPartID
            // 
            this.lblPartID.AutoSize = true;
            this.lblPartID.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartID.Location = new System.Drawing.Point(1320, 9);
            this.lblPartID.Name = "lblPartID";
            this.lblPartID.Size = new System.Drawing.Size(476, 108);
            this.lblPartID.TabIndex = 9;
            this.lblPartID.Text = "PART ID#";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(67, 866);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(275, 108);
            this.lblTime.TabIndex = 13;
            this.lblTime.Text = "TIME";
            // 
            // btnHrInc
            // 
            this.btnHrInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHrInc.Location = new System.Drawing.Point(167, 41);
            this.btnHrInc.Name = "btnHrInc";
            this.btnHrInc.Size = new System.Drawing.Size(409, 118);
            this.btnHrInc.TabIndex = 14;
            this.btnHrInc.Text = "+";
            this.btnHrInc.UseVisualStyleBackColor = true;
            this.btnHrInc.Click += new System.EventHandler(this.btnHrInc_Click);
            // 
            // btnHrDec
            // 
            this.btnHrDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHrDec.Location = new System.Drawing.Point(167, 578);
            this.btnHrDec.Name = "btnHrDec";
            this.btnHrDec.Size = new System.Drawing.Size(409, 118);
            this.btnHrDec.TabIndex = 15;
            this.btnHrDec.Text = "-";
            this.btnHrDec.UseVisualStyleBackColor = true;
            this.btnHrDec.Click += new System.EventHandler(this.btnHrDec_Click);
            // 
            // btnMinDec
            // 
            this.btnMinDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinDec.Location = new System.Drawing.Point(687, 578);
            this.btnMinDec.Name = "btnMinDec";
            this.btnMinDec.Size = new System.Drawing.Size(409, 118);
            this.btnMinDec.TabIndex = 17;
            this.btnMinDec.Text = "-";
            this.btnMinDec.UseVisualStyleBackColor = true;
            this.btnMinDec.Click += new System.EventHandler(this.btnMinDec_Click);
            // 
            // btnMinInc
            // 
            this.btnMinInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinInc.Location = new System.Drawing.Point(687, 41);
            this.btnMinInc.Name = "btnMinInc";
            this.btnMinInc.Size = new System.Drawing.Size(409, 118);
            this.btnMinInc.TabIndex = 16;
            this.btnMinInc.Text = "+";
            this.btnMinInc.UseVisualStyleBackColor = true;
            this.btnMinInc.Click += new System.EventHandler(this.btnMinInc_Click);
            // 
            // btnSecDec
            // 
            this.btnSecDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSecDec.Location = new System.Drawing.Point(1224, 578);
            this.btnSecDec.Name = "btnSecDec";
            this.btnSecDec.Size = new System.Drawing.Size(409, 118);
            this.btnSecDec.TabIndex = 19;
            this.btnSecDec.Text = "-";
            this.btnSecDec.UseVisualStyleBackColor = true;
            this.btnSecDec.Click += new System.EventHandler(this.btnSecDec_Click);
            // 
            // btnSecInc
            // 
            this.btnSecInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSecInc.Location = new System.Drawing.Point(1224, 41);
            this.btnSecInc.Name = "btnSecInc";
            this.btnSecInc.Size = new System.Drawing.Size(409, 118);
            this.btnSecInc.TabIndex = 18;
            this.btnSecInc.Text = "+";
            this.btnSecInc.UseVisualStyleBackColor = true;
            this.btnSecInc.Click += new System.EventHandler(this.btnSecInc_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(618, 866);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(263, 91);
            this.btnStart.TabIndex = 20;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(887, 866);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(222, 91);
            this.btnStop.TabIndex = 21;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // taktTimer
            // 
            this.taktTimer.Interval = 1000;
            this.taktTimer.Tick += new System.EventHandler(this.taktTimer_Tick);
            // 
            // cbPorts
            // 
            this.cbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPorts.FormattingEnabled = true;
            this.cbPorts.Location = new System.Drawing.Point(794, 756);
            this.cbPorts.Name = "cbPorts";
            this.cbPorts.Size = new System.Drawing.Size(302, 81);
            this.cbPorts.TabIndex = 22;
            this.cbPorts.SelectedIndexChanged += new System.EventHandler(this.cbPorts_SelectedIndexChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(535, 756);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(228, 73);
            this.lblPort.TabIndex = 23;
            this.lblPort.Text = "PORT:";
            // 
            // lightTimer
            // 
            this.lightTimer.Interval = 1500;
            this.lightTimer.Tick += new System.EventHandler(this.lightTimer_Tick);
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(25, 196);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(48, 13);
            this.lblClients.TabIndex = 24;
            this.lblClients.Text = "lblClients";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1829, 1035);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.cbPorts);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSecDec);
            this.Controls.Add(this.btnSecInc);
            this.Controls.Add(this.btnMinDec);
            this.Controls.Add(this.btnMinInc);
            this.Controls.Add(this.btnHrDec);
            this.Controls.Add(this.btnHrInc);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblPartID);
            this.Controls.Add(this.lblPS);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.lblWC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblWC;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblPS;
        private System.Windows.Forms.Label lblPartID;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnHrInc;
        private System.Windows.Forms.Button btnHrDec;
        private System.Windows.Forms.Button btnMinDec;
        private System.Windows.Forms.Button btnMinInc;
        private System.Windows.Forms.Button btnSecDec;
        private System.Windows.Forms.Button btnSecInc;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer taktTimer;
        private System.Windows.Forms.ComboBox cbPorts;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Timer lightTimer;
        private System.Windows.Forms.Label lblClients;
    }
}

