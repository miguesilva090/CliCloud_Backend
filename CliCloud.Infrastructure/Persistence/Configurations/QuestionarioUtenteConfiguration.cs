using CliCloud.Domain.Entities.ProcessoClinico;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class QuestionarioUtenteConfiguration : IEntityTypeConfiguration<QuestionarioUtente>
  {
    public void Configure(EntityTypeBuilder<QuestionarioUtente> builder)
    {
      builder.ToTable("QuestionariosUtente", "ProcessoClinico");
    }
  }
}

