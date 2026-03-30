using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs
{
    public class CreateEntidadeFinanceiraRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public string? RuaId { get; set; }
        public string? CodigoPostalId { get; set; }
        public string? FreguesiaId { get; set; }
        public string? ConcelhoId { get; set; }
        public string? DistritoId { get; set; }
        public string? PaisId { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public int? Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<CreateEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadeFinanceira
        public string? Abreviatura { get; set; }
        public required string PaisPrefixo { get; set; }
        public required string TipoEntidadeFinanceiraId { get; set; }
        public CondicaoSns? CondicaoSns { get; set; }
    }

    public class CreateEntidadeFinanceiraValidator : AbstractValidator<CreateEntidadeFinanceiraRequest>
    {
        public CreateEntidadeFinanceiraValidator()
        {
            _ = RuleFor(x => x.Nome)
              .NotEmpty()
              .WithMessage("O campo Nome é obrigatório.");

            // Restantes campos (incluindo TipoEntidadeId, PaisPrefixo, TipoEntidadeFinanceiraId, Email, UrlFoto, etc.)
            // ficam opcionais neste fluxo simplificado.
            //
            // Se no futuro for necessário reforçar estas validações para outros casos de uso,
            // podemos voltar a adicionar regras específicas aqui.
        }
    }
}
