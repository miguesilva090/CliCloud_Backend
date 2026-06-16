using CliCloud.Domain.Entities.Empresas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
  {
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
      builder.ToTable("Empresa", "Empresas");

      // Relação N:1 com Banco
      builder.HasOne(e => e.Banco)
        .WithMany()
        .HasForeignKey(e => e.BancoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(e => e.CondicaoPagamento)
        .WithMany()
        .HasForeignKey(e => e.CondicaoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(e => e.ModoPagamento)
        .WithMany()
        .HasForeignKey(e => e.ModoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      // Legado: CInstit (Organismo associado à Empresa)
      builder.HasOne(e => e.Organismo)
        .WithMany()
        .HasForeignKey(e => e.OrganismoId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}

