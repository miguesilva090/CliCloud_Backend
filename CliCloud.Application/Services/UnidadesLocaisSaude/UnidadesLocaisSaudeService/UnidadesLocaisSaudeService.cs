using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.DTOs;
using CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.Specifications;
using UnidadesLocaisSaudeEntity = CliCloud.Domain.Entities.UnidadesLocaisSaude.UnidadesLocaisSaude;

namespace CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService
{
  public class UnidadesLocaisSaudeService : IUnidadesLocaisSaudeService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public UnidadesLocaisSaudeService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<UnidadesLocaisSaudeLightDTO>>> GetUnidadesLocaisSaudeLightAsync(string keyword = "")
    {
      UnidadesLocaisSaudeSearchList specification = new(keyword);
      IEnumerable<UnidadesLocaisSaudeLightDTO> list =
        await _repository.GetListAsync<UnidadesLocaisSaudeEntity, UnidadesLocaisSaudeLightDTO, Guid>(specification);

      return ResponseFactory.Success<IEnumerable<UnidadesLocaisSaudeLightDTO>>(list);
    }
  }
}

