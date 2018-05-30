using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using StampsApp.data;
using StampsApp.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StampsApp
{
    public partial class MainForm : Form
    {
        #region Members
        private BasePapers basePapers;
        private Preference ini = Preference.Instance;
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

            Debug.Print($"[{info.Name}]");
            Cursor.Current = Cursors.WaitCursor;
            var img = (Bitmap)ImageUtils.Absdiff(info, item.Name);

            //using (var tesseract = new Tesseract("I:/tesseract-ocr/tessdata/", "eng", OcrEngineMode.Default))
            //using (var mat = new Image<Bgr, byte>(img).Mat)
            //{
            //    tesseract.SetImage(mat);
            //    var i = tesseract.Recognize();
            //    var text = tesseract.GetUTF8Text();

            //    Debug.Print($"OCR[{text}]");
            //}
            PictureBox.Image?.Dispose();
            PictureBox.Image = img;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region PictureBox
        private void AntPicture_DoubleClick(object sender, EventArgs e)
        {
            var item = (PictureInfo)FileListBox.SelectedItem;

            if (item == null)
            {
                return;
            }
            var uuid = Guid.NewGuid().ToString();
            var filename = ini.ImageDir + "stamps/" + uuid + ".jpg";

            using (var bmp = AntPicture.TrimImage((Bitmap)PictureBox.Image))
            {
                ImageUtils.Save(filename, bmp);
            }
        }
        #endregion

        #region 開始/終了
        private void Initialize()
        {
            basePapers = new BasePapers("C:/Users/dss/Desktop/images/pages/");
            //var paper = basePapers.BasePaperList[12];
            //var conv = new ImageConverter();

            //PictureBox.Image = (Image)conv.ConvertFrom(paper.Image);
            AntPicture.Parent = PictureBox;
            AntPicture.Location = new Point();
        }

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion
    }
}
