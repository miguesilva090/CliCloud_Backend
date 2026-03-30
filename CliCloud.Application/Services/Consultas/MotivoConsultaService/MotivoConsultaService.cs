using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService
{
  public class MotivoConsultaService : IMotivoConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public MotivoConsultaService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<MotivoConsultaDTO>>> GetAllAsync()
    {
      var list = await _repository.GetListAsync<MotivoConsulta, MotivoConsultaDTO, Guid>();
      return ResponseFactory.Success(list);
    }
  }
}
