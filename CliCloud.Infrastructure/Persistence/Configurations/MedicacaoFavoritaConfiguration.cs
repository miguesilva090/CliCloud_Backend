using CliCloud.Domain.Entities.Prescricao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MedicacaoFavoritaConfiguration : IEntityTypeConfiguration<MedicacaoFavorita>
    {
        public void Configure(EntityTypeBuilder<MedicacaoFavorita> builder)
        {
            builder.ToTable("MedicacaoFavorita", "Prescricao");

            builder.HasOne(x => x.Medico)
                .WithMany()
                .HasForeignKey(x => x.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Cnpem).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EmbId).HasMaxLength(50);
            builder.Property(x => x.Designacao).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Dosagem).HasMaxLength(254);
            builder.Property(x => x.DescricaoEmbalagem).HasMaxLength(500);
            builder.Property(x => x.FormaFarmaceutica).HasMaxLength(200);
            builder.Property(x => x.PrincipioAtivo).HasMaxLength(500);
            builder.Property(x => x.Posologia).HasMaxLength(1000);

            builder.HasIndex(x => new { x.MedicoId, x.Cnpem })
                .IsUnique()
                .HasFilter("[DeletedOn] IS NULL")
                .HasDatabaseName("IX_MedicacaoFavorita_MedicoId_Cnpem");
        }
    }
}