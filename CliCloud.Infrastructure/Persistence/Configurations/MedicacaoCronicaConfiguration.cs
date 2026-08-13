using CliCloud.Domain.Entities.Prescricao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MedicacaoCronicaConfiguration : IEntityTypeConfiguration<MedicacaoCronica>
    {
        public void Configure(EntityTypeBuilder<MedicacaoCronica> builder)
        {
            builder.ToTable("MedicacaoCronica", "Prescricao");

            builder.HasOne(x => x.Utente)
                .WithMany()
                .HasForeignKey(x => x.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Cnpem).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EmbId).HasMaxLength(50);
            builder.Property(x => x.Designacao).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Dosagem).HasMaxLength(254);
            builder.Property(x => x.DescricaoEmbalagem).HasMaxLength(500);
            builder.Property(x => x.FormaFarmaceutica).HasMaxLength(200);
            builder.Property(x => x.PrincipioAtivo).HasMaxLength(500);
            builder.Property(x => x.Posologia).HasMaxLength(1000);

            builder.HasIndex(x => new { x.UtenteId, x.Cnpem })
                .IsUnique()
                .HasFilter("[DeletedOn] IS NULL")
                .HasDatabaseName("IX_MedicacaoCronica_UtenteId_Cnpem");
        }
    }
}
