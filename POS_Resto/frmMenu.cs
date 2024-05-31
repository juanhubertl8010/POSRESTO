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
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        public void GetData(string qry)
        {

            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvCatID);
            lb.Items.Add(dgvCategory);
            string semua = $"SELECT * FROM db_resto.Menu;";
            MainClass.LoadData(qry, dgvMenu, lb, semua);
        }

        public void RefreshDataGridView()
        {
            string pilihSemua = $"SELECT m.menu_id, m.nama_menu, m.harga, m.kategori_id, k.nama_kategori FROM menu m, kategori k WHERE m.kategori_id = k.kategori_id AND m.status = 1 ORDER BY 1;";
            GetData(pilihSemua);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmMenuAdd());
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            string pilihNama = $"SELECT m.menu_id, m.nama_menu, m.harga, m.kategori_id, k.nama_kategori FROM menu m, kategori k WHERE m.kategori_id = k.kategori_id AND m.status = 1 AND m.nama_menu LIKE '%{txtSearch.Text}%';";
            GetData(pilihNama);
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMenu.CurrentCell.OwningColumn.Name == "dgvEdit")
            {
                frmMenuAdd frm = new frmMenuAdd();
                frm.noid = 1;
                frm.id = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvId"].Value);
                frm.cid = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvCatID"].Value);
                frm.txtName.Text = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvName"].Value);
                frm.txtPrice.Text = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvPrice"].Value);
                frm.cbCategory.Text = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvCategory"].Value);
                //frm.cbRole.Text = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvRole"].Value);
                frm.label1.Text = "Update Menu";
                frm.ShowDialog();
                RefreshDataGridView();
            }

            if (dgvMenu.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Yakin Ingin Dihapus?") == DialogResult.Yes)
                {
                    string id = Convert.ToString(dgvMenu.CurrentRow.Cells["dgvId"].Value);
                    string qry = $"UPDATE Menu SET `status` = 0 WHERE menu_id = '{id}'";

                    Hashtable ht = new Hashtable();
                    MainClass.SQL(qry, ht);

                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Delete Successfully");
                    RefreshDataGridView();
                }

            }
        }
    }
}
