using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Main
{
    public partial class Frm_Logout : Form
    {
        public Frm_Logout()
        {
            InitializeComponent();
        }
        Rectangle Resolution = Screen.PrimaryScreen.WorkingArea;
        string xmlfile = Application.StartupPath + "\\Config.xml";
        bool Creatednew;//Verifica se o teclado esta em execução

        private void Frm_Logout_Load(object sender, EventArgs e)
        {
            panelLogout.Location = new Point(Resolution.Width / 2 - panelLogout.Width / 2, Resolution.Height / 2 - panelLogout.Height / 2);

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            XDocument xDocument = XDocument.Load(xmlfile);

            var Logout = from d in xDocument.Descendants("Logout")//Lê tudo o que e button
                         select new
                         {
                             User = (string)d.Element("User").Value,
                             Password = (string)d.Element("Password").Value,
                         };


            foreach (var x in Logout)
            {
                if (textBoxUser.Text.Equals(x.User) && this.textBoxPass.Text.Equals(x.Password))
                    Application.Exit();
                else
                    labelMsgLogout.Text = "Error: The Username or Password may be incorrect";
            }


        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxUser_Click(object sender, EventArgs e)
        {
            Mutex mut = new Mutex(true, "someuniqeid", out Creatednew);

            if (Creatednew)//this app isn't already running
                Process.Start(Application.StartupPath + @"\Keyboard.exe");
            else
                Creatednew = false;
        }

        private void textBoxPass_Click(object sender, EventArgs e)
        {           
            Mutex mut = new Mutex(true, "someuniqeid", out Creatednew);

            if (Creatednew)//this app isn't already running
                Process.Start(Application.StartupPath + @"\Keyboard.exe");
            else
                Creatednew = false;
        }

        private void textBoxUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                //Faz uma simulaçao do buttonOk
                buttonOk.PerformClick();
        }

      


    }
}
