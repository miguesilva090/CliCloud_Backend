using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Core.ClinicaService.DTOs;
using CliCloud.Application.Services.Core.ClinicaService.Filters;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Infrastructure.Encryption;
using CliCloud.Domain.Entities.Core.Tratamentos;

namespace CliCloud.Application.Services.Core.ClinicaService
{
  public class ClinicaService : IClinicaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;

    public ClinicaService(
      IRepositoryAsync repository,
      IMapper mapper,
      IEncryptionService encryptionService
    )
    {
      _repository = repository;
      _mapper = mapper;
      _encryptionService = encryptionService;
    }

    private string? EncryptIfPlain(string? value)
    {
      if (string.IsNullOrWhiteSpace(value)) return value;

      try
      {
        _ = _encryptionService.DecryptString(value);
        return value;
      }
      catch
      {
        return _encryptionService.EncryptString(value);
      }
    }

    private string? DecryptIfEncrypted(string? value)
    {
      if (string.IsNullOrWhiteSpace(value)) return value;

      try
      {
        return _encryptionService.DecryptString(value);
      }
      catch
      {
        return value;
      }
    }

    private void DecryptDtoSecrets(ClinicaDTO dto)
    {
      dto.AtUser = DecryptIfEncrypted(dto.AtUser);
      dto.AtPass = DecryptIfEncrypted(dto.AtPass);
    }

    public async Task<Response<IEnumerable<ClinicaDTO>>> GetClinicaAsync(string keyword = "")
    {
      var spec = new ClinicaSearchList(keyword);
      var list = await _repository.GetListAsync<Clinica, ClinicaDTO, Guid>(spec);
      foreach (var dto in list)
      {
        DecryptDtoSecrets(dto);
      }
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ClinicaLightDTO>>> GetClinicaLightAsync(string keyword = "")
    {
      var spec = new ClinicaSearchList(keyword);
      var list = await _repository.GetListAsync<Clinica, ClinicaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ClinicaTableDTO>> GetClinicaPaginatedAsync(ClinicaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ClinicaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Clinica, ClinicaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ClinicaTableDTO>>> GetAllClinicaAsync(ClinicaAllFilter filter)
    {
      try
      {
        filter ??= new ClinicaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ClinicaSearchTable(filters, order);
        var list = await _repository.GetListAsync<Clinica, ClinicaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<ClinicaTableDTO>>(ex.Message); }
    }

    public async Task<Response<ClinicaDTO>> GetClinicaAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (dto != null)
        {
          DecryptDtoSecrets(dto);
          dto.ConfiguracaoTratamentos = await GetConfiguracaoTratamentosOrNullAsync(id);
        }
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<ClinicaDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateClinicaAsync(CreateClinicaRequest request)
    {
      var spec = new ClinicaMatchNome(request.Nome);
      if (await _repository.ExistsAsync<Clinica, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Clínica com este nome já existe.");
      var entity = _mapper.Map<Clinica>(request);
      entity.TipoEntidade = EntidadeTipo.Clinica;

      // Legacy: ATCUDUser/ATCUDPass são persistidos encriptados.
      entity.AtUser = EncryptIfPlain(entity.AtUser);
      entity.AtPass = EncryptIfPlain(entity.AtPass);

      try
      {
        var created = await _repository.CreateAsync<Clinica, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateClinicaAsync(UpdateClinicaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Clinica, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Clínica não encontrada.");
      if (existing.Nome != request.Nome)
      {
        var spec = new ClinicaMatchNome(request.Nome);
        if (await _repository.ExistsAsync<Clinica, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe uma clínica com este nome.");
      }
      _mapper.Map(request, existing);
      existing.TipoEntidade = EntidadeTipo.Clinica;

      // Legacy: ATCUDUser/ATCUDPass são persistidos encriptados.
      existing.AtUser = EncryptIfPlain(existing.AtUser);
      existing.AtPass = EncryptIfPlain(existing.AtPass);

      try
      {
        var updated = await _repository.UpdateAsync<Clinica, Guid>(existing);
        _ = await _repository.SaveChangesAsync();

        if (request.GravarConfiguracaoTratamentos == true)
        {
          var tratamentosRequest = new AtualizarConfiguracaoTratamentosRequest
          {
            TipoSrvTratamentos = request.TipoSrvTratamentos,
            AreaPrestacaoDefeitoAreaZ = request.AreaPrestacaoDefeitoAreaZ,
            ControlarAparelhos = request.ControlarAparelhos,

            Segundos = request.Segundos,
            FaltasMax = request.FaltasMax,
            FaltasConsecutivasMax = request.FaltasConsecutivasMax,
            Taxamoderadora = request.Taxamoderadora,
            CredencialExternaAdse = request.CredencialExternaAdse,

            TipoPagamento = request.TipoPagamento,
            AvisoInqueritoSessoesDiarias = request.AvisoInqueritoSessoesDiarias,
          };

          var tratamentosRes =
            await GuardarConfiguracaoTratamentosAsync(id, tratamentosRequest);

          if (tratamentosRes.Status != ResponseStatus.Success)
          {
            var msg =
              tratamentosRes.Messages.TryGetValue("$", out var messages) &&
              messages is { Count: > 0 }
                ? messages.First()
                : "Falha ao guardar configurações de tratamentos.";
            return ResponseFactory.Fail<Guid>(msg);
          }
        }

        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    private async Task<ConfiguracaoTratamentosDTO?> GetConfiguracaoTratamentosOrNullAsync(
      Guid clinicaId
    )
    {
      var spec = new ConfiguracaoTratamentosPorClinicaSpec(clinicaId);
      var lista = await _repository.GetListAsync<ConfiguracaoTratamentos, Guid>(spec);
      var entidade = lista.FirstOrDefault();
      if (entidade == null) return null;

      return new ConfiguracaoTratamentosDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        TipoSrvTratamentos = entidade.TipoSrvTratamentos,
        AreaPrestacaoDefeitoAreaZ = entidade.AreaPrestacaoDefeitoAreaZ,
        ControlarAparelhos = entidade.ControlarAparelhos,
        Segundos = entidade.Segundos,
        FaltasMax = entidade.FaltasMax,
        FaltasConsecutivasMax = entidade.FaltasConsecutivasMax,
        Taxamoderadora = entidade.Taxamoderadora,
        CredencialExternaAdse = entidade.CredencialExternaAdse,
        TipoPagamento = entidade.TipoPagamento,
        AvisoInqueritoSessoesDiarias = entidade.AvisoInqueritoSessoesDiarias
      };
    }

    private async Task<Response<Guid>> GuardarConfiguracaoTratamentosAsync(
      Guid clinicaId,
      AtualizarConfiguracaoTratamentosRequest request
    )
    {
      try
      {
        var spec = new ConfiguracaoTratamentosPorClinicaSpec(clinicaId);
        var lista = await _repository.GetListAsync<ConfiguracaoTratamentos, Guid>(spec);
        var entidade = lista.FirstOrDefault();

        if (entidade == null)
        {
          entidade = new ConfiguracaoTratamentos
          {
            ClinicaId = clinicaId,
          };

          Aplicar(entidade, request);
          var criado = await _repository.CreateAsync<ConfiguracaoTratamentos, Guid>(entidade);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(criado.Id);
        }

        Aplicar(entidade, request);
        var atualizado = await _repository.UpdateAsync<ConfiguracaoTratamentos, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(atualizado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void Aplicar(
      ConfiguracaoTratamentos entidade,
      AtualizarConfiguracaoTratamentosRequest request
    )
    {
      entidade.TipoSrvTratamentos = request.TipoSrvTratamentos;
      entidade.AreaPrestacaoDefeitoAreaZ = request.AreaPrestacaoDefeitoAreaZ;
      entidade.ControlarAparelhos = request.ControlarAparelhos;

      entidade.Segundos = request.Segundos;
      entidade.FaltasMax = request.FaltasMax;
      entidade.FaltasConsecutivasMax = request.FaltasConsecutivasMax;
      entidade.Taxamoderadora = request.Taxamoderadora;
      entidade.CredencialExternaAdse = request.CredencialExternaAdse;

      entidade.TipoPagamento = request.TipoPagamento;
      entidade.AvisoInqueritoSessoesDiarias = request.AvisoInqueritoSessoesDiarias;
    }

    public async Task<Response<Guid>> DeleteClinicaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Clinica, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleClinicaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Clinica, Guid>(id);
          if (e == null) { fail.Add($"Clínica {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Clinica, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch { fail.Add($"Clínica {id}."); _repository.ClearChangeTracker(); }
      }
      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    public async Task<Response<Guid>> SetDefaultClinicaAsync(Guid id, bool porDefeito)
    {
      try
      {
        var existing = await _repository.GetByIdAsync<Clinica, Guid>(id);
        if (existing == null) return ResponseFactory.Fail<Guid>("Clínica não encontrada.");

        // Legacy behavior: limpar todos e, se ativo, marcar apenas a escolhida como pordefeito.
        await _repository.ExecuteSqlRawAsync(
          "UPDATE [Core].[Clinica] SET PorDefeito = 0 WHERE DeletedOn IS NULL"
        );

        if (porDefeito)
        {
          await _repository.ExecuteSqlRawAsync(
            "UPDATE [Core].[Clinica] SET PorDefeito = 1 WHERE Id = {0} AND DeletedOn IS NULL",
            id
          );
        }

        return ResponseFactory.Success(id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<AvisosClinicaLegacyDTO>> GetAvisosClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<AvisosClinicaLegacyDTO>("Clínica não encontrada.");

        return ResponseFactory.Success(new AvisosClinicaLegacyDTO
        {
          MsgFaltaPagamento = clinica.MsgFaltaPagamento,
          MsgCredenciais = clinica.MsgCredenciais
        });
      }
      catch (Exception ex) { return ResponseFactory.Fail<AvisosClinicaLegacyDTO>(ex.Message); }
    }

    public async Task<Response<int[]>> GetFolgasClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<int[]>("Clínica não encontrada.");

        var folgas = new List<int>(7);
        if (clinica.FolgaSeg == true) folgas.Add(1);
        if (clinica.FolgaTer == true) folgas.Add(2);
        if (clinica.FolgaQua == true) folgas.Add(3);
        if (clinica.FolgaQui == true) folgas.Add(4);
        if (clinica.FolgaSex == true) folgas.Add(5);
        if (clinica.FolgaSab == true) folgas.Add(6);
        if (clinica.FolgaDom == true) folgas.Add(7);

        return ResponseFactory.Success(folgas.ToArray());
      }
      catch (Exception ex) { return ResponseFactory.Fail<int[]>(ex.Message); }
    }

    public async Task<Response<int?>> GetPortaCartaoClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<int?>("Clínica não encontrada.");

        return ResponseFactory.Success(clinica.PortaLeitorCartoes);
      }
      catch (Exception ex) { return ResponseFactory.Fail<int?>(ex.Message); }
    }

    public async Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasAutocompleteAsync(string? q)
    {
      try
      {
        var spec = new ClinicaAutocompleteSearchList(q);
        var list = await _repository.GetListAsync<Clinica, Guid>(spec);

        var result = list
          .Where(c => !string.IsNullOrWhiteSpace(c.Nome))
          .Select(c => new AutoCompleteItemDTO
          {
            Value = c.Id.ToString(),
            Label = c.Nome.Trim()
          })
          .ToList();

        return ResponseFactory.Success<IEnumerable<AutoCompleteItemDTO>>(result);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AutoCompleteItemDTO>>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasSelectedAutocompleteAsync(string? q, Guid currentClinicaId)
    {
      try
      {
        var spec = new ClinicaAutocompleteSearchList(q);
        var list = await _repository.GetListAsync<Clinica, Guid>(spec);

        var result = list
          .Where(c => !string.IsNullOrWhiteSpace(c.Nome))
          .Select(c => new AutoCompleteItemDTO
          {
            Value = c.Id.ToString(),
            Label = c.Nome.Trim()
          })
          .ToList();

        var current = await _repository.GetByIdAsync<Clinica, Guid>(currentClinicaId);
        if (current != null && !string.IsNullOrWhiteSpace(current.Nome))
        {
          var curValue = current.Id.ToString();
          if (!result.Any(x => string.Equals(x.Value, curValue, StringComparison.OrdinalIgnoreCase)))
          {
            result.Insert(0, new AutoCompleteItemDTO { Value = curValue, Label = current.Nome.Trim() });
          }
        }

        if (result.Count > 10) result = result.Take(10).ToList();

        return ResponseFactory.Success<IEnumerable<AutoCompleteItemDTO>>(result);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AutoCompleteItemDTO>>(ex.Message);
      }
    }
  }
}
