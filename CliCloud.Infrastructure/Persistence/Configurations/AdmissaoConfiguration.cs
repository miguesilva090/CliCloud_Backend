using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class AdmissaoConfiguration : IEntityTypeConfiguration<Admissao>
  {
    public void Configure(EntityTypeBuilder<Admissao> builder)
    {
      builder.ToTable("Admissao", "Consultas");

      builder.HasIndex(x => x.Data);
      builder.HasIndex(x => new { x.Data, x.UtenteId });
      builder.HasIndex(x => x.ConsultaMarcacaoId)
        .IsUnique()
        .HasFilter("[ConsultaMarcacaoId] IS NOT NULL");

      builder.HasOne(x => x.Utente)
        .WithMany()
        .HasForeignKey(x => x.UtenteId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(x => x.ConsultaMarcacao)
        .WithOne()
        .HasForeignKey<Admissao>(x => x.ConsultaMarcacaoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(x => x.Medico)
        .WithMany()
        .HasForeignKey(x => x.MedicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(x => x.Especialidade)
        .WithMany()
        .HasForeignKey(x => x.EspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(x => x.Tecnico)
        .WithMany()
        .HasForeignKey(x => x.TecnicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(x => x.Funcionario)
        .WithMany()
        .HasForeignKey(x => x.FuncionarioId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.MedicoExterno)
        .WithMany()
        .HasForeignKey(x => x.MedicoExternoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.Sala)
        .WithMany()
        .HasForeignKey(x => x.SalaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.MotivoConsulta)
        .WithMany()
        .HasForeignKey(x => x.MotivoConsultaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.TipoAdmissao)
        .WithMany()
        .HasForeignKey(x => x.TipoAdmissaoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.TipoConsultaItem)
        .WithMany()
        .HasForeignKey(x => x.TipoConsultaId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(x => x.Organismo)
        .WithMany()
        .HasForeignKey(x => x.OrganismoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.Seguradora)
        .WithMany()
        .HasForeignKey(x => x.SeguradoraId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.Tratamento)
        .WithMany()
        .HasForeignKey(x => x.TratamentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.DoencaPrincipal)
        .WithMany()
        .HasForeignKey(x => x.DoencaPrincipalId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.DoencaSecundaria)
        .WithMany()
        .HasForeignKey(x => x.DoencaSecundariaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasMany(x => x.Servicos)
        .WithOne(s => s.Admissao)
        .HasForeignKey(s => s.AdmissaoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
