using AutoMapper;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.Filters;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.Specifications;
namespace CliCloud.Application.Services.Tratamentos.GoniometriasService
{
    public class GoniometriasService : IGoniometriasService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GoniometriasService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<GoniometriasDTO>>> GetGoniometriasAsync(string keyword = "")
        {
            GoniometriasSearchList specification = new(keyword); // ardalis specification
            IEnumerable<GoniometriasDTO> list = await _repository.GetListAsync<Goniometrias, GoniometriasDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<GoniometriasDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<GoniometriasLightDTO>>> GetGoniometriasLightAsync(string keyword = "")
        {
            GoniometriasSearchList specification = new(keyword);
            IEnumerable<GoniometriasLightDTO> list = await _repository.GetListAsync<Goniometrias, GoniometriasLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GoniometriasLightDTO>>(list);
        }


        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<GoniometriasTableDTO>> GetGoniometriasPaginatedAsync(GoniometriasTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            GoniometriasSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<GoniometriasTableDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<Goniometrias, GoniometriasTableDTO, Guid>(
                    filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Goniometrias (non-paginated)
        public async Task<Response<IEnumerable<GoniometriasTableDTO>>> GetAllGoniometriasAsync(GoniometriasAllFilter filter)
        {
            try
            {
                filter ??= new GoniometriasAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                GoniometriasSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<GoniometriasTableDTO> list = await _repository.GetListAsync<Goniometrias, GoniometriasTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<GoniometriasTableDTO>>(list);
            }
            catch( Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<GoniometriasTableDTO>>(ex.Message);
            }
        }


        // get single Goniometrias by Id 
        public async Task<Response<GoniometriasDTO>> GetGoniometriasAsync(Guid id)
        {
            try
            {
                GoniometriasDTO dto = await _repository.GetByIdAsync<Goniometrias, GoniometriasDTO, Guid>(id);
                return ResponseFactory.Success<GoniometriasDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GoniometriasDTO>(ex.Message);
            }
        }

        // get single Goniometrias by Descricao (exact match)
        public async Task<Response<GoniometriasDTO>> GetGoniometriasByDescricaoAsync(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<GoniometriasDTO>("Descricao não pode ser vazia");
                }

                GoniometriasMatchDescricao specification = new(descricao);
                IEnumerable<GoniometriasDTO> results =
                    await _repository.GetListAsync<Goniometrias, GoniometriasDTO, Guid>(specification);

                GoniometriasDTO? goniometrias = results.FirstOrDefault();
                if (goniometrias == null)
                {
                    return ResponseFactory.Fail<GoniometriasDTO>("Não foi encontrada nenhuma Goniometria com a descrição fornecida");

                }

                return ResponseFactory.Success<GoniometriasDTO>(goniometrias);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GoniometriasDTO>(ex.Message);
            }
        }

        // create new Goniometrias
        public async Task<Response<Guid>> CreateGoniometriasAsync(CreateGoniometriasRequest request)
        {
            GoniometriasMatchDescricao specification = new(request.Descricao);
            bool goniometriaExists = await _repository.ExistsAsync<Goniometrias, Guid>(specification);
            if (goniometriaExists)
            {
                return ResponseFactory.Fail<Guid>("Goniometria já existe");
            }

            Goniometrias newGoniometrias = _mapper.Map(request, new Goniometrias()); // map dto to domain entity

            try
            {
                Goniometrias response = await _repository.CreateAsync<Goniometrias, Guid>(newGoniometrias); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Goniometrias
        public async Task<Response<Guid>> UpdateGoniometriasAsync(UpdateGoniometriasRequest request, Guid id)
        {
            Goniometrias goniometriaInDb = await _repository.GetByIdAsync<Goniometrias, Guid>(id);
            if (goniometriaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            string currentDescricao = goniometriaInDb.Descricao?.Trim() ?? string.Empty;
            string requestedDescricao = request.Descricao.Trim();

            if (!string.Equals(currentDescricao, requestedDescricao, StringComparison.OrdinalIgnoreCase))
            {
                GoniometriasMatchDescricao specification = new(request.Descricao);
                bool descricaoExists = await _repository.ExistsAsync<Goniometrias, Guid>(specification);
                if (descricaoExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma Goniometria com esta descrição");
                }
            }

            _ = _mapper.Map(request, goniometriaInDb);

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(goniometriaInDb.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Goniometrias
        public async Task<Response<Guid>> DeleteGoniometriasAsync(Guid id)
        {
            try
            {
                Goniometrias? Goniometrias = await _repository.RemoveByIdAsync<Goniometrias, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Goniometrias.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Goniometrias
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleGoniometriasAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach( Guid id in idsList)
                {
                    try
                    {
                        Goniometrias? entity = await _repository.GetByIdAsync<Goniometrias, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Goniometria com ID {id} não encontrada.");
                            continue;
                        }

                        Goniometrias? deletedEntity = await _repository.RemoveByIdAsync<Goniometrias, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar Goniometria com ID {id}.");
                        }
                    }
                    catch
                    {
                        failedDeletions.Add($"Falha ao eliminar Goniometria com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }

                if (successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} goniometrias.");
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

