using CliCloud.Domain.Entities.Utentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class UtentePatologiaComparticipacaoConfiguration 
        : IEntityTypeConfiguration<UtentePatologiaComparticipacao>
    {
        public void Configure(EntityTypeBuilder<UtentePatologiaComparticipacao> builder)
        {
            builder.ToTable("UtentePatologiaComparticipacao", "Utentes");

            builder.HasOne(x => x.Utente)
                .WithMany()
                .HasForeignKey(x => x.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Designacao).HasMaxLength(200);

            builder.HasIndex(x => new { x.UtenteId, x.CodigoComparticipacao })
                .IsUnique()
                .HasDatabaseName("IX_UtentePatologiaComparticipacao_UtenteId_CodigoComparticipacao");
        }
    }
}