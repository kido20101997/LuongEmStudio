using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuongEmStudio
{
    public partial class ShowDetailScheduleMake : Form
    {
        private Controller controller;
        private DateTime DateMake;
        public ShowDetailScheduleMake(DateTime dateMake)
        {
            InitializeComponent();
            this.DateMake = dateMake;
            controller = new Controller();
        }

        private void ShowDetailScheduleMake_Load(object sender, EventArgs e)
        {
            controller.GetScheduleMakeByDatetime(DateMake, dtgvLichMake);
        }

        private void btcancel_Click(object sender, EventArgs e)
        {
            if (dtgvLichMake.SelectedRows.Count == 1)
            {
                DialogResult kq = MessageBox.Show("Bạn có muốn hủy lịch make-up này không?", "Xác nhận hủy lịch make-up", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                ExecutionResult ex = new ExecutionResult();
                if (kq == DialogResult.Yes)
                {

                    foreach (DataGridViewRow row in dtgvLichMake.SelectedRows)
                    {
                        string bookingid = row.Cells[8].Value?.ToString();
                        ex = controller.CancelMakeUp(bookingid);
                        if (ex.Status)
                        {
                            controller.GetScheduleMakeByDatetime(DateMake, dtgvLichMake);
                            MessageBox.Show("Hủy lịch OK", "Hủy lịch OK", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        else
                            MessageBox.Show("Hủy lịch lỗi", "Hủy lịch lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
                MessageBox.Show("Bạn chưa chọn lịch nào để hủy", "Lỗi chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btupdate_Click(object sender, EventArgs e)
        {
            BaseDataMakeInfo.IDlichMake = "";
            if (dtgvLichMake.SelectedRows.Count == 1)
            {
                DialogResult kq = MessageBox.Show("Bạn có muốn sửa lịch make-up này không?", "Xác nhận sửa lịch make-up", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                ExecutionResult ex = new ExecutionResult();
                if (kq == DialogResult.Yes)
                {
                    if (dtgvLichMake.SelectedRows.Count == 1)
                    {
                        foreach (DataGridViewRow row in dtgvLichMake.SelectedRows)
                        {
                            BaseDataMakeInfo.IDlichMake = row.Cells[8].Value?.ToString();
                            this.DialogResult = DialogResult.OK;
                            break;
                        }
                        this.Close();
                    }
                }
            }
            else
                MessageBox.Show("Bạn chưa chọn lịch nào để sửa", "Lỗi chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowDetailScheduleMake_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
