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
    public partial class FPDFSave : Form
    {
        public FPDFSave()
        {
            InitializeComponent();
        }

        public void SetStatus(int count, int total)
        {
            lblStatus.Text = count.ToString() + " of " + total.ToString();
            Application.DoEvents();
        }
    }

}
