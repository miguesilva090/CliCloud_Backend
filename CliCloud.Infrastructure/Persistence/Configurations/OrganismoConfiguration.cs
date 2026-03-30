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

      // Configurar enum conversions
      builder.Property(o => o.CondicaoPagamento)
        .HasConversion<int>();

      builder.Property(o => o.TipoModoPagamento)
        .HasConversion<int>();

      // Relacionamento N:1 com Banco
      builder.HasOne(o => o.Banco)
        .WithMany()
        .HasForeignKey(o => o.BancoId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
