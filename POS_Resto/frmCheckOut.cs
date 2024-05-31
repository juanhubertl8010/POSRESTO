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
    public partial class frmCheckOut : Form
    {
        public frmCheckOut()
        {
            InitializeComponent();
        }

        public double amount;
        public string pesananaID;

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            // 23.43
            double amount;
            double receipt;
            double change;

            double.TryParse(txtBillAmount.Text, out amount);
            double.TryParse(txtReceived.Text, out receipt);
            
            change = receipt - amount;
            txtChange.Text = change.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtReceived.Text == "")
            {
                guna2MessageDialog1.Show("Gaboleh kosong yaa");
                return;
            }
            if(Convert.ToInt32(txtChange.Text) <= 0)
            {
                guna2MessageDialog1.Show("Duitnya Kurang Bang");
                return;
            }
            string qry = @"UPDATE pesanan SET total = @total, kembalian = @change, received = @received, kondisi = 'Paid' WHERE pesanan_id = @id;";

            Hashtable ht = new Hashtable();
            ht.Add("@id", pesananaID);
            ht.Add("@total", txtBillAmount.Text);
            ht.Add("@change",txtChange.Text);
            ht.Add("@received", txtReceived.Text);


            MainClass.SQL(qry, ht);

            string qry2 = @"UPDATE meja set status = 1 WHERE nomor_meja = (SELECT nomor_meja FROM pesanan WHERE pesanan_id = @id);";

            Hashtable ht2 = new Hashtable();
            ht2.Add("@id", pesananaID);
            MainClass.SQL(qry2, ht2);


            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Show("Saved Successfully");
            this.Close();
        }

        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text =  amount.ToString();
        }

        private void txtReceived_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan digit
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 adalah kode karakter backspace
            {
                e.Handled = true; // Mencegah karakter yang tidak valid ditampilkan di textbox
            }
        }
    }
}
