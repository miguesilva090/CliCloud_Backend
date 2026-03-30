using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Filters;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService
{
    public class FichaClinicaSecaoTemplateService : IFichaClinicaSecaoTemplateService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public FichaClinicaSecaoTemplateService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full List
        public async Task<Response<IEnumerable<FichaClinicaSecaoTemplateDTO>>> GetFichaClinicaSecaoTemplateAsync(string keyword = "")
        {
            FichaClinicaSecaoTemplateSearchList specification = new(keyword);
            IEnumerable<FichaClinicaSecaoTemplateDTO> list =
                await _repository.GetListAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FichaClinicaSecaoTemplateDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplatePaginatedAsync(
            FichaClinicaSecaoTemplateTableFilter filter
        )
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = (filter.Sorting != null) ? GSHelpers.GenerateOrderByString(filter) : "";
            FichaClinicaSecaoTemplateSearchTable specification =
                new(filter.Keyword, dynamicOrder);

            PaginatedResponse<FichaClinicaSecaoTemplateDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(
                    filter.PageNumber,
                    filter.PageSize,
                    specification
                );

            return pagedResponse;
        }

        // get single by Id
        public async Task<Response<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplateAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoTemplateDTO dto =
                    await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FichaClinicaSecaoTemplateDTO>(ex.Message);
            }
        }

        // create new
        public async Task<Response<Guid>> CreateFichaClinicaSecaoTemplateAsync(CreateFichaClinicaSecaoTemplateRequest request)
        {
            FichaClinicaSecaoTemplateMatchCodigo specification = new(request.Codigo);
            bool exists = await _repository.ExistsAsync<FichaClinicaSecaoTemplate, Guid>(specification);
            if (exists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um template com este código.");
            }

            FichaClinicaSecaoTemplate entity = _mapper.Map<FichaClinicaSecaoTemplate>(request);

            try
            {
                FichaClinicaSecaoTemplate response =
                    await _repository.CreateAsync<FichaClinicaSecaoTemplate, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update
        public async Task<Response<Guid>> UpdateFichaClinicaSecaoTemplateAsync(
            UpdateFichaClinicaSecaoTemplateRequest request,
            Guid id
        )
        {
            FichaClinicaSecaoTemplate entityInDb =
                await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
            if (entityInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Template de secção de ficha clínica não encontrado.");
            }

            _ = _mapper.Map(request, entityInDb);

            try
            {
                FichaClinicaSecaoTemplate response =
                    await _repository.UpdateAsync<FichaClinicaSecaoTemplate, Guid>(entityInDb);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete single
        public async Task<Response<Guid>> DeleteFichaClinicaSecaoTemplateAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoTemplate? entity =
                    await _repository.RemoveByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoTemplateAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = new();
                List<string> failedDeletions = new();

                foreach (Guid id in idsList)
                {
                    try
                    {
                        FichaClinicaSecaoTemplate? entity =
                            await _repository.GetByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Template de ficha clínica com ID {id} não encontrado.");
                            continue;
                        }

                        FichaClinicaSecaoTemplate? deletedEntity =
                            await _repository.RemoveByIdAsync<FichaClinicaSecaoTemplate, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar template de ficha clínica com ID {id}.");
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Erro ao eliminar template de ficha clínica com ID {id}.");
                        _repository.ClearChangeTracker();
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
                        $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} templates de ficha clínica."
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

