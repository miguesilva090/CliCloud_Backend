using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.Specifications
{
  public class ServicoConsultaSearchList : Specification<ServicoConsulta>
  {
    public ServicoConsultaSearchList(string? keyword = "", Guid? consultaId = null)
    {
      if (consultaId.HasValue)
      {
        _ = Query.Where(x => x.ConsultaId == consultaId.Value);
      }

      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.ConsultaId == g
            || x.ServicoId == g
            || x.ExameId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.CodigoArtigo != null && x.CodigoArtigo.Contains(keyword))
            || (x.NomeArtigo != null && x.NomeArtigo.Contains(keyword))
            || (x.Dente != null && x.Dente.Contains(keyword))
            || (x.NCheque != null && x.NCheque.Contains(keyword))
          );
        }
      }

      _ = Query.OrderBy(x => x.Linha);
    }
  }
}

