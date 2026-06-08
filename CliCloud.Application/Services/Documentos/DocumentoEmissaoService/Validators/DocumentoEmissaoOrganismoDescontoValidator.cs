#nullable enable

using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Validators;

/// <summary>
/// Legado: <c>TFatura.GuardarOrUpdate</c> — organismos ADM / SADGNR / SADPSP sem descontos
/// em faturas emitidas directamente ao organismo (<c>c_utente is null</c>).
/// </summary>
public static class DocumentoEmissaoOrganismoDescontoValidator
{
    public const string MensagemLegado =
        "Este organismo não pode ter descontos. O valor deve ser o que está na tabela de subsistemas de serviços";

    public static bool EhFaturacaoAOrganismo(EmitirDocumentoRequest request) =>
        request.OrganismoId.HasValue
        && request.OrganismoId.Value != Guid.Empty
        && !request.UtenteId.HasValue;

    public static bool OrganismoRestringeDescontos(Organismo organismo) =>
        organismo.ADM || organismo.SADGNR || organismo.SADPSP;

    public static string? Validar(
        Organismo? organismo,
        EmitirDocumentoRequest request,
        decimal totalDescontoDocumento)
    {
        if (!EhFaturacaoAOrganismo(request))
            return null;

        if (organismo is null)
            return "Organismo não encontrado.";

        if (!OrganismoRestringeDescontos(organismo))
            return null;

        if (totalDescontoDocumento > 0m)
            return MensagemLegado;

        return null;
    }
}
