using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class SeparadorConfiguration : IEntityTypeConfiguration<Separador>
{
    public void Configure(EntityTypeBuilder<Separador> builder)
    {
        builder.ToTable("Separador", "ProcessoClinico");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Codigo)
            .HasMaxLength(50)
            .HasDefaultValue(string.Empty);

        builder.HasIndex(x => x.Codigo)
            .HasFilter("[Codigo] <> ''")
            .IsUnique();

        builder.Property(x => x.Ordem)
            .IsRequired();

        builder.Property(x => x.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
