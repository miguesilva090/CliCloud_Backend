using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ListaEsperaConsultaConfiguration : IEntityTypeConfiguration<ListaEsperaConsulta>
{
    public void Configure(EntityTypeBuilder<ListaEsperaConsulta> builder)
    {
        builder.ToTable("ListaEsperaConsulta", "Consultas");

        builder.HasIndex(x => x.Data);
        builder.HasIndex(x => new { x.Data, x.UtenteId });
        builder.HasIndex(x => x.ConsultaMarcacaoId)
            .IsUnique()
            .HasFilter("[ConsultaMarcacaoId] IS NOT NULL");

        builder.Property(x => x.Credencial).HasMaxLength(100);
        builder.Property(x => x.Obs).HasMaxLength(4000);

        builder.HasOne(x => x.Utente)
            .WithMany()
            .HasForeignKey(x => x.UtenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Medico)
            .WithMany()
            .HasForeignKey(x => x.MedicoId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.Especialidade)
            .WithMany()
            .HasForeignKey(x => x.EspecialidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Organismo)
            .WithMany()
            .HasForeignKey(x => x.OrganismoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Prioridade)
            .WithMany()
            .HasForeignKey(x => x.PrioridadeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.TipoConsultaItem)
            .WithMany()
            .HasForeignKey(x => x.TipoConsultaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ConsultaMarcacao)
            .WithOne()
            .HasForeignKey<ListaEsperaConsulta>(x => x.ConsultaMarcacaoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}