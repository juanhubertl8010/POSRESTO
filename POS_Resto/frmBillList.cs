using POS_Resto.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace POS_Resto
{
    public partial class frmBillList : Form
    {
        public frmBillList()
        {
            InitializeComponent();
        }

        public string PesananID;
        private void frmBillList_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void LoadData(string qry)
        {
            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvTotal);
            string semua = $"SELECT * FROM db_resto.pesanan;";
            MainClass.LoadData(qry, dgvBill, lb, semua);
        }

        public void RefreshDataGridView()
        {
            string pilihSemua = $"SELECT pesanan_id, nomor_meja, nama_waiter, tipe_pesanan, kondisi, total FROM pesanan WHERE kondisi <> 'Pending' AND status = '1' ORDER BY kondisi;";
            LoadData(pilihSemua);
        }

        private void dgvBill_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBill.CurrentCell.OwningColumn.Name == "dgvEdit")
            {
                if(dgvBill.CurrentRow.Cells["dgvStatus"].Value.ToString() == "Paid")
                {
                    guna2MessageDialog1.Show("Pesanan Sudah dibayar");
                    return;
                }
                PesananID = Convert.ToString(dgvBill.CurrentRow.Cells["dgvId"].Value);
                this.Close();
                RefreshDataGridView();
            }
            if (dgvBill.CurrentCell.OwningColumn.Name == "dgvPrint")
            {
                string PesananID = Convert.ToString(dgvBill.CurrentRow.Cells["dgvId"].Value);
                string qry = $"SELECT * FROM pesanan p INNER JOIN detail_pesanan d ON d.pesanan_id = p.pesanan_id INNER JOIN menu m ON m.menu_id = d.menu_id WHERE p.pesanan_id = '{PesananID}';";
                MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
                MainClass.con.Open();
              //  MyDataSet dataSet = new MyDataSet();
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                MainClass.con.Close();
                frmPrint frm = new frmPrint();
                reportBill cr = new reportBill();
                
              
                cr.SetDatabaseLogon("sa", "123");
                cr.SetDataSource(dt);
                //  cr.DataSourceConnections[0].IntegratedSecurity = true;

                frm.crystalReportViewer1.ReportSource = cr;
                //frm.crystalReportViewer1.Show();
                frm.crystalReportViewer1.Refresh();
               
                frm.Show();

            }

        }
    }
}
