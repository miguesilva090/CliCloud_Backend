using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;
using CliCloud.Application.Utility;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Filters;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Specifications;

// After creating this service:
// -- 1. Create a MapaBodyChart domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<MapaBodyChart> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a MapaBodyChart api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService
{
    public class MapaBodyChartService : IMapaBodyChartService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public MapaBodyChartService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<MapaBodyChartDTO>>> GetMapaBodyChartAsync(string keyword = "")
        {
            MapaBodyChartSearchList specification = new(keyword); // ardalis specification
            IEnumerable<MapaBodyChartDTO> list = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<MapaBodyChartDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MapaBodyChartLightDTO>>> GetMapaBodyChartLightAsync(string keyword = "")
        {
            MapaBodyChartSearchList specification = new(keyword);
            IEnumerable<MapaBodyChartLightDTO> list = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MapaBodyChartLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<MapaBodyChartTableDTO>> GetMapaBodyChartPaginatedAsync(MapaBodyChartTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            MapaBodyChartSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<MapaBodyChartTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<MapaBodyChart, MapaBodyChartTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all MapaBodyChart (non-paginated)
        public async Task<Response<IEnumerable<MapaBodyChartTableDTO>>> GetAllMapaBodyChartAsync(MapaBodyChartAllFilter filter)
        {
            try
            {
                filter ??= new MapaBodyChartAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MapaBodyChartSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MapaBodyChartTableDTO> list = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MapaBodyChartTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MapaBodyChartTableDTO>>(ex.Message);
            }
        }


        // get single MapaBodyChart by Id 
        public async Task<Response<MapaBodyChartDTO>> GetMapaBodyChartAsync(Guid id)
        {
            try
            {
                MapaBodyChartByIdWithMarkers specification = new(id);
                IEnumerable<MapaBodyChartDTO> list = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartDTO, Guid>(specification);
                MapaBodyChartDTO? dto = list.FirstOrDefault();
                if (dto == null)
                {
                    return ResponseFactory.Fail<MapaBodyChartDTO>("Não encontrado");
                }

                if (dto.Marcadores == null || dto.Marcadores.Count == 0)
                {
                    MapaBodyChart entity = await _repository.GetByIdAsync<MapaBodyChart, Guid>(id);
                    if (entity != null)
                    {
                        List<MarcadorBodyChart> novos = GetDefaultMarkers(entity.Nome, entity.Id);
                        if (novos.Count > 0)
                        {
                            foreach (MarcadorBodyChart m in novos)
                            {
                                _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(m);
                            }
                            _ = await _repository.SaveChangesAsync();

                            IEnumerable<MapaBodyChartDTO> refreshed = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartDTO, Guid>(specification);
                            dto = refreshed.FirstOrDefault() ?? dto;
                        }
                    }
                }

                return ResponseFactory.Success<MapaBodyChartDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MapaBodyChartDTO>(ex.Message);
            }
        }

        private static List<MarcadorBodyChart> GetDefaultMarkers(string nome, Guid mapaId)
        {
            List<MarcadorBodyChart> markers = [];
            string n = (nome ?? string.Empty).ToLowerInvariant();

            if (n.Contains("body") || n.Contains("chart"))
            {
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Bloqueio/disfunção", CorHex = "#f59e0b" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Hipertonicidade", CorHex = "#ef4444" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Hipotonicidade", CorHex = "#3b82f6" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Irradiação da Dor", CorHex = "#a855f7" });
            }
            else if (n.Contains("ósseo") || n.Contains("osseo"))
            {
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Fratura", CorHex = "#ef4444" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Contusão", CorHex = "#f97316" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Lesão Lítica", CorHex = "#fde047" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Lesão benigna", CorHex = "#22c55e" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Doença Metabólica", CorHex = "#0ea5e9" });
            }
            else if (n.Contains("muscular"))
            {
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Estiramento", CorHex = "#22c55e" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Contusão", CorHex = "#f97316" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Contratura", CorHex = "#e11d48" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Ruptura", CorHex = "#7c3aed" });
                markers.Add(new MarcadorBodyChart { MapaBodyChartId = mapaId, Titulo = "Dor Muscular Tardia", CorHex = "#14b8a6" });
            }

            return markers;
        }

        // get single MapaBodyChart by Nome 
        public async Task<Response<MapaBodyChartDTO>> GetMapaBodyChartByNomeAsync(string nome)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(nome))
                {
                    return ResponseFactory.Fail<MapaBodyChartDTO>("Nome não pode ser vazio");
                }

                MapaBodyChartMatchName specification = new(nome);
                IEnumerable<MapaBodyChartDTO> results = await _repository.GetListAsync<MapaBodyChart, MapaBodyChartDTO, Guid>(specification);
                MapaBodyChartDTO? MapaBodyChart = results.FirstOrDefault();

                if(MapaBodyChart == null)
                {
                    return ResponseFactory.Fail<MapaBodyChartDTO>("Não foi encontrado nenhum Body Chart com o Nome fornecido");
                }

                return ResponseFactory.Success<MapaBodyChartDTO>(MapaBodyChart);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<MapaBodyChartDTO>(ex.Message);
            }
        }

        // create new MapaBodyChart
        public async Task<Response<Guid>> CreateMapaBodyChartAsync(CreateMapaBodyChartRequest request)
        {
            MapaBodyChartMatchName specification = new(request.Nome); // ardalis specification 
            bool MapaBodyChartExists = await _repository.ExistsAsync<MapaBodyChart, Guid>(specification);
            if (MapaBodyChartExists)
            {
                return ResponseFactory.Fail<Guid>("MapaBodyChart já existe");
            }

            MapaBodyChart newMapaBodyChart = _mapper.Map(request, new MapaBodyChart()); // map dto to domain entity

            try
            {
                MapaBodyChart response = await _repository.CreateAsync<MapaBodyChart, Guid>(newMapaBodyChart); // create new entity 

                // criar marcadores padrão para mapas novos, consoante o Nome
                string nomeLower = (response.Nome ?? string.Empty).ToLowerInvariant();

                // Body Chart superficial
                if (nomeLower.Contains("body") || nomeLower.Contains("chart"))
                {
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Bloqueio/disfunção",
                        CorHex = "#f59e0b",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Hipertonicidade",
                        CorHex = "#ef4444",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Hipotonicidade",
                        CorHex = "#3b82f6",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Irradiação da Dor",
                        CorHex = "#a855f7",
                    });
                }
                // Mapa Ósseo
                else if (nomeLower.Contains("ósseo") || nomeLower.Contains("osseo") || nomeLower.Contains("osseo"))
                {
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Fratura",
                        CorHex = "#ef4444",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Contusão",
                        CorHex = "#f97316",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Lesão Lítica",
                        CorHex = "#fde047",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Lesão benigna",
                        CorHex = "#22c55e",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Doença Metabólica",
                        CorHex = "#0ea5e9",
                    });
                }
                // Mapa Muscular
                else if (nomeLower.Contains("muscular"))
                {
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Estiramento",
                        CorHex = "#22c55e",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Contusão",
                        CorHex = "#f97316",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Contratura",
                        CorHex = "#e11d48",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Ruptura",
                        CorHex = "#7c3aed",
                    });
                    _ = await _repository.CreateAsync<MarcadorBodyChart, Guid>(new MarcadorBodyChart
                    {
                        MapaBodyChartId = response.Id,
                        Titulo = "Dor Muscular Tardia",
                        CorHex = "#14b8a6",
                    });
                }

                _ = await _repository.SaveChangesAsync(); // save changes to db (mapa + marcadores)
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update MapaBodyChart
        public async Task<Response<Guid>> UpdateMapaBodyChartAsync(UpdateMapaBodyChartRequest request, Guid id)
        {
            MapaBodyChart MapaBodyChartInDb = await _repository.GetByIdAsync<MapaBodyChart, Guid>(id); // get existing entity
            if (MapaBodyChartInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            MapaBodyChart updatedMapaBodyChart = _mapper.Map(request, MapaBodyChartInDb); // map dto to domain entity

            try
            {
                MapaBodyChart response = await _repository.UpdateAsync<MapaBodyChart, Guid>(updatedMapaBodyChart);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete MapaBodyChart
        public async Task<Response<Guid>> DeleteMapaBodyChartAsync(Guid id)
        {
            try
            {
                MapaBodyChart? MapaBodyChart = await _repository.RemoveByIdAsync<MapaBodyChart, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(MapaBodyChart.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple MapaBodyChart 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMapaBodyChartAsync(IEnumerable<Guid> ids) 
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach(Guid id in idsList)
                {
                    try
                    {
                        MapaBodyChart? entity = await _repository.GetByIdAsync<MapaBodyChart, Guid>(id);
                        if(entity == null)
                        {
                            failedDeletions.Add($"Body Chart com ID {id} não encontrado");
                            continue;
                        }

                        MapaBodyChart? deletedEntity = await _repository.RemoveByIdAsync<MapaBodyChart, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar Body Chart com ID {id}.");
                        }
                    }
                    catch
                    {
                        failedDeletions.Add($"Falha ao eliminar Body Chart com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }

                if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Body Charts.");
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

