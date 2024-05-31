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
using MySql.Data.MySqlClient;
using System.Data;


namespace POS_Resto
{
    public partial class frmAddCustomer : Form
    {
        DataTable dataSimpan = new DataTable();
        public static readonly string con_string = "server=localhost;uid=root;pwd=;database=db_resto ; ";
        public static MySqlConnection con = new MySqlConnection(con_string);
        public frmAddCustomer()
        {
            InitializeComponent();
        }
        public int noid = 0;
        public virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            frmCustomer frm = new frmCustomer();
            frm.RefreshDataGridView();
        }
        public  string id;
        public virtual void btnSave_Click(object sender, EventArgs e)
        {
            
            if(txtName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || txtAddress.Text == "")
            {
                guna2MessageDialog1.Show("Gaboleh Kosong Yaa");
                //MessageBox.Show("Gaboleh Kosong Yaa");
                return;
            }
            
            string qry = "";
            string name = txtName.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            int urutan = MainClass.jumlahRow + 1;
            //id = "";
            int status = 1;

            if (noid == 0) // untuk insert nilai baru
            {
                
                id = "PLG0" + urutan.ToString();
                qry = $"INSERT INTO pelanggan VALUES ('{id}', '{name}', '{email}', '{phone}', '{address}', '{status}');";
                noid = 0;

            }
            else // untuk update customer
            {
                qry = $"UPDATE pelanggan SET nama = '{name}', email = '{email}', telepon = '{phone}', alamat = '{address}' WHERE pelanggan_id = '{id}'; ";
                noid = 0;
            }
            

            Hashtable ht = new Hashtable();
            ht.Add("pelanggan_id", id);
            ht.Add("nama", name);
            ht.Add("email", email);
            ht.Add("telepon", phone);
            ht.Add("alamat", address);
            ht.Add("status", status);
            frmCustomer frmCustomerObj = new frmCustomer();
            MainClass.SQL(qry, ht);
  
            frmCustomerObj.RefreshDataGridView();
            guna2MessageDialog1.Show("Success");
            txtName.Focus();
        }

        private void frmAddCustomer_Load(object sender, EventArgs e)
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
