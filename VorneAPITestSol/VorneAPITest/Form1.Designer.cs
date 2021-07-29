
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
            this.lblWC = new System.Windows.Forms.Label();
            this.lblClock = new System.Windows.Forms.Label();
            this.lblPS = new System.Windows.Forms.Label();
            this.lblPartID = new System.Windows.Forms.Label();
            this.btnHrInc = new System.Windows.Forms.Button();
            this.btnHrDec = new System.Windows.Forms.Button();
            this.btnMinDec = new System.Windows.Forms.Button();
            this.btnMinInc = new System.Windows.Forms.Button();
            this.btnSecDec = new System.Windows.Forms.Button();
            this.btnSecInc = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.taktTimer = new System.Windows.Forms.Timer(this.components);
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.cbSounds = new System.Windows.Forms.ComboBox();
            this.checkShuffle = new System.Windows.Forms.CheckBox();
            this.nShuffles = new System.Windows.Forms.NumericUpDown();
            this.lblShuffle = new System.Windows.Forms.Label();
            this.playTimer = new System.Windows.Forms.Timer(this.components);
            this.nEfficiency = new System.Windows.Forms.NumericUpDown();
            this.btnCalcTT = new System.Windows.Forms.Button();
            this.lblEfficiency = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nShuffles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEfficiency)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWC
            // 
            this.lblWC.AutoSize = true;
            this.lblWC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWC.Location = new System.Drawing.Point(1294, 816);
            this.lblWC.Name = "lblWC";
            this.lblWC.Size = new System.Drawing.Size(41, 24);
            this.lblWC.TabIndex = 0;
            this.lblWC.Text = "WC";
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 56.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.Location = new System.Drawing.Point(66, 173);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(331, 85);
            this.lblClock.TabIndex = 2;
            this.lblClock.Text = "00:00:00";
            this.lblClock.Click += new System.EventHandler(this.lblClock_Click);
            // 
            // lblPS
            // 
            this.lblPS.AutoSize = true;
            this.lblPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPS.Location = new System.Drawing.Point(77, 32);
            this.lblPS.Name = "lblPS";
            this.lblPS.Size = new System.Drawing.Size(201, 24);
            this.lblPS.TabIndex = 8;
            this.lblPS.Text = "PRODUCTION STATE";
            // 
            // lblPartID
            // 
            this.lblPartID.AutoSize = true;
            this.lblPartID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartID.Location = new System.Drawing.Point(1320, 9);
            this.lblPartID.Name = "lblPartID";
            this.lblPartID.Size = new System.Drawing.Size(92, 24);
            this.lblPartID.TabIndex = 9;
            this.lblPartID.Text = "PART ID#";
            // 
            // btnHrInc
            // 
            this.btnHrInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHrInc.Location = new System.Drawing.Point(167, 97);
            this.btnHrInc.Name = "btnHrInc";
            this.btnHrInc.Size = new System.Drawing.Size(91, 62);
            this.btnHrInc.TabIndex = 14;
            this.btnHrInc.Text = "+";
            this.btnHrInc.UseVisualStyleBackColor = true;
            this.btnHrInc.Click += new System.EventHandler(this.btnHrInc_Click);
            // 
            // btnHrDec
            // 
            this.btnHrDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHrDec.Location = new System.Drawing.Point(167, 634);
            this.btnHrDec.Name = "btnHrDec";
            this.btnHrDec.Size = new System.Drawing.Size(91, 62);
            this.btnHrDec.TabIndex = 15;
            this.btnHrDec.Text = "-";
            this.btnHrDec.UseVisualStyleBackColor = true;
            this.btnHrDec.Click += new System.EventHandler(this.btnHrDec_Click);
            // 
            // btnMinDec
            // 
            this.btnMinDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinDec.Location = new System.Drawing.Point(687, 634);
            this.btnMinDec.Name = "btnMinDec";
            this.btnMinDec.Size = new System.Drawing.Size(91, 62);
            this.btnMinDec.TabIndex = 17;
            this.btnMinDec.Text = "-";
            this.btnMinDec.UseVisualStyleBackColor = true;
            this.btnMinDec.Click += new System.EventHandler(this.btnMinDec_Click);
            // 
            // btnMinInc
            // 
            this.btnMinInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinInc.Location = new System.Drawing.Point(687, 97);
            this.btnMinInc.Name = "btnMinInc";
            this.btnMinInc.Size = new System.Drawing.Size(91, 62);
            this.btnMinInc.TabIndex = 16;
            this.btnMinInc.Text = "+";
            this.btnMinInc.UseVisualStyleBackColor = true;
            this.btnMinInc.Click += new System.EventHandler(this.btnMinInc_Click);
            // 
            // btnSecDec
            // 
            this.btnSecDec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSecDec.Location = new System.Drawing.Point(1224, 634);
            this.btnSecDec.Name = "btnSecDec";
            this.btnSecDec.Size = new System.Drawing.Size(91, 62);
            this.btnSecDec.TabIndex = 19;
            this.btnSecDec.Text = "-";
            this.btnSecDec.UseVisualStyleBackColor = true;
            this.btnSecDec.Click += new System.EventHandler(this.btnSecDec_Click);
            // 
            // btnSecInc
            // 
            this.btnSecInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSecInc.Location = new System.Drawing.Point(1224, 97);
            this.btnSecInc.Name = "btnSecInc";
            this.btnSecInc.Size = new System.Drawing.Size(91, 62);
            this.btnSecInc.TabIndex = 18;
            this.btnSecInc.Text = "+";
            this.btnSecInc.UseVisualStyleBackColor = true;
            this.btnSecInc.Click += new System.EventHandler(this.btnSecInc_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(618, 903);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 54);
            this.btnStart.TabIndex = 20;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(887, 903);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(81, 54);
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
            // pbLogo
            // 
            this.pbLogo.Location = new System.Drawing.Point(516, 5);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(102, 48);
            this.pbLogo.TabIndex = 22;
            this.pbLogo.TabStop = false;
            // 
            // cbSounds
            // 
            this.cbSounds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSounds.FormattingEnabled = true;
            this.cbSounds.Location = new System.Drawing.Point(916, 228);
            this.cbSounds.Name = "cbSounds";
            this.cbSounds.Size = new System.Drawing.Size(121, 21);
            this.cbSounds.TabIndex = 23;
            this.cbSounds.SelectedIndexChanged += new System.EventHandler(this.cbSounds_SelectedIndexChanged);
            // 
            // checkShuffle
            // 
            this.checkShuffle.AutoSize = true;
            this.checkShuffle.Location = new System.Drawing.Point(592, 378);
            this.checkShuffle.Name = "checkShuffle";
            this.checkShuffle.Size = new System.Drawing.Size(59, 17);
            this.checkShuffle.TabIndex = 24;
            this.checkShuffle.Text = "Shuffle";
            this.checkShuffle.UseVisualStyleBackColor = true;
            this.checkShuffle.CheckedChanged += new System.EventHandler(this.checkShuffle_CheckedChanged);
            // 
            // nShuffles
            // 
            this.nShuffles.Location = new System.Drawing.Point(678, 375);
            this.nShuffles.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.nShuffles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nShuffles.Name = "nShuffles";
            this.nShuffles.Size = new System.Drawing.Size(49, 20);
            this.nShuffles.TabIndex = 25;
            this.nShuffles.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nShuffles.ValueChanged += new System.EventHandler(this.nShuffles_ValueChanged);
            // 
            // lblShuffle
            // 
            this.lblShuffle.AutoSize = true;
            this.lblShuffle.Location = new System.Drawing.Point(611, 400);
            this.lblShuffle.Name = "lblShuffle";
            this.lblShuffle.Size = new System.Drawing.Size(95, 13);
            this.lblShuffle.TabIndex = 27;
            this.lblShuffle.Text = "...every x minute(s)";
            // 
            // playTimer
            // 
            this.playTimer.Interval = 60000;
            this.playTimer.Tick += new System.EventHandler(this.playTimer_Tick);
            // 
            // nEfficiency
            // 
            this.nEfficiency.Location = new System.Drawing.Point(286, 377);
            this.nEfficiency.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nEfficiency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nEfficiency.Name = "nEfficiency";
            this.nEfficiency.Size = new System.Drawing.Size(44, 20);
            this.nEfficiency.TabIndex = 28;
            this.nEfficiency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnCalcTT
            // 
            this.btnCalcTT.Location = new System.Drawing.Point(63, 453);
            this.btnCalcTT.Name = "btnCalcTT";
            this.btnCalcTT.Size = new System.Drawing.Size(90, 23);
            this.btnCalcTT.TabIndex = 29;
            this.btnCalcTT.Text = "Get Cycle Time";
            this.btnCalcTT.UseVisualStyleBackColor = true;
            this.btnCalcTT.Click += new System.EventHandler(this.btnCalcTT_Click);
            // 
            // lblEfficiency
            // 
            this.lblEfficiency.AutoSize = true;
            this.lblEfficiency.Location = new System.Drawing.Point(336, 379);
            this.lblEfficiency.Name = "lblEfficiency";
            this.lblEfficiency.Size = new System.Drawing.Size(37, 13);
            this.lblEfficiency.TabIndex = 30;
            this.lblEfficiency.Text = "% EFF";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1829, 1035);
            this.Controls.Add(this.lblEfficiency);
            this.Controls.Add(this.btnCalcTT);
            this.Controls.Add(this.nEfficiency);
            this.Controls.Add(this.lblShuffle);
            this.Controls.Add(this.nShuffles);
            this.Controls.Add(this.checkShuffle);
            this.Controls.Add(this.cbSounds);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSecDec);
            this.Controls.Add(this.btnSecInc);
            this.Controls.Add(this.btnMinDec);
            this.Controls.Add(this.btnMinInc);
            this.Controls.Add(this.btnHrDec);
            this.Controls.Add(this.btnHrInc);
            this.Controls.Add(this.lblPartID);
            this.Controls.Add(this.lblPS);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.lblWC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nShuffles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEfficiency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblWC;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblPS;
        private System.Windows.Forms.Label lblPartID;
        private System.Windows.Forms.Button btnHrInc;
        private System.Windows.Forms.Button btnHrDec;
        private System.Windows.Forms.Button btnMinDec;
        private System.Windows.Forms.Button btnMinInc;
        private System.Windows.Forms.Button btnSecDec;
        private System.Windows.Forms.Button btnSecInc;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer taktTimer;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.ComboBox cbSounds;
        private System.Windows.Forms.CheckBox checkShuffle;
        private System.Windows.Forms.NumericUpDown nShuffles;
        private System.Windows.Forms.Label lblShuffle;
        private System.Windows.Forms.Timer playTimer;
        private System.Windows.Forms.NumericUpDown nEfficiency;
        private System.Windows.Forms.Button btnCalcTT;
        private System.Windows.Forms.Label lblEfficiency;
    }
}

