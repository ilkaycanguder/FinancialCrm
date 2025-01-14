using FinancialCrm.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmBankProcesses : Form
    {
        public FrmBankProcesses()
        {
            InitializeComponent();
        }
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();
        private void btnCategories_Click(object sender, EventArgs e)
        {
            FrmCategories frmCategories = new FrmCategories();
            frmCategories.Show();
            this.Hide();
        }

        private void btnBanks_Click(object sender, EventArgs e)
        {
            FrmBanks frmBanks = new FrmBanks();
            frmBanks.Show();
            this.Hide();
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            FrmBilling frmBilling = new FrmBilling();
            frmBilling.Show();
            this.Hide();
        }

        private void btnSpendings_Click(object sender, EventArgs e)
        {
            FrmSpendings frmSpendings = new FrmSpendings();
            frmSpendings.Show();
            this.Hide();
        }

        private void btnBankProcesses_Click(object sender, EventArgs e)
        {
            FrmBankProcesses frmBankProcesses = new FrmBankProcesses();
            frmBankProcesses.Show();
            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            FrmDashboard frmDashboard = new FrmDashboard();
            frmDashboard.Show();
            this.Hide();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.Show();
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

        private void FrmBankProcesses_Load(object sender, EventArgs e)
        {
            var values = (from bankProcess in db.BankProcesses
                          join bank in db.Banks on bankProcess.BankId equals bank.BankId
                          select new
                          {
                              bankProcess.BankProcessId,
                              bankProcess.Description,
                              bankProcess.ProcessDate,
                              bankProcess.ProcessType,
                              bankProcess.Amount,
                              BankTitle = bank.BankTitle
                          }).ToList();

            dataGridView1.DataSource = values;
        }

        private void btnRemoveBankProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // DataGridView'den seçilen satırı kontrol et
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Seçilen satırdaki BankProcessId'yi al
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int bankProcessId = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["BankProcessId"].Value);

                    // Banka işlemi kaydını veritabanında bul
                    var bankProcess = db.BankProcesses.Find(bankProcessId);

                    // Eğer banka işlemi bulunduysa
                    if (bankProcess != null)
                    {
                        // Banka işlemini sil
                        db.BankProcesses.Remove(bankProcess);
                        db.SaveChanges();

                        MessageBox.Show("Banka işlemi başarıyla silindi", "Banka İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Silme işleminden sonra DataGridView'i güncelle
                        var values = db.BankProcesses
                                       .Join(db.Banks,
                                             bp => bp.BankId,
                                             b => b.BankId,
                                             (bp, b) => new
                                             {
                                                 bp.BankProcessId,
                                                 bp.Description,
                                                 bp.ProcessDate,
                                                 bp.ProcessType,
                                                 bp.Amount,
                                                 BankTitle = b.BankTitle 
                                             })
                                       .ToList();

                        // DataGridView'in veri kaynağını güncelle
                        dataGridView1.DataSource = values;
                    }
                    else
                    {
                        MessageBox.Show("Banka işlemi bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Eğer satır seçilmediyse kullanıcıya mesaj göster
                    MessageBox.Show("Silmek istediğiniz satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
