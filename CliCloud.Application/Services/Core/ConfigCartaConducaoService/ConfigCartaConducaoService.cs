using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigCartaConducaoService.DTOs;
using CliCloud.Application.Services.Core.ConfigCartaConducaoService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigCartaConducaoService
{
    public class ConfigCartaConducaoService(IRepositoryAsync repository) : IConfigCartaConducaoService 
    {
        private readonly IRepositoryAsync _repository = repository; 
        
        public async Task<Response<ConfigCartaConducaoDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
        {
            try
            {
                var spec = new ConfigCartaConducaoPorClinicaSpec(clinicaId);
                var entity = ( await _repository.GetListAsync<ConfigCartaConducao, Guid>(spec)).FirstOrDefault();

                if (entity == null)
                    return ResponseFactory.Fail<ConfigCartaConducaoDTO>("Configuração de carta de condução não encontrada");

                return ResponseFactory.Success(new ConfigCartaConducaoDTO{
                    Id = entity.Id,
                    ClinicaId = entity.ClinicaId,
                    UrlOnline = entity.UrlOnline,
                    UrlOffline = entity.UrlOffline,
                    Utilizador = entity.Utilizador,
                    Password = entity.Password,
                    AutoridadeSaudePublica = entity.AutoridadeSaudePublica
                });
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ConfigCartaConducaoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigCartaConducaoRequest request)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.UrlOnline)) return ResponseFactory.Fail<Guid>("URL online é obrigatório");
                if(string.IsNullOrWhiteSpace(request.UrlOffline)) return ResponseFactory.Fail<Guid>("URL offline é obrigatório");
                if(string.IsNullOrWhiteSpace(request.Utilizador)) return ResponseFactory.Fail<Guid>("Utilizador é obrigatório");
                if(string.IsNullOrWhiteSpace(request.Password)) return ResponseFactory.Fail<Guid>("Password é obrigatória");

                var spec = new ConfigCartaConducaoPorClinicaSpec(clinicaId);
                var entity = (await _repository.GetListAsync<ConfigCartaConducao, Guid>(spec)).FirstOrDefault();

                if(entity == null)
                {
                    entity = new ConfigCartaConducao {ClinicaId = clinicaId};
                }

                entity.UrlOnline = request.UrlOnline.Trim();
                entity.UrlOffline = request.UrlOffline.Trim();
                entity.Utilizador = request.Utilizador.Trim();
                entity.Password = request.Password;
                entity.AutoridadeSaudePublica = request.AutoridadeSaudePublica;
                
                if (entity.Id == Guid.Empty)
                    await _repository.CreateAsync<ConfigCartaConducao, Guid>(entity);
                else
                    await _repository.UpdateAsync<ConfigCartaConducao, Guid>(entity);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}