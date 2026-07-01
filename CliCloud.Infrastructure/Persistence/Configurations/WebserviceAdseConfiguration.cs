using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Entities.Organismos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class WebserviceAdseConfiguration : IEntityTypeConfiguration<WebserviceAdse>
{
    public void Configure(EntityTypeBuilder<WebserviceAdse> builder)
    {
        builder.ToTable("WebserviceAdse", "Faturacao");
        builder.HasIndex(x => x.ClinicaId).IsUnique().HasFilter("[DeletedOn] IS NULL ");

        builder.HasOne<Organismo>()
            .WithMany()
            .HasForeignKey(x => x.OrganismoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
