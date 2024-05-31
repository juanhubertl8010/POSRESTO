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
    public partial class frmStaff : Form
    {
        public frmStaff()
        {
            InitializeComponent();
        }

        public void GetData(string qry)
        {

            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvAddress);
            lb.Items.Add(dgvPhone);
            lb.Items.Add(dgvRole);
            string semua = $"SELECT * FROM db_resto.Karyawan;";
            MainClass.LoadData(qry, dgvStaff, lb, semua);
           
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }
        public virtual void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmStaffAdd());
            //frmAddCustomer frm = new frmAddCustomer();
            //frm.ShowDialog();
            
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
           
        }
        public void RefreshDataGridView()
        {
            string pilihSemua = $"SELECT karyawan_id, nama, alamat, telepon, jabatan FROM Karyawan WHERE `status` = 1 ;";
            GetData(pilihSemua);
        }


        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmStaffAdd());
            RefreshDataGridView();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string pilihNama = $"SELECT karyawan_id, nama, alamat, telepon, jabatan FROM Karyawan WHERE `status` = 1 AND nama LIKE '%{txtSearch.Text}%';";
            GetData(pilihNama);
        }

        private void dgvStaff_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStaff.CurrentCell.OwningColumn.Name == "dgvEdit")
            {
                frmStaffAdd frm = new frmStaffAdd();
                frm.noid = 1;
                frm.id = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvId"].Value);
                frm.txtName.Text = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvName"].Value);
                frm.txtAddress.Text = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvAddress"].Value);
                frm.txtPhone.Text = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvPhone"].Value);
                frm.cbRole.Text = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvRole"].Value);
                frm.label1.Text = "Update Staff";
                frm.ShowDialog();
                RefreshDataGridView();
            }

            if (dgvStaff.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Yakin Ingin Dihapus?") == DialogResult.Yes)
                {
                    string id = Convert.ToString(dgvStaff.CurrentRow.Cells["dgvId"].Value);
                    string qry = $"UPDATE Karyawan SET `status` = 0 WHERE karyawan_id = '{id}'";

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
