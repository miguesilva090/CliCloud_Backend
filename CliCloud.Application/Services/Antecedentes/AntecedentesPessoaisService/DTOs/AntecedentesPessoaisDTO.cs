using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs
{
    public class AntecedentesPessoaisDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid? DoencaId { get; set; }
        public string? NomeDoenca { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public DateTime? Data { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }

    }
}

