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
            this.score = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shield = new System.Windows.Forms.Label();
            this.lblGameOver = new System.Windows.Forms.Label();
            this.lblwin = new System.Windows.Forms.Label();
            this.pbTrohpy = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrohpy)).BeginInit();
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
            // score
            // 
            this.score.AutoSize = true;
            this.score.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.score.ForeColor = System.Drawing.Color.White;
            this.score.Location = new System.Drawing.Point(92, 12);
            this.score.Name = "score";
            this.score.Size = new System.Drawing.Size(18, 19);
            this.score.TabIndex = 6;
            this.score.Text = "0";
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
            // lblGameOver
            // 
            this.lblGameOver.AutoSize = true;
            this.lblGameOver.Font = new System.Drawing.Font("Felix Titling", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameOver.ForeColor = System.Drawing.Color.White;
            this.lblGameOver.Location = new System.Drawing.Point(275, 200);
            this.lblGameOver.Name = "lblGameOver";
            this.lblGameOver.Size = new System.Drawing.Size(271, 47);
            this.lblGameOver.TabIndex = 9;
            this.lblGameOver.Text = "GAME OVER";
            this.lblGameOver.Visible = false;
            // 
            // lblwin
            // 
            this.lblwin.AutoSize = true;
            this.lblwin.Font = new System.Drawing.Font("Felix Titling", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblwin.ForeColor = System.Drawing.Color.White;
            this.lblwin.Location = new System.Drawing.Point(305, 200);
            this.lblwin.Name = "lblwin";
            this.lblwin.Size = new System.Drawing.Size(229, 47);
            this.lblwin.TabIndex = 10;
            this.lblwin.Text = "YOU WIN !";
            this.lblwin.Visible = false;
            // 
            // pbTrohpy
            // 
            this.pbTrohpy.Image = ((System.Drawing.Image)(resources.GetObject("pbTrohpy.Image")));
            this.pbTrohpy.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbTrohpy.InitialImage")));
            this.pbTrohpy.Location = new System.Drawing.Point(377, 133);
            this.pbTrohpy.Name = "pbTrohpy";
            this.pbTrohpy.Size = new System.Drawing.Size(62, 64);
            this.pbTrohpy.TabIndex = 11;
            this.pbTrohpy.TabStop = false;
            this.pbTrohpy.Visible = false;
            // 
            // Fase_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.pbTrohpy);
            this.Controls.Add(this.lblwin);
            this.Controls.Add(this.lblGameOver);
            this.Controls.Add(this.shield);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.score);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fase_1";
            this.Text = "Alien Invasion";
            ((System.ComponentModel.ISupportInitialize)(this.pbTrohpy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label score;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label shield;
        private System.Windows.Forms.Label lblGameOver;
        private System.Windows.Forms.Label lblwin;
        private System.Windows.Forms.PictureBox pbTrohpy;
    }
}

