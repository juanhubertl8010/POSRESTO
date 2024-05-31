using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace POS_Resto
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }
        public int MainID = 0;
        public string OrderType;
        public string driverID = "";
        public string cust_name = "";
        public string cust_phone = "";
        public string alamat_cust = "";
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            dgvPOS.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            MenuPanel.Controls.Clear();
            LoadMenu();
        }

        private void AddCategory()
        {
            string qry = "SELECT * FROM Kategori";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            CategoryPanel.Controls.Clear();
            Guna.UI2.WinForms.Guna2Button btn2 = new Guna.UI2.WinForms.Guna2Button();
            btn2.FillColor = Color.FromArgb(255, 181, 36);
            btn2.Size = new Size(136, 45);
            btn2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btn2.Text = "All Categories";
            btn2.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btn2.Click += new EventHandler(_Click);
            CategoryPanel.Controls.Add(btn2);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
                    btn.FillColor = Color.FromArgb(255, 181, 36);
                    btn.Size = new Size(136, 45);
                    btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    btn.Text = row["nama_kategori"].ToString();

                    //Event For Click
                    btn.Click += new EventHandler(_Click);

                    CategoryPanel.Controls.Add(btn);
                }
                
            }
        }

        private void _Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = (Guna.UI2.WinForms.Guna2Button)sender;
            if(btn.Text == "All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return;
            }
            string selectedCategory = btn.Text.Trim().ToLower();

            foreach (var item in MenuPanel.Controls)
            {
                var menu = (ucProduct)item;
                if (menu.Mcategory.ToLower() == selectedCategory)
                {
                    menu.Visible = true; // Menu ditampilkan jika kategori cocok
                }
                else
                {
                    menu.Visible = false; // Menu disembunyikan jika kategori tidak cocok
                }
            }
            //foreach (var item in MenuPanel.Controls)
            //{
            //    var menu = (ucProduct)item;
            //    menu.Visible = menu.Mcategory.ToLower().Contains(btn.Text.Trim().ToLower());
            //}
        }

        private void AddItems(string id, string name, string cat, double price, Image image)
        {
            var menu = new ucProduct()
            {
                Mname = name,
                Mprice = price,
                Mcategory = cat,
                MImage = image,
                id = Convert.ToString(id)
            };

            MenuPanel.Controls.Add(menu);
            menu.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                foreach (DataGridViewRow item in dgvPOS.Rows)
                {
                    //this will check it product already there then a one to quantity and update price 
                    if (Convert.ToString(item.Cells["dgvId"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) * double.Parse(item.Cells["dgvPrice"].Value.ToString().ToString());
                        GetTotal();
                        return;
                    }
                    
                }
                //this line add new product
                dgvPOS.Rows.Add(new object[] {wdg.id, wdg.Mname, 1, wdg.Mprice, wdg.Mprice });
                GetTotal();// penemapatan kurang sempurna
            };
        }

        //Getting menu from database
        private void LoadMenu()
        {
            string qry = "SELECT m.menu_id, m.nama_menu, m.harga, m.mgambar, m.kategori_id, k.nama_kategori FROM menu m, kategori k WHERE m.kategori_id = k.kategori_id AND m.status = 1 ORDER BY 1;";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                Byte[] imageArray = (byte[])item["mgambar"];
                byte[] imagebyteArray = imageArray;

                AddItems(item["menu_id"].ToString(), item["nama_menu"].ToString(), item["nama_kategori"].ToString(), Convert.ToDouble(item["harga"]), Image.FromStream(new MemoryStream(imageArray)));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in MenuPanel.Controls)
            {
                var menu = (ucProduct)item;
                menu.Visible = menu.Mname.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void GetTotal()
        {
            double total = 0;
            lblTotal.Text = "";
            // for berjalan jika qty masih ada(permasalahan terletak pada klik)
            foreach (DataGridViewRow item in dgvPOS.Rows)
            {
                total += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }

            lblTotal.Text = total.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblTable.Visible = false;
            dgvPOS.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "0.00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblCust.Text = "";
            lblDriver.Text = "";
            lblTable.Visible = false;
            lblTable.Visible = false;
            OrderType = "Delivery";
            frmDelivery frm = new frmDelivery();
            frm.pesananId = id;
            frm.orderType = OrderType;

            MainClass.BlurBackground(frm);
            
            lblDriver.Text = "Driver: " + frm.driverName;
            lblCust.Text = "Customer: " + frm.customerName;
            driverID = frm.driverId;
            cust_name = frm.customerName;
            cust_phone = frm.txtPhone.Text;
            alamat_cust = frm.txtAddress.Text;
            lblDriver.Visible = true;
            lblCust.Visible = true;
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblTable.Visible = false;
            OrderType = "Take Away";

            //frmDelivery frm = new frmDelivery();
            //frm.pesananId = id;
            //frm.orderType = OrderType;

            //MainClass.BlurBackground(frm);
        }

        private void btnDine_Click(object sender, EventArgs e)
        {
            //butuh form untuk pilih table dan pilih waiter
            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            if (frm.tableName != "")
            {
                lblTable.Text = frm.tableName;
                lblTable.Visible = true;
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            if (frm2.waiterName != "")
            {
                lblWaiter.Text = frm2.waiterName;
                lblWaiter.Visible = true;
            }
            else
            {
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }
            OrderType = "Dine In";
        }

        public string pesananID = "";
        private void btnKot_Click(object sender, EventArgs e)
        {
            if(lblTotal.Text == "0.00" || lblTotal.Text == "") // masi tembus
            {
                guna2MessageDialog1.Show("Pesanan Harus Lengkap");
                return;
            }
            string qry = "SELECT * FROM pesanan;";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            int urutan = 0;
            foreach (DataRow row in dt.Rows)
            {
                urutan++;
            }
            urutan = urutan + 1;
            //simpan data ke database
            

            string qry1 = ""; // table pesanan
            string qry2 = ""; // table detail_pesanan mengambil data dari dgv pos, detail id dan main id adalah angka

            string detailID;
            int status = 1;

            if(MainID == 0) // insert
            {
                if(urutan >= 10)
                {
                    pesananID = "PSN0" + urutan.ToString();
                }
                if(urutan >= 100)
                {
                    pesananID = "PSN" + urutan.ToString();
                }
                if(urutan < 10)
                {
                    pesananID = "PSN00" + urutan.ToString();
                }
               
                qry1 = @"INSERT INTO pesanan VALUES(@ID, @tanggal_pesanan, @waktu_pesanan, @nomor_meja, @nama_waiter, @kondisi, @tipe_pesanan, @total, @received, @kembalian, @status, @driver_id, @cust_name, @cust_phone, @alamat_cust) ;";
                MainID = 1;
            }
            else // update
            {
                qry1 = @"UPDATE pesanan SET kondisi = @kondisi, total = @total, received = @received, kembalian = @kembalian WHERE pesanan_id = @ID;";
            }

            MySqlCommand command = new MySqlCommand(qry1, MainClass.con);

            command.Parameters.AddWithValue("@ID", pesananID);
            command.Parameters.AddWithValue("@tanggal_pesanan",Convert.ToDateTime(DateTime.Now.Date));
            command.Parameters.AddWithValue("@waktu_pesanan", DateTime.Now.ToLongTimeString());
            command.Parameters.AddWithValue("@nomor_meja",lblTable.Text);
            command.Parameters.AddWithValue("@nama_waiter",lblWaiter.Text);
            command.Parameters.AddWithValue("@kondisi", "Pending");
            command.Parameters.AddWithValue("@tipe_pesanan",OrderType);
            command.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // as we only saving data for kitchen value will update when payment received
            command.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            command.Parameters.AddWithValue("@kembalian", Convert.ToDouble(0));
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@driver_id", driverID);
            command.Parameters.AddWithValue("@cust_name", cust_name);
            command.Parameters.AddWithValue("@cust_phone", cust_phone);
            command.Parameters.AddWithValue("@alamat_cust", alamat_cust);

            if (MainClass.con.State == ConnectionState.Closed)
            {
                MainClass.con.Open();
            }
            if (MainID == 0)
            {
               MainID = Convert.ToInt32(command.ExecuteScalar());
            }
            else
            {
                command.ExecuteNonQuery();
            }
            if (MainClass.con.State == ConnectionState.Open)
            {
                MainClass.con.Close();
            }
            int detaill = 0;
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                detaill = detaill + 1;
                
                detailID = "D" + pesananID + detaill.ToString();
             
                //detailID = Convert.ToString(row.Cells["dgvId"].Value);
                //if(detailID == "")
                //{
                    qry2 = @"INSERT INTO detail_pesanan VALUES (@detail_id, @pesanan_id, @menu_id, @qty, @harga, @jumlah);";
                //}
                //else
                //{
                //    qry2 = @"UPDATE detail_pesanan SET menu_id = @menu_id, qty = @qty, harga = @harga, jumlah = @jumlah WHERE detail_id = @detail_id ;";
                //}

                MySqlCommand cmd2 = new MySqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@detail_id", detailID);
                cmd2.Parameters.AddWithValue("@pesanan_id", pesananID);
                cmd2.Parameters.AddWithValue("@menu_id", row.Cells["dgvId"].Value);
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@harga", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@jumlah", Convert.ToDouble(row.Cells["dgvAmount"].Value));
                if (MainClass.con.State == ConnectionState.Closed)
                {
                    MainClass.con.Open();
                }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open)
                {
                    MainClass.con.Close();
                }

               
                MainID = 0;
               
                detailID = "";

                lblCust.Text = "";
                lblDriver.Text = "";
                lblCust.Visible = false;
                lblDriver.Visible = false;
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblTable.Visible = false;
                lblWaiter.Visible = false;
                lblTotal.Text = "0.00";
            }
            guna2MessageDialog1.Show("Saved Successfully");
            dgvPOS.Rows.Clear();
            detaill = 0;
        }

        public string id = "";
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if(frm.PesananID != "")
            {
                id = frm.PesananID;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            if(id == null)
            {
                return;
            }
            string qry = $"SELECT * FROM pesanan p INNER JOIN detail_pesanan d ON p.pesanan_id = d.pesanan_id INNER JOIN menu m ON d.menu_id = m.menu_id WHERE p.pesanan_id = '{id}'";
            MySqlCommand cmd2 = new MySqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            MySqlDataAdapter adapter2 = new MySqlDataAdapter(cmd2);
            adapter2.Fill(dt2);

            dgvPOS.Rows.Clear();

            if(dt2.Rows[0]["tipe_pesanan"].ToString() == "Delivery")
            {
                btnDelivery.Checked = true;
                lblWaiter.Visible = false;
                lblTable.Visible = false;
            }
            else if(dt2.Rows[0]["tipe_pesanan"].ToString() == "Take Away")
            {
                btnTake.Checked = true;
                lblWaiter.Visible = false;
                lblTable.Visible = false;
            }
            else
            {
                btnDine.Checked = true;
                lblWaiter.Visible = true;
                lblTable.Visible = true;
            }

            foreach (DataRow item in dt2.Rows)
            {
                lblTable.Text = item["nomor_meja"].ToString();
                lblWaiter.Text = item["nama_waiter"].ToString();
                string detailid = item["detail_id"].ToString();
                string menuName = item["nama_menu"].ToString();
                string qty = item["qty"].ToString();
                string harga = item["harga"].ToString();
                string amount = item["jumlah"].ToString();
               
                object[] obj = { detailid, menuName, qty, harga, amount};
                dgvPOS.Rows.Add(obj);
            }
            GetTotal();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if(lblTotal.Text == "0.00")
            {
                guna2MessageDialog1.Show("Pesanan masih kosong");
                return;
            }
            frmCheckOut frm = new frmCheckOut();
            frm.pesananaID = id;
            frm.amount = Convert.ToDouble(lblTotal.Text);
            MainClass.BlurBackground(frm);

            pesananID = "";
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "0.00";
            dgvPOS.Rows.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string qry2 = @"UPDATE meja set status = 1 WHERE nomor_meja = @id;";

            Hashtable ht2 = new Hashtable();
            ht2.Add("@id", lblTable.Text);
            MainClass.SQL(qry2, ht2);
            lblTable.Text = "Table";
            lblWaiter.Text = "";
            lblDriver.Text = "";
            lblCust.Text = "";
            lblDriver.Visible = false;
            lblCust.Visible = false;
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "0.00";
            dgvPOS.Rows.Clear ();

        }
    }
}
