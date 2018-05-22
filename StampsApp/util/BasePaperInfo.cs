﻿namespace StampsApp.util
{
    class BasePaperInfo
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public double Distance { get; set; }

        public BasePaperInfo Clone()
        {
            return (BasePaperInfo)MemberwiseClone();
        }
    }
}
