using CliCloud.Domain.Entities.Funcionarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
  {
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Funcionario", "Funcionarios");
    }
  }
}
