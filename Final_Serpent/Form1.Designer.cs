﻿namespace Final_Serpent
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gameTimer = new System.Windows.Forms.Timer(components);
            canvas = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // gameTimer
            // 

            ///////////////////////////////////////////////////////////////////////////////// switched this out for faster timer.sd
            //gameTimer.Tick += gameTimer_Tick;
            // 
            // canvas
            // 
            canvas.BorderStyle = BorderStyle.FixedSingle;
            canvas.Location = new Point(10, 10);
            canvas.Margin = new Padding(1);
            canvas.Name = "canvas";
            canvas.Padding = new Padding(1);
            canvas.Size = new Size(800, 500);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += canvas_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 521);
            Controls.Add(canvas);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private PictureBox canvas;
    }
}
