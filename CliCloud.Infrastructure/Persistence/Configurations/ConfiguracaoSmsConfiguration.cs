using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ConfiguracaoSmsConfiguration : IEntityTypeConfiguration<ConfiguracaoSms>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoSms> builder)
        {
            builder.ToTable("ConfiguracaoSms", "Core");

            builder.HasIndex(x => x.ClinicaId).IsUnique();
            builder.HasIndex(x => new {x.ClinicaId, x.Ativo});

            builder.Property(x => x.UsenditArpoone).HasDefaultValue(1);

            builder.HasOne(x => x.Clinica)
                .WithMany()
                .HasForeignKey(x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}