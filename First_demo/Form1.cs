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
using static First_demo.NewFolderName;


namespace First_demo
{
    public partial class Form1 : Form
    {
        //some global variables
        private
        DirectoryInfo leftDirect;
        DirectoryInfo rightDirect;
        string notepadLink = "C:\\Windows\\system32\\notepad.exe";
        string vscodeLink = "C:\\Users\\nguyn\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe";
        bool listview1_isActived = true;
        

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
            leftDirect = new DirectoryInfo("C:\\");
            rightDirect = new DirectoryInfo("C:\\");
            comboBox1.SelectedItem = comboBox1.Items[0];
            comboBox2.SelectedItem = comboBox2.Items[0];
            Fill(leftDirect, listView1);
            Fill(rightDirect, listView2);

            //leftDirect.TextChanged += Change_address;
            //rightDirect.TextChanged += Change_address;
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
                    //Icon icon = GetIcon(directoryNum.FullName, false, false);
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
                    //Icon icon = GetIcon(directoryNum.FullName, false, false);
                }
            }
            listViewNum.EndUpdate();
        }

        private void Change_address(object sender, EventArgs args)
        {
            TextBox textBoxNum = sender as TextBox;
            if (textBoxNum.Text.Contains("\r\n") || textBoxNum.Text.Contains("\n"))
            {
                textBoxNum.Text = textBoxNum.Text.Replace("\r\n", "");
                textBoxNum.Text = textBoxNum.Text.Replace("\n", "");
            }
        }

        //Refresh Button
        private void refreshButton1_Clicks(object sender, EventArgs e)
        {
            listView1.Refresh();
            textBox1.Refresh();
            Fill(leftDirect, listView1);
        }
        private void refreshButton2_Clicks(object sender, EventArgs e)
        {
            listView2.Refresh();
            textBox2.Refresh();
            Fill(rightDirect, listView2);
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
            if (listview1_isActived == true)
            {
                fileName = listview_1click(listView1);
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
            if (MessageBox.Show("Bạn muốn mở bằng VS Code không??", @"Noticed me !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(vscodeLink, file.FullName);
            }
            else
            {
                Process.Start(notepadLink, file.FullName);
            }
            //bug nhẹ là không nhận dấu space, vd: New Text document.txt mở bằng vscode ra 3 file mới??, mở notepad bình thường
        }
        private void listview1_click(object sender, EventArgs e)
        {
            listview1_isActived = true;
        }
        private void listview2_click(object sender, EventArgs e)
        {
            listview1_isActived = false;
        }
        private void editFile_Click(object sender, EventArgs e)
        {
            if (listview1_isActived == true)
            {
                editFile_button(listView1);
            }
            else
            {
                editFile_button(listView2);
            }
        }

        /*this is some of stackoverflow code for icon system

        [DllImport("Shell32.dll")]
        private static extern int SHGetFileInfo(
                string pszPath, uint dwFileAttributes,
                out SHFILEINFO psfi, uint cbfileInfo,
                SHGFI uFlags);

        private struct SHFILEINFO
        {
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = "";
            }
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            public string szDisplayName;
            public string szTypeName;
        };

        private enum SHGFI
        {
            SmallIcon = 0x00000001,
            OpenIcon = 0x00000002,
            LargeIcon = 0x00000000,
            Icon = 0x00000100,
            DisplayName = 0x00000200,
            Typename = 0x00000400,
            SysIconIndex = 0x00004000,
            LinkOverlay = 0x00008000,
            UseFileAttributes = 0x00000010
        }

        public static Icon GetIcon(string strPath, bool bSmall, bool bOpen)
        {
            SHFILEINFO info = new SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags;

            if (bSmall)
                flags = SHGFI.Icon | SHGFI.SmallIcon;
            else
                flags = SHGFI.Icon | SHGFI.LargeIcon;

            if (bOpen) flags = flags | SHGFI.OpenIcon;

            SHGetFileInfo(strPath, 0, out info, (uint)cbFileInfo, flags);

            return Icon.FromHandle(info.hIcon);
        }*/

        //make new folder
        
        private void makeNewFolder(ListView list1, ListView list2, DirectoryInfo direct1, DirectoryInfo direct2)
        {
            int countSameFolder = 1;
            string defaultName = "New Folder";

            //this is using for listview1
            if (listview1_isActived == true)
            {
                //direct1 = textBox1 as DirectoryInfo;
                FileInfo[] file = direct1.GetFiles();

                string subFolder = Path.Combine(direct1.FullName, defaultName);
                if (!Directory.Exists(subFolder))
                {
                    Directory.CreateDirectory(Path.Combine(direct1.FullName, defaultName));
                }
                else
                {
                    ++countSameFolder;
                    string subName = defaultName + " " + (countSameFolder).ToString();
                    while (Directory.Exists(Path.Combine(direct1.FullName, subName)))
                    {
                        countSameFolder++;
                        subName = defaultName + " " + (countSameFolder).ToString();
                    }
                    Directory.CreateDirectory(Path.Combine(direct1.FullName, subName));
                }
                listView1.Refresh();
                Fill(leftDirect, listView1);
            }

            //this is using for listview2
            else
            {
                FileInfo[] file = direct2.GetFiles();

                string subFolder = Path.Combine(direct2.FullName, defaultName);
                if (!Directory.Exists(subFolder))
                {
                    Directory.CreateDirectory(Path.Combine(direct2.FullName, defaultName));
                }
                else
                {
                    ++countSameFolder;
                    string subName = defaultName + " " + (countSameFolder).ToString();
                    while(Directory.Exists(Path.Combine(direct2.FullName, subName)))
                    {
                        countSameFolder++;
                        subName = defaultName + " " + (countSameFolder).ToString();
                    }
                    Directory.CreateDirectory(Path.Combine(direct2.FullName, subName));
                }
                listView2.Refresh();
                Fill(rightDirect, listView2);
            }
        }

        private void newFolder_click(object sender, EventArgs e)
        {
            makeNewFolder(listView1, listView2, leftDirect, rightDirect);
        }


        //this shit will be delete file

        public bool IsDirectoryEmpty(string path)
        {
            string[] dirs = System.IO.Directory.GetDirectories(path);
            string[] files = System.IO.Directory.GetFiles(path);
            return dirs.Length == 0 && files.Length == 0;
            //true is no sub
        }

        private void deleteFile(ListView listViewNum, DirectoryInfo directNum)
        {
            //delete file thoi
            if (listViewNum.SelectedItems[0].Name == "File")
            {
                if (MessageBox.Show("Bạn có chắc là muốn xóa file này không??", @"Noticed me !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FileInfo file = listViewNum.SelectedItems[0].Tag as FileInfo;
                    file.Delete();
                }
            }
            else
            {
                if (IsDirectoryEmpty(directNum.FullName))
                {
                    if (MessageBox.Show("Bạn có chắc là muốn xóa cái folder này không??", @"Noticed me !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        directNum.Delete();
                    }
                }
                else
                {
                    if (MessageBox.Show("Bạn có chắc là muốn xóa cái folder này không??\n Trong folder này còn chứa dữ liệu đó!!!", @"Noticed me !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //delete cac file o trong folder
                        directNum = listViewNum.SelectedItems[0].Tag as DirectoryInfo;
                        foreach (FileInfo file in directNum.EnumerateFiles())
                            File.Delete(file.FullName);
                        //foreach (string directory in Directory.GetDirectories(directNum.FullName))
                        //    Directory.Delete(directory);
                        foreach (DirectoryInfo dir in directNum.EnumerateDirectories())
                        {
                            dir.Delete(true);
                        }
                        Directory.Delete(directNum.FullName);
                    }
                }
            }
        }
        private void delete_click(object sender, EventArgs e)
        {
            if (listview1_isActived)
            {
                deleteFile(listView1, leftDirect);
                Fill(leftDirect, listView1);
            }
            else
            {
                deleteFile(listView2, rightDirect);
                Fill(rightDirect, listView2);

            }
        }

        //and now this shit will be the move file and folder :>

        private void moveFileOrFolder(ListView source, ListView desti, DirectoryInfo directSource, DirectoryInfo directDesti)
        {
            //move 1 file thôi nè
            if (source.SelectedItems[0].Name == "File")
            {
                FileInfo file = source.SelectedItems[0].Tag as FileInfo;
                string sourcePath = Path.Combine(textBox2.Text, file.Name);
                File.Move(file.FullName, sourcePath);
            }
            //move 1 folder thôi nè
            else
            {
                directSource = source.SelectedItems[0].Tag as DirectoryInfo;
                directDesti = desti.SelectedItems[0].Tag as DirectoryInfo;
                string destination = Path.Combine(directDesti.FullName, directSource.Name);
                Directory.Move(directSource.FullName, destination);
            }
        }
        private void move_Click(object sender, EventArgs e)
        {
            if (listview1_isActived)
            {
                moveFileOrFolder(listView1, listView2, leftDirect, rightDirect);
            }
            else
            {
                moveFileOrFolder(listView2, listView1, rightDirect, leftDirect);
            }
        }
    }
}
