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
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
        }

        public void GetData(string qry)
        {
            
            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvEmail);
            lb.Items.Add(dgvPhone);
            lb.Items.Add(dgvAddress);
            string semua = $"SELECT * FROM db_resto.pelanggan;";
            MainClass.LoadData(qry, dgvCustomer, lb, semua);
        }

        public virtual void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmAddCustomer());
            //frmAddCustomer frm = new frmAddCustomer();
            //frm.ShowDialog();
            RefreshDataGridView();
        }


        private void frmCustomer_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            string pilihNama = $"SELECT pelanggan_id, nama, email, telepon, alamat FROM pelanggan WHERE `status` = 1 AND nama LIKE '%{txtSearch.Text}%';";
            GetData(pilihNama);
        }
        public void RefreshDataGridView()
        {
            string pilihSemua = $"SELECT pelanggan_id, nama, email, telepon, alamat FROM pelanggan WHERE `status` = 1 ;";
            GetData(pilihSemua);
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.CurrentCell.OwningColumn.Name == "dgvEdit")
            {
                frmAddCustomer frm = new frmAddCustomer();
                frm.noid = 1;
                frm.id = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvId"].Value);
                frm.txtName.Text = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvName"].Value);
                frm.txtEmail.Text = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvEmail"].Value);
                frm.txtPhone.Text = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvPhone"].Value);
                frm.txtAddress.Text = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvAddress"].Value);
                frm.label1.Text = "Update Customer";
                frm.ShowDialog();
                RefreshDataGridView();
            }

            if(dgvCustomer.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Yakin Ingin Dihapus?") == DialogResult.Yes)
                {
                    string id = Convert.ToString(dgvCustomer.CurrentRow.Cells["dgvId"].Value);
                    string qry = $"UPDATE pelanggan SET `status` = 0 WHERE pelanggan_id = '{id}'";

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
