
namespace GameOfSnake
{
    partial class PlayingForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PlayButton = new System.Windows.Forms.Button();
            this.PointsLabel = new System.Windows.Forms.Label();
            this.PointsNumber = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.activeBonusesPictureBox = new System.Windows.Forms.PictureBox();
            this.GameOverLabel = new System.Windows.Forms.Label();
            this.HintLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeBonusesPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGreen;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // PlayButton
            // 
            this.PlayButton.BackgroundImage = global::GameOfSnake.Properties.Resources.ButtonImage;
            this.PlayButton.FlatAppearance.BorderSize = 0;
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PlayButton.Location = new System.Drawing.Point(624, 334);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(140, 37);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // PointsLabel
            // 
            this.PointsLabel.AutoSize = true;
            this.PointsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PointsLabel.Location = new System.Drawing.Point(618, 9);
            this.PointsLabel.Name = "PointsLabel";
            this.PointsLabel.Size = new System.Drawing.Size(90, 31);
            this.PointsLabel.TabIndex = 2;
            this.PointsLabel.Text = "Points";
            // 
            // PointsNumber
            // 
            this.PointsNumber.AutoSize = true;
            this.PointsNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PointsNumber.Location = new System.Drawing.Point(620, 40);
            this.PointsNumber.Name = "PointsNumber";
            this.PointsNumber.Size = new System.Drawing.Size(24, 25);
            this.PointsNumber.TabIndex = 3;
            this.PointsNumber.Text = "0";
            // 
            // buttonBack
            // 
            this.buttonBack.BackgroundImage = global::GameOfSnake.Properties.Resources.ButtonImage;
            this.buttonBack.FlatAppearance.BorderSize = 0;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBack.Location = new System.Drawing.Point(624, 376);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(140, 36);
            this.buttonBack.TabIndex = 4;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // activeBonusesPictureBox
            // 
            this.activeBonusesPictureBox.BackColor = System.Drawing.Color.LimeGreen;
            this.activeBonusesPictureBox.Location = new System.Drawing.Point(625, 68);
            this.activeBonusesPictureBox.Name = "activeBonusesPictureBox";
            this.activeBonusesPictureBox.Size = new System.Drawing.Size(140, 260);
            this.activeBonusesPictureBox.TabIndex = 5;
            this.activeBonusesPictureBox.TabStop = false;
            this.activeBonusesPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.activeBonusesPictureBox_Paint);
            // 
            // GameOverLabel
            // 
            this.GameOverLabel.AutoSize = true;
            this.GameOverLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameOverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GameOverLabel.Location = new System.Drawing.Point(191, 179);
            this.GameOverLabel.Name = "GameOverLabel";
            this.GameOverLabel.Size = new System.Drawing.Size(219, 42);
            this.GameOverLabel.TabIndex = 6;
            this.GameOverLabel.Text = "Game Over";
            this.GameOverLabel.Visible = false;
            // 
            // HintLabel
            // 
            this.HintLabel.AutoSize = true;
            this.HintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HintLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HintLabel.Location = new System.Drawing.Point(8, 415);
            this.HintLabel.Name = "HintLabel";
            this.HintLabel.Size = new System.Drawing.Size(331, 24);
            this.HintLabel.TabIndex = 7;
            this.HintLabel.Text = "Use WASD keys to control your snake!";
            // 
            // PlayingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.HintLabel);
            this.Controls.Add(this.GameOverLabel);
            this.Controls.Add(this.activeBonusesPictureBox);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.PointsNumber);
            this.Controls.Add(this.PointsLabel);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.Name = "PlayingForm";
            this.Text = "Game of Snake";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayingForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PlayingForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeBonusesPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label PointsLabel;
        private System.Windows.Forms.Label PointsNumber;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.PictureBox activeBonusesPictureBox;
        private System.Windows.Forms.Label GameOverLabel;
        private System.Windows.Forms.Label HintLabel;
    }
}

