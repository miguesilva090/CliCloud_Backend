using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class PedidoConsultaConfiguration : IEntityTypeConfiguration<PedidoConsulta>
{
  public void Configure(EntityTypeBuilder<PedidoConsulta> builder)
  {
    builder.ToTable("PedidoConsulta", "Consultas");
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).HasColumnName("Codigo").ValueGeneratedOnAdd();

    builder.Property(x => x.Hora).HasMaxLength(5);
    builder.Property(x => x.CodigoMedico).HasMaxLength(10);
    builder.Property(x => x.Ficheiro).HasMaxLength(260);

    builder.HasIndex(x => x.ClinicaId);

    builder.HasOne<Clinica>()
      .WithMany()
      .HasForeignKey(x => x.ClinicaId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.UtentePedido)
      .WithMany()
      .HasForeignKey(x => x.CodigoPedidosConsultaUtente)
      .HasPrincipalKey(x => x.Id)
      .OnDelete(DeleteBehavior.Restrict);
  }
}

public class PedidoConsultaUtenteConfiguration : IEntityTypeConfiguration<PedidoConsultaUtente>
{
  public void Configure(EntityTypeBuilder<PedidoConsultaUtente> builder)
  {
    builder.ToTable("PedidoConsultaUtente", "Consultas");
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).HasColumnName("Codigo").ValueGeneratedOnAdd();

    builder.Property(x => x.Nome).HasMaxLength(100);
    builder.Property(x => x.Email).HasMaxLength(250);
    builder.Property(x => x.Telemovel).HasMaxLength(20);
    builder.Property(x => x.NIF).HasMaxLength(15);
  }
}
