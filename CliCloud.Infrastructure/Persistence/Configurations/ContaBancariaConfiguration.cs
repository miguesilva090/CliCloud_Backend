using CliCloud.Domain.Entities.Bancos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
    {
        public void Configure(EntityTypeBuilder<ContaBancaria> builder)
        {
            builder.ToTable("ContaBancaria", "Bancos");

            builder.HasIndex(x => x.Numero);

            builder
                .HasOne(x => x.Banco)
                .WithMany()
                .HasForeignKey(x => x.BancoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}