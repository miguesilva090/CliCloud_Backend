using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class HistoriaDentariaConfiguration : IEntityTypeConfiguration<HistoriaDentaria>
    {
        public void Configure(EntityTypeBuilder<HistoriaDentaria> builder)
        {
            builder.ToTable("HistoriaDentaria", "Estomatologia");
            builder.HasIndex(x => new { x.UtenteId, x.DataRegisto });
            builder.Property(x => x.HistoriaHtml).HasColumnType("nvarchar(max)");
            _ = builder.HasOne(x => x.Utente).WithMany().HasForeignKey(x => x.UtenteId);
            _ = builder.HasOne(x => x.Medico).WithMany().HasForeignKey(x => x.MedicoId);
        }
    }
}
