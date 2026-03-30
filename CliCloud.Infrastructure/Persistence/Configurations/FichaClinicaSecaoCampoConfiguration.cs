using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class FichaClinicaSecaoCampoConfiguration : IEntityTypeConfiguration<FichaClinicaSecaoCampo>
    {
        public void Configure(EntityTypeBuilder<FichaClinicaSecaoCampo> builder)
        {
            builder.ToTable("FichaClinicaSecaoCampo", "ProcessoClinico");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.TipoCampo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.NumeroLinhas)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(c => c.Ordem)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .IsRequired();

            builder.HasOne(c => c.Separador)
                .WithMany()
                .HasForeignKey(c => c.SeparadorId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}

