using StampsApp.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StampsApp.IO
{
    public class Samples
    {
        private Preference ini = Preference.Instance;

        public void RotateImage()
        {
            var baseDir = ini.PosDir;

            foreach (var file in Directory.GetFiles(baseDir, "*.jpg"))
            {
                var ix = file.IndexOf('.');
                var last = file.LastIndexOf('.');

                if (ix != last)
                {
                    continue;
                }
                var name = Path.GetFileNameWithoutExtension(file);

                using (var src = Image.FromFile(file))
                {
                    var w = src.Width;
                    var h = src.Height;
                    var len = (int)Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2));
                    var half = len / 2;

                    for (var degree = 1.0f; degree < 360; degree += 1)
                    {
                        var dstFile = baseDir + $"{name}.{degree}.jpeg";

                        if (File.Exists(dstFile))
                        {
                            continue;
                        }
                        using (var dst = new Bitmap(len, len))
                        using (var g = Graphics.FromImage(dst))
                        {
                            g.Clear(Color.White);
                            g.TranslateTransform(half, half);
                            g.RotateTransform(degree);
                            g.DrawImage(src, -w / 2, -h / 2);
                            dst.Save(dstFile, ImageFormat.Jpeg);
                        }
                    }
                }
            }
        }

        public void Save()
        {
            var posFile = ini.PosDir + "list.txt";
            var negFile = ini.NegDir + "neglist.txt";

            RotateImage();
            using (var writer = new StreamWriter(posFile, append: false))
            {
                foreach (var file in Directory.GetFiles(ini.PosDir, "*.jp*"))
                {
                    var name = Path.GetFileName(file);

                    using (var img = Image.FromFile(file))
                    {
                        var width = img.Width;
                        var height = img.Height;
                        var line = new List<string>{
                            name, "1", "0", "0", width.ToString(), height.ToString(),
                        };
                        writer.WriteLine(string.Join(" ", line));
                    }
                }
            }
            using (var writer = new StreamWriter(negFile, append: false))
            {
                var negList = new List<string>();
                var rel = Path.GetFileName(Path.GetDirectoryName(ini.NegDir));

                Debug.Print(rel);
                foreach (var ptn in new string[] { "*.jpg", "*.png" })
                {
                    negList.AddRange(Directory.GetFiles(ini.NegDir, ptn));
                }
                foreach (var file in negList)
                {
                    var name = "./" + rel + "/" + Path.GetFileName(file);

                    writer.WriteLine(name);
                    Debug.Print(name);
                }
            }
        }
    }
}
