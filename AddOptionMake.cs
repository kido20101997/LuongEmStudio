using LuongEmStudio.BaseData;
using Newtonsoft.Json;
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
    public partial class AddOptionMake : Form
    {
        public class MakeUpPackage
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string GiaMake { get; set; }
            public string ChiTietMake { get; set; }
        }
        public AddOptionMake()
        {
            InitializeComponent();
        }

        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }
        private void AddOptionMake_Load(object sender, EventArgs e)
        {

        }

        private void tbGiaMake_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbGiaMake_TextChanged(object sender, EventArgs e)
        {
            string text = tbGiaMake.Text.Replace(" Vnđ", "").Trim();

            if (!decimal.TryParse(text, out _))
            {
                tbGiaMake.Text = "0";
            }
        }
        private void tbGiaMake_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbGiaMake.Text))
            {
                tbGiaMake.Text = "0 Vnđ";
            }
            else
            {
                tbGiaMake.Text = $"{decimal.Parse(tbGiaMake.Text.Replace(" Vnđ", "").Replace(".", "").Trim()):N0} Vnđ";
            }
        }
        private void tbGiaMake_Enter(object sender, EventArgs e)
        {
            if (tbGiaMake.Text.EndsWith(" Vnđ"))
            {
                tbGiaMake.Text = tbGiaMake.Text.Replace(" Vnđ", "").Trim();
            }
        }

        private void btCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(BaseDataSPInfo.MakeUpOption, "MakeUpOption.json");

            // Danh sách để lưu nhiều gói
            List<MakeUpPackage> packages = new List<MakeUpPackage>();

            // Nếu file đã tồn tại, đọc dữ liệu cũ vào danh sách
            if (File.Exists(filePath))
            {
                string oldJson = File.ReadAllText(filePath);
                packages = JsonConvert.DeserializeObject<List<MakeUpPackage>>(oldJson) ?? new List<MakeUpPackage>();
            }

            // Tạo gói mới và thêm vào danh sách
            MakeUpPackage newPackage = new MakeUpPackage
            {
                Id = Guid.NewGuid().ToString(),  
                Name = tbnameOption.Text.Trim(),
                GiaMake = tbGiaMake.Text.Replace(" Vnđ", "").Trim(), // Lưu giá mà không có "VNĐ"
                ChiTietMake = tbChiTietMake.Text
            };
            packages.Add(newPackage);

            string jsonData = JsonConvert.SerializeObject(packages, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);

            tbGiaMake.Text = "";
            tbChiTietMake.Text = "";
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
