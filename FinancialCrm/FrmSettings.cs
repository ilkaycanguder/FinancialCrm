using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                // SaveFileDialog ile dosya kaydetme yeri seç
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "BAK Dosyası|*.bak";
                saveFileDialog.Title = "Veritabanı Yedekleme";
                saveFileDialog.FileName = "FinancialCrmBackup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = saveFileDialog.FileName;

                    // MSSQL'de veritabanını yedekle
                    string connectionString = "Server=MSI\\ILKAYCANGUDER;Database=FinancialCrmDb;integrated Security=true";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string backupQuery = $"BACKUP DATABASE FinancialCrmDb TO DISK = '{backupPath}'";

                        using (SqlCommand command = new SqlCommand(backupQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Veritabanı başarıyla yedeklendi!", "Yedekleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedekleme işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            FrmDashboard dashboard = new FrmDashboard();
            dashboard.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            // Eski şifreyi doğrulamak için veritabanında kontrol yapalım
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Server=MSI\ILKAYCANGUDER;Database=FinancialCrmDb;Integrated Security=True"))
                {
                    connection.Open();

                    // Kullanıcı adı olarak global değişkeni kullanıyoruz
                    string query = "SELECT Password FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", FrmLogin.CurrentUsername);  // FrmLogin'dan alınan kullanıcı adı

                        string storedPassword = cmd.ExecuteScalar()?.ToString(); // Eski şifreyi al

                        if (storedPassword == null)
                        {
                            MessageBox.Show("Kullanıcı bulunamadı.");
                            return;
                        }

                        // Eski şifreyi kontrol et
                        if (storedPassword != oldPassword)
                        {
                            MessageBox.Show("Eski şifre yanlış. Lütfen tekrar deneyin.");
                            return;
                        }

                        // Yeni şifreyi güncelle
                        string updateQuery = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("@NewPassword", newPassword); // Yeni şifre
                            updateCmd.Parameters.AddWithValue("@Username", FrmLogin.CurrentUsername); // Kullanıcı adı

                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Şifreniz başarıyla değiştirildi.");
                            }
                            else
                            {
                                MessageBox.Show("Bir hata oluştu. Lütfen tekrar deneyin.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
