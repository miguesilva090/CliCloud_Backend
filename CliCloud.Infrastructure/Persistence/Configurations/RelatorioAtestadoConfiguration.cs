using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class RelatorioAtestadoConfiguration : IEntityTypeConfiguration<RelatorioAtestado> 
    {
        public void Configure(EntityTypeBuilder<RelatorioAtestado> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TextoHtml)
                .IsRequired();

            builder.HasOne(x => x.Utente)
                .WithMany()
                .HasForeignKey(x => x.UtenteId);

            builder.HasOne(x => x.Medico)
                .WithMany()
                .HasForeignKey(x => x.MedicoId);
        }
    }
}