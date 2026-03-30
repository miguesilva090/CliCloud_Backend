using CliCloud.Domain.Entities.Core.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class SmsRecebidoConfiguration : IEntityTypeConfiguration<SmsRecebido>
    {
        public void Configure(EntityTypeBuilder<SmsRecebido> builder)
        {
            builder.ToTable("SmsRecebido", "Core");

            builder.HasIndex( x => new {x.ClinicaId, x.DataHoraRecebimento});
            builder.HasIndex( x => new {x.ClinicaId, x.NumeroOrigem});

            builder.HasOne( x => x.Clinica)
                .WithMany()
                .HasForeignKey(x => x.ClinicaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}