using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.WebApi.Controllers.Core
{
    [Route("client/core/[controller]")]
    [ApiController]
    public class ContextoClinicaController : ControllerBase
    {
        private readonly IRepositoryAsync _repository;
        
        public ContextoClinicaController(IRepositoryAsync repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "client")]
        [HttpGet("disponiveis")]
        public async Task<IActionResult> GetClinicasDisponiveisAsync()
        {
            var clinicas = (await _repository.GetListAsync<Clinica, Guid>())
                .Where(c => c.DeletedOn == null)
                .OrderBy(c => c.Nome)
                .Select(c => new ClinicaDisponivelDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                })
                .ToList();

            return Ok(ResponseFactory.Success<IEnumerable<ClinicaDisponivelDTO>>(clinicas));
        }

        [Authorize(Roles = "client")]
        [HttpGet("estado")]
        public async Task<IActionResult> GetEstadoContextoAsync()
        {
            var uidRaw = User.FindFirstValue("uid");
            if (string.IsNullOrWhiteSpace(uidRaw) || !Guid.TryParse(uidRaw, out var userIdLicencas))
                return BadRequest("Utilizador inválido");

            if (!Request.Headers.TryGetValue("X-Client-Id", out var clientHeader)
                || !Guid.TryParse(clientHeader.FirstOrDefault(), out var clientIdLicencas))
                return BadRequest("Id Cliente inválido");

            var mapAtivas = (await _repository.GetListAsync<LicencaUserClinicaMap, Guid>())
                .Where(x =>
                    x.ClienteIdLicencas == clientIdLicencas &&
                    x.UserIdLicencas == userIdLicencas &&
                    x.Ativo &&
                    x.DeletedOn == null)
                .ToList();

            var response = new ContextoClinicaEstadoResponse
            {
                RequerSelecao = mapAtivas.Count == 0,
                ClinicaAssociadaCount = mapAtivas.Count,
                ClinicaDefaultId = mapAtivas.FirstOrDefault(x => x.IsDefault)?.ClinicaId,
            };

            return Ok(ResponseFactory.Success(response));
        }

        [Authorize(Roles = "client")]
        [HttpPost("selecionar")]
        public async Task<IActionResult> SelecionarClinica([FromBody] SelecionarClinicaRequest request)
        {
            var uidRaw = User.FindFirstValue("uid");
            if(string.IsNullOrWhiteSpace(uidRaw) || !Guid.TryParse(uidRaw, out var userIdLicencas))
                return BadRequest("Utilizador inválido");
            
            if(!Request.Headers.TryGetValue("X-Client-Id", out var clientHeader)
                || !Guid.TryParse(clientHeader.FirstOrDefault(), out var clientIdLicencas))
                return BadRequest("Id Cliente inválido");

            var clinica = await _repository.GetByIdAsync<Clinica, Guid>(request.ClinicaId);
            if(clinica is null)
                return BadRequest("Clínica não encontrada");

            var existentes = ( await _repository.GetListAsync<LicencaUserClinicaMap, Guid>())
                .Where(x => 
                    x.ClienteIdLicencas == clientIdLicencas &&
                    x.UserIdLicencas == userIdLicencas &&
                    x.Ativo &&
                    x.DeletedOn == null)
                .ToList();

            foreach(var item in existentes)
            {
                item.IsDefault = false;
                await _repository.UpdateAsync<LicencaUserClinicaMap, Guid>(item);
            }

            var alvo = existentes.FirstOrDefault(x => x.ClinicaId == request.ClinicaId);
            if( alvo is null)
            {
                alvo = new LicencaUserClinicaMap
                {
                    Id = Guid.NewGuid(),
                    ClienteIdLicencas = clientIdLicencas,
                    UserIdLicencas = userIdLicencas,
                    ClinicaId = request.ClinicaId,
                    Ativo = true,
                    IsDefault = true,
                };
                await _repository.CreateAsync<LicencaUserClinicaMap, Guid>(alvo);
            }
            else 
            {
                alvo.Ativo = true;
                alvo.IsDefault = true;
                await _repository.UpdateAsync<LicencaUserClinicaMap, Guid>(alvo);
            }

            await _repository.SaveChangesAsync();
            return Ok(CliCloud.Application.Common.Wrapper.Response.Success());
        }
    }

    public class SelecionarClinicaRequest 
    {
        public Guid ClinicaId { get; set; }
    }

    public class ClinicaDisponivelDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class ContextoClinicaEstadoResponse
    {
        public bool RequerSelecao { get; set; }
        public int ClinicaAssociadaCount { get; set; }
        public Guid? ClinicaDefaultId { get; set; }
    }
}
