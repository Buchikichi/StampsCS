﻿using StampsApp.data;
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
        #region Members
        private BasePapers basePapers;
        #endregion

        #region Events
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
            var item = (PictureInfo)FileListBox.SelectedItem;
            var info = basePapers.ChooseBasePaper(item.Name);

            Debug.Print("[" + info.Name + "]");
            Cursor.Current = Cursors.WaitCursor;
            PictureBox.Image?.Dispose();
            PictureBox.Image = ImageUtils.Absdiff(info, item.Name);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region 開始/終了
        private void Initialize()
        {
            basePapers = new BasePapers("C:/Users/dss/Desktop/images/pages/");
            //var paper = basePapers.BasePaperList[12];
            //var conv = new ImageConverter();

            //PictureBox.Image = (Image)conv.ConvertFrom(paper.Image);
        }

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion
    }
}
