// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace NAPS
{
    public class CScannedImage
    {
        private Bitmap baseImage;
        private Bitmap thumbnail;

        private int thumbnailWidth = 196;
        private int thumbnailHeight = 196;
        private CScanSettings.BitDepth bitDepth;

        public CScanSettings.BitDepth BitDepth
        {
            get { return bitDepth; }
            set { bitDepth = value; }
        }

        public Bitmap Thumbnail
        {
            get
            {
                return thumbnail;
            }
        }

        public Bitmap BaseImage
        {
            get { return baseImage; }
            set { baseImage = value; thumbnail = null; }
        }

        //Modified from http://www.deltasblog.co.uk/code-snippets/c-resizing-a-bitmap-image/
        private Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height )
		{
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(sourceBMP, 0, 0, width, height);
            	g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;            	
            }
            return result;
 		}        

        public CScannedImage()
        {            
        }

        public CScannedImage(Bitmap img, CScanSettings.BitDepth BitDepth)
        {
            baseImage = (Bitmap)img.Clone();
            thumbnail = ResizeBitmap(img, thumbnailWidth, thumbnailHeight);

            if (bitDepth == CScanSettings.BitDepth.BLACKWHITE)
            {
                baseImage = CImageHelper.CopyToBpp((Bitmap)img, 1);
                img.Dispose();
            }
            else
            {
                baseImage = img;
            }
            this.BitDepth = BitDepth;
        }

        internal void RotateFlip(RotateFlipType rotateFlipType)
        {
            baseImage.RotateFlip(rotateFlipType);
            thumbnail = ResizeBitmap(baseImage, thumbnailWidth, thumbnailHeight);
        }
    }
}
