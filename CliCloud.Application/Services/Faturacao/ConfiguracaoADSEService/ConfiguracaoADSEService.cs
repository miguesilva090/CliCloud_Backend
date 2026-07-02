using AutoMapper;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ConfiguracaoADSE;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Filters;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService
{
  public class ConfiguracaoADSEService : IConfiguracaoADSEService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ConfiguracaoADSEService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<ConfiguracaoADSEDTO>> ObterConfiguracaoPorEmpresaAsync(Guid empresaId)
    {
      try
      {
        ConfiguracaoADSEPorEmpresaSpec spec = new(empresaId);
        ConfiguracaoADSE? entity = (await _repository.GetListAsync<ConfiguracaoADSE, Guid>(spec)).FirstOrDefault();

        if (entity == null)
        {
          return ResponseFactory.Success(new ConfiguracaoADSEDTO
          {
            Id = Guid.Empty,
            EmpresaId = empresaId
          });
        }

        return ResponseFactory.Success(_mapper.Map<ConfiguracaoADSEDTO>(entity));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoADSEDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid empresaId, GuardarConfiguracaoADSERequest request)
    {
      try
      {
        ConfiguracaoADSEPorEmpresaSpec spec = new(empresaId);
        ConfiguracaoADSE entity = (await _repository.GetListAsync<ConfiguracaoADSE, Guid>(spec)).FirstOrDefault()
          ?? new ConfiguracaoADSE { EmpresaId = empresaId };

        if (entity.DeletedOn != null)
        {
          entity.DeletedOn = null;
          entity.DeletedBy = null;
        }

        ApplyRequest(entity, request, empresaId);

        if (entity.Id == Guid.Empty)
        {
          entity.Id = Guid.NewGuid();
          _ = await _repository.CreateAsync<ConfiguracaoADSE, Guid>(entity);
        }
        else
        {
          _ = await _repository.UpdateAsync<ConfiguracaoADSE, Guid>(entity);
        }

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (DbUpdateException ex)
      {
        string message = ex.InnerException?.Message ?? ex.Message;
        if (message.Contains("IX_ConfiguracaoADSE_EmpresaId", StringComparison.OrdinalIgnoreCase))
          return ResponseFactory.Fail<Guid>("Já existe configuração ADSE para esta empresa.");

        return ResponseFactory.Fail<Guid>(message);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<ConfiguracaoADSEDTO>>> GetConfiguracaoADSEAsync(string keyword = "")
    {
      ConfiguracaoADSESearchList specification = new(keyword);
      IEnumerable<ConfiguracaoADSEDTO> list = await _repository.GetListAsync<ConfiguracaoADSE, ConfiguracaoADSEDTO, Guid>(specification);
      return ResponseFactory.Success<IEnumerable<ConfiguracaoADSEDTO>>(list);
    }

    public async Task<PaginatedResponse<ConfiguracaoADSEDTO>> GetConfiguracaoADSEPaginatedAsync(ConfiguracaoADSETableFilter filter)
    {
      if (!string.IsNullOrEmpty(filter.Keyword))
      {
        filter.PageNumber = 1;
      }

      string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      ConfiguracaoADSESearchTable specification = new(filter?.Keyword ?? "", dynamicOrder, filter?.EmpresaId);
      PaginatedResponse<ConfiguracaoADSEDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<ConfiguracaoADSE, ConfiguracaoADSEDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
      return pagedResponse;
    }

    public async Task<Response<ConfiguracaoADSEDTO>> GetConfiguracaoADSEAsync(Guid id)
    {
      try
      {
        ConfiguracaoADSEDTO dto = await _repository.GetByIdAsync<ConfiguracaoADSE, ConfiguracaoADSEDTO, Guid>(id);
        return ResponseFactory.Success<ConfiguracaoADSEDTO>(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoADSEDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateConfiguracaoADSEAsync(CreateConfiguracaoADSERequest request)
    {
      try
      {
        ConfiguracaoADSEPorEmpresaSpec spec = new(request.EmpresaId);
        ConfiguracaoADSE? existing = (await _repository.GetListAsync<ConfiguracaoADSE, Guid>(spec)).FirstOrDefault();
        if (existing != null)
          return ResponseFactory.Fail<Guid>("Já existe configuração ADSE para esta empresa.");

        ConfiguracaoADSE entity = _mapper.Map<ConfiguracaoADSE>(request);
        entity.Id = Guid.NewGuid();
        _ = await _repository.CreateAsync<ConfiguracaoADSE, Guid>(entity);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateConfiguracaoADSEAsync(UpdateConfiguracaoADSERequest request, Guid id)
    {
      try
      {
        if (id == Guid.Empty)
        {
          return await GuardarConfiguracaoAsync(request.EmpresaId, ToGuardarRequest(request));
        }

        ConfiguracaoADSE entity;
        try
        {
          entity = await _repository.GetByIdAsync<ConfiguracaoADSE, Guid>(id);
        }
        catch (InvalidOperationException)
        {
          return await GuardarConfiguracaoAsync(request.EmpresaId, ToGuardarRequest(request));
        }

        ApplyRequest(entity, request, request.EmpresaId);
        _ = await _repository.UpdateAsync<ConfiguracaoADSE, Guid>(entity);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteConfiguracaoADSEAsync(Guid id)
    {
      try
      {
        ConfiguracaoADSE? entity = await _repository.RemoveByIdAsync<ConfiguracaoADSE, Guid>(id);
        if (entity == null)
          return ResponseFactory.Fail<Guid>("ConfiguracaoADSE não encontrada");
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void ApplyRequest(ConfiguracaoADSE entity, GuardarConfiguracaoADSERequest request, Guid empresaId)
    {
      entity.EmpresaId = empresaId;
      entity.UrlADSE = request.UrlADSE?.Trim();
      entity.Dominio = request.Dominio?.Trim();
      entity.Utilizador = request.Utilizador?.Trim();
      if (!string.IsNullOrWhiteSpace(request.Password))
        entity.Password = request.Password;
      entity.OrganismoId = request.OrganismoId;
      entity.NumeroLocal = request.NumeroLocal;
      if (!string.IsNullOrWhiteSpace(request.PasswordLocal))
        entity.PasswordLocal = request.PasswordLocal;
      entity.UrlPasta = request.UrlPasta?.Trim();
    }

    private static void ApplyRequest(ConfiguracaoADSE entity, UpdateConfiguracaoADSERequest request, Guid empresaId)
    {
      ApplyRequest(entity, ToGuardarRequest(request), empresaId);
    }

    private static GuardarConfiguracaoADSERequest ToGuardarRequest(UpdateConfiguracaoADSERequest request) => new()
    {
      UrlADSE = request.UrlADSE,
      Dominio = request.Dominio,
      Utilizador = request.Utilizador,
      Password = request.Password,
      OrganismoId = request.OrganismoId,
      NumeroLocal = request.NumeroLocal,
      PasswordLocal = request.PasswordLocal,
      UrlPasta = request.UrlPasta,
    };
  }
}
