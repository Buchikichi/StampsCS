using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace StampsApp.util
{
    class ImageUtils
    {
        private static Rectangle CalcRect(params Mat[] images)
        {
            var rect = new Rectangle();
            var width = int.MaxValue;
            var height = int.MaxValue;

            foreach (var mat in images)
            {
                width = Math.Min(width, mat.Width);
                height = Math.Min(height, mat.Height);
            }
            rect.Width = width;
            rect.Height = height;
            return rect;
        }

        private static Mat ToGray(Mat src)
        {
            var gray = new Mat();

            CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
            return gray;
        }

        private static Mat Trim(Mat src, Rectangle rect)
        {
            // return src.Clone(rect);
            using (var srcBitmap = src.Bitmap)
            using (var dest = new Bitmap(rect.Width, rect.Height))
            using (var g = Graphics.FromImage(dest))
            {
                g.DrawImage(srcBitmap, 0, 0);
                return new Image<Bgr, byte>(dest).Mat;
            }
        }

        public static Image Absdiff(string bgName, string fgName)
        {
            using (var subt = new BackgroundSubtractorMOG2())
            using (var bg = new Mat(bgName))
            using (var fg = new Mat(fgName))
            using (var bgGray = ToGray(bg))
            using (var fgGray = ToGray(fg))
            {
                var diff = new Mat();
                var outMat = new Mat();
                var rect = CalcRect(bgGray, fgGray);
                var src1 = Trim(bgGray, rect);
                var src2 = Trim(fgGray, rect);

                //var cap = new VideoCapture();
                //var v = new VideoWriter();

                //cap.Read(bgGray);
                //v.Write(bgGray);
                //v.Write(fgGray);

                // TODO findTransformECC
                //Cv2.Absdiff(src1, src2, diff);
                CvInvoke.AbsDiff(src1, src2, diff);
                //subt.Apply(src1, outMat);
                //subt.Apply(src2, diff);
                //Cv2.Threshold(diff, diff, 50, 255, ThresholdTypes.Binary);
                CvInvoke.Threshold(diff, diff, 50, 255, ThresholdType.Binary);
                //Cv2.BitwiseNot(diff, diff);
                CvInvoke.BitwiseNot(diff, diff);
                //using (var stream = new MemoryStream(diff.ImEncode()))
                //{
                //    return Image.FromStream(stream);
                //}
                return diff.Bitmap;
            }
        }
    }
}
