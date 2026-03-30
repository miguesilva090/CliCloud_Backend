using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class HistoricoSmsConfiguration : IEntityTypeConfiguration<HistoricoSms>
    {
        public void Configure(EntityTypeBuilder<HistoricoSms> builder)
        {
            builder.ToTable("HistoricoSms", "Core");

            builder.HasIndex( x => new { x.ClinicaId, x.IdMensagem}).IsUnique();
            builder.HasIndex( x => new { x.ClinicaId, x.DataHoraCriacao});
            builder.HasIndex( x => new { x.ClinicaId, x.Status});
            builder.HasIndex( x => new { x.ClinicaId, x.Modulo});

            builder.HasOne( x => x.Clinica)
                .WithMany()
                .HasForeignKey( x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}