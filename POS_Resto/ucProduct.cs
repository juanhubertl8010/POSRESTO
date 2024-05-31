using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Resto
{
    
    public partial class ucProduct : UserControl
    {
        public ucProduct()
        {
            InitializeComponent();
        }
        public event EventHandler onSelect = null;

        public string id { get; set; }
        public double Mprice { get; set; }
        public string Mcategory { get; set; }
        public string Mname
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public Image MImage
        {
            get { return txtImage.Image; }
            set { txtImage.Image = value; }
        }

        private void txtImage_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }


    }
}
