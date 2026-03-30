using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Filters;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService
{
    public class AvaliacaoAntropometricaService : IAvaliacaoAntropometricaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AvaliacaoAntropometricaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AvaliacaoAntropometricaDTO>>> GetAvaliacaoAntropometricaAsync(string keyword = "")
        {
            var specification = new AvaliacaoAntropometricaSearchList(keyword);
            var list = await _repository.GetListAsync<AvaliacaoAntropometrica, AvaliacaoAntropometricaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AvaliacaoAntropometricaDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<AvaliacaoAntropometricaLightDTO>>> GetAvaliacaoAntropometricaLightAsync(string keyword = "")
        {
            var specification = new AvaliacaoAntropometricaSearchList(keyword);
            var list = await _repository.GetListAsync<AvaliacaoAntropometrica, AvaliacaoAntropometricaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AvaliacaoAntropometricaLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AvaliacaoAntropometricaDTO>> GetAvaliacaoAntropometricaPaginatedAsync(AvaliacaoAntropometricaTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new AvaliacaoAntropometricaSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<AvaliacaoAntropometrica, AvaliacaoAntropometricaDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }

        // get all (non-paginated)
        public async Task<Response<IEnumerable<AvaliacaoAntropometricaTableDTO>>> GetAllAvaliacaoAntropometricaAsync(AvaliacaoAntropometricaAllFilter filter)
        {
            try
            {
                filter ??= new AvaliacaoAntropometricaAllFilter();
                var dynamicOrder = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var specification = new AvaliacaoAntropometricaSearchTable(filters, dynamicOrder);
                var list = await _repository.GetListAsync<AvaliacaoAntropometrica, AvaliacaoAntropometricaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<AvaliacaoAntropometricaTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<AvaliacaoAntropometricaTableDTO>>(ex.Message);
            }
        }


        // get single AvaliacaoAntropometrica by Id 
        public async Task<Response<AvaliacaoAntropometricaDTO>> GetAvaliacaoAntropometricaAsync(Guid id)
        {
            try
            {
                AvaliacaoAntropometricaDTO dto = await _repository.GetByIdAsync<AvaliacaoAntropometrica, AvaliacaoAntropometricaDTO, Guid>(id);
                return ResponseFactory.Success<AvaliacaoAntropometricaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AvaliacaoAntropometricaDTO>(ex.Message);
            }
        }

        // create new AvaliacaoAntropometrica
        public async Task<Response<Guid>> CreateAvaliacaoAntropometricaAsync(CreateAvaliacaoAntropometricaRequest request)
        {
            var specification = new AvaliacaoAntropometricaMatchName(request.UtenteId, request.Data, request.Hora);
            bool avaliacaoAntropometricaExists = await _repository.ExistsAsync<AvaliacaoAntropometrica, Guid>(specification);
            if (avaliacaoAntropometricaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de avaliação antropométrica para o mesmo utente, data e hora.");
            }

            AvaliacaoAntropometrica newAvaliacaoAntropometrica = _mapper.Map(request, new AvaliacaoAntropometrica());

            try
            {
                AvaliacaoAntropometrica response = await _repository.CreateAsync<AvaliacaoAntropometrica, Guid>(newAvaliacaoAntropometrica);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AvaliacaoAntropometrica
        public async Task<Response<Guid>> UpdateAvaliacaoAntropometricaAsync(UpdateAvaliacaoAntropometricaRequest request, Guid id)
        {
            AvaliacaoAntropometrica avaliacaoAntropometricaInDb = await _repository.GetByIdAsync<AvaliacaoAntropometrica, Guid>(id);
            if (avaliacaoAntropometricaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AvaliacaoAntropometrica updatedAvaliacaoAntropometrica = _mapper.Map(request, avaliacaoAntropometricaInDb);

            try
            {
                AvaliacaoAntropometrica response = await _repository.UpdateAsync<AvaliacaoAntropometrica, Guid>(updatedAvaliacaoAntropometrica);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AvaliacaoAntropometrica
        public async Task<Response<Guid>> DeleteAvaliacaoAntropometricaAsync(Guid id)
        {
            try
            {
                AvaliacaoAntropometrica? avaliacaoAntropometrica = await _repository.RemoveByIdAsync<AvaliacaoAntropometrica, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(avaliacaoAntropometrica.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAvaliacaoAntropometricaAsync(IEnumerable<Guid> ids)
        {
            try
            {
                var idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (var id in idsList)
                {
                    try
                    {
                        var deletedEntity = await _repository.RemoveByIdAsync<AvaliacaoAntropometrica, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"AvaliacaoAntropometrica com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"AvaliacaoAntropometrica com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} avaliações antropométricas.";
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}
