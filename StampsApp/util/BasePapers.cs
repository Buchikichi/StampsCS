using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace StampsApp.util
{
    class BasePapers
    {
        #region Members
        private const int SRC_X = 50;
        private const int SRC_WIDTH = 1600;
        private const int SRC_HEIGHT = 800;
        private const double BASE_RATIO = .1;
        private const int BASE_WIDTH = (int)(SRC_WIDTH * BASE_RATIO);
        private const int BASE_HEIGHT = (int)(SRC_HEIGHT * BASE_RATIO);

        public List<BasePaperInfo> BasePaperList { get; } = new List<BasePaperInfo>();
        #endregion

        public BasePaperInfo GetBasePaper(string filename)
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
            return list[0];
        }

        #region 開始/終了
        private byte[] LoadImage(string filename)
        {
            var conv = new ImageConverter();

            using (var img = new Bitmap(BASE_WIDTH, BASE_HEIGHT))
            using (var g = Graphics.FromImage(img))
            using (var src = Image.FromFile(filename))
            {
                var srcRect = new Rectangle(SRC_X, 0, SRC_WIDTH, SRC_HEIGHT);
                var destRect = new Rectangle(0, 0, BASE_WIDTH, BASE_HEIGHT);

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
