using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace StampsApp.util
{
    class BasePapers
    {
        #region Members
        private const double BASE_RATIO = .2;
        private Preference ini = Preference.Instance;

        public List<BasePaperInfo> BasePaperList { get; } = new List<BasePaperInfo>();
        #endregion

        private void CalcDiff(BasePaperInfo info, string srcFile)
        {
            var src = LoadImage(srcFile, 1);
            var dst = LoadImage(info.Filename, 1);
            var diff = ImageUtils.CalcDiff(src, dst);
            var sq = Math.Abs(diff.Width * diff.Height);

            Debug.Print("diff=" + diff.Width + ":" + diff.Height + "/sq=" + sq);
            if (sq < 40)
            {
                info.DiffW = diff.Width;
                info.DiffH = diff.Height;
            }
        }

        public BasePaperInfo ChooseBasePaper(string filename)
        {
            var list = new List<BasePaperInfo>();
            var bytes = LoadImage(filename);
            var conv = new ImageConverter();

            using (var targetImage = (Bitmap)conv.ConvertFrom(bytes))
            using (var targetMat = new Image<Bgr, byte>(targetImage).Mat)
            {
                {
                    foreach (var element in BasePaperList)
                    {
                        var paper = element.Clone();

                        using (var img = (Bitmap)conv.ConvertFrom(paper.Image))
                        using (var mat = new Image<Bgr, byte>(img).Mat)
                        {
                            paper.Distance = ImageUtils.CalcDistance(targetMat, mat);
                            list.Add(paper);
                        }
                    }
                }
            }
            list.Sort((a, b) => (int)(a.Distance * 10000 - b.Distance * 10000));
            //list.ForEach(info => Debug.Print(info.Name + ":" + info.Distance));
            var result = list[0];
            CalcDiff(result, filename);
            return result;
        }

        #region 開始/終了
        private byte[] LoadImage(string filename, double ratio = BASE_RATIO)
        {
            var srcRect = ini.BaseRectangle;
            var baseWidth = (int)(srcRect.Width * ratio);
            var baseHeight = (int)(srcRect.Height * ratio);
            var conv = new ImageConverter();

            using (var img = new Bitmap(baseWidth, baseHeight))
            using (var g = Graphics.FromImage(img))
            using (var src = Image.FromFile(filename))
            {
                var destRect = new Rectangle(0, 0, baseWidth, baseHeight);

                g.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
                return (byte[])conv.ConvertTo(img, typeof(byte[]));
            }
        }

        public BasePapers(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }
            var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
            var cnt = 0;

            foreach (var filename in files)
            {
                var name = "base" + cnt.ToString("00");

                BasePaperList.Add(new BasePaperInfo
                {
                    Filename = filename,
                    Name = name,
                    Image = LoadImage(filename),
                });
                cnt++;
            }
        }
        #endregion
    }
}
