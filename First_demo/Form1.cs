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
using static First_demo.ExtractIcon;
using static First_demo.NewFolderName;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


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

        //prevent edit combobox
        private void Combo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void Combo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
        private void detailView(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView2.View = View.Details;
        }
        private void Fill(DirectoryInfo directoryNum, ListView listViewNum)
        {
            listViewNum.BeginUpdate();
            listViewNum.Items.Clear();

            ImageList icons = new ImageList();
            icons.ImageSize = new Size(30, 30);
            icons.ColorDepth = ColorDepth.Depth32Bit;
            icons.Images.Add(Image.FromFile("D:\\Study Ground\\Univer\\Term 6\\Windows coding\\Project\\Project-Windows-Form\\First_demo\\folder.png"));

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
                    lvi.SubItems.Add(dir.LastWriteTime.ToString());
                    lvi.ImageIndex = 0;
                    listViewNum.Items.Add(lvi);
                }
            }
            int countIcon = 1;
            foreach (FileInfo file in files)
            {
                //bool isHidden = (File.GetAttributes(file.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden;
                //if (!isHidden)
                {
                    ListViewItem lvi = new ListViewItem(file.Name);
                    lvi.SubItems.Add("File");
                    lvi.Tag = file;
                    lvi.Name = "File";
                    lvi.SubItems.Add(file.LastWriteTime.ToString());
                    icons.Images.Add(Icon.ExtractAssociatedIcon(file.FullName));
                    lvi.ImageIndex = countIcon;
                    ++countIcon;
                    listViewNum.Items.Add(lvi);
                }
            }
            listViewNum.LargeImageList = icons;
            listViewNum.SmallImageList = icons;
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
            Fill(leftDirect, listView1);
            listView1.Refresh();
            textBox1.Refresh();
        }
        private void refreshButton2_Clicks(object sender, EventArgs e)
        {
            Fill(rightDirect, listView2);
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
            string path;
            try
            {
                if (listview1_isActived == true)
                {
                    path = Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                    vi.readFile(path);
                }
                else
                {
                    path = Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                    vi.readFile(path);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            //cứ mở file thì oke mà folder quăng exception nhóe, tính năng đó, đâu phải chỗ nào cũng try catch đâu
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
        //show notepad
        private void notepad_menuStrip(object sender, EventArgs e)
        {
            Process.Start(notepadLink);
        }
        //Help
        private void help_menuStrip(object sender, EventArgs e)
        {
            Process.Start("Help.pdf");
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

                string subFolder = Path.Combine(textBox1.Text, defaultName);
                if (!Directory.Exists(subFolder))
                {
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, defaultName));
                }
                else
                {
                    ++countSameFolder;
                    string subName = defaultName + " " + (countSameFolder).ToString();
                    while (Directory.Exists(Path.Combine(textBox1.Text, subName)))
                    {
                        countSameFolder++;
                        subName = defaultName + " " + (countSameFolder).ToString();
                    }
                    Directory.CreateDirectory(Path.Combine(textBox1.Text, subName));
                }
                listView1.Refresh();
                Fill(leftDirect, listView1);
            }

            //this is using for listview2
            else
            {
                FileInfo[] file = direct2.GetFiles();

                string subFolder = Path.Combine(textBox2.Text, defaultName);
                if (!Directory.Exists(subFolder))
                {
                    Directory.CreateDirectory(Path.Combine(textBox2.Text, defaultName));
                }
                else
                {
                    ++countSameFolder;
                    string subName = defaultName + " " + (countSameFolder).ToString();
                    while (Directory.Exists(Path.Combine(textBox2.Text, subName)))
                    {
                        countSameFolder++;
                        subName = defaultName + " " + (countSameFolder).ToString();
                    }
                    Directory.CreateDirectory(Path.Combine(textBox2.Text, subName));
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
        private void deleteFile(ListView listViewNum, DirectoryInfo directNum, TextBox textBoxNum)
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
                if (IsDirectoryEmpty(Path.Combine(textBoxNum.Text, listViewNum.SelectedItems[0].Text)))
                {
                    if (MessageBox.Show("Bạn có chắc là muốn xóa cái folder này không??", @"Noticed me !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.Delete(Path.Combine(textBoxNum.Text, listViewNum.SelectedItems[0].Text));
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
                deleteFile(listView1, leftDirect, textBox1);
                //Fill(leftDirect, listView1);
            }
            else
            {
                deleteFile(listView2, rightDirect, textBox2);
                //Fill(rightDirect, listView2);

            }
        }
        //make context menu
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listview1_isActived)
                refreshButton1_Clicks(sender, e);
            else
                refreshButton2_Clicks(sender, e);
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete_click(sender, e);
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            editFile_Click(sender, e);
        }
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewFile(sender, e);
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy_Click(sender, e);
        }
        private void moveToolStripMenuItem_click(object sender, EventArgs e)
        {
            move_click(sender, e);
        }
        //quick change path using textbox   
        private void editTextBox_left(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == (char)13)
            {
                DirectoryInfo direct = new DirectoryInfo(textBox1.Text);
                if (!direct.Exists)
                {
                    MessageBox.Show("Path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    leftDirect = direct;
                    Fill(direct, listView1);
                }
            }
        }
        private void editTextBox_right(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == (char)13)
            {
                DirectoryInfo direct = new DirectoryInfo(textBox2.Text);
                if (!direct.Exists)
                {
                    MessageBox.Show("Path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    leftDirect = direct;
                    Fill(direct, listView2);
                }
            }
        }
        //copy 1 file and folder
        //đoạn này khá khó nhầm nên comment để nhớ thôi <3
        private void copy_Click(object sender, EventArgs e)
        {
            //list 1 qua list 2
            //copy 1 file nè
            if (MessageBox.Show("Bạn có chắc là muốn copy không", "Noticed!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                if (listview1_isActived)
                {
                    if (listView1.SelectedItems[0].Name == "File") //cái đang chọn phải là file mới chạy đoạn này
                    {
                        FileInfo file = listView1.SelectedItems[0].Tag as FileInfo;
                        if (File.Exists(Path.Combine(textBox2.Text, file.Name)))    //xet dieu kien file bi trung, hoi co over write khong
                        {
                            if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm file bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                File.Copy(file.FullName, Path.Combine(textBox2.Text, file.Name), true);
                        }
                        else //doan nay thi no khong co bi trung nen cu copy thoai mai
                            File.Copy(file.FullName, Path.Combine(textBox2.Text, file.Name));
                    }


                    if (listView1.SelectedItems[0].Name != "File")
                    {
                        if (IsDirectoryEmpty(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)))
                        //điều kiện check có subfile/subfolder trong cái folder cần copy, trong list 1 nhóe
                        //chỉ xét ở trường hợp list1 qua list 2 trước
                        {
                            //không có sub thì chạy cái này
                            DirectoryInfo folder = listView1.SelectedItems[0].Tag as DirectoryInfo;
                            if (!Directory.Exists(Path.Combine(textBox2.Text, folder.Name)))
                            //nếu không tồn tại đường dẫn combine, copy qua tạo đường dẫn mới
                            {
                                Directory.CreateDirectory(Path.Combine(textBox2.Text, folder.Name));
                            }

                            else
                            {
                                //đây là bị trùng nè, hỏi có sure kèo không?
                                if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm folder của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                {
                                    Directory.Delete(Path.Combine(textBox2.Text, folder.Name));
                                    Directory.CreateDirectory(Path.Combine(textBox2.Text, folder.Name));
                                }
                            }
                        }
                        else
                        //có sub thì chạy đoạn này
                        {
                            if (MessageBox.Show("Bạn chắc chứ??\n Trong folder này còn chứa dữ liệu đó!!", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {

                                //trường hợp có sub và không trùng
                                if (!Directory.Exists(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text)))
                                {
                                    DirectoryInfo ahihi = listView1.SelectedItems[0].Tag as DirectoryInfo;
                                    Directory.CreateDirectory(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text));
                                    foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                    {
                                        dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), dir.Name));
                                    }
                                    foreach (FileInfo file in ahihi.EnumerateFiles())
                                    {
                                        File.Move(file.FullName, Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), file.Name));
                                    }



                                }
                                else
                                //trường hợp có sub và bị trùng
                                {
                                    if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm cho file và folder con bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                    {
                                        //deleteFile(listView2, rightDirect, textBox2);

                                        DirectoryInfo ahihi = listView1.SelectedItems[0].Tag as DirectoryInfo;
                                        Directory.CreateDirectory(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text));
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Copy(file.FullName, Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), file.Name), true);
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                //list2 qua list1
                else
                {
                    if (listView2.SelectedItems[0].Name == "File") //vẫn phải là file
                    {
                        FileInfo file = listView2.SelectedItems[0].Tag as FileInfo;
                        if (File.Exists(Path.Combine(textBox1.Text, file.Name)))    //check xem có tồn tại đường dẫn file không(cái đường dẫn copy file rồi í)
                        {
                            if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm file của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                File.Copy(file.FullName, Path.Combine(textBox1.Text, file.Name), true); //true ở đây là boolean overwrite
                        }
                        else //không bị trùng nên tẹt ga
                            File.Copy(file.FullName, Path.Combine(textBox1.Text, file.Name));
                    }
                    //copy 1 folder    
                    //copy folder mới có trường hợp có subfolder/subfile thôi, xét cái này trước tiên để coi nó có phải là copy 1 file hay là 1 nùi
                    if (listView2.SelectedItems[0].Name != "File")
                    {
                        //list 2 qua list 1
                        if (IsDirectoryEmpty(Path.Combine(textBox2.Text, listView2.SelectedItems[0].Text)))
                        //điều kiện check có subfile/subfolder trong cái folder cần copy, trong list 1 nhóe
                        {
                            //không có sub thì chạy cái này
                            DirectoryInfo folder = listView2.SelectedItems[0].Tag as DirectoryInfo;
                            if (!Directory.Exists(Path.Combine(textBox1.Text, folder.Name)))
                            //nếu không tồn tại đường dẫn combine, copy qua tạo đường dẫn mới
                            {
                                Directory.CreateDirectory(Path.Combine(textBox1.Text, folder.Name));
                            }

                            else
                            {
                                //đây là bị trùng nè, hỏi có sure kèo không?
                                if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm cho folder của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                {
                                    Directory.Delete(Path.Combine(textBox1.Text, folder.Name));
                                    Directory.CreateDirectory(Path.Combine(textBox1.Text, folder.Name));
                                }
                            }
                        }
                        else
                        //có sub thì chạy đoạn này
                        {
                            if (MessageBox.Show("Bạn chắc chứ??\n Trong folder này còn có dữ liệu đó!!", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {
                                /*OVERWRITE*/
                                //trường hợp có sub và không trùng
                                if (!Directory.Exists(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text)))
                                {
                                    DirectoryInfo ahihi = listView2.SelectedItems[0].Tag as DirectoryInfo;
                                    Directory.CreateDirectory(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text));
                                    foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                    {
                                        dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), dir.Name));
                                    }
                                    foreach (FileInfo file in ahihi.EnumerateFiles())
                                    {
                                        File.Copy(file.FullName, Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), file.Name));
                                    }



                                }
                                else
                                //trường hợp có sub và bị trùng
                                /*OVERWRITE ALL LIST 2*/
                                {
                                    if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm cho file và folder của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                    {
                                        //deleteFile(listView1, leftDirect, textBox1);
                                        DirectoryInfo ahihi = listView2.SelectedItems[0].Tag as DirectoryInfo;
                                        Directory.CreateDirectory(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text));
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Copy(file.FullName, Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), file.Name), true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        private void move_click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc là muốn move không", "Noticed!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                if (listview1_isActived)
                {
                    if (listView1.SelectedItems[0].Name == "File") //cái đang chọn phải là file mới chạy đoạn này
                    {
                        FileInfo file = listView1.SelectedItems[0].Tag as FileInfo;
                        if (File.Exists(Path.Combine(textBox2.Text, file.Name)))    //xét điều kiện file bị trùng, có thì hỏi overwrite không
                        {
                            if (MessageBox.Show("Bạn chắc không??\n Điều này sẽ làm file của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {
                                File.Delete(Path.Combine(textBox2.Text, file.Name));
                                File.Move(file.FullName, Path.Combine(textBox2.Text, file.Name));
                            }
                        }
                        else
                            //đoạn này file không bị trùng nên move bình thường
                            File.Move(file.FullName, Path.Combine(textBox2.Text, file.Name));
                    }


                    if (listView1.SelectedItems[0].Name != "File")
                    {
                        if (IsDirectoryEmpty(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)))
                        //điều kiện check có subfile/subfolder trong cái folder cần move, trong list 1 nhóe
                        //chỉ xét ở trường hợp list1 qua list 2 trước
                        {
                            //không có sub thì chạy cái này
                            DirectoryInfo folder = listView1.SelectedItems[0].Tag as DirectoryInfo;
                            if (!Directory.Exists(Path.Combine(textBox2.Text, folder.Name)))
                            //nếu không tồn tại đường dẫn combine, copy qua tạo đường dẫn mới
                            {
                                Directory.Move(folder.FullName, Path.Combine(textBox2.Text, folder.Name));
                            }

                            else
                            {
                                //đây là bị trùng nè, hỏi có sure kèo không?
                                if (MessageBox.Show("Bạn có chắc không??\n Điều này sẽ làm cho folder bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                {
                                    Directory.Delete(Path.Combine(textBox2.Text, folder.Name));
                                    Directory.CreateDirectory(Path.Combine(textBox2.Text, folder.Name));
                                }
                            }
                        }
                        else
                        //có sub thì chạy đoạn này
                        {
                            if (MessageBox.Show("Bạn chắc không??\n Điều này sẽ làm folder của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {

                                //trường hợp có sub và không trùng
                                if (!Directory.Exists(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text)))
                                {
                                    //dùng cái copy qua chứ move qua nó mất đường dẫn 
                                    DirectoryInfo ahihi = listView1.SelectedItems[0].Tag as DirectoryInfo;
                                    Directory.CreateDirectory(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text));
                                    foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                    {
                                        Directory.Move(dir.FullName, Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), dir.Name));
                                    }
                                    foreach (FileInfo file in ahihi.EnumerateFiles())
                                    {
                                        File.Move(file.FullName, Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), file.Name));
                                    }



                                }
                                else
                                //trường hợp có sub và bị trùng
                                {
                                    if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm cả file và folder con bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                    {
                                        //deleteFile(listView2, rightDirect, textBox2);

                                        DirectoryInfo ahihi = listView1.SelectedItems[0].Tag as DirectoryInfo;
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            Directory.Delete(Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Delete(Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), file.Name));
                                        }

                                        Directory.CreateDirectory(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text));
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Move(file.FullName, Path.Combine(Path.Combine(textBox2.Text, listView1.SelectedItems[0].Text), file.Name));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
//---------------------------------------------------------------------------------------------------------------------------------------------------------
//list2 qua list1
                else
                {
                    if (listView2.SelectedItems[0].Name == "File") //cái đang chọn phải là file mới chạy đoạn này
                    {
                        FileInfo file = listView2.SelectedItems[0].Tag as FileInfo;
                        if (File.Exists(Path.Combine(textBox1.Text, file.Name)))    //xét điều kiện file bị trùng, có thì hỏi overwrite không
                        {
                            if (MessageBox.Show("Bạn chắc không??\n Điều này sẽ làm file của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {
                                File.Delete(Path.Combine(textBox1.Text, file.Name));
                                File.Move(file.FullName, Path.Combine(textBox1.Text, file.Name));
                            }
                        }
                        else
                            //đoạn này file không bị trùng nên move bình thường
                            File.Move(file.FullName, Path.Combine(textBox1.Text, file.Name));
                    }


                    if (listView2.SelectedItems[0].Name != "File")
                    {
                        if (IsDirectoryEmpty(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text)))
                        //điều kiện check có subfile/subfolder trong cái folder cần move, trong list 1 nhóe
                        //chỉ xét ở trường hợp list1 qua list 2 trước
                        {
                            //không có sub thì chạy cái này
                            DirectoryInfo folder = listView2.SelectedItems[0].Tag as DirectoryInfo;
                            if (!Directory.Exists(Path.Combine(textBox1.Text, folder.Name)))
                            //nếu không tồn tại đường dẫn combine, copy qua tạo đường dẫn mới
                            {
                                Directory.Move(folder.FullName, Path.Combine(textBox1.Text, folder.Name));
                            }

                            else
                            {
                                //đây là bị trùng nè, hỏi có sure kèo không?
                                if (MessageBox.Show("Bạn có chắc không??\n Điều này sẽ làm cho folder bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                {
                                    Directory.Delete(Path.Combine(textBox1.Text, folder.Name));
                                    Directory.CreateDirectory(Path.Combine(textBox1.Text, folder.Name));
                                }
                            }
                        }
                        else
                        //có sub thì chạy đoạn này
                        {
                            if (MessageBox.Show("Bạn chắc không??\n Điều này sẽ làm folder của bạn bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {

                                //trường hợp có sub và không trùng
                                if (!Directory.Exists(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text)))
                                {
                                    //dùng cái copy qua chứ move qua nó mất đường dẫn 
                                    DirectoryInfo ahihi = listView2.SelectedItems[0].Tag as DirectoryInfo;
                                    Directory.CreateDirectory(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text));
                                    foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                    {
                                        Directory.Move(dir.FullName, Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), dir.Name));
                                    }
                                    foreach (FileInfo file in ahihi.EnumerateFiles())
                                    {
                                        File.Move(file.FullName, Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), file.Name));
                                    }
                                }
                                else
                                //trường hợp có sub và bị trùng
                                {
                                    if (MessageBox.Show("Bạn chắc chứ??\n Điều này làm cả file và folder con bị overwrite", "Noticed!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                    {
                                        //deleteFile(listView2, rightDirect, textBox2);

                                        DirectoryInfo ahihi = listView2.SelectedItems[0].Tag as DirectoryInfo;
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            Directory.Delete(Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Delete(Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), file.Name));
                                        }

                                        Directory.CreateDirectory(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text));
                                        foreach (DirectoryInfo dir in ahihi.EnumerateDirectories())
                                        {
                                            dir.CreateSubdirectory(Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), dir.Name));
                                        }
                                        foreach (FileInfo file in ahihi.EnumerateFiles())
                                        {
                                            File.Move(file.FullName, Path.Combine(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text), file.Name));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
