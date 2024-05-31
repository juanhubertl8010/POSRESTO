using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace POS_Resto
{
    public partial class frmMenuAdd : Form
    {
        public frmMenuAdd()
        {
            InitializeComponent();
        }
        public int noid = 0;

        public string cid = "a";
        public string id;
       
        string filePath;
        byte[] imageBytes;
        byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);

                // Mengonversi gambar menjadi byte array
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length));
                }
            }
        }

        private void frmMenuAdd_Load(object sender, EventArgs e)
        {
            // for cb fill
            string qry = "SELECT kategori_id AS 'id', nama_kategori AS 'name' FROM kategori";
            MainClass.CBFill(qry, cbCategory);
            UpdateLoadData();
            if(cid == "")
            {
                return;
            }
            else
            {
                cbCategory.SelectedValue = cid;
            }
           
           

        }

        private void btnSave_Click_2(object sender, EventArgs e)
        {
            if (txtName.Text == "" || cbCategory.Text == "" || txtPrice.Text == "")
            {
                guna2MessageDialog1.Show("Gaboleh Kosong Yaa");
                return;
            }


            string qry = "";
            string name = txtName.Text;
            string category = cbCategory.SelectedValue.ToString();
            double price = Convert.ToDouble(txtPrice.Text);
            byte[] image = imageBytes;
           
            int urutan = MainClass.jumlahRow + 1;
            //id = "";


            int status = 1;



            if (noid == 0) // untuk insert nilai baru
            {
                if(urutan >= 100)
                {
                    id = "MN" + urutan.ToString();
                }
                if (urutan >= 10)
                {
                    id = "MN0" + urutan.ToString();
                }
                qry = "INSERT INTO Menu (menu_id, nama_menu, harga, kategori_id, mgambar, status) VALUES (@menu_id, @nama_menu, @harga, @kategori_id, @mgambar, @status);";
                noid = 0;
            }
            else // untuk update Menu
            {
                qry = "UPDATE Menu SET nama_menu = @nama_menu, harga = @harga, kategori_id = @kategori_id, mgambar = @mgambar WHERE menu_id = @menu_id;";
            }


            Hashtable ht = new Hashtable();
            ht.Add("@menu_id", id);
            ht.Add("@nama_menu", txtName.Text);
            ht.Add("@harga", Convert.ToDouble(txtPrice.Text));
            ht.Add("@kategori_id", cbCategory.SelectedValue.ToString());
            if(noid == 0)
            {
                ht.Add("@mgambar", imageBytes);
            }
            else
            {
                ht.Add("@mgambar", imageByteArray);
            }
            
            ht.Add("@status", status);
            frmCustomer frmCustomerObj = new frmCustomer();
            MainClass.SQL(qry, ht);
            frmCustomerObj.RefreshDataGridView();
            guna2MessageDialog1.Show("Success");
            noid = 0;
            txtName.Text = "";
            txtPrice.Text = "";
            txtImage.Image = POS_Resto.Properties.Resources.food;
            cbCategory.SelectedIndex = -1;
            txtName.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateLoadData()
        {
            // Pastikan koneksi database telah dibuka
            if (MainClass.con.State != ConnectionState.Open)
            {
                MainClass.con.Open();
            }

            string qry = $"SELECT * FROM menu WHERE menu_id = '{id}';";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtName.Text = reader["nama_menu"].ToString();
                    txtPrice.Text = reader["harga"].ToString();

                    if (reader["mgambar"] != DBNull.Value)
                    {
                        imageByteArray = (byte[])reader["mgambar"];
                        

                        if (imageByteArray.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(imageByteArray))
                            {
                                try
                                {
                                    txtImage.Image = Image.FromStream(ms);
                                }
                                catch (ArgumentException ex)
                                {
                                    // Tampilkan pesan kesalahan jika ada masalah dalam membuat objek Image
                                    MessageBox.Show("Gagal memuat gambar: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            txtImage.Image = null; // Set gambar ke null jika tidak ada gambar tersedia
                        }
                    }
                    else
                    {
                        txtImage.Image = null; // Set gambar ke null jika tidak ada gambar tersedia
                    }
                }
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cek apakah karakter yang ditekan bukan digit
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 adalah kode karakter backspace
            {
                e.Handled = true; // Mencegah karakter yang tidak valid ditampilkan di textbox
            }
        }
    }
}
