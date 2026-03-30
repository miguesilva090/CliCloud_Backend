using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Filters;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService
{
    public class AvaliacaoPosturalService : IAvaliacaoPosturalService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AvaliacaoPosturalService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        public async Task<Response<IEnumerable<AvaliacaoPosturalDTO>>> GetAvaliacaoPosturalAsync(string keyword = "")
        {
            var specification = new AvaliacaoPosturalSearchList(keyword);
            var list = await _repository.GetListAsync<AvaliacaoPostural, AvaliacaoPosturalDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AvaliacaoPosturalDTO>>(list);
        }

        public async Task<Response<IEnumerable<AvaliacaoPosturalLightDTO>>> GetAvaliacaoPosturalLightAsync(string keyword = "")
        {
            var specification = new AvaliacaoPosturalSearchList(keyword);
            var list = await _repository.GetListAsync<AvaliacaoPostural, AvaliacaoPosturalLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<AvaliacaoPosturalLightDTO>>(list);
        }

        public async Task<PaginatedResponse<AvaliacaoPosturalDTO>> GetAvaliacaoPosturalPaginatedAsync(AvaliacaoPosturalTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new AvaliacaoPosturalSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<AvaliacaoPostural, AvaliacaoPosturalDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }

        public async Task<Response<IEnumerable<AvaliacaoPosturalTableDTO>>> GetAllAvaliacaoPosturalAsync(AvaliacaoPosturalAllFilter filter)
        {
            try
            {
                filter ??= new AvaliacaoPosturalAllFilter();
                var dynamicOrder = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var specification = new AvaliacaoPosturalSearchTable(filters, dynamicOrder);
                var list = await _repository.GetListAsync<AvaliacaoPostural, AvaliacaoPosturalTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<AvaliacaoPosturalTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<AvaliacaoPosturalTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<AvaliacaoPosturalDTO>> GetAvaliacaoPosturalAsync(Guid id)
        {
            try
            {
                AvaliacaoPosturalDTO dto = await _repository.GetByIdAsync<AvaliacaoPostural, AvaliacaoPosturalDTO, Guid>(id);
                return ResponseFactory.Success<AvaliacaoPosturalDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AvaliacaoPosturalDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAvaliacaoPosturalAsync(CreateAvaliacaoPosturalRequest request)
        {
            var specification = new AvaliacaoPosturalMatchName(request.UtenteId, request.Data, request.Hora);
            bool avaliacaoPosturalExists = await _repository.ExistsAsync<AvaliacaoPostural, Guid>(specification);
            if (avaliacaoPosturalExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de avaliação postural para o mesmo utente, data e hora.");
            }

            AvaliacaoPostural newAvaliacaoPostural = _mapper.Map(request, new AvaliacaoPostural());

            try
            {
                AvaliacaoPostural response = await _repository.CreateAsync<AvaliacaoPostural, Guid>(newAvaliacaoPostural);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAvaliacaoPosturalAsync(UpdateAvaliacaoPosturalRequest request, Guid id)
        {
            AvaliacaoPostural avaliacaoPosturalInDb = await _repository.GetByIdAsync<AvaliacaoPostural, Guid>(id);
            if (avaliacaoPosturalInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AvaliacaoPostural updatedAvaliacaoPostural = _mapper.Map(request, avaliacaoPosturalInDb);

            try
            {
                AvaliacaoPostural response = await _repository.UpdateAsync<AvaliacaoPostural, Guid>(updatedAvaliacaoPostural);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteAvaliacaoPosturalAsync(Guid id)
        {
            try
            {
                AvaliacaoPostural? avaliacaoPostural = await _repository.RemoveByIdAsync<AvaliacaoPostural, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(avaliacaoPostural.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAvaliacaoPosturalAsync(IEnumerable<Guid> ids)
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
                        var deletedEntity = await _repository.RemoveByIdAsync<AvaliacaoPostural, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"AvaliacaoPostural com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"AvaliacaoPostural com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} avaliações posturais.";
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
