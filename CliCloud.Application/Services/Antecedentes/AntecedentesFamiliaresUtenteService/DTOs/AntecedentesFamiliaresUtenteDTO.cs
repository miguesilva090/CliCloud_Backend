using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs
{
    public class AntecedentesFamiliaresUtenteDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid? DoencaId { get; set; }
        public string? NomeDoenca { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public DateTime? Data { get; set; }
        public Guid GrauParentescoId { get; set; }
        public string? GrauParentesco { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}

