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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace POS_Resto
{
    public partial class frmKitchen : Form
    {
        public frmKitchen()
        {
            InitializeComponent();
        }

        private void frmKitchen_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            PanelKitchen.Controls.Clear();
            string qry1 = "SELECT * FROM pesanan WHERE kondisi = 'Pending';";
            MySqlCommand cmd1 = new MySqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd1);
            adapter.Fill(dt1);

            FlowLayoutPanel p1;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 350;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10,10,10,10);

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(50, 55, 89);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 125;
                p2.FlowDirection = FlowDirection.TopDown;
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lbl1 = new Label();
                lbl1.ForeColor = Color.White;
                lbl1.Margin = new Padding(10, 10, 3, 0);
                lbl1.AutoSize = true;

                Label lbl2 = new Label();
                lbl2.ForeColor = Color.White;
                lbl2.Margin = new Padding(10, 5, 3, 0);
                lbl2.AutoSize = true;

                Label lbl3 = new Label();
                lbl3.ForeColor = Color.White;
                lbl3.Margin = new Padding(10, 5, 3, 0);
                lbl3.AutoSize = true;

                Label lbl4 = new Label();
                lbl4.ForeColor = Color.White;
                lbl4.Margin = new Padding(10, 5, 3, 10);
                lbl4.AutoSize = true;

               
                lbl1.Text = "Table : " + dt1.Rows[i]["nomor_meja"].ToString();
                lbl2.Text = "Waiter Name : " + dt1.Rows[i]["nama_waiter"].ToString();
                lbl3.Text = "Order Time : " + dt1.Rows[i]["waktu_pesanan"].ToString();
                lbl4.Text = "Order Type : " + dt1.Rows[i]["tipe_pesanan"].ToString();

                p2.Controls.Add(lbl1);
                p2.Controls.Add(lbl2);
                p2.Controls.Add(lbl3);
                p2.Controls.Add(lbl4);

                p1.Controls.Add(p2);

                // sekarang tambah produk

                string pesananID;

                pesananID = Convert.ToString(dt1.Rows[i]["pesanan_id"]);

                string qry2 = $"SELECT * FROM pesanan p INNER JOIN detail_pesanan d ON p.pesanan_id = d.pesanan_id INNER JOIN menu m ON d.menu_id = m.menu_id WHERE p.pesanan_id = '{pesananID}'" ;
                MySqlCommand cmd2 = new MySqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(cmd2);
                adapter2.Fill(dt2);

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Label lbl5 = new Label();
                    lbl5.ForeColor = Color.Black;
                    lbl5.Margin = new Padding(10, 5, 3, 0);
                    lbl5.AutoSize = true;

                    int no = j + 1;
                    lbl5.Text = "" + no + " " + dt2.Rows[j]["nama_menu"].ToString() + " " + dt2.Rows[j]["qty"].ToString();
                    p1.Controls.Add(lbl5);

                }

                // tambah button untuk mengganti status pesanan

                Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
                btn.BorderRadius = 8;
                btn.Size = new Size(100, 35);
                btn.FillColor = Color.FromArgb(255, 181, 36);
                btn.Margin = new Padding(30, 5, 3, 10);
                btn.Text = "Complete";
                btn.Tag = dt1.Rows[i]["pesanan_id"].ToString();// simpan id

                p1.Controls.Add(btn);

                btn.Click += new EventHandler(btn_click);
                PanelKitchen.Controls.Add(p1);
            }


        }

        private void btn_click(object sender, EventArgs e)
        {
            string id = Convert.ToString((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());

            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("Yakin Udah Selesai?") == DialogResult.Yes)
            {
                string qry = @"UPDATE pesanan SET kondisi = 'Complete' WHERE pesanan_id = @ID";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", id);

                MainClass.SQL(qry,ht);

                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Saved Successfully");
            }
            GetOrders();        
        }
    }
}
