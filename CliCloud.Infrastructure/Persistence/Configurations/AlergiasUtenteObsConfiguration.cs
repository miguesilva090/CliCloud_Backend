using CliCloud.Domain.Entities.Alergias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AlergiasUtenteObsConfiguration : IEntityTypeConfiguration<AlergiasUtenteObs>
    {
        public void Configure(EntityTypeBuilder<AlergiasUtenteObs> builder)
        {
            builder.ToTable("AlergiasUtenteObs", "Alergias");

            builder.HasOne(a => a.Utente)
                .WithMany()
                .HasForeignKey(a => a.UtenteId);
        }
    }
}
