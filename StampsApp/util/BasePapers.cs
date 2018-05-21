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
        private const int SRC_HEIGHT = 640;
        private const int BASE_WIDTH = SRC_WIDTH / 4;
        private const int BASE_HEIGHT = SRC_HEIGHT / 4;

        public List<BasePaperInfo> BasePaperList { get; } = new List<BasePaperInfo>();
        #endregion

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
