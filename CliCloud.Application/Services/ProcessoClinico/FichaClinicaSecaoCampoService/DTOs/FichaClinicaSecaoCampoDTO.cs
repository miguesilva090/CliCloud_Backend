using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs
{
    public class FichaClinicaSecaoCampoDTO : IDto
    {
        public Guid Id { get; set; }

        public Guid SeparadorId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string TipoCampo { get; set; } = string.Empty;

        public int NumeroLinhas { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; }

        public string SeparadorNome { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}

