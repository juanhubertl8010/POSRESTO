using System;
using System.Collections;
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
    public partial class frmSignup : Form
    {
        public frmSignup()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {

            if (txtName.Text == "" || txtPass.Text == "" || txtPhone.Text == "" || txtUserName.Text == "")
            {
                guna2MessageDialog1.Show("Gaboleh Kosong woi");
                return;
            }
            string qry = $"INSERT INTO users (username, upass, uName, uPhone) VALUES ('{txtUserName.Text}', '{txtPass.Text}', '{txtName.Text}', '{txtPhone.Text}');";
            Hashtable ht = new Hashtable();
           
            if (MainClass.SQL(qry, ht) > 0)
            {
                txtName.Text = "";
                txtPass.Text = "";
                txtUserName.Text = "";
                txtPhone.Text = "";
                guna2MessageDialog1.Show("Create Account Success");
            }
       
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan digit
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 adalah kode karakter backspace
            {
                e.Handled = true; // Mencegah karakter yang tidak valid ditampilkan di textbox
            }
        }
    }
}
