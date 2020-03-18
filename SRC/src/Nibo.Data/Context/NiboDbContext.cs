using Microsoft.EntityFrameworkCore;
using Nibo.Business.Models;
using System.Linq;

namespace Nibo.Data.Context
{
    public class NiboDbContext : DbContext
    {
        public NiboDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> Accounts { get; set; }
        public DbSet<BankStatement> Statements { get; set; }
        public DbSet<BankTransactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NiboDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e=> e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
