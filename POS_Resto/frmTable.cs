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
    public partial class frmTable : Form
    {
        public frmTable()
        {
            InitializeComponent();
        }

        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTable.CurrentCell.OwningColumn.Name == "dgvEdit")
            {
                frmAddTable frm = new frmAddTable(); // 
                frm.noid = 1;
                frm.id = Convert.ToString(dgvTable.CurrentRow.Cells["dgvId"].Value);
                frm.txtNameTable.Text = Convert.ToString(dgvTable.CurrentRow.Cells["dgvName"].Value);
                frm.txtKapasitas.Text = Convert.ToString(dgvTable.CurrentRow.Cells["dgvKapasitas"].Value);
                frm.label1.Text = "Update Table";
                //frm.ShowDialog();
                MainClass.BlurBackground(frm);

            }

            if (dgvTable.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Yakin Ingin Dihapus?") == DialogResult.Yes)
                {
                    string id = Convert.ToString(dgvTable.CurrentRow.Cells["dgvId"].Value);
                    string qry = $"UPDATE meja SET `status` = 0 WHERE meja_id = '{id}'";

                    Hashtable ht = new Hashtable();
                    MainClass.SQL(qry, ht);
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Delete Successfully");
                   
                }

            }
        }

        private void frmTable_Load(object sender, EventArgs e)
        {
            string pilihSemua = $"SELECT meja_id, nomor_meja, kapasitas FROM meja WHERE `status` = 1 ;";
            GetData(pilihSemua);
        }

        public void GetData(string qry)
        {

            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvKapasitas);
            string semua = $"SELECT * FROM db_resto.meja;";
            MainClass.LoadData(qry, dgvTable, lb, semua);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string pilihNama = $"SELECT meja_id, nomor_meja, kapasitas FROM meja WHERE `status` = 1 AND nomor_meja LIKE '%{txtSearch.Text}%';";
            GetData(pilihNama);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmAddTable());
            string pilihSemua = $"SELECT meja_id, nomor_meja, kapasitas FROM meja WHERE `status` = 1 ;";
            GetData(pilihSemua);
        }
    }
}
