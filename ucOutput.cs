using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Des
{
    public partial class ucOutput : UserControl
    {
        public RichTextBox RtbOutput
        {
            get { return rtbOutput; }
            set { rtbOutput = value; }
        }

        public ucOutput()
        {
            InitializeComponent();
        }
    }
}
