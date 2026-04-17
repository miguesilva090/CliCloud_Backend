using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ExamesSemPapelOperacaoConfiguration : IEntityTypeConfiguration<ExamesSemPapelOperacao>
{
  public void Configure(EntityTypeBuilder<ExamesSemPapelOperacao> builder)
  {
    builder.ToTable("ExamesSemPapelOperacao", "Consultas");
    builder.HasKey(x => x.Id);

    builder.Property(x => x.RequisicaoId).HasMaxLength(120).IsRequired();
    builder.Property(x => x.AreaPrestacao).HasMaxLength(20);

    builder.HasIndex(x => new { x.ClinicaId, x.RequisicaoId }).IsUnique();
  }
}
