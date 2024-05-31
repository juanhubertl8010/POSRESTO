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
    public partial class frmMain : Form
    {
        //public static Size Instance { get; internal set; }

        public frmMain()
        {
            InitializeComponent();
        }

        static frmMain _obj;
        public static frmMain Instance
        {
            get { if (_obj == null) { _obj = new frmMain(); } return _obj; }
        }

        // method to add controls in main form

        public void AddControls(Form f)
        {
            centerPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            centerPanel.Controls.Add(f);
            f.Show();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            labelUser.Text = MainClass.USER;
            _obj = this;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

      

        private void centerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            AddControls(new frmCustomer());
        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            AddControls(new frmTable());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaff());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            AddControls(new frmMenu());
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.Show();
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            AddControls(new frmKitchen());
        }
    }
}
