// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
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
    public partial class FChooseProfile : Form
    {
        private List<CScanSettings> settings;

        private CScanSettings profile;

        public CScanSettings Profile
        {
            get { return profile; }
        }

        public FChooseProfile()
        {
            InitializeComponent();
        }

        private void FChooseProfile_Load(object sender, EventArgs e)
        {
            lvProfiles.LargeImageList = ilProfileIcons.IconsList;
            settings = CSettings.LoadProfiles();
            foreach (CScanSettings profile in settings)
            {
                lvProfiles.Items.Add(profile.DisplayName, profile.IconID);
            }
        }

        private void lvProfiles_ItemActivate(object sender, EventArgs e)
        {
            profile = settings[lvProfiles.SelectedItems[0].Index];
            this.Close();
        }
    }
}