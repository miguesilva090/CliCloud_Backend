using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.Specifications;

/// <summary>
/// Evita List.Contains(enum) → OPENJSON WITH (SQL Server / TPT Tecnico).
/// Filtra com OR de igualdades (valores conhecidos 1..3).
/// </summary>
public sealed class TecnicosAtivosByTiposSpec : Specification<Tecnico>
{
  public TecnicosAtivosByTiposSpec(IReadOnlyCollection<TipoTecnico> tipos)
  {
    bool incluiFisio = tipos.Contains(TipoTecnico.Fisioterapeuta);
    bool incluiAux = tipos.Contains(TipoTecnico.Auxiliar);
    bool incluiOutro = tipos.Contains(TipoTecnico.Outro);

    _ = Query
      .Where(x => x.DeletedOn == null)
      .Where(x =>
        (incluiFisio && x.TipoTecnico == TipoTecnico.Fisioterapeuta)
        || (incluiAux && x.TipoTecnico == TipoTecnico.Auxiliar)
        || (incluiOutro && x.TipoTecnico == TipoTecnico.Outro)
      )
      .OrderBy(x => x.Nome);
  }
}
