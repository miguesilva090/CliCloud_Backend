using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.TipoAdmissaoService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.TipoAdmissaoService
{
  public class TipoAdmissaoService : ITipoAdmissaoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public TipoAdmissaoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<TipoAdmissaoDTO>>> GetAllAsync()
    {
      var list = await _repository.GetListAsync<TipoAdmissao, TipoAdmissaoDTO, Guid>();
      return ResponseFactory.Success(list);
    }
  }
}

