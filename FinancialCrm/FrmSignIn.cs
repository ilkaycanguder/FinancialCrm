using FinancialCrm.Models;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmSignIn : Form
    {
        public FrmSignIn()
        {
            InitializeComponent();
            SetFormRoundCorners();
        }
        private void SetFormRoundCorners()
        {
            // Formun köşe yuvarlama yarıçapını belirleyin.
            int radius = 20; // Köşe yuvarlama yarıçapı (örneğin 20px)

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.Show();
            this.Hide();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            try
            {
                using (var context = new FinancialCrmDbContext())
                {
                    var newUser = new Users
                    {
                        Username = username,
                        Password = password
                    };

                    context.Users.Add(newUser);
                    int rowsAffected = context.SaveChanges(); 

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kullanıcı başarıyla kaydedildi.");

                        this.Close();
                        FrmLogin loginForm = new FrmLogin();
                        loginForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu. Lütfen tekrar deneyin.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Red;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Black;
        }
    }
}
