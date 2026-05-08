using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class UpdateSinistradoRequest : IDto 
    {
        public Guid UtenteId { get; set; }
        public Guid? EstadoSinistroId { get; set; }
        public DateTime? DataAcidente { get; set; }
        public DateTime? DataParticipacao { get; set; }
        public string? NumeroProcesso { get; set; }
        public string? Observacoes { get; set; }
        public string? Relatorio { get; set; }
        public List<SinistradoLinhaServicoDTO> LinhasServico { get; set; } = [];
    }

    public class UpdateSinistradoValidator : AbstractValidator<UpdateSinistradoRequest>
    {
        public UpdateSinistradoValidator()
        {
            RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}