using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs
{
    public class AntecedentesFamiliaresUtenteLightDTO : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid? DoencaId { get; set; }
        public string? NomeDoenca { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public DateTime? Data { get; set; }
        public Guid GrauParentescoId { get; set; }
        public string? GrauParentesco { get; set; }
    }
}