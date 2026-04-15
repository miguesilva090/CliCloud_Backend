using CliCloud.Domain.Entities.Core.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfiguracaoEmailAutomaticoConfiguration : IEntityTypeConfiguration<ConfiguracaoEmailAutomatica>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoEmailAutomatica> builder)
    {
        builder.ToTable("ConfiguracaoEmailAutomatico", "Core");

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo}).IsUnique();
        builder.HasIndex(x => new { x.ClinicaId, x.Ativo});

        builder.Property(x => x.Ativo).HasDefaultValue(0);

        builder.HasOne(x => x.Clinica)
            .WithMany()
            .HasForeignKey(x => x.ClinicaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}