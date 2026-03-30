using CliCloud.Domain.Entities.ProcessoClinico;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class HabitosEViciosConfiguration : IEntityTypeConfiguration<HabitosEVicios>
    {
        public void Configure(EntityTypeBuilder<HabitosEVicios> builder)
        {
            builder.ToTable("HabitosEVicios", "ProcessoClinico");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Utente)
                .WithMany()
                .HasForeignKey(x => x.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.QuantidadeAgua).HasMaxLength(100);
            builder.Property(x => x.BebidasAlcoolicas).HasMaxLength(200);
            builder.Property(x => x.QuantidadeAlcool).HasMaxLength(100);
            builder.Property(x => x.QuantosFumaDia).HasMaxLength(50);
            builder.Property(x => x.Drogas).HasMaxLength(200);
            builder.Property(x => x.OutrosVicios).HasMaxLength(200);
            builder.Property(x => x.FarmacosSemReceita).HasMaxLength(200);
            builder.Property(x => x.TipoExercicioFisico).HasMaxLength(200);
            builder.Property(x => x.FrequenciaExFisico).HasMaxLength(100);

            builder.Property(x => x.ObservacoesHabitosAlimentaresEVicios).HasMaxLength(1000);
            builder.Property(x => x.ObservacoesHabitosMedicamentosExercicioFisico).HasMaxLength(1000);
        }
    }
}