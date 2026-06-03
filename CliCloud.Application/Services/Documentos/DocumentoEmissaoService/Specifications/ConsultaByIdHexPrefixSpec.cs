using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

/// <summary>Consulta cujo Id (formato N, sem hífens) começa pelo prefixo (códigos CONS- truncados).</summary>
public sealed class ConsultaByIdHexPrefixSpec : Specification<Consulta>
{
    public ConsultaByIdHexPrefixSpec(string hexPrefix)
    {
        var prefix = hexPrefix.Replace("-", "", StringComparison.Ordinal);
        if (prefix.Length < 8)
        {
            Query.Where(_ => false);
            return;
        }

        Query.Where(c =>
            EF.Functions.Like(
                c.Id.ToString().Replace("-", ""),
                prefix + "%"));
    }
}
