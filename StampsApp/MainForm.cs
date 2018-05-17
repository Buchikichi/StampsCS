using StampsApp.data;
using StampsApp.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace StampsApp
{
    public partial class MainForm : Form
    {
        private List<string> ListFiles(string[] nameList)
        {
            var list = new List<string>();

            foreach (var name in nameList)
            {
                if (Directory.Exists(name))
                {
                    var files = Directory.GetFiles(name, "*", SearchOption.AllDirectories);

                    list.AddRange(files);
                }
                else if (File.Exists(name))
                {
                    list.Add(name);
                }
            }
            return list;
        }

        #region Events
        private void FileListBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            e.Effect = DragDropEffects.Copy;
        }

        private void FileListBox_DragDrop(object sender, DragEventArgs e)
        {
            var nameList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var list = ListFiles(nameList);

            foreach (var name in list)
            {
                if (!name.EndsWith(".jpg") && !name.EndsWith(".jpeg"))
                {
                    continue;
                }
                FileListBox.Items.Add(new PictureInfo
                {
                    Name = name,
                });
            }
        }

        private void FileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bgName = "C:/Users/dss/Desktop/images/pages/1700171.jpg";
            var item = (PictureInfo)FileListBox.SelectedItem;

            Debug.Print("[" + item.Name + "]");
            PictureBox.Image?.Dispose();
            PictureBox.Image = ImageUtils.Absdiff(bgName, item.Name);
        }
        #endregion

        #region 開始/終了
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion
    }
}
