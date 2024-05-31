using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace POS_Resto
{
    internal class MainClass
    {
        public static readonly string con_string = "server=localhost;uid=root;pwd=root123456@;database=db_resto ; ";
        public static MySqlConnection con = new MySqlConnection(con_string);
     
        //method to check user validation

        public static bool IsValidUUser(string user, string pass)
        {
            bool isValid = false;
            string query = $"select * from users where username = '{user}' and upass = '{pass}'";
            MySqlCommand sqlCommand = new MySqlCommand(query,con);
            DataTable dt = new DataTable();
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                isValid = true;
                USER = dt.Rows[0]["uName"].ToString();
            }

            return isValid;
        }

        // create property for username
        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

      
        // method for crud operation
        public static int SQL(string qry, Hashtable ht)
        {
            int res = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry,con);
                cmd.CommandType = CommandType.Text;
                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
            return res;
        }

        // For Loading data from database
        public static int jumlahRow;
        public static void LoadData(string qry, DataGridView gv, ListBox lb, string semua)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colnam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colnam1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
               

                //string query = $"SELECT * FROM db_resto.pelanggan;";
                string query = semua;
                MySqlCommand command = new MySqlCommand(semua, con);
                MySqlDataAdapter adapterr = new MySqlDataAdapter(command);
                DataTable dt2 = new DataTable();
                adapterr.Fill(dt2);
                jumlahRow = dt2.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        public static void BlurBackground(Form model)
        {
            Form Background = new Form();
            using (model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = new Size(1518,903);
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                model.Owner = Background;
                model.ShowDialog(Background);
                Background.Dispose();
            }
        }

       
        // for cb fill
        public static void CBFill(string qry, ComboBox cb)
        {
            MySqlCommand cmd = new MySqlCommand(qry, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }

       
    }
}
