using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;

namespace StampsApp.util
{
    class ImageUtils
    {
        private const int MAX_ITERATION = 5;
        private static Preference ini = Preference.Instance;

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

        private static Mat Trim(Mat src, Rectangle rect)
        {
            // return src.Clone(rect);
            using (var srcBitmap = src.Bitmap)
            using (var dest = new Bitmap(rect.Width, rect.Height))
            using (var g = Graphics.FromImage(dest))
            {
                g.DrawImage(srcBitmap, -rect.X, -rect.Y);
                var mat = new Image<Bgr, byte>(dest).Mat;

                CvInvoke.CvtColor(mat, mat, ColorConversion.Bgr2Gray);
                return mat;
            }
        }

        private static void Affine(BasePaperInfo bgInfo, Mat dst)
        {
            var w = ((bgInfo.DiffW + 1) * 2) - 1;
            float[,] matrix = { { 1.0f, 0.0f, w }, { 0.0f, 1.0f, bgInfo.DiffH } };
            var warp = new Matrix<float>(matrix);

            CvInvoke.WarpAffine(dst, dst, warp, dst.Size);
        }

        private static void Transform(Mat src, Mat dst)
        {
            var src2 = new Mat();
            var dst2 = new Mat();
            var warp = Mat.Eye(3, 3, DepthType.Cv32F, 1);
            var criteria = new MCvTermCriteria(MAX_ITERATION, 1e-10);

            CvInvoke.Canny(src, src2, 250, 255);
            CvInvoke.Canny(dst, dst2, 250, 255);
            try
            {
                CvInvoke.FindTransformECC(dst2, src2, warp, MotionType.Homography, criteria);
                CvInvoke.WarpPerspective(dst, dst, warp, src.Size);
            }
            catch (Exception e)
            {
                // nop
            }
        }

        private static void FindContours(Mat mat)
        {
            using (var contours = new VectorOfVectorOfPoint())
            {
                var bin = new Mat();

                CvInvoke.Threshold(mat, bin, 80, 255, ThresholdType.Binary);
                CvInvoke.FindContours(bin, contours, null, RetrType.List, ChainApproxMethod.LinkRuns);
                CvInvoke.BitwiseNot(mat, mat);
                CvInvoke.CvtColor(mat, mat, ColorConversion.Gray2Bgr);
                for (var ix = 0; ix < contours.Size; ix++)
                {
                    var area = CvInvoke.ContourArea(contours[ix]);

                    if (area < 4000 || 90000 < area)
                    {
                        continue;
                    }
                    using (var vec = new VectorOfPoint())
                    {
                        var peri = CvInvoke.ArcLength(contours[ix], true);

                        CvInvoke.ApproxPolyDP(contours[ix], vec, peri * .1, true);
                        if (vec.Size != 4)
                        {
                            continue;
                        }
                        var cn = new VectorOfVectorOfPoint();
                        cn.Push(vec);
                        CvInvoke.DrawContours(mat, cn, 0, new MCvScalar(0, 0, 200), 8);
                    }
                }
            }
        }

        public static double CalcDistance(Mat src, Mat dst)
        {
            var detector = new AKAZE();
            var matcher = new BFMatcher(DistanceType.Hamming2);
            var srcKeyPoints = new VectorOfKeyPoint();
            var dstKeyPoints = new VectorOfKeyPoint();
            var srcDes = new UMat();
            var dstDes = new UMat();
            var matches = new VectorOfVectorOfDMatch();
            var k = 2;
            var total = 0.0;

            detector.DetectAndCompute(src, null, srcKeyPoints, srcDes, false);
            detector.DetectAndCompute(dst, null, dstKeyPoints, dstDes, false);
            matcher.Add(srcDes);
            matcher.KnnMatch(dstDes, matches, k, null);
            foreach (var ary in matches.ToArrayOfArray())
            {
                foreach (var match in ary)
                {
                    total += match.Distance;
                }
            }
            return total / matches.Size;
        }

        public static Size CalcDiff(byte[] src, byte[] dst)
        {
            var conv = new ImageConverter();

            using (var srcImage = (Bitmap)conv.ConvertFrom(src))
            using (var srcMat = new Image<Bgr, byte>(srcImage).Mat)
            using (var dstImage = (Bitmap)conv.ConvertFrom(dst))
            using (var dstMat = new Image<Bgr, byte>(dstImage).Mat)
            {
                CvInvoke.CvtColor(srcMat, srcMat, ColorConversion.Bgr2Gray);
                CvInvoke.CvtColor(dstMat, dstMat, ColorConversion.Bgr2Gray);
                var srcM = CvInvoke.Moments(srcMat);
                var dstM = CvInvoke.Moments(dstMat);
                var srcPt = new Point((int)(srcM.M10 / srcM.M00), (int)(srcM.M01 / srcM.M00));
                var dstPt = new Point((int)(dstM.M10 / dstM.M00), (int)(dstM.M01 / dstM.M00));

                return new Size(srcPt.X - dstPt.X, srcPt.Y - dstPt.Y);
            }
        }

        public static Image Lines(string filename)
        {
            using (var mat = new Mat(filename, ImreadModes.ReducedGrayscale2))
            {
                var min = mat.Height / 20;
                CvInvoke.Canny(mat, mat, 250, 255, 3);
                var lines = CvInvoke.HoughLinesP(mat, 1, Math.PI / 180, 50, min);
                CvInvoke.BitwiseNot(mat, mat);
                var bmp = mat.Bitmap;
                var dest = new Bitmap(bmp.Width, bmp.Height);

                using (var g = Graphics.FromImage(dest))
                {
                    g.DrawImage(bmp, 0, 0);
                    foreach (var seg in lines)
                    {
                        g.DrawLine(Pens.Red, seg.P1, seg.P2);
                    }
                }
                return dest;
            }
        }

        public static Image Absdiff(BasePaperInfo bgInfo, string fgName)
        {
            using (var bg = new Mat(bgInfo.Filename))
            using (var fg = new Mat(fgName))
            {
                var rect = ini.ClipRectangle;
                using (var bgGray = Trim(bg, rect))
                using (var fgGray = Trim(fg, rect))
                {
                    Affine(bgInfo, fgGray);
                    Transform(bgGray, fgGray);
                    var diff = new Mat();

                    CvInvoke.AbsDiff(bgGray, fgGray, diff);
                    //FindContours(diff);
                    //CvInvoke.Threshold(diff, diff, 30, 255, ThresholdType.Binary);
                    CvInvoke.BitwiseNot(diff, diff);
                    return diff.Bitmap;
                }
            }
        }
    }
}
