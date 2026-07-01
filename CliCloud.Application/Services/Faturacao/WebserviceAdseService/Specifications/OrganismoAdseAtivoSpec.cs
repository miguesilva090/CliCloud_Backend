using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.Specifications;

/// <summary>
/// Convenção alinhada ao FE (organismos): status 1 = Ativo; null também é considerado ativo.
/// O enum <see cref="Status.Ativo"/> (= 0) mantém-se por compatibilidade.
/// </summary>
internal static class OrganismoAdseAtivoSpec
{
    internal static bool EstaAtivo(Status? status) =>
        status is null || status == Status.Ativo || (int)status == 1;
}
