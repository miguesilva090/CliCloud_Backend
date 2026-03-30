using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
    public class UtenteSubsistemaLinhaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid? OrganismoId { get; set; }
        public OrganismoLightDTO? Organismo { get; set; }
        public string? Designacao { get; set; }
        public string? NumeroBeneficiario { get; set; }
        public string? Sigla { get; set; }
        public string? NomeBeneficiario { get; set; }
        public DateOnly? DataCartao { get; set; }
        public string? NumeroApolice { get; set; }
        public Guid? EmpresaId { get; set; }
        public EmpresaLightDTO? Empresa { get; set; }
    }
}
