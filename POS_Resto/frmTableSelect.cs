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
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }

        public string tableName;
        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            string qry = "SELECT * FROM meja WHERE status = '1';";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
                btn.Text = row["nomor_meja"].ToString();
                btn.Width = 150;
                btn.Height = 50;
                btn.FillColor = Color.FromArgb(50, 55, 89);
                btn.BorderRadius = 5;
                if (row["kapasitas"].ToString() == "2 orang")
                {
                    btn.HoverState.FillColor = Color.FromArgb(255, 181, 36);
                    btn.HoverState.ForeColor = Color.White;
                }
                if (row["kapasitas"].ToString() == "4 orang")
                {
                    btn.HoverState.FillColor = Color.FromArgb(241, 85, 126);
                    btn.HoverState.ForeColor = Color.White;
                }
                if (row["kapasitas"].ToString() == "6 orang")
                {
                    btn.HoverState.FillColor = Color.FromArgb(192, 0, 192);
                    btn.HoverState.ForeColor = Color.White;
                }






                //Event For Click
                btn.Click += new EventHandler(_Click);
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void _Click(object sender, EventArgs e)
        {
            tableName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            string qry = @"UPDATE meja SET status = '0' WHERE nomor_meja = @nomor_meja;";
            Hashtable ht = new Hashtable();
            ht.Add("@nomor_meja", tableName);
            MainClass.SQL(qry, ht);
            this.Close();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
