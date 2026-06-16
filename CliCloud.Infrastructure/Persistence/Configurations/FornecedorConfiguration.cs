using CliCloud.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
  {
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
      builder.ToTable("Fornecedor", "Fornecedores");

      // Configurar enum conversions
      builder.Property(f => f.Moeda)
        .HasConversion<int>();

      builder.Property(f => f.TipoFornecedor)
        .HasConversion<int>();

      builder.Property(f => f.Origem)
        .HasConversion<int>();

      builder.HasOne(f => f.CondicaoPagamento)
        .WithMany()
        .HasForeignKey(f => f.CondicaoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(f => f.ModoPagamento)
        .WithMany()
        .HasForeignKey(f => f.ModoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
