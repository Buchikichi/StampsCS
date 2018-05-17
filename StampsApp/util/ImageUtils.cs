using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;

namespace StampsApp.util
{
    class ImageUtils
    {
        private static Rect CalcRect(params Mat[] images)
        {
            var rect = new Rect();
            var width = int.MaxValue;
            var height = int.MaxValue;

            foreach (var mat in images)
            {
                var rectangle = mat.BoundingRect();

                width = Math.Min(width, rectangle.Width);
                height = Math.Min(height, rectangle.Height);
            }
            rect.Width = width;
            rect.Height = height;
            return rect;
        }

        public static Image Absdiff(string bgName, string fgName)
        {
            using (var subt = BackgroundSubtractorMOG2.Create())
            using (var bg = new Mat(bgName, ImreadModes.Color))
            using (var fg = new Mat(fgName, ImreadModes.Color))
            using (var bgGray = bg.CvtColor(ColorConversionCodes.BGR2GRAY))
            using (var fgGray = fg.CvtColor(ColorConversionCodes.BGR2GRAY))
            {
                var diff = new Mat();
                var outMat = new Mat();
                var rect = CalcRect(bgGray, fgGray);
                var src1 = bgGray.Clone(rect);
                var src2 = fgGray.Clone(rect);

                //var cap = new VideoCapture();
                //var v = new VideoWriter();

                //cap.Read(bgGray);
                //v.Write(bgGray);
                //v.Write(fgGray);

                // TODO findTransformECC
                //Cv2.Absdiff(src1, src2, diff);
                subt.Apply(src1, outMat);
                subt.Apply(src2, diff);
                Cv2.Threshold(diff, diff, 50, 255, ThresholdTypes.Binary);
                //Cv2.BitwiseNot(diff, diff);
                using (var stream = new MemoryStream(diff.ImEncode()))
                {
                    return Image.FromStream(stream);
                }
            }
        }
    }
}
