using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class LoteDirectConfiguration : IEntityTypeConfiguration<LoteDirect>
    {
        public void Configure(EntityTypeBuilder<LoteDirect> builder)
        {
            builder.ToTable("LoteDirect", "Credenciais");

            builder.HasIndex(x => x.Credencial);
            builder.HasIndex(x => x.NumeroLote);
            builder.HasIndex(x => x.CodigoOrganismo);
            builder.HasIndex(x => x.Historico);
            builder.HasIndex(x => x.UtenteId);

            builder.HasOne(x => x.Utente)
                .WithMany()
                .HasForeignKey(x => x.UtenteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Medico)
                .WithMany()
                .HasForeignKey(x => x.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MedicoExterno)
                .WithMany()
                .HasForeignKey(x => x.MedicoExternoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoServicoRegisto)
                .WithMany()
                .HasForeignKey(x => x.TipoServicoRegistoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ServicoConsultaRegisto)
                .WithMany()
                .HasForeignKey(x => x.ServicoConsultaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}