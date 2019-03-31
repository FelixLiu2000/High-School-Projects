namespace PhysicsElectricFieldsSimulator
{
    partial class Main
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkFixed = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCharge = new System.Windows.Forms.Button();
            this.lblMass = new System.Windows.Forms.Label();
            this.txtMass = new System.Windows.Forms.TextBox();
            this.lblCharge = new System.Windows.Forms.Label();
            this.txtCharge = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkFixed);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnCharge);
            this.panel1.Controls.Add(this.lblMass);
            this.panel1.Controls.Add(this.txtMass);
            this.panel1.Controls.Add(this.lblCharge);
            this.panel1.Controls.Add(this.txtCharge);
            this.panel1.Location = new System.Drawing.Point(12, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(133, 268);
            this.panel1.TabIndex = 0;
            // 
            // chkFixed
            // 
            this.chkFixed.AutoSize = true;
            this.chkFixed.Location = new System.Drawing.Point(15, 101);
            this.chkFixed.Name = "chkFixed";
            this.chkFixed.Size = new System.Drawing.Size(88, 17);
            this.chkFixed.TabIndex = 8;
            this.chkFixed.Text = "Fixed Charge";
            this.chkFixed.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Red;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(15, 217);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 38);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Green;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(15, 173);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 38);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "START SIM";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnCharge
            // 
            this.btnCharge.Location = new System.Drawing.Point(15, 129);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(100, 38);
            this.btnCharge.TabIndex = 5;
            this.btnCharge.Text = "Place New Charge";
            this.btnCharge.UseVisualStyleBackColor = true;
            this.btnCharge.Click += new System.EventHandler(this.BtnCharge_Click);
            // 
            // lblMass
            // 
            this.lblMass.AutoSize = true;
            this.lblMass.Location = new System.Drawing.Point(12, 55);
            this.lblMass.Name = "lblMass";
            this.lblMass.Size = new System.Drawing.Size(32, 13);
            this.lblMass.TabIndex = 4;
            this.lblMass.Text = "Mass";
            // 
            // txtMass
            // 
            this.txtMass.Location = new System.Drawing.Point(15, 74);
            this.txtMass.MaxLength = 4;
            this.txtMass.Name = "txtMass";
            this.txtMass.Size = new System.Drawing.Size(100, 20);
            this.txtMass.TabIndex = 2;
            // 
            // lblCharge
            // 
            this.lblCharge.AutoSize = true;
            this.lblCharge.Location = new System.Drawing.Point(12, 13);
            this.lblCharge.Name = "lblCharge";
            this.lblCharge.Size = new System.Drawing.Size(64, 13);
            this.lblCharge.TabIndex = 2;
            this.lblCharge.Text = "Charge (+/-)";
            // 
            // txtCharge
            // 
            this.txtCharge.Location = new System.Drawing.Point(15, 32);
            this.txtCharge.MaxLength = 4;
            this.txtCharge.Name = "txtCharge";
            this.txtCharge.Size = new System.Drawing.Size(100, 20);
            this.txtCharge.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(9, 390);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(163, 15);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Press Start Sim to Begin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 45);
            this.label1.TabIndex = 9;
            this.label1.Text = "Felix Liu\r\nSPH4U1\r\nMr. Adams\r\n";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "Main";
            this.Text = "FelixElectricFieldsSimulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Main_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMass;
        private System.Windows.Forms.TextBox txtMass;
        private System.Windows.Forms.Label lblCharge;
        private System.Windows.Forms.TextBox txtCharge;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFixed;
    }
}

