using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AcordoConfiguration : IEntityTypeConfiguration<Acordos>
    {
        public void Configure(EntityTypeBuilder<Acordos> builder)
        {
            builder.ToTable("Acordos", "Exames");

            builder.HasOne(a => a.TipoExame)
                .WithMany()
                .HasForeignKey(a => a.TipoExameId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Organismo)
                .WithMany()
                .HasForeignKey(a => a.OrganismoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => new { a.TipoExameId, a.OrganismoId })
                .IsUnique();
        }
    }
}
