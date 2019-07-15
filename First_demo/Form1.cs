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
using System.Diagnostics;
using System.Windows.Input;
using static First_demo.ViewForm;
using System.Runtime.InteropServices;


namespace First_demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //load drive
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

        //fill the list of file in list view
        private void gridView(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
            listView2.View = View.LargeIcon;
        }
        private void listView(object sender, EventArgs e)
        {
            listView1.View = View.List;
            listView2.View = View.List;
        }

        private void Fill(string s, ListView listViewNum)
        {
            if (Directory.Exists(s))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(s);
                FileInfo[] files = directoryInfo.GetFiles();
                DirectoryInfo[] Directories = directoryInfo.GetDirectories();

                listViewNum.Items.Clear();
                try
                {
                    foreach (DirectoryInfo d in Directories)
                    {
                        bool isHidden = (File.GetAttributes(d.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        if (!isHidden)
                        {
                            ListViewItem lvi = new ListViewItem(d.Name);
                            lvi.SubItems.Add("Folder");
                            listViewNum.Items.Add(lvi);
                        }
                    }

                    foreach (FileInfo d in files)
                    {
                        bool isHidden = (File.GetAttributes(d.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        if (!isHidden)
                        {
                            ListViewItem lvi = new ListViewItem(d.Name);
                            lvi.SubItems.Add("File");
                            listViewNum.Items.Add(lvi);
                        }
                    }
                }
                catch (System.Exception except)
                {
                    MessageBox.Show(except.Message);
                }
            }
        }

        //Refresh Button
        private void refreshButton1_Clicks(object sender, EventArgs e)
        {
            listView1.Refresh();
            textBox1.Refresh();
        }
        private void refreshButton2_Clicks(object sender, EventArgs e)
        {
            listView2.Refresh();
            textBox2.Refresh();
        }
        //Event Click
        private void List1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string fileName = listView1.Items[listView1.SelectedIndex].ToString();
            textBox1.Text = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            string currentPath = textBox1.Text;
            Fill(currentPath, listView1);
        }
        private void List2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "\\" + listView2.SelectedItems[0].Text; 
            string currentPath = textBox2.Text;
            Fill(currentPath, listView2);
        }
        //read file
        public void viewFile(object sender, EventArgs e)
        {
            ViewForm vi = new ViewForm();
            string fileName = listView1.SelectedItems[0].Text;
            string currentPath = Path.GetFullPath(fileName);
            vi.readFile(currentPath);

        }
        //about button in menu strip
        private void MenuStripAbout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                                "Nguyễn Tuấn Phùng\n 1753087", "Visit", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk
                                ) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.instagram.com/tuanphung09/");
            }

        }
        // exit button in menu strip
        private void MenuStripExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //icon in list view
       
    }
}

