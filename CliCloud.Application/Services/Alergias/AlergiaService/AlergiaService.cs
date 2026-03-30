using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Alergias.AlergiaService.DTOs;
using CliCloud.Application.Services.Alergias.AlergiaService.Filters;
using CliCloud.Application.Services.Alergias.AlergiaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.Alergias.AlergiaService
{
    public class AlergiaService : IAlergiaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AlergiaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        public async Task<Response<IEnumerable<AlergiaDTO>>> GetAlergiaAsync(string keyword = "")
        {
            AlergiaSearchList specification = new(keyword);
            IEnumerable<AlergiaDTO> list = await _repository.GetListAsync<Alergia, AlergiaDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<AlergiaLightDTO>>> GetAlergiaLightAsync(string keyword = "")
        {
            AlergiaSearchList specification = new(keyword);
            IEnumerable<AlergiaLightDTO> list = await _repository.GetListAsync<Alergia, AlergiaLightDTO, Guid>(specification);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<AlergiaTableDTO>> GetAlergiaPaginatedAsync(AlergiaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            AlergiaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<AlergiaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Alergia, AlergiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }


        public async Task<Response<AlergiaDTO>> GetAlergiaAsync(Guid id)
        {
            try
            {
                AlergiaDTO dto = await _repository.GetByIdAsync<Alergia, AlergiaDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AlergiaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateAlergiaAsync(CreateAlergiaRequest request)
        {
            AlergiaMatchDescricao specification = new(request.Descricao);
            bool alergiaExists = await _repository.ExistsAsync<Alergia, Guid>(specification);
            if (alergiaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe uma Alergia com esta descrição.");
            }

            Alergia newAlergia = _mapper.Map(request, new Alergia());

            try
            {
                Alergia response = await _repository.CreateAsync<Alergia, Guid>(newAlergia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateAlergiaAsync(UpdateAlergiaRequest request, Guid id)
        {
            Alergia? alergiaInDb = await _repository.GetByIdAsync<Alergia, Guid>(id);
            if (alergiaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Alergia não encontrada.");
            }

            string currentDescricao = alergiaInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao?.Trim() ?? string.Empty;

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                AlergiaMatchDescricao specification = new(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<Alergia, Guid>(specification);
                if (descricaoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma Alergia com esta descrição.");
                }
            }

            Alergia updatedAlergia = _mapper.Map(request, alergiaInDb);

            try
            {
                Alergia response = await _repository.UpdateAsync<Alergia, Guid>(updatedAlergia);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteAlergiaAsync(Guid id)
        {
            try
            {
                Alergia? alergia = await _repository.RemoveByIdAsync<Alergia, Guid>(id);
                if (alergia == null)
                {
                    return ResponseFactory.Fail<Guid>("Alergia não encontrada.");
                }
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(alergia.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAlergiaAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (Guid id in idsList)
                {
                    try
                    {
                        Alergia? entity = await _repository.GetByIdAsync<Alergia, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Alergia com ID {id} não encontrada.");
                            continue;
                        }

                        Alergia? deletedEntity = await _repository.RemoveByIdAsync<Alergia, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Alergia com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Alergia com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} alergias.");
                }
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

