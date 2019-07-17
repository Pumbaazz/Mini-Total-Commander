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
        bool listview1_isActived = true;



        string notepadlink = "C:\\Windows\\Notepad.exe";
        string vscodeLink = "C:\\Users\\nguyn\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe";


        public Form1()
        {
            InitializeComponent();

            //textBox1.TextChanged += Change_address;
            //textBox2.TextChanged += Change_address;
            //textBox1.KeyPress += EnterKey_addressLeft;
            //textBox2.KeyPress += EnterKey_addressRight;
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
            //comboBox1.SelectedItem = comboBox1.Items[0];
            //Fill(comboBox1.Items[0].ToString(), listView1);

            //comboBox2.SelectedItem = comboBox2.Items[0];
            //Fill(comboBox2.Items[0].ToString(), listView2);

            ////textBox1.Text = comboBox1.Items[0].ToString();
            leftDirect = new DirectoryInfo("C:\\");
            rightDirect = new DirectoryInfo("C:\\");
        }


        private void cmbDrive1_SelectedIndexChanged(object sender, EventArgs e)
        {
            leftDirect = new DirectoryInfo(comboBox1.SelectedItem.ToString());
            textBox1.Text = comboBox1.SelectedItem.ToString();
            Fill(leftDirect, listView1);
        }

        private void cmbDrive2_SelectedIndexChanged(object sender, EventArgs e)
        {
            rightDirect = new DirectoryInfo(comboBox2.SelectedItem.ToString());
            textBox2.Text = comboBox2.SelectedItem.ToString();
            Fill(rightDirect, listView2);
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

        private void Fill(DirectoryInfo directoryNum, ListView listViewNum)
        {
            listViewNum.BeginUpdate();
            listViewNum.Items.Clear();
            if (directoryNum.Parent != null)
            {
                ListViewItem rollBack = new ListViewItem("...");
                rollBack.Tag = directoryNum;
                rollBack.Name = "Directory";
                rollBack.SubItems.Add("--DIR--");
                listViewNum.Items.Add(rollBack);
            }
            DirectoryInfo[] Directories = directoryNum.GetDirectories();
            FileInfo[] files = directoryNum.GetFiles();
        foreach (DirectoryInfo dir in Directories)
        {
                if (!dir.Attributes.HasFlag(FileAttributes.System))
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
            listViewNum.EndUpdate();   
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
            //textBox1.Text = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            listview1_isActived = true;
            Fill(leftDirect, listView1);
        }
        private string listview_1click(ListView listNum)
        {
            return listNum.SelectedItems[0].Text;
        }
        private void List2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listview1_isActived = false;
            Fill(rightDirect, listView2);
        }

        private void listView_doubleClick(ListView listviewNum, DirectoryInfo directNum, TextBox textBoxNum)
        {
            if (listviewNum.SelectedItems[0].Name == "File")
            {
                FileInfo file = listviewNum.SelectedItems[0].Tag as FileInfo;
                Process.Start(file.FullName);
            }
            else
            {
                if (listviewNum.SelectedItems[0].Text != "...")
                {
                    directNum = listviewNum.SelectedItems[0].Tag as DirectoryInfo;
                }
                else
                {
                    directNum = (listviewNum.SelectedItems[0].Tag as DirectoryInfo).Parent;
                }
            }
            textBoxNum.Text = directNum.FullName;
            Fill(directNum, listviewNum);
        }

        private void listView1_2Click(object sender, EventArgs e)
        {
            listview1_isActived = true;
            listView_doubleClick(listView1, leftDirect, textBox1);
        }

        private void listView2_2Click(object sender, EventArgs e)
        {
            listview1_isActived = false;
            listView_doubleClick(listView2, leftDirect, textBox2);
        }
        //read file
        public void viewFile(object sender, EventArgs e)
        {
            ViewForm vi = new ViewForm();
            string fileName;
            if(listview1_isActived == true)
            {
                fileName= listview_1click(listView1);
                vi.readFile(textBox1.Text + "\\" + fileName);
            }
            else
            {
                fileName = listview_1click(listView2);
                vi.readFile(textBox2.Text + "\\" + fileName);
            }
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

        //edit file
        private void editFile_button(ListView listViewNum)
        {
            FileInfo file = listViewNum.SelectedItems[0].Tag as FileInfo;

        }

        private void editFile_Click(object sender, EventArgs e)
        {
            if(listview1_isActived == true)
            {
                editFile_button(listView1);
            }
            else
            {
                editFile_button(listView2);
            }
        }
    }
}

