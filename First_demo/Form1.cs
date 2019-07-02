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
            }
        }

        private void cmbDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Fill(comboBox1.Text);
        }
        void Fill(string s)
        {
            DirectoryInfo Directory = new DirectoryInfo(s);
            DirectoryInfo[] Directories = Directory.GetDirectories();
            FileInfo[] files = Directory.GetFiles();
            foreach (DirectoryInfo d in Directories)
            {
                ListViewItem lvi = new ListViewItem(d.Name);
                lvi.SubItems.Add("Folder");
                listView1.Items.Add(lvi);
            }
            foreach (FileInfo d in files)
            {
                ListViewItem lvi = new ListViewItem(d.Name);
                lvi.SubItems.Add("File");
                listView1.Items.Add(lvi);
            }
        }
    }
}

