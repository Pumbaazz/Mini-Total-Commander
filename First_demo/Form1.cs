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
        //some global variables
        private
        DirectoryInfo leftDirect;
        DirectoryInfo rightDirect;
        string notepadlink = "C:\\Windows\\Notepad.exe";
        string vscodeLink = "C:\\Users\\nguyn\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe";


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
            comboBox1.SelectedItem = comboBox1.Items[0];
            Fill(comboBox1.Items[0].ToString(), listView1);

            comboBox2.SelectedItem = comboBox2.Items[0];
            Fill(comboBox2.Items[0].ToString(), listView2);

            //textBox1.Text = comboBox1.Items[0].ToString();
            textBox2.Text = comboBox2.Items[0].ToString();
            //leftDirect = new DirectoryInfo("C:\\");
            //rightDirect = new DirectoryInfo("C:\\");
        }
    

        private void cmbDrive1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var drive1 = comboBox1.SelectedItem.ToString();
                textBox1.Text = drive1;
                Fill(drive1, listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbDrive2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var drive2 = comboBox2.SelectedItem.ToString();
                textBox2.Text = drive2;
                Fill(drive2, listView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    foreach (DirectoryInfo dir in Directories)
                    {
                        //bool isHidden = (File.GetAttributes(dir.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        //if (!isHidden)
                        {
                            ListViewItem lvi = new ListViewItem(dir.Name);
                            lvi.SubItems.Add("Folder");
                            lvi.Tag = dir;
                            lvi.Name = "Directory";
                            lvi.SubItems.Add("<--DIR-->");
                            lvi.SubItems.Add(dir.LastWriteTime.ToString());
                            listViewNum.Items.Add(lvi);
                        }
                    }

                    foreach (FileInfo file in files)
                    {
                        //bool isHidden = (File.GetAttributes(file.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        //if (!isHidden)
                        {
                            ListViewItem lvi = new ListViewItem(file.Name);
                            lvi.SubItems.Add("File");
                            lvi.Tag = file;
                            lvi.Name = "File";
                            //lvi.SubItems.Add(FormattedSize(file.Length));
                            lvi.SubItems.Add(file.LastWriteTime.ToString());
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
            textBox1.Text = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            Fill(textBox1.Text, listView1);
        }
        private string listview_1click(ListView listNum)
        {
            return listNum.SelectedItems[0].Text;
        }
        private void List2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "\\" + listView2.SelectedItems[0].Text;
            Fill(textBox2.Text, listView2);
        }

        private void listView_doubleClick(ListView listviewNum, TextBox textBoxNum)
        {
            if(listviewNum.SelectedItems[0].Name == "File")
            {
                string fileName = textBoxNum.Text + listview_1click(listviewNum);
                Process.Start(fileName);
            }
            else 
            {
                textBoxNum.Text = textBoxNum.Text + listview_1click(listviewNum);
            }
            
            Fill(textBoxNum.Text, listviewNum);
        }

        private void listView1_2Click(object sender, EventArgs e)
        {
            listView_doubleClick(listView1, textBox1);
        }

        private void listView2_2Click(object sender, EventArgs e)
        {
            listView_doubleClick(listView2, textBox2);
        }
        //read file
        public void viewFile(object sender, EventArgs e)
        {
            ViewForm vi = new ViewForm();
            string fileName = listview_1click(listView1);
            vi.readFile(textBox1.Text + "\\" + fileName);

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

        //show vs code 
        private void vscode_menuStrip(object sender, EventArgs e)
        {
            Process.Start(vscodeLink);
        }
    }
}

