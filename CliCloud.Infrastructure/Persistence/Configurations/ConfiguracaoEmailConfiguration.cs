using CliCloud.Domain.Entities.Core.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfiguracaoEmailConfiguration : IEntityTypeConfiguration<ConfiguracaoEmail>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoEmail> builder)
    {
        builder.ToTable("ConfiguracaoEmail", "Core");

        builder.HasIndex(x => x.ClinicaId).IsUnique();

        builder.HasOne(x => x.Clinica)
            .WithMany()
            .HasForeignKey(x => x.ClinicaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}