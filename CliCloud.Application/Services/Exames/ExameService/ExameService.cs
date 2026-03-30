using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Application.Services.Exames.ExameService.DTOs;
using CliCloud.Application.Services.Exames.ExameService.Filters;
using CliCloud.Application.Services.Exames.ExameService.Specifications;

namespace CliCloud.Application.Services.Exames.ExameService
{
  public class ExameService : IExameService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ExameService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ExameDTO>>> GetExameAsync(string keyword = "")
    {
      var spec = new ExameSearchList(keyword);
      var list = await _repository.GetListAsync<Exame, ExameDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ExameLightDTO>>> GetExameLightAsync(string keyword = "")
    {
      var spec = new ExameSearchList(keyword);
      var list = await _repository.GetListAsync<Exame, ExameLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ExameTableDTO>> GetExamePaginatedAsync(ExameTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ExameSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Exame, ExameTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ExameTableDTO>>> GetAllExameAsync(ExameAllFilter? filter)
    {
      try
      {
        filter ??= new ExameAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ExameSearchTable(filters, order);
        var list = await _repository.GetListAsync<Exame, ExameTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<ExameTableDTO>>(ex.Message); }
    }

    public async Task<Response<ExameDTO>> GetExameAsync(Guid id)
    {
      try
      {
        var spec = new ExameByIdWithLinhas(id);
        var entity = await _repository.GetByIdAsync<Exame, Guid>(id, spec);
        var dto = _mapper.Map<ExameDTO>(entity);
        return ResponseFactory.Success(dto);
      }
      catch (InvalidOperationException) { return ResponseFactory.Fail<ExameDTO>("Prescrição não encontrada."); }
      catch (Exception ex) { return ResponseFactory.Fail<ExameDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateExameAsync(CreateExameRequest request)
    {
      if (!string.IsNullOrWhiteSpace(request.NumeroPrescricao))
      {
        var specNumero = new ExameMatchNumeroPrescricao(request.NumeroPrescricao);
        if (await _repository.ExistsAsync<Exame, Guid>(specNumero))
          return ResponseFactory.Fail<Guid>("Já existe uma prescrição com este Nº Prescrição.");
      }
      var entity = _mapper.Map<Exame>(request);
      try
      {
        var created = await _repository.CreateAsync<Exame, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateExameAsync(UpdateExameRequest request, Guid id)
    {
      Exame existing;
      try
      {
        var spec = new ExameByIdWithLinhas(id);
        existing = await _repository.GetByIdAsync<Exame, Guid>(id, spec);
      }
      catch (InvalidOperationException)
      {
        return ResponseFactory.Fail<Guid>("Prescrição de exame não encontrada.");
      }
      if (!string.IsNullOrWhiteSpace(request.NumeroPrescricao) && request.NumeroPrescricao != existing.NumeroPrescricao)
      {
        var specNumero = new ExameMatchNumeroPrescricao(request.NumeroPrescricao, id);
        if (await _repository.ExistsAsync<Exame, Guid>(specNumero))
          return ResponseFactory.Fail<Guid>("Já existe outra prescrição com este Nº Prescrição.");
      }
      _mapper.Map(request, existing);
      existing.Linhas.Clear();
      foreach (var req in request.Linhas)
        existing.Linhas.Add(new ExameLinha { ExameId = id, TipoExameId = req.TipoExameId, Quantidade = req.Quantidade, Recomendacoes = req.Recomendacoes });
      try
      {
        var updated = await _repository.UpdateAsync<Exame, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> DeleteExameAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Exame, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleExameAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Exame, Guid>(id);
          if (e == null) { fail.Add($"Prescrição {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Exame, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch { fail.Add($"Prescrição {id}."); _repository.ClearChangeTracker(); }
      }
      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    public async Task<Response<ExamePrescricaoReportDTO>> GetExameReportAsync(Guid exameId)
    {
      try
      {
        var spec = new ExameByIdWithLinhas(exameId);
        var exame = await _repository.GetByIdAsync<Exame, Guid>(exameId, spec);
        if (exame == null)
        {
          return ResponseFactory.Fail<ExamePrescricaoReportDTO>("Prescrição de exame não encontrada.");
        }

        var dto = new ExamePrescricaoReportDTO
        {
          Id = exame.Id,
          NumeroPrescricao = exame.NumeroPrescricao ?? string.Empty,
          DataPrescricao = exame.DataPrescricao,
          UtenteNome = exame.Utente?.Nome ?? string.Empty,
          UtenteNumero = exame.Utente?.NumeroUtente,
          OrganismoNome = exame.Organismo?.Nome,
          MedicoNome = exame.Medico?.Nome,
          Linhas = exame.Linhas
            .OrderBy(l => l.CreatedOn)
            .Select(l => new ExamePrescricaoReportLinhaDTO
            {
              Codigo = l.TipoExame?.EAN ?? string.Empty,
              Designacao = l.TipoExame?.Designacao ?? string.Empty,
              Quantidade = l.Quantidade,
            })
            .ToList(),
        };

        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ExamePrescricaoReportDTO>(ex.Message);
      }
    }

    /// <summary>
    /// Resultado de Exames por prescrição (ExameId).
    /// Devolve um registo por linha da prescrição (ExameLinha),
    /// enriquecido com alguns dados do TipoExame e com os campos de resultado.
    /// </summary>
    public async Task<Response<IEnumerable<ResultadoExameTableDTO>>> GetResultadosByExameAsync(Guid exameId)
    {
      try
      {
        var spec = new ExameByIdWithLinhas(exameId);
        var exame = await _repository.GetByIdAsync<Exame, Guid>(exameId, spec);
        if (exame == null)
        {
          return ResponseFactory.Fail<IEnumerable<ResultadoExameTableDTO>>("Prescrição de exame não encontrada.");
        }

        var list = exame.Linhas.Select(l => new ResultadoExameTableDTO
        {
          Id = l.Id,
          ExameId = exame.Id,
          TipoExameId = l.TipoExameId,
          NomeExame = l.TipoExame != null ? l.TipoExame.Designacao : null,
          Quantidade = l.Quantidade,
          Valor = l.ResultadoValor,
          Referencia = l.ResultadoReferencia,
          Obs = l.ResultadoObs,
        }).ToList();

        return ResponseFactory.Success<IEnumerable<ResultadoExameTableDTO>>(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ResultadoExameTableDTO>>(ex.Message);
      }
    }

    /// <summary>
    /// Atualiza ou insere os dados de resultado (valor/referência/observações) de uma linha de exame.
    /// Isto corresponde ao comportamento do ecrã "Resultados de Exames" do legado.
    /// </summary>
    public async Task<Response<Guid>> UpsertResultadoLinhaAsync(Guid exameId, Guid linhaId, string? valor, string? referencia, string? obs)
    {
      try
      {
        var spec = new ExameByIdWithLinhas(exameId);
        var exame = await _repository.GetByIdAsync<Exame, Guid>(exameId, spec);
        if (exame == null)
        {
          return ResponseFactory.Fail<Guid>("Prescrição de exames não encontrada.");
        }

        var linha = exame.Linhas.FirstOrDefault(l => l.Id == linhaId);
        if (linha == null)
        {
          return ResponseFactory.Fail<Guid>("Linha de exame não encontrada.");
        }

        linha.ResultadoValor = string.IsNullOrWhiteSpace(valor) ? null : valor;
        linha.ResultadoReferencia = string.IsNullOrWhiteSpace(referencia) ? null : referencia;
        linha.ResultadoObs = string.IsNullOrWhiteSpace(obs) ? null : obs;

        _ = await _repository.UpdateAsync<Exame, Guid>(exame);
        _ = await _repository.SaveChangesAsync();

        return ResponseFactory.Success(linha.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}
