using System.IO;

namespace StampsApp.data
{
    class PictureInfo
    {
        #region Method
        public override string ToString()
        {
            return FileName;
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public string FileName
        {
            get
            {
                return Path.GetFileName(Name);
            }
}
        #endregion
    }
}
