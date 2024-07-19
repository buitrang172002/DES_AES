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
    public partial class ucCompare : UserControl
    {
        public RichTextBox RtbBanro_DES 
        { 
            get { return rtbBanro_DES; } 
            set { rtbBanro_DES = value; }
        }
        public RichTextBox RtbBanro_AES 
        { 
            get { return rtbBanro_AES; }
            set { rtbBanro_AES = value; }
        }
        public RichTextBox RtbBanma_DES 
        { 
            get { return rtbBanma_DES; } 
            set { rtbBanma_DES = value; }
        }
        public RichTextBox RtbBanma_AES 
        { 
            get { return rtbBanma_AES; } 
            set { rtbBanma_AES = value; }
        }
        public TextBox TbTocdomahoa_DES 
        { 
            get { return tbTocdomahoa_DES; } 
            set { tbTocdomahoa_DES = value; }
        }
        public TextBox TbTocdomahoa_AES 
        { 
            get { return tbTocdomahoa_AES; } 
            set { tbTocdomahoa_AES = value; }
        }
        public TextBox TbTocdogiaima_DES 
        { 
            get { return tbTocdogiaima_DES; } 
            set { tbTocdogiaima_DES = value; }
        }
        public TextBox TbTocdogiaima_AES 
        { 
            get { return tbTocdogiaima_AES; } 
            set { tbTocdogiaima_AES = value; }
        }

        public ucCompare()
        {
            InitializeComponent();
        }
    }
}
