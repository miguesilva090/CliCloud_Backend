using CliCloud.Domain.Entities.Core.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class HistoricoEmailConfiguration : IEntityTypeConfiguration<HistoricoEmail>
{
    public void Configure(EntityTypeBuilder<HistoricoEmail> builder)
    {
        builder.ToTable("HistoricoEmail", "Core");

        builder.HasIndex(x => new { x.ClinicaId, x.DataHoraCriacao});
        builder.HasIndex(x => new { x.ClinicaId, x.Status});

        builder.HasOne(x => x.Clinica)
            .WithMany()
            .HasForeignKey(x => x.ClinicaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}