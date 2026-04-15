using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TeleconsultaAcessoLogConfiguration : IEntityTypeConfiguration<TeleconsultaAcessoLog>
  {
    public void Configure(EntityTypeBuilder<TeleconsultaAcessoLog> builder)
    {
      builder.ToTable("TeleconsultaAcessoLog", "Consultas");

      builder.HasIndex(x => new { x.TeleconsultaSessaoId, x.CreatedOn });
      builder.HasIndex(x => new { x.ClinicaId, x.CreatedOn });

      builder.Property(x => x.UserId).HasMaxLength(120);
      builder.Property(x => x.Papel).IsRequired().HasMaxLength(30);
      builder.Property(x => x.Acao).IsRequired().HasMaxLength(40);
      builder.Property(x => x.Mensagem).HasMaxLength(500);
      builder.Property(x => x.Ip).HasMaxLength(100);
      builder.Property(x => x.UserAgent).HasMaxLength(500);

      builder.HasOne(x => x.TeleconsultaSessao)
        .WithMany()
        .HasForeignKey(x => x.TeleconsultaSessaoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
