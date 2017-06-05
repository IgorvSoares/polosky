using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoIncrementVersions
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // Concise way to do this suggested by james_carter:
            lblVersion.Text = Application.ProductVersion;


        }
        private void Form1_Load(object sender, EventArgs e)
        {
           

           

        }
    }
}
