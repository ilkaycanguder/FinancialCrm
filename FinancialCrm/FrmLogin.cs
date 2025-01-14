using FinancialCrm.Models;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmLogin : Form
    {
        private string connectionString = "Server=MSI\\ILKAYCANGUDER;Database=FinancialCrmDb;integrated Security=true";
        public static string CurrentUsername { get; set; }
        public FrmLogin()
        {
            InitializeComponent();
            SetFormRoundCorners();
        }
        private void SetFormRoundCorners()
        {
            int radius = 30; // Köşe yuvarlama yarıçapı (örneğin 20px)

            // Formun boyutlarını al
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            // Yuvarlak köşeler için GraphicsPath oluştur
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.Left, rect.Top, radius, radius, 180, 90); // Sol üst
            path.AddArc(rect.Right - radius - 1, rect.Top, radius, radius, 270, 90); // Sağ üst
            path.AddArc(rect.Right - radius - 1, rect.Bottom - radius - 1, radius, radius, 0, 90); // Sağ alt
            path.AddArc(rect.Left, rect.Bottom - radius - 1, radius, radius, 90, 90); // Sol alt

            this.Region = new Region(path);
        }
        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Red;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Black;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            CurrentUsername = txtUsername.Text;
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Kullanıcı doğrulama
            using (var context = new FinancialCrmDbContext())
            {
                try
                {
                    // LINQ ile kullanıcıyı sorgula
                    var user = context.Users
                                      .FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user != null)
                    {
                        MessageBox.Show("Giriş başarılı!", "Hoşgeldiniz", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Dashboard formunu aç
                        FrmDashboard dashboard = new FrmDashboard();
                        dashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz kullanıcı adı veya parola.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            FrmSignIn frmSignIn = new FrmSignIn();
            frmSignIn.Show();
            this.Hide();
        }
    }
}
