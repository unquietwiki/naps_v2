// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace NAPS
{
    public class CPageSizes
    {
        public enum PageSize
        {
            A5,
            A4,
            A3,
            LEGAL,
            LETTER
        }

        public static string[] GetPageSizeList()
        {
            return new string[] { "A5 (148x210 mm)", "A4 (210x297 mm)", "A3 (297x420 mm)", "US LEGAL (8.5x14 in)", "US LETTER (8.5x11 in)" };
        }

        public static Size TranslatePageSize(PageSize pageSize)
        {
            List<Size> sizes = new List<Size>();
            sizes.Add(new Size(5826, 8267));
            sizes.Add(new Size(8267, 11692));
            sizes.Add(new Size(11692, 16535));
            sizes.Add(new Size(8500, 14000));
            sizes.Add(new Size(8500, 11000));

            return sizes[(int)pageSize];
        }
    }
}
