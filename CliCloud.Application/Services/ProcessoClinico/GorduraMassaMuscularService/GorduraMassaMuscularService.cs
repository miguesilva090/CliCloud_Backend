using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Filters;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService
{
    public class GorduraMassaMuscularService : IGorduraMassaMuscularService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GorduraMassaMuscularService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<GorduraMassaMuscularDTO>>> GetGorduraMassaMuscularAsync(string keyword = "")
        {
            var specification = new GorduraMassaMuscularSearchList(keyword);
            var list = await _repository.GetListAsync<GorduraMassaMuscular, GorduraMassaMuscularDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GorduraMassaMuscularDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<GorduraMassaMuscularLightDTO>>> GetGorduraMassaMuscularLightAsync(string keyword = "")
        {
            var specification = new GorduraMassaMuscularSearchList(keyword);
            var list = await _repository.GetListAsync<GorduraMassaMuscular, GorduraMassaMuscularLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GorduraMassaMuscularLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<GorduraMassaMuscularDTO>> GetGorduraMassaMuscularPaginatedAsync(GorduraMassaMuscularTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new GorduraMassaMuscularSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<GorduraMassaMuscular, GorduraMassaMuscularDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }

        // get all (non-paginated)
        public async Task<Response<IEnumerable<GorduraMassaMuscularTableDTO>>> GetAllGorduraMassaMuscularAsync(GorduraMassaMuscularAllFilter filter)
        {
            try
            {
                filter ??= new GorduraMassaMuscularAllFilter();
                var dynamicOrder = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var specification = new GorduraMassaMuscularSearchTable(filters, dynamicOrder);
                var list = await _repository.GetListAsync<GorduraMassaMuscular, GorduraMassaMuscularTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<GorduraMassaMuscularTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<GorduraMassaMuscularTableDTO>>(ex.Message);
            }
        }


        // get single GorduraMassaMuscular by Id 
        public async Task<Response<GorduraMassaMuscularDTO>> GetGorduraMassaMuscularAsync(Guid id)
        {
            try
            {
                GorduraMassaMuscularDTO dto = await _repository.GetByIdAsync<GorduraMassaMuscular, GorduraMassaMuscularDTO, Guid>(id);
                return ResponseFactory.Success<GorduraMassaMuscularDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GorduraMassaMuscularDTO>(ex.Message);
            }
        }

        // create new GorduraMassaMuscular
        public async Task<Response<Guid>> CreateGorduraMassaMuscularAsync(CreateGorduraMassaMuscularRequest request)
        {
            var specification = new GorduraMassaMuscularMatchName(request.UtenteId, request.Data, request.Hora);
            bool gorduraMassaMuscularExists = await _repository.ExistsAsync<GorduraMassaMuscular, Guid>(specification);
            if (gorduraMassaMuscularExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de gordura/massa muscular para o mesmo utente, data e hora.");
            }

            GorduraMassaMuscular newGorduraMassaMuscular = _mapper.Map(request, new GorduraMassaMuscular());

            try
            {
                GorduraMassaMuscular response = await _repository.CreateAsync<GorduraMassaMuscular, Guid>(newGorduraMassaMuscular);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update GorduraMassaMuscular
        public async Task<Response<Guid>> UpdateGorduraMassaMuscularAsync(UpdateGorduraMassaMuscularRequest request, Guid id)
        {
            GorduraMassaMuscular gorduraMassaMuscularInDb = await _repository.GetByIdAsync<GorduraMassaMuscular, Guid>(id);
            if (gorduraMassaMuscularInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            GorduraMassaMuscular updatedGorduraMassaMuscular = _mapper.Map(request, gorduraMassaMuscularInDb);

            try
            {
                GorduraMassaMuscular response = await _repository.UpdateAsync<GorduraMassaMuscular, Guid>(updatedGorduraMassaMuscular);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete GorduraMassaMuscular
        public async Task<Response<Guid>> DeleteGorduraMassaMuscularAsync(Guid id)
        {
            try
            {
                GorduraMassaMuscular? gorduraMassaMuscular = await _repository.RemoveByIdAsync<GorduraMassaMuscular, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(gorduraMassaMuscular.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGorduraMassaMuscularAsync(IEnumerable<Guid> ids)
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
                        var deletedEntity = await _repository.RemoveByIdAsync<GorduraMassaMuscular, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"GorduraMassaMuscular com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"GorduraMassaMuscular com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} registos de gordura/massa muscular.";
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
