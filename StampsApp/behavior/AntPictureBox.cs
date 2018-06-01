using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StampsApp.behavior
{
    public partial class AntPictureBox : PictureBox
    {
        #region Members
        private bool began;
        private Point? beginPt;
        private Point endPt;

        private Rectangle AntRect
        {
            get
            {
                if (!beginPt.HasValue)
                {
                    return default(Rectangle);
                }
                var width = Math.Abs(endPt.X - beginPt.Value.X);
                var height = Math.Abs(endPt.Y - beginPt.Value.Y);
                var x = Math.Min(beginPt.Value.X, endPt.X);
                var y = Math.Min(beginPt.Value.Y, endPt.Y);

                return new Rectangle(x, y, width, height);
            }
        }
        #endregion

        #region Events
        private void AntPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var pt = new Point(e.X, e.Y);

            if (AntRect.Contains(pt))
            {
                return;
            }
            began = true;
            beginPt = pt;
            endPt = pt;
            //Debug.Print($"MouseDown[{e.X}/{e.Y}]");
            Refresh();
        }

        private void AntPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!began)
            {
                return;
            }
            endPt = new Point(e.X, e.Y);
            //Debug.Print($"MouseMove[{e.X}/{e.Y}]");
            Refresh();
        }

        private void AntPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            began = false;
        }

        private void AntPictureBox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            using (var pen = new Pen(Brushes.Red, 1))
            {
                g.DrawRectangle(pen, AntRect);
            }
        }
        #endregion

        #region Method
        public Bitmap TrimImage(Bitmap srcImg)
        {
            var rect = AntRect;

            var scaleW = (double)Width / srcImg.Width;
            var scaleH = (double)Height / (srcImg.Height);
            var scale = Math.Min(scaleW, scaleH);
            var width = (int)(rect.Width / scale);
            var height = (int)(rect.Height / scale);
            var diffX = (int)(Width - (srcImg.Width * scale)) / 2;
            var diffY = (int)(Height - (srcImg.Height * scale)) / 2;
            var x = (int)((rect.X - diffX) / scale);
            var y = (int)((rect.Y - diffY) / scale);
            var bmp = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(srcImg, -x, -y);
            }
            return bmp;
        }

        public void Reset()
        {
            beginPt = null;
            Refresh();
        }
        #endregion

        #region 開始/終了
        public AntPictureBox()
        {
            InitializeComponent();
        }

        public AntPictureBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion
    }
}
