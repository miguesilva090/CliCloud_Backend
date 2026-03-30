using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.Filters;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.ProcessoClinico;

namespace CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService
{
  public class QuestionarioUtenteService : IQuestionarioUtenteService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public QuestionarioUtenteService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<QuestionarioUtenteDTO>> GetQuestionarioAsync(Guid id)
    {
      try
      {
        QuestionarioUtenteDTO dto =
          await _repository.GetByIdAsync<QuestionarioUtente, QuestionarioUtenteDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<QuestionarioUtenteDTO>(ex.Message);
      }
    }

    public async Task<PaginatedResponse<QuestionarioUtenteTableDTO>> GetQuestionariosPaginatedAsync(
      QuestionarioUtenteTableFilter filter
    )
    {
      string dynamicOrder =
        filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
      List<TableFilter> tableFilters = filter.Filters ?? [];
      QuestionarioUtenteSearchTable specification = new(tableFilters, dynamicOrder);

      PaginatedResponse<QuestionarioUtenteTableDTO> pagedResponse =
        await _repository.GetPaginatedResultsAsync<
          QuestionarioUtente,
          QuestionarioUtenteTableDTO,
          Guid
        >(filter.PageNumber, filter.PageSize, specification);

      return pagedResponse;
    }

    public async Task<Response<Guid>> CreateQuestionarioAsync(CreateQuestionarioUtenteRequest request)
    {
      QuestionarioUtente entity = _mapper.Map(request, new QuestionarioUtente());
      entity.DataCriacao = DateTime.UtcNow;

      try
      {
        QuestionarioUtente response =
          await _repository.CreateAsync<QuestionarioUtente, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(response.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateQuestionarioAsync(
      UpdateQuestionarioUtenteRequest request,
      Guid id
    )
    {
      QuestionarioUtente entityInDb =
        await _repository.GetByIdAsync<QuestionarioUtente, Guid>(id);
      if (entityInDb == null)
      {
        return ResponseFactory.Fail<Guid>("Not Found");
      }

      QuestionarioUtente updated = _mapper.Map(request, entityInDb);

      try
      {
        QuestionarioUtente response =
          await _repository.UpdateAsync<QuestionarioUtente, Guid>(updated);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(response.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteQuestionarioAsync(Guid id)
    {
      try
      {
        QuestionarioUtente? removed =
          await _repository.RemoveByIdAsync<QuestionarioUtente, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(removed.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}

