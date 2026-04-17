using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ExamesSemPapelAssinaturaSessaoConfiguration
  : IEntityTypeConfiguration<ExamesSemPapelAssinaturaSessao>
{
  public void Configure(EntityTypeBuilder<ExamesSemPapelAssinaturaSessao> builder)
  {
    builder.ToTable("ExamesSemPapelAssinaturaSessao", "Consultas");
    builder.HasKey(x => x.Id);

    builder.Property(x => x.CMedico).HasMaxLength(50).IsRequired();
    builder.Property(x => x.TipoCartao).HasMaxLength(20).IsRequired();
    builder.Property(x => x.DigestValue).HasMaxLength(4000).IsRequired();
    builder.Property(x => x.SignatureValue).HasMaxLength(4000).IsRequired();
    builder.Property(x => x.Assinatura).HasMaxLength(8000).IsRequired();
    builder.Property(x => x.AssinaturaSubCA).HasMaxLength(8000).IsRequired();

    builder.HasIndex(x => x.UtilizadorId).IsUnique();
  }
}
