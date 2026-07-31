using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
    public class UtenteSearchTable : Specification<Utente>
    {
        public UtenteSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.Utente);

          _ = Query
            .Include(x => x.Rua)
              .ThenInclude(x => x.Freguesia)
              .ThenInclude(x => x.Concelho)
              .ThenInclude(x => x.Distrito)
              .ThenInclude(x => x.Pais)
            .Include(x => x.EstadoCivil)
            .Include(x => x.GrupoSanguineo)
            .Include(x => x.ProvenienciaUtente)
            .Include(x => x.Habilitacao)
            .Include(x => x.Profissao)
            .Include(x => x.Sexo)
            .Include(x => x.EntidadeContactos);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Nome.Contains(filter.Value));
                    }
                    break;
                  case "numeroContribuinte":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinte.Contains(filter.Value));
                    }
                    break;
                  case "rua.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Nome.Contains(filter.Value));
                    }
                    break;
                  case "rua.freguesia.nome":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Rua != null && x.Rua.Freguesia != null && x.Rua.Freguesia.Nome.Contains(filter.Value));
                    }
                    break;
                  case "entidadetipoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int entidadeTipoId))
                    {
                      if(entidadeTipoId > 0 && entidadeTipoId <= 9)
                      {
                        _ = Query.Where(x => x.TipoEntidade == (EntidadeTipo)entidadeTipoId);
                      }
                    }
                    break;
                  case "dataNascimento":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && DateOnly.TryParse(filter.Value, out DateOnly dataNascimento))
                    {
                      _ = Query.Where(x => x.DataNascimento.HasValue && x.DataNascimento.Value == dataNascimento);
                    }
                    break;
                  case "sexo":
                  case "sexoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid sexoId))
                    {
                      _ = Query.Where(x => x.SexoId == sexoId);
                    }
                    break;
                  case "estadocivilid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid estadoCivilId))
                    {
                      _ = Query.Where(x => x.EstadoCivilId == estadoCivilId);
                    }
                    break;
                  case "gruposanguineoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid grupoSanguineoId))
                    {
                      _ = Query.Where(x => x.GrupoSanguineoId == grupoSanguineoId);
                    }
                    break;
                  case "profissaoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid profissaoId))
                    {
                      _ = Query.Where(x => x.ProfissaoId == profissaoId);
                    }
                    break;
                  case "numeroCartaoIdentificacao":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroCartaoIdentificacao != null && x.NumeroCartaoIdentificacao.Contains(filter.Value));
                    }
                    break;
                  case "numeroUtente":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroUtente != null && x.NumeroUtente.Contains(filter.Value));
                    }
                    break;
                  case "numeroSegurancaSocial":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroSegurancaSocial != null && x.NumeroSegurancaSocial.Contains(filter.Value));
                    }
                    break;
                  case "desistencia":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool desistencia))
                    {
                      _ = Query.Where(x => x.Desistencia == desistencia);
                    }
                    break;
                  case "cronico":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool cronico))
                    {
                      _ = Query.Where(x => x.Cronico == cronico);
                    }
                    break;
                  case "tipoConsulta":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<TipoConsulta>(filter.Value, true, out TipoConsulta tipoConsulta))
                    {
                      _ = Query.Where(x => x.TipoConsulta == tipoConsulta);
                    }
                    break;
                  default:
                    break;
                }
              }
            }

            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
