// Not Another PDF Scanner v2
// Original Copyright � 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NAPS
{
    public partial class FChooseIcon : Form
    {
        private int iconID;

        public int IconID
        {
            get { return iconID; }
            set { iconID = value; }
        }

        public FChooseIcon()
        {
            InitializeComponent();
            iconID = -1;
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            iconID = iconList.SelectedItems[0].Index;
            this.Close();
        }

        private void FChooseIcon_Load(object sender, EventArgs e)
        {
            iconList.LargeImageList = ilProfileIcons.IconsList;
            int  i = 0;
            foreach (Image icon in ilProfileIcons.IconsList.Images)
            {
                iconList.Items.Add("", i);
                i++;
            }
        }
    }
}