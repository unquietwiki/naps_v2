// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NAPS
{
    public partial class ILProfileIcons : Component
    {
        public ILProfileIcons()
        {
            InitializeComponent();
        }

        public ILProfileIcons(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
