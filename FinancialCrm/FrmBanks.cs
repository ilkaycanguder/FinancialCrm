using FinancialCrm.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmBanks : Form
    {
        public FrmBanks()
        {
            InitializeComponent();
        }
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();
        private void FrmBanks_Load(object sender, EventArgs e)
        {
            // Banka Bakiyeleri
            var ziraatBankBalance = db.Banks.Where(x => x.BankTitle == "Ziraat Bankası").Select(y => y.BankBalance).FirstOrDefault();
            var vakifBankBalance = db.Banks.Where(x => x.BankTitle == "Vakıfbank").Select(y => y.BankBalance).FirstOrDefault();
            var isBankBalance = db.Banks.Where(x => x.BankTitle == "İş Bankası").Select(y => y.BankBalance).FirstOrDefault();
            var yapikrediBalance = db.Banks.Where(x => x.BankTitle == "Yapıkredi").Select(y => y.BankBalance).FirstOrDefault();
            var denizbankBalance = db.Banks.Where(x => x.BankTitle == "Denizbank").Select(y => y.BankBalance).FirstOrDefault();
            var akbankBalance = db.Banks.Where(x => x.BankTitle == "Akbank").Select(y => y.BankBalance).FirstOrDefault();

            lblisBankasiBalance.Text = isBankBalance.ToString() + " ₺";
            lblVakifbankBalance.Text = vakifBankBalance.ToString() + " ₺";
            lblZiraatBankBalance.Text = ziraatBankBalance.ToString() + " ₺";
            lblYapikrediBalance.Text = yapikrediBalance.ToString() + " ₺";
            lblDenizbankBalance.Text = denizbankBalance.ToString() + " ₺";
            lblAkbankBalance.Text = akbankBalance.ToString() + " ₺";

            // Banka Hareketleri
            var bankProcess1 = db.BankProcesses.OrderByDescending(x => x.BankProcessId).Take(1).FirstOrDefault();
            lblBankProcess1.Text = bankProcess1.Description + " " + bankProcess1.Amount + " " + (bankProcess1.ProcessDate.HasValue ? bankProcess1.ProcessDate.Value.ToString("dd.MM.yyyy HH:mm") : "Tarih yok");

            var bankProcess2 = db.BankProcesses.OrderByDescending(x => x.BankProcessId).Take(2).Skip(1).FirstOrDefault();
            lblBankProcess2.Text = bankProcess2.Description + " " + bankProcess2.Amount + " " + (bankProcess2.ProcessDate.HasValue ? bankProcess2.ProcessDate.Value.ToString("dd.MM.yyyy HH:mm") : "Tarih yok");

            var bankProcess3 = db.BankProcesses.OrderByDescending(x => x.BankProcessId).Take(3).Skip(2).FirstOrDefault();
            lblBankProcess3.Text = bankProcess3.Description + " " + bankProcess3.Amount + " " + (bankProcess3.ProcessDate.HasValue ? bankProcess3.ProcessDate.Value.ToString("dd.MM.yyyy HH:mm") : "Tarih yok");

            var bankProcess4 = db.BankProcesses.OrderByDescending(x => x.BankProcessId).Take(4).Skip(3).FirstOrDefault();
            lblBankProcess4.Text = bankProcess4.Description + " " + bankProcess4.Amount + " " + (bankProcess4.ProcessDate.HasValue ? bankProcess4.ProcessDate.Value.ToString("dd.MM.yyyy HH:mm") : "Tarih yok");

            var bankProcess5 = db.BankProcesses.OrderByDescending(x => x.BankProcessId).Take(5).Skip(4).FirstOrDefault();
            lblBankProcess5.Text = bankProcess5.Description + " " + bankProcess5.Amount + " " + (bankProcess5.ProcessDate.HasValue ? bankProcess5.ProcessDate.Value.ToString("dd.MM.yyyy HH:mm") : "Tarih yok");
        }

        private void btnBillForm_Click(object sender, EventArgs e)
        {
            FrmSpendings spendingForm = new FrmSpendings();
            spendingForm.Show();
            this.Hide();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FrmSettings settingsForm = new FrmSettings();
            settingsForm.Show();
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

        private void btnCategories_Click(object sender, EventArgs e)
        {
            FrmCategories categoriesForm = new FrmCategories();
            categoriesForm.Show();
            this.Hide();
        }

        private void btnBanks_Click(object sender, EventArgs e)
        {
            FrmBanks banksForm = new FrmBanks();
            banksForm.Show();
            this.Hide();
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {
            FrmBilling billingForm = new FrmBilling();  
            billingForm.Show();
            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            FrmDashboard frmDashboard = new FrmDashboard();
            frmDashboard.Show();
            this.Hide();
        }

        private void btnBankProcess_Click(object sender, EventArgs e)
        {
            FrmBankProcesses bankProcessesForm = new FrmBankProcesses();
            bankProcessesForm.Show();
            this.Hide();
        }
    }
}
