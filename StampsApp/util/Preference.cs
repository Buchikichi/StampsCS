using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace StampsApp.util
{
    class Preference
    {
        public static readonly Preference Instance = new Preference();
        private const string INI_FILE = "preference.ini";
        private const string CATEGORY_IMAGE = "IMAGE";
        private const string CATEGORY_FILE = "FILE";

        #region Method
        public int GetInt(string appName, string key, int defaultValue = 0)
        {
            return (int)GetPrivateProfileInt(appName, key, defaultValue, Filename);
        }

        public string GetString(string appName, string key, string defaultValue = "")
        {
            var buff = new StringBuilder(1024);

            GetPrivateProfileString(appName, key, defaultValue, buff, Convert.ToUInt32(buff.Capacity), Filename);
            return buff.ToString();
        }

        public Rectangle GetRectangle(string appName, string key, string defaultValue = "")
        {
            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;
            var str = GetString(appName, key);
            var elements = str.Split(',');

            if (4 <= elements.Length)
            {
                int.TryParse(elements[0], out x);
                int.TryParse(elements[1], out y);
                int.TryParse(elements[2], out width);
                int.TryParse(elements[3], out height);
            }
            return new Rectangle(x, y, width, height);
        }
        #endregion

        #region Properties
        public string Filename { get; }

        public int TopMargin => GetInt(CATEGORY_IMAGE, nameof(TopMargin));
        public Rectangle BaseRectangle => GetRectangle(CATEGORY_IMAGE, nameof(BaseRectangle));
        public Rectangle ClipRectangle => GetRectangle(CATEGORY_IMAGE, nameof(ClipRectangle));
        public int JpegQuality => GetInt(CATEGORY_IMAGE, nameof(JpegQuality));

        public Rectangle DetectRectangle => GetRectangle(CATEGORY_IMAGE, nameof(DetectRectangle));

        public string ImageDir => GetString(CATEGORY_FILE, nameof(ImageDir));
        public string PosDir => ImageDir + "/" + GetString(CATEGORY_FILE, nameof(PosDir));
        public string NegDir => ImageDir + "/" + GetString(CATEGORY_FILE, nameof(NegDir));
        #endregion

        #region 開始/終了
        protected Preference()
        {
            Filename = Application.StartupPath + '\\' + INI_FILE;
            if (!File.Exists(Filename))
            {
                MessageBox.Show($"INIファイルがありません。\n[{Filename}]");
            }
        }
        #endregion

        #region KERNEL32
        [DllImport("KERNEL32.DLL")]
        public static extern uint
             GetPrivateProfileString(string lpAppName,
             string lpKeyName, string lpDefault,
             StringBuilder lpReturnedString, uint nSize,
             string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
            GetPrivateProfileInt(string lpAppName,
            string lpKeyName, int nDefault, string lpFileName);
        #endregion
    }
}
