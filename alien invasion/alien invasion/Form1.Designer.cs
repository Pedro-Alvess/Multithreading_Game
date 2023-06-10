namespace alien_invasion
{
    partial class Fase_1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fase_1));
            this.label1 = new System.Windows.Forms.Label();
            this.socore = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shield = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "SCORE:";
            // 
            // socore
            // 
            this.socore.AutoSize = true;
            this.socore.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.socore.ForeColor = System.Drawing.Color.White;
            this.socore.Location = new System.Drawing.Point(92, 12);
            this.socore.Name = "socore";
            this.socore.Size = new System.Drawing.Size(18, 19);
            this.socore.TabIndex = 6;
            this.socore.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(640, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "SHIELD:";
            // 
            // shield
            // 
            this.shield.AutoSize = true;
            this.shield.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shield.ForeColor = System.Drawing.Color.White;
            this.shield.Location = new System.Drawing.Point(713, 12);
            this.shield.Name = "shield";
            this.shield.Size = new System.Drawing.Size(55, 19);
            this.shield.TabIndex = 8;
            this.shield.Text = "100 %";
            // 
            // Fase_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.shield);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.socore);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fase_1";
            this.Text = "Alien Invasion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label socore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label shield;
    }
}

