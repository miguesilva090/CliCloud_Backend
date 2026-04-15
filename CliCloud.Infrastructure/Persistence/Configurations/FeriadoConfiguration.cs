using CliCloud.Domain.Entities.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class FeriadoConfiguration : IEntityTypeConfiguration<Feriado>
{
    public void Configure(EntityTypeBuilder<Feriado> builder)
    {
        builder.ToTable("Feriado", "Utility");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClinicaId).IsRequired();
        builder.Property(x => x.Data).IsRequired();
        builder.Property(x => x.Designacao).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Ativo).IsRequired();

        builder.HasIndex(x => new { x.ClinicaId, x.Data }).IsUnique();

        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).HasMaxLength(50);
    }
}