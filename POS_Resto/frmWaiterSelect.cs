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
    public partial class frmWaiterSelect : Form
    {
        public frmWaiterSelect()
        {
            InitializeComponent();
        }

        public string waiterName;
        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            string qry = "SELECT * FROM karyawan WHERE jabatan Like 'Waiter';";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
                btn.Text = row["nama"].ToString();
                btn.Width = 150;
                btn.Height = 50;
                btn.FillColor = Color.FromArgb(255, 181, 36);
                btn.BorderRadius = 8;
                btn.HoverState.FillColor = Color.FromArgb(50, 55, 89);
                btn.HoverState.ForeColor = Color.White;

                //Event For Click
                btn.Click += new EventHandler(_Click);
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void _Click(object sender, EventArgs e)
        {
            waiterName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
