using CliCloud.Domain.Entities.Alergias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AlergiaUtenteConfiguration : IEntityTypeConfiguration<AlergiaUtente>
    {
        public void Configure(EntityTypeBuilder<AlergiaUtente> builder)
        {
            builder.ToTable("AlergiaUtente", "Alergias");

            builder.HasOne(a => a.Utente)
                .WithMany()
                .HasForeignKey(a => a.UtenteId);

            builder.HasOne(a => a.Alergia)
                .WithMany()
                .HasForeignKey(a => a.AlergiaId)
                .IsRequired(false);

            builder.HasOne(a => a.GrauAlergia)
                .WithMany()
                .HasForeignKey(a => a.GrauAlergiaId)
                .IsRequired(false);
        }
    }
}
