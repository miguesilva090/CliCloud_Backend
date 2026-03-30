using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ConfiguracaoSmsAutomaticaConfiguration : IEntityTypeConfiguration<ConfiguracaoSmsAutomatica>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoSmsAutomatica> builder)
        {
            builder.ToTable("ConfiguracaoSmsAutomatica", "Core");

            builder.HasIndex(x => new { x.ClinicaId , x.Codigo}).IsUnique();
            builder.HasIndex(x => new { x.ClinicaId , x.Ativo});

            builder.Property(x => x.Ativo).HasDefaultValue(0);
            builder.Property(x => x.TodosMedicos).HasDefaultValue(false);

            builder.HasOne( x => x.Clinica)
                .WithMany()
                .HasForeignKey(x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}