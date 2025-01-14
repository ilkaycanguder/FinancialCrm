using FinancialCrm.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FinancialCrm
{
    public partial class FrmSpendings : Form
    {
        public FrmSpendings()
        {
            InitializeComponent();
        }
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();
        private void FrmSpendings_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadSpendings();
        }

        private void LoadCategories()
        {
            var categories = db.Categories.ToList();
            cmbCategories.DataSource = categories;
            cmbCategories.DisplayMember = "CategoryName";
            cmbCategories.ValueMember = "CategoryId";
        }

        private void LoadSpendings()
        {
            var spendingList = (from s in db.Spendings
                                join c in db.Categories on s.CategoryId equals c.CategoryId
                                select new
                                {
                                    s.SpendingId,
                                    s.SpendingTitle,
                                    s.SpendingAmount,
                                    s.SpendingDate,
                                    CategoryName = c.CategoryName
                                }).ToList();

            dataGridView1.DataSource = spendingList;
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            FrmCategories frmCategories = new FrmCategories();
            frmCategories.Show();
            this.Hide();
        }

        private void btnBanksForm_Click(object sender, EventArgs e)
        {
            FrmBanks frmBanks = new FrmBanks();
            frmBanks.Show();
            this.Hide();
        }

        private void btnBillings_Click(object sender, EventArgs e)
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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            FrmDashboard frmDashboard = new FrmDashboard();
            frmDashboard.Show();
            this.Show();
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

        private void btnBankProcesses_Click(object sender, EventArgs e)
        {
            FrmBankProcesses frmBankProcesses = new FrmBankProcesses();
            frmBankProcesses.Show();
            this.Hide();
        }

        private void btnSpendingList2_Click(object sender, EventArgs e)
        {
            var spendingList = (from s in db.Spendings
                                join c in db.Categories on s.CategoryId equals c.CategoryId
                                select new
                                {
                                    s.SpendingId,
                                    s.SpendingTitle,
                                    s.SpendingAmount,
                                    s.SpendingDate,
                                    CategoryName = c.CategoryName
                                }).ToList();

            dataGridView1.DataSource = spendingList;
        }

        private void btnCreateSpending2_Click(object sender, EventArgs e)
        {
            try
            {
                string title = txtSpendingTitle.Text;
                decimal amount;
                if (!decimal.TryParse(txtSpendingAmount.Text, out amount))
                {
                    MessageBox.Show("Geçerli bir tutar giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime date;
                if (!DateTime.TryParse(txtSpendingDate.Text, out date))
                {
                    MessageBox.Show("Geçerli bir tarih giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbCategories.SelectedValue == null)
                {
                    MessageBox.Show("Kategori seçilmedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int categoryId = (int)cmbCategories.SelectedValue;

                Spendings spendings = new Spendings
                {
                    SpendingTitle = title,
                    SpendingAmount = amount,
                    SpendingDate = date,
                    CategoryId = categoryId
                };

                db.Spendings.Add(spendings);
                db.SaveChanges();
                MessageBox.Show("Gider Başarılı Bir Şekilde Sisteme Eklendi", "Giderler", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSpendings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveSpending2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtSpendingId.Text);
            var removeValue = db.Spendings.Find(id);

            if (removeValue != null)
            {
                db.Spendings.Remove(removeValue);
                db.SaveChanges();

                MessageBox.Show("Harcamalar Başarılı Bir Şekilde Sistemden Silindi", "Harcamalar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Harcama Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var values = db.Spendings
                       .Join(db.Categories,
                             s => s.CategoryId,
                             c => c.CategoryId,
                             (spendingItem, category) => new
                             {
                                 spendingItem.SpendingId,
                                 spendingItem.SpendingTitle,
                                 spendingItem.SpendingAmount,
                                 spendingItem.SpendingDate,
                                 CategoryName = category.CategoryName 
                             })
                       .ToList();

            dataGridView1.DataSource = values;
        }

        private void btnUpdateSpending2_Click(object sender, EventArgs e)
        {
            string spendingTitle = txtSpendingTitle.Text;
            decimal spendingAmount = decimal.Parse(txtSpendingAmount.Text);
            string spendingDate = txtSpendingDate.Text; 
            int categoryId = int.Parse(cmbCategories.SelectedValue.ToString());
            int id = int.Parse(txtSpendingId.Text); 

            var spending = db.Spendings.Find(id);

            if (spending != null)
            {
                spending.SpendingTitle = spendingTitle;
                spending.SpendingAmount = spendingAmount;
                spending.SpendingDate = DateTime.Parse(spendingDate);
                spending.CategoryId = categoryId;

                db.SaveChanges();

                MessageBox.Show("Harcama Başarılı Bir Şekilde Güncellendi", "Harcamalar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Harcama Bulunamadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var values = db.Spendings
                 .Join(db.Categories,
                       s => s.CategoryId,
                       c => c.CategoryId,
                       (spendingItem, category) => new
                       {
                           spendingItem.SpendingId,
                           spendingItem.SpendingTitle,
                           spendingItem.SpendingAmount,
                           spendingItem.SpendingDate,
                           CategoryName = category.CategoryName 
                       })
                 .ToList();

            dataGridView1.DataSource = values;
        }
    }
}
