using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;


namespace PianoKeyboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool actionDo,actionRe,actionMi,actionFa,actionSol,actionLa,actionSi = false;
        bool keyup,keydown = false;
        


        
        private void playSimpleSoundDo()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/DoMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundRe()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/ReMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundMi()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/MiMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundFa()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/FaMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundSol()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/SolMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundLa()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/LaMaior.wav");
            simpleSound.Play();
        }

        private void playSimpleSoundSi()
        {
            SoundPlayer simpleSound = new SoundPlayer(Application.StartupPath + "/Sounds/SiMaior.wav");
            simpleSound.Play();
        }
        
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
                   
            if (e.KeyCode == Keys.Q)
            {
                if (actionDo == false)
                {
                    buttonDo.PerformClick();
                    actionDo = true;
                }
                   
                if (actionDo == true)
                {
                    buttonDo.Enabled = false;

                }
            }
          

            if (e.KeyCode == Keys.W)
            {
                if (actionRe == false)
                {
                    buttonRe.PerformClick();
                    actionRe = true;
                }

                if (actionRe == true)
                {
                    buttonRe.Enabled = false;

                }
            }

            if (e.KeyCode == Keys.E)
            {
                if (actionMi == false)
                {
                    buttonMi.PerformClick();
                    actionMi = true;
                }

                if (actionMi == true)
                {
                    buttonMi.Enabled = false;

                }
            }

            if (e.KeyCode == Keys.R)
            {
                if (actionFa == false)
                {
                    buttonFa.PerformClick();
                    actionFa = true;
                }

                if (actionFa == true)
                {
                    buttonFa.Enabled = false;

                }
            }
           
            if (e.KeyCode == Keys.T)
            {
                if (actionSol == false)
                {
                    buttonSol.PerformClick();
                    actionSol = true;
                }

                if (actionSol == true)
                {
                    buttonSol.Enabled = false;

                }
            }
            
            if (e.KeyCode == Keys.Y)
            {
                if (actionLa == false)
                {
                    buttonLa.PerformClick();
                    actionLa = true;
                }

                if (actionLa == true)
                {
                    buttonLa.Enabled = false;

                }
            }
            
            if (e.KeyCode == Keys.U)
            {
                if (actionSi == false)
                {
                    buttonSi.PerformClick();
                    actionSi = true;
                }

                if (actionSi == true)
                {
                    buttonSi.Enabled = false;

                }
            }
        
        }
        

        private void buttonDo_Click(object sender, EventArgs e)
        {
            playSimpleSoundDo();
            
            Thread.Sleep(120);
        }

        private void buttonRe_Click(object sender, EventArgs e)
        {
            playSimpleSoundRe();
            Thread.Sleep(120);

        }

        private void buttonMi_Click(object sender, EventArgs e)
        {
            playSimpleSoundMi();
           Thread.Sleep(120);
        }

        private void buttonFa_Click(object sender, EventArgs e)
        {
            playSimpleSoundFa();
            Thread.Sleep(120);
        }

        private void buttonSol_Click(object sender, EventArgs e)
        {
            playSimpleSoundSol();
            Thread.Sleep(120);
        }

        private void buttonLa_Click(object sender, EventArgs e)
        {
            playSimpleSoundLa();
            Thread.Sleep(120);
        }

        private void buttonSi_Click(object sender, EventArgs e)
        {
            playSimpleSoundSi();
            Thread.Sleep(120);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                if (actionDo == true)
                {
                    buttonDo.Enabled = true;
                    actionDo = false;

                }
            }


            if (e.KeyCode == Keys.W)
            {
                if (actionRe == true)
                {
                    buttonRe.Enabled = true;
                    actionRe = false;

                }
            }

            if (e.KeyCode == Keys.E)
            {
                if (actionMi == true)
                {
                    buttonMi.Enabled = true;
                    actionMi = false;

                }
            }

            if (e.KeyCode == Keys.R)
            {
                if (actionFa == true)
                {
                    buttonFa.Enabled = true;
                    actionFa = false;

                }
            }

            if (e.KeyCode == Keys.T)
            {
                if (actionSol == true)
                {
                    buttonSol.Enabled = true;
                    actionSol = false;

                }
            }

            if (e.KeyCode == Keys.Y)
            {
                if (actionLa == true)
                {
                    buttonLa.Enabled = true;
                    actionLa = false;

                }
            }

            if (e.KeyCode == Keys.U)
            {
                if (actionSi == true)
                {
                    buttonSi.Enabled = true;
                    actionSi = false;

                }
            }
        }

        
    
    
    }
}
