using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs
{
    public class UpsertFichaClinicaSecaoConteudoLoteRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid SeparadorId { get; set; }
        public List<UpsertFichaClinicaSecaoConteudoLoteItemRequest> Itens { get; set; } = [];
    }

    public class UpsertFichaClinicaSecaoConteudoLoteItemRequest
    {
        public Guid CampoId { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class UpsertFichaClinicaSecaoConteudoLoteRequestValidator : AbstractValidator<UpsertFichaClinicaSecaoConteudoLoteRequest>
    {
        public UpsertFichaClinicaSecaoConteudoLoteRequestValidator()
        {
            RuleFor(x => x.UtenteId).NotEmpty();
            RuleFor(x => x.SeparadorId).NotEmpty();
            RuleFor(x => x.Itens).NotNull();

            RuleForEach(x => x.Itens).ChildRules(item => 
            {
                item.RuleFor(i => i.CampoId).NotEmpty();
                item.RuleFor(i => i.Texto).NotNull();
            });
        }
    }
}