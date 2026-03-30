using CliCloud.Domain.Entities.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class EntidadePessoaConfiguration : IEntityTypeConfiguration<EntidadePessoa>
  {
    public void Configure(EntityTypeBuilder<EntidadePessoa> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("EntidadePessoa", "Utility");

      builder.HasOne(ep => ep.EstadoCivil)
        .WithMany()
        .HasForeignKey(ep => ep.EstadoCivilId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(ep => ep.Habilitacao)
        .WithMany()
        .HasForeignKey(ep => ep.HabilitacaoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(ep => ep.Profissao)
        .WithMany()
        .HasForeignKey(ep => ep.ProfissaoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(ep => ep.Sexo)
        .WithMany()
        .HasForeignKey(ep => ep.SexoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);
    }
  }
}
