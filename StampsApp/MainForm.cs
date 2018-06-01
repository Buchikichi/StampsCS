using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using StampsApp.data;
using StampsApp.IO;
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

        private void Ocr(Bitmap img)
        {
            using (var tesseract = new Tesseract("I:/tesseract-ocr/tessdata/", "eng", OcrEngineMode.Default))
            using (var mat = new Image<Bgr, byte>(img).Mat)
            {
                tesseract.SetImage(mat);
                var i = tesseract.Recognize();
                var text = tesseract.GetUTF8Text();

                Debug.Print($"OCR[{text}]");
            }
        }

        private void Detect(Bitmap img)
        {
            var halfW = img.Width * .5;
            var halfH = img.Height * .5;
            var cascade = new CascadeClassifier("C:/Users/dss/Desktop/images/cascade/cascade.xml");
            var minRect = ini.DetectRectangle;

            //cascade.Read();
            Debug.Print("Detect");
            using (var mat = new Image<Bgr, byte>(img).Mat)
            {
                //CvInvoke.Resize(mat, mat, new Size((int)halfW, (int)halfH));
                var rectangles = cascade.DetectMultiScale(mat);

                Debug.Print($"Detect:{rectangles.Length}");
                foreach (var rect in rectangles)
                {
                    if (rect.Width < minRect.Width || rect.Height < minRect.Height)
                    {
                        continue;
                    }
                    CvInvoke.Rectangle(mat, rect, new MCvScalar(255, 0, 0), 2);
                }
                PictureBox.Image?.Dispose();
                PictureBox.Image = mat.Bitmap;
            }
        }

        private void FileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var item = (PictureInfo)FileListBox.SelectedItem;
            var info = basePapers.ChooseBasePaper(item.Name);

            Debug.Print($"[{info.Name}]");
            var img = (Bitmap)ImageUtils.Absdiff(info, item.Name);

            //Ocr(img);
            if (DetectCheckBox.Checked)
            {
                Detect(img);
            } else
            {
                PictureBox.Image?.Dispose();
                PictureBox.Image = img;
            }
            Cursor.Current = Cursors.Default;
        }

        private void CreateSampleButton_Click(object sender, EventArgs e)
        {
            var samples = new Samples();

            samples.Save();
            MessageBox.Show($"Samples saved.");
        }
        #endregion

        #region PictureBox
        private void SaveWorking(PictureInfo info)
        {
            var filename = ini.PosDir + "work.txt";
            using (var writer = new StreamWriter(filename, append: true))
            {
                writer.WriteLine(info.Name);
            }
        }

        private void AntPicture_DoubleClick(object sender, EventArgs e)
        {
            var item = (PictureInfo)FileListBox.SelectedItem;

            if (item == null)
            {
                return;
            }
            var uuid = Guid.NewGuid().ToString();
            var filename = ini.PosDir + uuid + ".jpg";

            using (var bmp = AntPicture.TrimImage((Bitmap)PictureBox.Image))
            {
                ImageUtils.Save(filename, bmp);
            }
            AntPicture.Reset();
            SaveWorking(item);
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
