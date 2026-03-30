using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class RelatorioExamesConfiguration : IEntityTypeConfiguration<RelatorioExames>
    {
        public void Configure(EntityTypeBuilder<RelatorioExames> builder)
        {
            builder.ToTable("RelatorioExames", "ProcessoClinico");
            builder.HasIndex(x => new {x.UtenteId, x.MedicoId}).IsUnique();
            builder.Property(x => x.Texto).HasColumnType("nvarchar(max)");
        }
    }
}