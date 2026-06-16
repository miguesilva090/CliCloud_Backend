using CliCloud.Domain.Entities.Organismos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class OrganismoConfiguration : IEntityTypeConfiguration<Organismo>
  {
    public void Configure(EntityTypeBuilder<Organismo> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Organismo", "Organismos");

      // Relacionamento N:1 com Banco
      builder.HasOne(o => o.Banco)
        .WithMany()
        .HasForeignKey(o => o.BancoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(o => o.CondicaoPagamento)
        .WithMany()
        .HasForeignKey(o => o.CondicaoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(o => o.ModoPagamento)
        .WithMany()
        .HasForeignKey(o => o.ModoPagamentoId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
