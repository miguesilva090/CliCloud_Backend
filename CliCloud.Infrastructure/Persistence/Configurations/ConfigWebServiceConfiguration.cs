using CliCloud.Domain.Entities.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfigWebServiceConfiguration : IEntityTypeConfiguration<ConfigWebService>
{
    public void Configure(EntityTypeBuilder<ConfigWebService> builder)
    {
        builder.ToTable("ConfigWebService", "Core");

        builder.HasKey( x => x.Id);
        builder.HasIndex(x => x.ClinicaId).IsUnique();

        builder.Property(x => x.UrlRnu).HasMaxLength(500);
        builder.Property(x => x.UrlAcss).HasMaxLength(500);
        builder.Property(x => x.LoginAcss).HasMaxLength(100);
        builder.Property(x => x.PasswordAcss).HasMaxLength(300);
        builder.Property(x => x.UserProxy).HasMaxLength(100);
        builder.Property(x => x.PasswordProxy).HasMaxLength(300);
        builder.Property(x => x.DominioProxy).HasMaxLength(100);

        builder.Property(x => x.UrlAcssRsp).HasMaxLength(500);
        builder.Property(x => x.LoginAcssRsp).HasMaxLength(100);
        builder.Property(x => x.PasswordAcssRsp).HasMaxLength(300);
        builder.Property(x => x.UserProxyRsp).HasMaxLength(100);
        builder.Property(x => x.PasswordProxyRsp).HasMaxLength(300);
        builder.Property(x => x.DominioProxyRsp).HasMaxLength(100);

        builder.Property(x => x.ProxyAutenticacao).HasMaxLength(500);
        builder.Property(x => x.TokenAutenticacao).HasMaxLength(500);
        builder.Property(x => x.LoginAutenticacao).HasMaxLength(100);
        builder.Property(x => x.PasswordAutenticacao).HasMaxLength(300);
    }
}