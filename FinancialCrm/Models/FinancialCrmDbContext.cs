using FinancialCrm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCrm.Models
{
    public class FinancialCrmDbContext : DbContext
    {
        public FinancialCrmDbContext()
            : base("name=FinancialCrmDbEntities")
        {
        }
        public DbSet<Users> Users { get; set; }
    }
}
