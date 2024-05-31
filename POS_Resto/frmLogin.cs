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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtboxUser.Text == "" || txtboxPass.Text == "")
            {
                guna2MessageDialog1.Show("Gaboleh Kosong woi");
                return;
            }
           if( MainClass.IsValidUUser(txtboxUser.Text, txtboxPass.Text) == false)
            {
                guna2MessageDialog1.Show("invalid username or password");
                return;
            }
            else
            {
                this.Hide();
                frmMain frm = new frmMain();
                frm.Show();
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            frmSignup frm = new frmSignup();
            MainClass.BlurBackground(frm);
        }
    }
}
