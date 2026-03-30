using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ConfiguracaoSmsAutomaticaMedicoConfiguration : IEntityTypeConfiguration<ConfiguracaoSmsAutomaticaMedico>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoSmsAutomaticaMedico> builder)
        {
            builder.ToTable("ConfiguracaoSmsAutomaticaMedico", "Core");

            builder.HasIndex(x => new { x.ClinicaId, x.CodigoConfiguracao, x.CodigoMedico}).IsUnique();

            builder.HasOne( x => x.Clinica)
                .WithMany()
                .HasForeignKey(x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}