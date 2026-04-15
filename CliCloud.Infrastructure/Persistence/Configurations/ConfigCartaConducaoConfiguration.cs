using CliCloud.Domain.Entities.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ConfigCartaConducaoConfiguration : IEntityTypeConfiguration<ConfigCartaConducao>
    {
        public void Configure(EntityTypeBuilder<ConfigCartaConducao> builder)
        {
            builder.ToTable("ConfigCartaConducao", "Core");

            builder.HasIndex(x => x.ClinicaId).IsUnique();

            builder.Property(x => x.UrlOnline).HasMaxLength(500);
            builder.Property(x => x.UrlOffline).HasMaxLength(500);
            builder.Property(x => x.Utilizador).HasMaxLength(150);
            builder.Property(x => x.Password).HasMaxLength(300);

            builder.HasOne<CliCloud.Domain.Entities.Core.Clinica>()
                .WithMany()
                .HasForeignKey(x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}