using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibo.Business.Models;

namespace Nibo.Data.Mappings
{
    public class BankTransactionsMapping : IEntityTypeConfiguration<BankTransactions>
    {
        public void Configure(EntityTypeBuilder<BankTransactions> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.TransactionValue)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.Statement)
                .WithMany(t => t.Transactions);

            builder.ToTable("BankTransactions");
        }
    }
}
