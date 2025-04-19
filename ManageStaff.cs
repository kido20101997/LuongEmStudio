using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuongEmStudio
{
    public partial class ManageStaff : Form
    {
        Controller controller;
        private string id = "";
        public ManageStaff()
        {
            InitializeComponent();
            controller = new Controller();
        }

        public void undo()
        {
            tbName.Text = "";
            tbsdt.Text = "";
            tbaddress.Text = "";
            tbnotes.Text = "";
            tbName.Focus(); tbName.SelectAll();
        }
        private void btadd_Click(object sender, EventArgs e)
        {
            ExecutionResult result = new ExecutionResult();
            string name = tbName.Text.Trim();
            string sdt = tbsdt.Text.Trim();
            string address = tbaddress.Text.Trim();
            string notes = tbnotes.Text.Trim();

            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbsdt.Text))
            {
                controller.ErrorMSG(lbmes, "Vui lòng điền đầy đủ tên và sdt nhân viên");
                return;
            }
            result = controller.AddStaff(name, sdt, address, notes);
            if (result.Status)
            {
                controller.SuccessMSG(lbmes, "Thêm nhân viên thành công");
                undo();
            }
            else
                controller.ErrorMSG(lbmes, "Lỗi Thêm nhân viên");
        }

        private void dtgvLichmake_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.id = "";
                DataGridViewRow row = dtgvStaffInfo.Rows[e.RowIndex];
                this.id += row.Cells[0].Value?.ToString();
                tbName.Text = row.Cells[1].Value?.ToString();
                tbsdt.Text = row.Cells[2].Value?.ToString();
                tbaddress.Text = row.Cells[6].Value?.ToString();
                tbnotes.Text = row.Cells[5].Value?.ToString();
            }
        }

        private void btupdate_Click(object sender, EventArgs e)
        {
            if (dtgvStaffInfo.SelectedRows.Count == 1)
            {
                ExecutionResult result = new ExecutionResult();
                string name = tbName.Text.Trim();
                string sdt = tbsdt.Text.Trim();
                string address = tbaddress.Text.Trim();
                string notes = tbnotes.Text.Trim();

                result = controller.UpdateStaff(this.id, name, sdt, address, notes);
                if (result.Status)
                {
                    undo();
                    controller.SuccessMSG(lbmes, "Sửa thông tin nv thành công !");
                    dtgvStaffInfo.DataSource = null;

                }
                else
                {
                    controller.ErrorMSG(lbmes, "Sửa thông tin nv lỗi !");
                }
            }
            else
                controller.ErrorMSG(lbmes, "Cần chọn nv cần sửa đổi trước!");
        }

        private void btfind_Click(object sender, EventArgs e)
        {
            ExecutionResult result = new ExecutionResult();
            string name = tbName.Text.Trim();
            string sdt = tbsdt.Text.Trim();
            dtgvStaffInfo.DataSource = null;

            if (string.IsNullOrEmpty(tbName.Text) && string.IsNullOrEmpty(tbsdt.Text))
            {
                controller.ErrorMSG(lbmes, "Vui lòng điền tên hoặc sdt nhân viên");
                return;
            }
            result = controller.SearchStaff(name, sdt);
            if (result.Status)
            {

                DataSet ds = (DataSet)result.Anything;
                dtgvStaffInfo.DataSource = ds.Tables[0];
                controller.SuccessMSG(lbmes, "Thêm nhân viên thành công");
                undo();
            }
            else
                controller.ErrorMSG(lbmes, "Lỗi Thêm nhân viên");
        }

        private void tbsdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbsdt.Focus();
            }
        }

        private void ManageStaff_Load(object sender, EventArgs e)
        {
            tbName.Focus();
        }

        private void tbsdt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbaddress.Focus();
            }
        }

        private void tbaddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbnotes.Focus();
            }
        }

        private void btdelete_Click(object sender, EventArgs e)
        {
            if (dtgvStaffInfo.SelectedRows.Count == 1)
            {
                ExecutionResult result = new ExecutionResult();

                result = controller.DeleteStaff(this.id);
                if (result.Status)
                {
                    undo();
                    controller.SuccessMSG(lbmes, "Xóa nv thành công !");
                    dtgvStaffInfo.DataSource = null;
                }
                else
                {
                    controller.ErrorMSG(lbmes, "Xóa nv lỗi !");
                }
            }
            else
                controller.ErrorMSG(lbmes, "Cần chọn nv cần Xóa trước!");
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;

            // Format chuỗi: viết hoa chữ cái đầu mỗi từ
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }

        private void tbaddress_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;

            // Format chuỗi: viết hoa chữ cái đầu mỗi từ
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }
    }
}
