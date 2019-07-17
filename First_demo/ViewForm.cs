using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace First_demo
{
    public partial class ViewForm : Form
    {
        public ViewForm()
        {
            InitializeComponent();
        }
        public void readFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str;
            str = sr.ReadToEnd();
            ViewForm view1 = new ViewForm();
            view1.Text = "View Dialog";
            view1.textBox1.Text = str;
            view1.Show();
            
            sr.Close();
            fs.Close();
        }
    }
}
