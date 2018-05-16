using OpenCvSharp;
using StampsApp.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //PictureBox.Image = Image.FromFile(item.Name);

            using (var knn = BackgroundSubtractorKNN.Create())
            using (var bg = new Mat(bgName, ImreadModes.Color))
            using (var fg = new Mat(item.Name, ImreadModes.Color))
            using (var bgGray = bg.CvtColor(ColorConversionCodes.BGR2GRAY))
            using (var fgGray = fg.CvtColor(ColorConversionCodes.BGR2GRAY))
            {
                var mats = new List<Mat>() { bgGray, fgGray };
                //var images = InputArray.Create(mats);
                var outMat = new Mat();
                var outArray = OutputArray.Create(outMat);
                var src1 = InputArray.Create(bgGray);
                var src2 = InputArray.Create(fgGray);

                var cap = new VideoCapture();
                var v = new VideoWriter();

                cap.Read(bgGray);
                v.Write(bgGray);
                v.Write(fgGray);
                //Cv2.Absdiff(bgGray, fgGray, outMat);
                knn.Apply(src1, outMat);

                var bytes = outMat.ImEncode();
                var img = Image.FromStream(new MemoryStream(bytes));

                PictureBox.Image = img;
            }
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
