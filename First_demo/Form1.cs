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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo d in drives)
            {
                comboBox1.Items.Add(d.Name);
                comboBox2.Items.Add(d.Name);
            }
        }

        private void cmbDrive1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill(comboBox1.Text, listView1);
        }

        private void cmbDrive2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill(comboBox2.Text, listView2);
        }

        void Fill(string s, ListView listViewNum)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(s);
            DirectoryInfo[] Directories = directoryInfo.GetDirectories();
            FileInfo[] files = directoryInfo.GetFiles();

            listViewNum.Items.Clear();

            foreach (DirectoryInfo d in Directories)
            {
                ListViewItem lvi = new ListViewItem(d.Name);
                lvi.SubItems.Add("Folder");
                listViewNum.Items.Add(lvi);
            }
            
            foreach (FileInfo d in files)
            {
                ListViewItem lvi = new ListViewItem(d.Name);
                lvi.SubItems.Add("File");
                listViewNum.Items.Add(lvi);
            }
        }
    }
}

