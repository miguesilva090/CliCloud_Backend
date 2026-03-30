using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico
{
  [Route("client/processo-clinico/[controller]")]
  [ApiController]
  public class QuestionarioUtenteController(IQuestionarioUtenteService questionarioUtenteService)
    : ControllerBase
  {
    private readonly IQuestionarioUtenteService _questionarioUtenteService = questionarioUtenteService;

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetQuestionarioAsync(Guid id)
    {
      Response<QuestionarioUtenteDTO> result = await _questionarioUtenteService.GetQuestionarioAsync(id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetQuestionariosPaginatedAsync(QuestionarioUtenteTableFilter filter)
    {
      PaginatedResponse<QuestionarioUtenteTableDTO> result =
        await _questionarioUtenteService.GetQuestionariosPaginatedAsync(filter);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateQuestionarioAsync(CreateQuestionarioUtenteRequest request)
    {
      Response<Guid> result = await _questionarioUtenteService.CreateQuestionarioAsync(request);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateQuestionarioAsync(UpdateQuestionarioUtenteRequest request, Guid id)
    {
      Response<Guid> result = await _questionarioUtenteService.UpdateQuestionarioAsync(request, id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteQuestionarioAsync(Guid id)
    {
      Response<Guid> result = await _questionarioUtenteService.DeleteQuestionarioAsync(id);
      return Ok(result);
    }
  }
}

