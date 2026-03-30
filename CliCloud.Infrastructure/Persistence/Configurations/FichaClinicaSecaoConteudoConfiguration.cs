using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class FichaClinicaSecaoConteudoConfiguration : IEntityTypeConfiguration<FichaClinicaSecaoConteudo>
    {
        public void Configure(EntityTypeBuilder<FichaClinicaSecaoConteudo> builder)
        {
            builder.ToTable("FichaClinicaSecaoConteudo", "ProcessoClinico");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Utente)
                .WithMany()
                .HasForeignKey(c => c.UtenteId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(c => c.Campo)
                .WithMany()
                .HasForeignKey(c => c.CampoId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(c => c.Texto)
                .IsRequired();
        }
    }

}