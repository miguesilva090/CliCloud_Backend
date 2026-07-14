using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ListaEsperaTratamentoConfiguration : IEntityTypeConfiguration<ListaEsperaTratamento>
{
    public void Configure(EntityTypeBuilder<ListaEsperaTratamento> builder)
    {
        builder.ToTable("ListaEsperaTratamento", "Tratamentos");

        builder.HasIndex(x => x.Ordem);
        builder.HasIndex(x => new { x.Historico, x.Ordem });
        builder.HasIndex(x => x.DataEntrada);

        builder.Property(x => x.Designacao).HasMaxLength(200);
        builder.Property(x => x.Credencial).HasMaxLength(100);
        builder.Property(x => x.Obs).HasMaxLength(4000);
        builder.Property(x => x.TecObs).HasMaxLength(4000);
        builder.Property(x => x.HoraDesejada).HasMaxLength(50);
        builder.Property(x => x.DuracaoTotal).HasMaxLength(50);

        builder
            .HasOne(x => x.Utente)
            .WithMany()
            .HasForeignKey(x => x.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Medico)
            .WithMany()
            .HasForeignKey(x => x.MedicoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Organismo)
            .WithMany()
            .HasForeignKey(x => x.OrganismoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Prioridade)
            .WithMany()
            .HasForeignKey(x => x.PrioridadeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.EstadoListaEspera)
            .WithMany()
            .HasForeignKey(x => x.EstadoListaEsperaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.LocalTratamento)
            .WithMany()
            .HasForeignKey(x => x.LocalTratamentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Patologia)
            .WithMany()
            .HasForeignKey(x => x.PatologiaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Sinistrado)
            .WithMany()
            .HasForeignKey(x => x.SinistradoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Seguradora)
            .WithMany()
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
