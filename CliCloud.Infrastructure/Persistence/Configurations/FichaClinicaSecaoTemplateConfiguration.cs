using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class FichaClinicaSecaoTemplateConfiguration : IEntityTypeConfiguration<FichaClinicaSecaoTemplate>
    {
        public void Configure(EntityTypeBuilder<FichaClinicaSecaoTemplate> builder)
        {
            builder.ToTable("FichaClinicaSecaoTemplate", "ProcessoClinico");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Codigo)
              .HasMaxLength(100)
              .IsRequired();

            builder.Property(t => t.UtilizadorId)
              .IsRequired();

            builder.Property(t => t.Nome)
              .HasMaxLength(200)
              .IsRequired();

            builder.Property(t => t.Descricao)
              .HasMaxLength(500);

            builder.Property(t => t.Ordem)
              .IsRequired();

            builder.Property(t => t.Ativo)
              .IsRequired();

            builder.HasIndex(t => new { t.UtilizadorId, t.Codigo }).IsUnique();
        }
    }
}