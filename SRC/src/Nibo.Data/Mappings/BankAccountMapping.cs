using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibo.Business.Models;

namespace Nibo.Data.Mappings
{
    public class BankAccountMapping : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.AgencyCode)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.HasOne(a => a.Bank);

            builder.ToTable("BankAccounts");
        }
    }
}
