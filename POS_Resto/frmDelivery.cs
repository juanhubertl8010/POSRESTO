using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace POS_Resto
{
    public partial class frmDelivery : Form
    {
        public frmDelivery()
        {
            InitializeComponent();
        }

        public string orderType = "";
        public string driverId = "";
        public string pesananId = "";
        public string customerId = "";
        public string driverName = "";
        public string customerName = "";
        private void frmDelivery_Load(object sender, EventArgs e)
        {
            if(orderType == "Take Away")
            {
                labelDriver.Visible = false;
                cbDriver.Visible = false;
                labelAddress.Visible = false;
                txtAddress.Visible = false;
            }

            string qry = "SELECT karyawan_id AS 'id', nama AS 'name' FROM karyawan WHERE jabatan = 'Driver';";
            MainClass.CBFill(qry, cbDriver);

            string qry2 = "SELECT pelanggan_id AS 'id', nama AS 'name', telepon, alamat FROM pelanggan WHERE status = 1;";
            MainClass.CBFill(qry2, cbCustomer);



            if (pesananId != "")
            {
                cbDriver.SelectedValue = driverId;
               
            }
            
        }

        private void cbCustomer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            customerId = cbCustomer.SelectedValue.ToString();
            customerName = cbCustomer.Text;
            
            string qry = $"SELECT * FROM pelanggan WHERE pelanggan_id = '{customerId}';";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                txtAddress.Text = item["alamat"].ToString();
                txtPhone.Text = item["telepon"].ToString();
            }
            
            

        }

        private void cbDriver_SelectionChangeCommitted(object sender, EventArgs e)
        {
            driverId = cbDriver.SelectedValue.ToString();
            driverName = cbDriver.Text;
        }
    }
}
