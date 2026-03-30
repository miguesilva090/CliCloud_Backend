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

      builder.Property(f => f.TipoModoPagamento)
        .HasConversion<int>();

      builder.Property(f => f.TipoFornecedor)
        .HasConversion<int>();

      builder.Property(f => f.CondicaoPagamento)
        .HasConversion<int>();

      builder.Property(f => f.Origem)
        .HasConversion<int>();
    }
  }
}
