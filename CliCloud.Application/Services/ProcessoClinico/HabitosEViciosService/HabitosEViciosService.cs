using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Filters;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService
{
    public class HabitosEViciosService : IHabitosEViciosService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public HabitosEViciosService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<HabitosEViciosDTO>>> GetHabitosEViciosAsync(string keyword = "")
        {
            var specification = new HabitosEViciosSearchList(keyword);
            var list = await _repository.GetListAsync<HabitosEVicios, HabitosEViciosDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HabitosEViciosDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<HabitosEViciosLightDTO>>> GetHabitosEViciosLightAsync(string keyword = "")
        {
            var specification = new HabitosEViciosSearchList(keyword);
            var list = await _repository.GetListAsync<HabitosEVicios, HabitosEViciosLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<HabitosEViciosLightDTO>>(list);
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<HabitosEViciosTableDTO>> GetHabitosEViciosPaginatedAsync(HabitosEViciosTableFilter filter)
        {
            var dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var specification = new HabitosEViciosSearchTable(filter.Filters ?? new List<TableFilter>(), dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<HabitosEVicios, HabitosEViciosTableDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }

        // get all (non-paginated)
        public async Task<Response<IEnumerable<HabitosEViciosTableDTO>>> GetAllHabitosEViciosAsync(HabitosEViciosAllFilter filter)
        {
            try
            {
                filter ??= new HabitosEViciosAllFilter();
                var dynamicOrder = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var specification = new HabitosEViciosSearchTable(filters, dynamicOrder);
                var list = await _repository.GetListAsync<HabitosEVicios, HabitosEViciosTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<HabitosEViciosTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<HabitosEViciosTableDTO>>(ex.Message);
            }
        }

        // get single HabitosEVicios by Id 
        public async Task<Response<HabitosEViciosDTO>> GetHabitosEViciosAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<HabitosEVicios, HabitosEViciosDTO, Guid>(id);
                return ResponseFactory.Success<HabitosEViciosDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<HabitosEViciosDTO>(ex.Message);
            }
        }

        // create new HabitosEVicios
        public async Task<Response<Guid>> CreateHabitosEViciosAsync(CreateHabitosEViciosRequest request)
        {
            var newHabitosEVicios = _mapper.Map(request, new HabitosEVicios());

            try
            {
                var response = await _repository.CreateAsync<HabitosEVicios, Guid>(newHabitosEVicios);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update HabitosEVicios
        public async Task<Response<Guid>> UpdateHabitosEViciosAsync(UpdateHabitosEViciosRequest request, Guid id)
        {
            var habitosEViciosInDb = await _repository.GetByIdAsync<HabitosEVicios, Guid>(id);
            if (habitosEViciosInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            var updatedHabitosEVicios = _mapper.Map(request, habitosEViciosInDb);

            try
            {
                var response = await _repository.UpdateAsync<HabitosEVicios, Guid>(updatedHabitosEVicios);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete HabitosEVicios
        public async Task<Response<Guid>> DeleteHabitosEViciosAsync(Guid id)
        {
            try
            {
                var habitosEVicios = await _repository.RemoveByIdAsync<HabitosEVicios, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(habitosEVicios.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleHabitosEViciosAsync(IEnumerable<Guid> ids)
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
                        var deletedEntity = await _repository.RemoveByIdAsync<HabitosEVicios, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"HabitosEVicios com ID {id} não encontrado.");
                        }
                    }
                    catch
                    {
                        failedDeletions.Add($"Erro ao eliminar HabitosEVicios com ID {id}.");
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} registos de Hábitos e Vícios."
                    );
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

