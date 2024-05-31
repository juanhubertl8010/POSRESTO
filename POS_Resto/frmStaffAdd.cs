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
    public partial class frmStaffAdd : Form
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }

        public int noid = 0;
        public virtual void btnClose_Click(object sender, EventArgs e)
        {
           
        }
        public string id;
        public virtual void btnSave_Click(object sender, EventArgs e)
        {
          

            
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text == "" || cbRole.Text == "" || txtPhone.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Gaboleh Kosong Yaa");
                return;
            }

            string qry = "";
            string name = txtName.Text;
            string role = cbRole.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            int urutan = MainClass.jumlahRow + 1;
            //id = "";
            int status = 1;

            if (noid == 0) // untuk insert nilai baru
            {
                if(urutan >= 10)
                {
                    id = "KR0" + urutan.ToString();
                }
                if(urutan >= 100)
                {
                    id = "KR" + urutan.ToString();
                }
                if(urutan < 10)
                {
                    id = "KR00" + urutan.ToString();
                }
                 // belum sempurna penambahannya
                qry = $"INSERT INTO Karyawan VALUES ('{id}', '{name}', '{address}', '{phone}', '{role}' , '{status}');";
                noid = 0;

            }
            else // untuk update customer
            {
                qry = $"UPDATE Karyawan SET nama = '{name}', alamat = '{address}', telepon = '{phone}', jabatan = '{role}' WHERE karyawan_id = '{id}'; ";
                noid = 0;
            }


            Hashtable ht = new Hashtable();
            ht.Add("karyawan_id", id);
            ht.Add("nama", name);
            ht.Add("alamat", address);
            ht.Add("telepon", phone);
            ht.Add("jabatan", role);
            ht.Add("status", status);
            frmCustomer frmCustomerObj = new frmCustomer();
            MainClass.SQL(qry, ht);

            frmCustomerObj.RefreshDataGridView();
            guna2MessageDialog1.Show("Success");
            txtName.Focus();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
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
