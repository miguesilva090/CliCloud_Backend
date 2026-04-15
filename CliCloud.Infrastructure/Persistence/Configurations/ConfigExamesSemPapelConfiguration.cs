using CliCloud.Domain.Entities.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfigExamesSemPapelConfiguration : IEntityTypeConfiguration<ConfigExamesSemPapel>
{
    public void Configure(EntityTypeBuilder<ConfigExamesSemPapel> builder)
    {
        builder.ToTable("ConfigExamesSemPapel", "Core");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ClinicaId).IsUnique();

        builder.Property(x => x.Username).HasMaxLength(100);
        builder.Property(x => x.Password).HasMaxLength(300);

        builder.Property(x => x.PesquisaPrestacao).HasMaxLength(500);
        builder.Property(x => x.Agendamento).HasMaxLength(500);
        builder.Property(x => x.Efetivacao).HasMaxLength(500);
        builder.Property(x => x.Anulacao).HasMaxLength(500);
        builder.Property(x => x.ConsultaCancelados).HasMaxLength(500);
        builder.Property(x => x.EfetuadosNaoPrescritos).HasMaxLength(500);
        builder.Property(x => x.TaxasModeradoras).HasMaxLength(500);

        builder.Property(x => x.RelatorioResultados).HasMaxLength(500);
        builder.Property(x => x.UsernamePartilhaResultados).HasMaxLength(100);
        builder.Property(x => x.PasswordPartilhaResultados).HasMaxLength(300);

        builder.Property(x => x.RelatorioResultadosSemRequisicao).HasMaxLength(500);
        builder.Property(x => x.UsernamePartilhaResultadosSemRequisicao).HasMaxLength(100);
        builder.Property(x => x.PasswordPartilhaResultadosSemRequisicao).HasMaxLength(300);

        builder.Property(x => x.AreaPrestacao).HasMaxLength(20);

    }
}