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
using static First_demo.Form1;

namespace First_demo
{
    public partial class NewFolderName : Form
    {
        public NewFolderName()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            //makeNewFolder(listViewNum, textBox1.Text);
        }

        private void makeNewFolder(ListView listViewNum, string fileName)
        {
            int count = 1;
            string defaultName = "New Folder";
            FileInfo file = listViewNum.SelectedItems[0].Tag as FileInfo;
            if (fileName == null)
            {
                if (!Directory.Exists(Path.Combine(file.FullName, defaultName)))
                {
                    Directory.CreateDirectory(Path.Combine(file.FullName, defaultName));
                }
            }
            else
            {
                string subFolder = Path.Combine(file.FullName, fileName);
                if (Directory.Exists(subFolder))
                {
                    string subName = fileName + (++count).ToString();
                    Directory.CreateDirectory(Path.Combine(file.FullName, subName));
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(file.FullName, fileName));
                }
            }
        }
    }
}
