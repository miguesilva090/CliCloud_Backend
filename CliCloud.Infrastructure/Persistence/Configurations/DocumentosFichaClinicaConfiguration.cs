using CliCloud.Domain.Entities.ProcessoClinico.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class DocumentosFichaClinicaConfiguration : IEntityTypeConfiguration<DocumentosFichaClinica> 
    {
        public void Configure(EntityTypeBuilder<DocumentosFichaClinica> builder)
        {
            builder.ToTable("DocumentosFichaClinica", "ProcessoClinico");

            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Utente)
                .WithMany()
                .HasForeignKey(d => d.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(d => d.Categoria)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(d => d.Tipo)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(d => d.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(d => d.NomeFicheiro)
                .HasMaxLength(260)
                .IsRequired();

            builder.Property(d => d.CaminhoRelativo)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(d => d.Terminacao)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(d => d.IsVideo)
                .IsRequired();

            builder.Property(d => d.UploadedByUserId)
                .IsRequired(false);
        }
    }
}