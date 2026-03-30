using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class MarcacaoConsultaConfiguration : IEntityTypeConfiguration<ConsultaMarcacao>
  {
    public void Configure(EntityTypeBuilder<ConsultaMarcacao> builder)
    {
      builder.ToTable("ConsultaMarcacao", "Consultas");

      builder.HasOne(m => m.Consulta)
        .WithOne(c => c.ConsultaMarcacao)
        .HasForeignKey<Consulta>(c => c.ConsultaMarcacaoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(m => m.Utente)
        .WithMany()
        .HasForeignKey(m => m.UtenteId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(m => m.Medico)
        .WithMany()
        .HasForeignKey(m => m.MedicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(m => m.Especialidade)
        .WithMany()
        .HasForeignKey(m => m.EspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(m => m.TipoConsultaItem)
        .WithMany()
        .HasForeignKey(m => m.TipoConsultaId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
