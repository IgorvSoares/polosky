using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        Main open = new Main();
        private void timerFadeIn_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)
            {
                open.Show();
                timerFadeIn.Stop();
                timerFadeOut.Start();
         
            }
            else
                this.Opacity += 0.07;
        }

        private void timerFadeOut_Tick(object sender, EventArgs e)
        {

            if (this.Opacity == 0)
            {
                timerFadeOut.Stop();
                this.TopMost = false;
       
            }
            else
                this.Opacity -= 0.10;
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            timerFadeIn.Start();
        }

       

     


    
    }
}
