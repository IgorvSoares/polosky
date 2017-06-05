namespace PianoKeyboard
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
            this.buttonDo = new System.Windows.Forms.Button();
            this.buttonRe = new System.Windows.Forms.Button();
            this.buttonMi = new System.Windows.Forms.Button();
            this.buttonFa = new System.Windows.Forms.Button();
            this.buttonSol = new System.Windows.Forms.Button();
            this.buttonLa = new System.Windows.Forms.Button();
            this.buttonSi = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonDo
            // 
            this.buttonDo.Location = new System.Drawing.Point(26, 191);
            this.buttonDo.Name = "buttonDo";
            this.buttonDo.Size = new System.Drawing.Size(75, 155);
            this.buttonDo.TabIndex = 0;
            this.buttonDo.Text = "Do";
            this.buttonDo.UseVisualStyleBackColor = true;
            this.buttonDo.Click += new System.EventHandler(this.buttonDo_Click);
            // 
            // buttonRe
            // 
            this.buttonRe.Location = new System.Drawing.Point(124, 191);
            this.buttonRe.Name = "buttonRe";
            this.buttonRe.Size = new System.Drawing.Size(75, 155);
            this.buttonRe.TabIndex = 0;
            this.buttonRe.Text = "Re";
            this.buttonRe.UseVisualStyleBackColor = true;
            this.buttonRe.Click += new System.EventHandler(this.buttonRe_Click);
            // 
            // buttonMi
            // 
            this.buttonMi.Location = new System.Drawing.Point(235, 191);
            this.buttonMi.Name = "buttonMi";
            this.buttonMi.Size = new System.Drawing.Size(75, 155);
            this.buttonMi.TabIndex = 1;
            this.buttonMi.Text = "Mi";
            this.buttonMi.UseVisualStyleBackColor = true;
            this.buttonMi.Click += new System.EventHandler(this.buttonMi_Click);
            // 
            // buttonFa
            // 
            this.buttonFa.Location = new System.Drawing.Point(337, 191);
            this.buttonFa.Name = "buttonFa";
            this.buttonFa.Size = new System.Drawing.Size(75, 155);
            this.buttonFa.TabIndex = 2;
            this.buttonFa.Text = "Fa";
            this.buttonFa.UseVisualStyleBackColor = true;
            this.buttonFa.Click += new System.EventHandler(this.buttonFa_Click);
            // 
            // buttonSol
            // 
            this.buttonSol.Location = new System.Drawing.Point(449, 191);
            this.buttonSol.Name = "buttonSol";
            this.buttonSol.Size = new System.Drawing.Size(75, 155);
            this.buttonSol.TabIndex = 3;
            this.buttonSol.Text = "Sol";
            this.buttonSol.UseVisualStyleBackColor = true;
            this.buttonSol.Click += new System.EventHandler(this.buttonSol_Click);
            // 
            // buttonLa
            // 
            this.buttonLa.Location = new System.Drawing.Point(551, 191);
            this.buttonLa.Name = "buttonLa";
            this.buttonLa.Size = new System.Drawing.Size(75, 155);
            this.buttonLa.TabIndex = 4;
            this.buttonLa.Text = "La";
            this.buttonLa.UseVisualStyleBackColor = true;
            this.buttonLa.Click += new System.EventHandler(this.buttonLa_Click);
            // 
            // buttonSi
            // 
            this.buttonSi.Location = new System.Drawing.Point(645, 191);
            this.buttonSi.Name = "buttonSi";
            this.buttonSi.Size = new System.Drawing.Size(75, 155);
            this.buttonSi.TabIndex = 5;
            this.buttonSi.Text = "Si";
            this.buttonSi.UseVisualStyleBackColor = true;
            this.buttonSi.Click += new System.EventHandler(this.buttonSi_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "CENAS";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 460);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSi);
            this.Controls.Add(this.buttonLa);
            this.Controls.Add(this.buttonSol);
            this.Controls.Add(this.buttonFa);
            this.Controls.Add(this.buttonMi);
            this.Controls.Add(this.buttonRe);
            this.Controls.Add(this.buttonDo);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDo;
        private System.Windows.Forms.Button buttonRe;
        private System.Windows.Forms.Button buttonMi;
        private System.Windows.Forms.Button buttonFa;
        private System.Windows.Forms.Button buttonSol;
        private System.Windows.Forms.Button buttonLa;
        private System.Windows.Forms.Button buttonSi;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}

