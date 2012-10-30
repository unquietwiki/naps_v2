// Not Another PDF Scanner v2
// Original Copyright � 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Collections.Generic;
using System.Text;

namespace NAPS
{
    public class CScanSettings
    {
        public enum ScanSource
        {
            GLASS,
            FEEDER,
            DUPLEX
        }

        public enum BitDepth
        {
            C24BIT,
            GRAYSCALE,
            BLACKWHITE
        }

        public enum DPI
        {
            DPI100,
            DPI200,
            DPI300,
            DPI600,
            DPI1200
        }

        public enum HorizontalAlign
        {
            LEFT,
            CENTER,
            RIGHT
        }

        public enum Scale
        {
            ONETOONE,
            ONETOTWO,
            ONETOFOUR,
            ONETOEIGHT
        }

        public enum Driver
        {
            WIA,
            TWAIN
        }

        private string deviceID;
        private Driver deviceDriver;

        private ScanSource source;
        private DPI resolution;
        private int brightnes;
        private int contrast;
        private bool showScanUI;
        private BitDepth depth;
        private int iconID;
        private string displayName;
        private CPageSizes.PageSize pageSize;
        private HorizontalAlign pageAlign;
        private Scale afterScanScale;

        public HorizontalAlign PageAlign
        {
            get { return pageAlign; }
            set { pageAlign = value; }
        }

        public Scale AfterScanScale
        {
            get { return afterScanScale; }
            set { afterScanScale = value; }
        }

        public CPageSizes.PageSize PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public bool ShowScanUI
        {
            get { return showScanUI; }
            set { showScanUI = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public int IconID
        {
            get { return iconID; }
            set { iconID = value; }
        }

        public BitDepth Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public string DeviceID
        {
            get{ return deviceID;}
            set{deviceID = value;}
        }

        public Driver DeviceDriver
        {
            get { return deviceDriver; }
            set { deviceDriver = value; }
        }
        
        public ScanSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public DPI Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public int Contrast
        {
            get { return contrast; }
            set { contrast = value; }
        }

        public int Brightnes
        {
            get { return brightnes; }
            set { brightnes = value; }
        }

        public CScanSettings()
        {
            ShowScanUI = false;
            DisplayName = "";
            IconID = 0;
            Depth = BitDepth.C24BIT;
            DeviceID = "";
            Source = ScanSource.GLASS;
            Resolution = DPI.DPI200;
            Contrast = 0;
            Brightnes = 0;
            PageSize = CPageSizes.PageSize.A4;
            PageAlign = HorizontalAlign.CENTER;
            AfterScanScale = Scale.ONETOONE;
            DeviceDriver = Driver.WIA;
        }
    }
}
