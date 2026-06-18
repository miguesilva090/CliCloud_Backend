using CliCloud.Domain.Entities.Stocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ArmazemConfiguration : IEntityTypeConfiguration<Armazem>
{
    public void Configure(EntityTypeBuilder<Armazem> builder)
    {
        builder.ToTable("Armazem", "Stocks");

        builder.Property(x => x.Nome).HasMaxLength(40);
        builder.Property(x => x.Morada).HasMaxLength(50);
        builder.Property(x => x.Localidade).HasMaxLength(50);
        builder.Property(x => x.Telefone).HasMaxLength(20);
        builder.Property(x => x.Fax).HasMaxLength(20);

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => x.ClinicaId);

        builder.HasOne(x => x.CodigoPostal)
            .WithMany()
            .HasForeignKey(x => x.CodigoPostalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
