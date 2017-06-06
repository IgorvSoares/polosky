namespace Main
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
            this.components = new System.ComponentModel.Container();
            this.timerDate = new System.Windows.Forms.Timer(this.components);
            this.timerDynamicForm = new System.Windows.Forms.Timer(this.components);
            this.timer_ScreenSaver = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxLogout = new System.Windows.Forms.PictureBox();
            this.Pic_Background = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Background)).BeginInit();
            this.SuspendLayout();
            // 
            // timerDate
            // 
            this.timerDate.Tick += new System.EventHandler(this.timerDate_Tick);
            // 
            // timerDynamicForm
            // 
            this.timerDynamicForm.Interval = 10000;
            this.timerDynamicForm.Tick += new System.EventHandler(this.timerDynamicForm_Tick);
            // 
            // timer_ScreenSaver
            // 
            this.timer_ScreenSaver.Interval = 10000;
            this.timer_ScreenSaver.Tick += new System.EventHandler(this.timer_ScreenSaver_Tick);
            // 
            // pictureBoxLogout
            // 
            this.pictureBoxLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxLogout.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogout.Location = new System.Drawing.Point(-19, 744);
            this.pictureBoxLogout.Name = "pictureBoxLogout";
            this.pictureBoxLogout.Size = new System.Drawing.Size(91, 77);
            this.pictureBoxLogout.TabIndex = 0;
            this.pictureBoxLogout.TabStop = false;
            this.pictureBoxLogout.DoubleClick += new System.EventHandler(this.pictureBoxLogout_DoubleClick);
            // 
            // Pic_Background
            // 
            this.Pic_Background.BackColor = System.Drawing.Color.Transparent;
            this.Pic_Background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_Background.Location = new System.Drawing.Point(0, 0);
            this.Pic_Background.Name = "Pic_Background";
            this.Pic_Background.Size = new System.Drawing.Size(1140, 800);
            this.Pic_Background.TabIndex = 1;
            this.Pic_Background.TabStop = false;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1140, 800);
            this.Controls.Add(this.pictureBoxLogout);
            this.Controls.Add(this.Pic_Background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerDate;
        private System.Windows.Forms.Timer timerDynamicForm;
        public System.Windows.Forms.Timer timer_ScreenSaver;
        private System.Windows.Forms.PictureBox pictureBoxLogout;
        public System.Windows.Forms.PictureBox Pic_Background;


    }
}

