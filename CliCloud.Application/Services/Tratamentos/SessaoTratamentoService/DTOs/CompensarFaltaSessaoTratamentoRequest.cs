using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs
{
    public class CompensarFaltaSessaoTratamentoRequest : IDto
    {
        public required string TratamentoId { get; set; }
        public DateTime? Data { get; set; }
        public string? HoraInic { get; set; }
        public string? Duracao { get; set; }

        public string? FisioterapeutaId { get; set; }
        public string? AuxiliarId { get; set; }
        public string? OutroTecnicoId { get; set; }

        public string? HoraFisio { get; set; }
        public string? HoraAux { get; set; }
        public string? HoraOutro { get; set; }
        public string? DuracaoFisio { get; set; }
        public string? DuracaoAux { get; set; }
        public string? DuracaoOutro { get; set; }
    }

    public class CompensarFaltaSessaoTratamentoValidator : AbstractValidator<CompensarFaltaSessaoTratamentoRequest>
    {
        public CompensarFaltaSessaoTratamentoValidator()
        {
            _ = RuleFor(x => x.TratamentoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("O Id do tratamento está inválido");
            _ = RuleFor(x => x.Data).NotNull().WithMessage("A data da sessão é obrigatória");
            _ = RuleFor(x => x.FisioterapeutaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
            _ = RuleFor(x => x.AuxiliarId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
            _ = RuleFor(x => x.OutroTecnicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
            _ = RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.FisioterapeutaId)
            || !string.IsNullOrWhiteSpace(x.AuxiliarId)
            || !string.IsNullOrWhiteSpace(x.OutroTecnicoId)).WithMessage("Selecione pelo menos um técnico");
        }
    }

}