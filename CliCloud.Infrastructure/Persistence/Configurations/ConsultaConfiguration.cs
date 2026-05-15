using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
  {
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
      builder.ToTable("Consulta", "Consultas");

      builder.HasOne(c => c.Utente)
        .WithMany()
        .HasForeignKey(c => c.UtenteId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(c => c.Medico)
        .WithMany()
        .HasForeignKey(c => c.MedicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(c => c.Especialidade)
        .WithMany()
        .HasForeignKey(c => c.EspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(c => c.Tecnico)
        .WithMany()
        .HasForeignKey(c => c.TecnicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(c => c.MedicoExterno)
        .WithMany()
        .HasForeignKey(c => c.MedicoExternoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Sala)
        .WithMany()
        .HasForeignKey(c => c.SalaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Documento)
        .WithMany()
        .HasForeignKey(c => c.DocumentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.TipoDocumento)
        .WithMany()
        .HasForeignKey(c => c.TipoDocumentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Organismo)
        .WithMany()
        .HasForeignKey(c => c.OrganismoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Tratamento)
        .WithMany()
        .HasForeignKey(c => c.TratamentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.TipoConsultaItem)
        .WithMany()
        .HasForeignKey(c => c.TipoConsultaId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(c => c.Funcionario)
        .WithMany()
        .HasForeignKey(c => c.FuncionarioId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Seguradora)
        .WithMany()
        .HasForeignKey(c => c.SeguradoraId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.Admissao)
        .WithOne(a => a.Consulta)
        .HasForeignKey<Consulta>(c => c.AdmissaoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasIndex(c => c.AdmissaoId)
        .IsUnique()
        .HasFilter("[AdmissaoId] IS NOT NULL");

      builder.HasOne(c => c.TipoAdmissao)
        .WithMany()
        .HasForeignKey(c => c.TipoAdmissaoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.DoencaPrincipal)
        .WithMany()
        .HasForeignKey(c => c.DoencaPrincipalId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(c => c.DoencaSecundaria)
        .WithMany()
        .HasForeignKey(c => c.DoencaSecundariaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasMany(c => c.Servicos)
        .WithOne(s => s.Consulta)
        .HasForeignKey(s => s.ConsultaId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
