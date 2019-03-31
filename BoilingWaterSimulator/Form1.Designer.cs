namespace FelixA2WaterSimulator
{
    partial class FelixA2WaterSimulator
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
            this.txtHeatInput = new System.Windows.Forms.TextBox();
            this.btnHeatSubmit = new System.Windows.Forms.Button();
            this.btnHeatSwitch = new System.Windows.Forms.Button();
            this.lblHeatStatus = new System.Windows.Forms.Label();
            this.btnEnableColors = new System.Windows.Forms.Button();
            this.lblColorsStatus = new System.Windows.Forms.Label();
            this.btnAddWater = new System.Windows.Forms.Button();
            this.lblGreatestHeat = new System.Windows.Forms.Label();
            this.lblLowestHeat = new System.Windows.Forms.Label();
            this.lblStartingPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtHeatInput
            // 
            this.txtHeatInput.Location = new System.Drawing.Point(8, 105);
            this.txtHeatInput.MaxLength = 2;
            this.txtHeatInput.Name = "txtHeatInput";
            this.txtHeatInput.Size = new System.Drawing.Size(110, 20);
            this.txtHeatInput.TabIndex = 0;
            this.txtHeatInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnHeatSubmit
            // 
            this.btnHeatSubmit.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btnHeatSubmit.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeatSubmit.Location = new System.Drawing.Point(8, 131);
            this.btnHeatSubmit.Name = "btnHeatSubmit";
            this.btnHeatSubmit.Size = new System.Drawing.Size(110, 27);
            this.btnHeatSubmit.TabIndex = 1;
            this.btnHeatSubmit.Text = "Set Heat (0-10)";
            this.btnHeatSubmit.UseVisualStyleBackColor = false;
            this.btnHeatSubmit.Click += new System.EventHandler(this.btnHeatSubmit_Click);
            // 
            // btnHeatSwitch
            // 
            this.btnHeatSwitch.BackColor = System.Drawing.Color.Maroon;
            this.btnHeatSwitch.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeatSwitch.ForeColor = System.Drawing.Color.White;
            this.btnHeatSwitch.Location = new System.Drawing.Point(19, 44);
            this.btnHeatSwitch.Name = "btnHeatSwitch";
            this.btnHeatSwitch.Size = new System.Drawing.Size(87, 45);
            this.btnHeatSwitch.TabIndex = 2;
            this.btnHeatSwitch.Text = "Heat Master Switch";
            this.btnHeatSwitch.UseVisualStyleBackColor = false;
            this.btnHeatSwitch.Click += new System.EventHandler(this.btnHeatSwitch_Click);
            // 
            // lblHeatStatus
            // 
            this.lblHeatStatus.BackColor = System.Drawing.Color.Black;
            this.lblHeatStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHeatStatus.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeatStatus.Location = new System.Drawing.Point(8, 9);
            this.lblHeatStatus.Name = "lblHeatStatus";
            this.lblHeatStatus.Size = new System.Drawing.Size(110, 29);
            this.lblHeatStatus.TabIndex = 3;
            this.lblHeatStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnableColors
            // 
            this.btnEnableColors.BackColor = System.Drawing.Color.Coral;
            this.btnEnableColors.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnableColors.Location = new System.Drawing.Point(8, 204);
            this.btnEnableColors.Name = "btnEnableColors";
            this.btnEnableColors.Size = new System.Drawing.Size(110, 35);
            this.btnEnableColors.TabIndex = 4;
            this.btnEnableColors.Text = "Enable IR Display";
            this.btnEnableColors.UseVisualStyleBackColor = false;
            this.btnEnableColors.Click += new System.EventHandler(this.btnEnableColors_Click);
            // 
            // lblColorsStatus
            // 
            this.lblColorsStatus.BackColor = System.Drawing.Color.Black;
            this.lblColorsStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColorsStatus.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorsStatus.Location = new System.Drawing.Point(8, 172);
            this.lblColorsStatus.Name = "lblColorsStatus";
            this.lblColorsStatus.Size = new System.Drawing.Size(110, 29);
            this.lblColorsStatus.TabIndex = 5;
            this.lblColorsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddWater
            // 
            this.btnAddWater.BackColor = System.Drawing.Color.White;
            this.btnAddWater.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddWater.ForeColor = System.Drawing.Color.Navy;
            this.btnAddWater.Location = new System.Drawing.Point(8, 253);
            this.btnAddWater.Name = "btnAddWater";
            this.btnAddWater.Size = new System.Drawing.Size(110, 43);
            this.btnAddWater.TabIndex = 6;
            this.btnAddWater.UseVisualStyleBackColor = false;
            this.btnAddWater.Click += new System.EventHandler(this.btnAddWater_Click);
            // 
            // lblGreatestHeat
            // 
            this.lblGreatestHeat.AutoSize = true;
            this.lblGreatestHeat.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreatestHeat.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblGreatestHeat.Location = new System.Drawing.Point(5, 318);
            this.lblGreatestHeat.Name = "lblGreatestHeat";
            this.lblGreatestHeat.Size = new System.Drawing.Size(0, 17);
            this.lblGreatestHeat.TabIndex = 7;
            // 
            // lblLowestHeat
            // 
            this.lblLowestHeat.AutoSize = true;
            this.lblLowestHeat.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowestHeat.ForeColor = System.Drawing.Color.Firebrick;
            this.lblLowestHeat.Location = new System.Drawing.Point(5, 387);
            this.lblLowestHeat.Name = "lblLowestHeat";
            this.lblLowestHeat.Size = new System.Drawing.Size(0, 17);
            this.lblLowestHeat.TabIndex = 8;
            // 
            // lblStartingPrompt
            // 
            this.lblStartingPrompt.AutoSize = true;
            this.lblStartingPrompt.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartingPrompt.Location = new System.Drawing.Point(5, 659);
            this.lblStartingPrompt.Name = "lblStartingPrompt";
            this.lblStartingPrompt.Size = new System.Drawing.Size(262, 36);
            this.lblStartingPrompt.TabIndex = 9;
            this.lblStartingPrompt.Text = "-Press \"Enter\" to Start and Pause Sim\r\n-Control Simulation With Buttons";
            // 
            // FelixA2WaterSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 704);
            this.Controls.Add(this.lblStartingPrompt);
            this.Controls.Add(this.lblLowestHeat);
            this.Controls.Add(this.lblGreatestHeat);
            this.Controls.Add(this.btnAddWater);
            this.Controls.Add(this.lblColorsStatus);
            this.Controls.Add(this.btnEnableColors);
            this.Controls.Add(this.lblHeatStatus);
            this.Controls.Add(this.btnHeatSwitch);
            this.Controls.Add(this.btnHeatSubmit);
            this.Controls.Add(this.txtHeatInput);
            this.DoubleBuffered = true;
            this.Name = "FelixA2WaterSimulator";
            this.Text = "Boiling Water Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FelixA2WaterSimulator_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHeatInput;
        private System.Windows.Forms.Button btnHeatSubmit;
        private System.Windows.Forms.Button btnHeatSwitch;
        private System.Windows.Forms.Label lblHeatStatus;
        private System.Windows.Forms.Button btnEnableColors;
        private System.Windows.Forms.Label lblColorsStatus;
        private System.Windows.Forms.Button btnAddWater;
        private System.Windows.Forms.Label lblGreatestHeat;
        private System.Windows.Forms.Label lblLowestHeat;
        private System.Windows.Forms.Label lblStartingPrompt;
    }
}

