using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.Filters;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService
{
  public class SubsistemaServicoService : ISubsistemaServicoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public SubsistemaServicoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<SubsistemaServicoDTO>>> GetSubsistemaServicoAsync(Guid? servicoId = null)
    {
      var spec = new SubsistemaServicoSearchList(servicoId);
      var list = await _repository.GetListAsync<SubsistemaServico, SubsistemaServicoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<SubsistemaServicoDTO>> GetSubsistemaServicoPaginatedAsync(SubsistemaServicoTableFilter filter)
    {
      if (filter.ServicoId.HasValue || filter.OrganismoId.HasValue)
      {
        filter.PageNumber = 1;
      }

      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new SubsistemaServicoSearchTable(filter.ServicoId, filter.OrganismoId, order);
      return await _repository.GetPaginatedResultsAsync<SubsistemaServico, SubsistemaServicoDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<SubsistemaServicoDTO>> GetSubsistemaServicoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<SubsistemaServico, SubsistemaServicoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<SubsistemaServicoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateSubsistemaServicoAsync(CreateSubsistemaServicoRequest request)
    {
      var spec = new SubsistemaServicoMatchName(request.ServicoId, request.OrganismoId, request.SubsistemaId);
      if (await _repository.ExistsAsync<SubsistemaServico, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Já existe configuração para este serviço/organismo/subsistema.");

      var entity = _mapper.Map<SubsistemaServico>(request);

      try
      {
        var created = await _repository.CreateAsync<SubsistemaServico, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateSubsistemaServicoAsync(UpdateSubsistemaServicoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<SubsistemaServico, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Subsistema de Serviço não encontrado.");

      // Garantir unicidade se chave composta mudar
      if (existing.ServicoId != request.ServicoId ||
          existing.OrganismoId != request.OrganismoId ||
          existing.SubsistemaId != request.SubsistemaId)
      {
        var spec = new SubsistemaServicoMatchName(request.ServicoId, request.OrganismoId, request.SubsistemaId);
        if (await _repository.ExistsAsync<SubsistemaServico, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe configuração para este serviço/organismo/subsistema.");
      }

      _ = _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<SubsistemaServico, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteSubsistemaServicoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<SubsistemaServico, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}

