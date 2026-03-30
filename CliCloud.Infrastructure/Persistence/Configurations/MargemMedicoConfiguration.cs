using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MargemMedicoConfiguration : IEntityTypeConfiguration<MargemMedico>
    {
        public void Configure(EntityTypeBuilder<MargemMedico> builder)
        {
            builder.ToTable("MargemMedico", "Medicos");

            builder.HasOne(m => m.Servico)
                .WithMany()
                .HasForeignKey(m => m.ServicoId);

            builder.HasOne(m => m.Medico)
                .WithMany()
                .HasForeignKey(m => m.MedicoId);

            builder.HasIndex(m => new { m.ServicoId, m.MedicoId }).IsUnique();
        }
    }
}