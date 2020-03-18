using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibo.Business.Models;

namespace Nibo.Data.Mappings
{
    public class BankStatementMapping : IEntityTypeConfiguration<BankStatement>
    {
        public void Configure(EntityTypeBuilder<BankStatement> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(s => s.BankAccount);

            builder.HasMany(s => s.Transactions)
                .WithOne(t => t.Statement)
                .HasForeignKey(t => t.BankStatementId);

            builder.ToTable("BankStatements");
        }
    }
}
