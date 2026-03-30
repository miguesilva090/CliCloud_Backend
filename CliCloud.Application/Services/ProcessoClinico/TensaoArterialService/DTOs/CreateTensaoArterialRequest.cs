using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs
{
    public class CreateTensaoArterialRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public int TensaoSistolica { get; set; }
        public int TensaoDiastolica { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateTensaoArterialValidator : AbstractValidator<CreateTensaoArterialRequest>
    {
        public CreateTensaoArterialValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
            _ = RuleFor(x => x.TensaoSistolica).NotEmpty();
            _ = RuleFor(x => x.TensaoDiastolica).NotEmpty();
        }
    }
}
