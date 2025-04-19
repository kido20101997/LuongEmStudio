using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WMPLib;

namespace LuongEmStudio
{
    public partial class MainMenu : Form
    {
        Controller controller;
        int speak = 0;
        public MainMenu()
        {
            InitializeComponent();
            controller = new Controller();
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
        private void MainMenu_Load(object sender, EventArgs e)
        {
            if (!File.Exists(BaseDataSPInfo.FileConfig))
                controller.CreateListSP();
            this.WindowState = FormWindowState.Maximized;
            timer1_Tick(sender, e);
            SaveCurrentVersion();
            loadLogo();
        }
        public void loadLogo()
        {
            string logoPathFile = Path.Combine(Application.StartupPath, "Logo", "logo.txt");
            string background = Path.Combine(Application.StartupPath, "Logo", "backgroud.txt");

            if (File.Exists(logoPathFile))
            {
                string savedPath = File.ReadAllText(logoPathFile);
                if (File.Exists(savedPath))
                {
                    using (var img = Image.FromFile(savedPath))
                    {
                        picLogo.Image = new Bitmap(img);
                    }
                }
            }
            if (File.Exists(background))
            {
                string savedPath = File.ReadAllText(background);
                if (File.Exists(savedPath))
                {
                    using (var img = Image.FromFile(savedPath))
                    {
                        picBackground.Image = new Bitmap(img);
                    }
                }
            }
        }
        private async Task<string> GetLatestVersionAsync()
        {
            string versionUrl = "https://github.com/kido20101997/LuongEmStudio/releases/download/v1.0.0/Ver.xml";
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(versionUrl);
            }
        }
        public void SaveCurrentVersion()
        {
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                string exePath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(exePath, "current_version.txt");

                if (File.Exists(filePath))
                {
                    string ver = File.ReadAllText(filePath).Trim();
                    if (ver != currentVersion)
                        File.WriteAllText(filePath, currentVersion);
                }
                else
                {
                    File.WriteAllText(filePath, currentVersion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lưu được ver của chương trình, liên hệ Kido", "Lỗi tạo ver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #region add chidren form
        private void ShowMdiForm(Form form)
        {
            foreach (Form control in this.MdiChildren)
            {
                control.Dispose();
            }

            if (!CheckMdiForm(form.Name))
            {
                form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.Show();
            }
            else
            {
                form.Activate();
            }
        }
        private bool CheckMdiForm(string FormName)
        {
            bool hasForm = false;
            foreach (Form f in this.MdiChildren)
            {
                if (f.Name == FormName)
                {
                    hasForm = true;
                }
            }
            return hasForm;
        }
        #endregion
        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = true;
        }

        private void MakeupSchedule_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = false;
            MakeupSchedule makeupSchedule = new MakeupSchedule();
            ShowMdiForm(makeupSchedule);
        }

        private void RentClothings_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = false;
            RentThings borrowReturn = new RentThings();
            ShowMdiForm(borrowReturn);
        }

        private void manageClothings_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = false;
            ManageThings managethings = new ManageThings();
            ShowMdiForm(managethings);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = false;
            ManageStaff manageStaff = new ManageStaff();
            ShowMdiForm(manageStaff);
        }

        private void report_Click(object sender, EventArgs e)
        {
            panelBackground.Visible = false;
            Summary summary = new Summary();
            ShowMdiForm(summary);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chưa làm ^_^", "hehe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private HashSet<string> remindedBookings = new HashSet<string>();

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (speak == 0)
            {
                await SpeakTextAsync("Amy Studio chúc cả nhà ngày mới tốt lành", "3302da9703af4c77a55cc9659b27aaef", 1);
                speak = 1;
            }
            DateTime now = DateTime.Now;
            DateTime remindBefore = now.AddMinutes(59);

            DataTable bookings = controller.GetUpcomingMakeupBookings(now, remindBefore);

            foreach (DataRow row in bookings.Rows)
            {
                string customer = row["customername"].ToString();
                DateTime scheduledDate = Convert.ToDateTime(row["scheduleddate"]);
                string bookingId = row["bookingid"].ToString();
                string location = row["locationmake"].ToString();

                if (!remindedBookings.Contains(bookingId))
                {
                    remindedBookings.Add(bookingId);
                    await SpeakTextAsync($"Cả nhà lưu ý, 1 tiếng nữa sẽ có lịch mếch ắp cho bạn {customer} vào lúc {scheduledDate:HH:mm} tại {location}", "3302da9703af4c77a55cc9659b27aaef", 2);
                    await showLichMake(customer, scheduledDate);
                }
            }
            CheckForUpdatesAsync();
        }
        public async Task showLichMake(string customer, DateTime scheduledDate)
        {
            MessageBox.Show($"⏰ Nhắc bạn: khách '{customer}' sẽ makeup lúc {scheduledDate:HH:mm dd/MM/yyyy}", "Nhắc lịch makeup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public async Task CheckForUpdatesAsync()
        {
            string latestVersion = "";
            string xmlContent = await GetLatestVersionAsync();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            XmlNode versionNode = xmlDoc.SelectSingleNode("/item/version");
            if (versionNode != null)
            {
                latestVersion = versionNode.InnerText;
            }
            else
            {
                throw new Exception("Version node not found in the XML.");
            }

            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (currentVersion != latestVersion)
            {
                Process.Start("LoadApplication.exe");
                Application.Exit();
            }
        }

        private void picBackground_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string logoFolder = Path.Combine(Application.StartupPath, "Logo");
                Directory.CreateDirectory(logoFolder);
                string appLogoPath = Path.Combine(logoFolder, "backgroud.png");
                File.Copy(ofd.FileName, appLogoPath, true);
                using (var img = Image.FromFile(appLogoPath))
                {
                    picBackground.Image = new Bitmap(img); ;
                }
                File.WriteAllText(logoFolder + @"\backgroud.txt", appLogoPath);
            }
        }

        private void picLogo_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string logoFolder = Path.Combine(Application.StartupPath, "Logo");
                Directory.CreateDirectory(logoFolder);
                string appLogoPath = Path.Combine(logoFolder, "UserLogo.png");
                File.Copy(ofd.FileName, appLogoPath, true);

                using (var img = Image.FromFile(appLogoPath))
                {
                    picLogo.Image = new Bitmap(img); ;
                }
                File.WriteAllText(logoFolder + @"\logo.txt", appLogoPath);
            }
        }

        private void closeApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public async Task SpeakTextAsync(string text, string apiKey, int repeatCount)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url1 = $"https://api.voicerss.org/?key={apiKey}&hl=vi-vn&src={Uri.EscapeDataString(text)}&c=MP3&r=0";
                    string url = $"https://api.voicerss.org/?key={apiKey}&hl=vi-vn&src={Uri.EscapeDataString(text)}&r=-2&c=mp3";

                    byte[] audioBytes = await client.GetByteArrayAsync(url);

                    string tempPath = Path.Combine(Path.GetTempPath(), $"voicerss_{Guid.NewGuid()}.mp3");
                    await File.WriteAllBytesAsync(tempPath, audioBytes);

                    WindowsMediaPlayer player1 = new WindowsMediaPlayer();

                    player1.URL = tempPath;
                    player1.PlayStateChange += (int newState) =>
                    {
                        if (newState == (int)WMPPlayState.wmppsStopped)
                        {
                            repeatCount--;
                            if (repeatCount > 0)
                            {
                                player1.controls.play();
                            }
                            else
                            {
                                if (File.Exists(tempPath))
                                    File.Delete(tempPath);
                            }
                        }
                    };
                    player1.controls.play();

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
