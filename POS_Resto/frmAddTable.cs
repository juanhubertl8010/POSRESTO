using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Resto
{
    public partial class frmAddTable : Form
    {
        public frmAddTable()
        {
            InitializeComponent();
        }
        public int noid = 0;
        public virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string id;
        public virtual void btnSave_Click(object sender, EventArgs e)
        {

            if (txtNameTable.Text == "" || txtKapasitas.Text == "")
            {
                MessageBox.Show("Gaboleh Kosong Yaa");
                return;
            } 

            string namaMeja = txtNameTable.Text;
            string qry = "SELECT * FROM meja;";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                if (namaMeja == item["nomor_meja"].ToString())
                {
                    guna2MessageDialog1.Show("Nomor Meja sudah ada");
                    return;
                }
            }

            string name = txtNameTable.Text;
            string kapasitas = txtKapasitas.Text;
            int urutan = MainClass.jumlahRow + 1;
            //id = "";
            int status = 1;

            if (noid == 0) // untuk insert nilai baru
            {
                if(urutan >= 100)
                {
                    id = "MJ" + urutan.ToString();
                }
                if(urutan >= 10)
                {
                    id = "MJ0" + urutan.ToString();
                }
                
                qry = $"INSERT INTO meja VALUES ('{id}', '{name}', '{kapasitas}', '{status}');";
                noid = 0;

            }
            else // untuk update customer
            {
                qry = $"UPDATE meja SET nomor_meja = '{name}', kapasitas = '{kapasitas}' WHERE meja_id = '{id}'; ";
                noid = 0;
            }


            Hashtable ht = new Hashtable();
            ht.Add("meja_id", id);
            ht.Add("nomor_meja", name);
            ht.Add("kapasitas", kapasitas);
            ht.Add("status", status);
            frmCustomer frmCustomerObj = new frmCustomer();
            MainClass.SQL(qry, ht);

            frmCustomerObj.RefreshDataGridView();
            guna2MessageDialog1.Show("Success");
            txtNameTable.Focus();
        }

        private void txtNameTable_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
