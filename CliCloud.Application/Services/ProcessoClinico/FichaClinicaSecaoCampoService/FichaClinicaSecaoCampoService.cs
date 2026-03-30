using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Filters;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService
{
    public class FichaClinicaSecaoCampoService : IFichaClinicaSecaoCampoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public FichaClinicaSecaoCampoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // get full list by separador
        public async Task<Response<IEnumerable<FichaClinicaSecaoCampoDTO>>> GetFichaClinicaSecaoCampoAsync(
            Guid separadorId,
            string keyword = ""
        )
        {
            FichaClinicaSecaoCampoSearchList specification = new(separadorId, keyword);
            IEnumerable<FichaClinicaSecaoCampoDTO> list =
                await _repository.GetListAsync<FichaClinicaSecaoCampo, FichaClinicaSecaoCampoDTO, Guid>(
                    specification
                );
            return ResponseFactory.Success<IEnumerable<FichaClinicaSecaoCampoDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FichaClinicaSecaoCampoDTO>> GetFichaClinicaSecaoCampoPaginatedAsync(
            FichaClinicaSecaoCampoTableFilter filter
        )
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder =
                filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;

            FichaClinicaSecaoCampoSearchTable specification =
                new(filter.SeparadorId, filter.Keyword, dynamicOrder);

            PaginatedResponse<FichaClinicaSecaoCampoDTO> pagedResponse =
                await _repository.GetPaginatedResultsAsync<
                    FichaClinicaSecaoCampo,
                    FichaClinicaSecaoCampoDTO,
                    Guid
                >(filter.PageNumber, filter.PageSize, specification);

            return pagedResponse;
        }

        // get single by Id
        public async Task<Response<FichaClinicaSecaoCampoDTO>> GetFichaClinicaSecaoCampoAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoCampoDTO dto =
                    await _repository.GetByIdAsync<FichaClinicaSecaoCampo, FichaClinicaSecaoCampoDTO, Guid>(
                        id
                    );
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FichaClinicaSecaoCampoDTO>(ex.Message);
            }
        }

        // create new
        public async Task<Response<Guid>> CreateFichaClinicaSecaoCampoAsync(
            CreateFichaClinicaSecaoCampoRequest request
        )
        {
            FichaClinicaSecaoCampoMatchName specification =
                new(request.SeparadorId, request.Nome);

            bool exists = await _repository.ExistsAsync<FichaClinicaSecaoCampo, Guid>(specification);
            if (exists)
            {
                return ResponseFactory.Fail<Guid>(
                    "Já existe um campo com este nome neste separador."
                );
            }

            FichaClinicaSecaoCampo entity = _mapper.Map<FichaClinicaSecaoCampo>(request);

            try
            {
                FichaClinicaSecaoCampo response =
                    await _repository.CreateAsync<FichaClinicaSecaoCampo, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update
        public async Task<Response<Guid>> UpdateFichaClinicaSecaoCampoAsync(
            UpdateFichaClinicaSecaoCampoRequest request,
            Guid id
        )
        {
            FichaClinicaSecaoCampo entityInDb =
                await _repository.GetByIdAsync<FichaClinicaSecaoCampo, Guid>(id);
            if (entityInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Campo de separador não encontrado.");
            }

            _ = _mapper.Map(request, entityInDb);

            try
            {
                FichaClinicaSecaoCampo response =
                    await _repository.UpdateAsync<FichaClinicaSecaoCampo, Guid>(entityInDb);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete single
        public async Task<Response<Guid>> DeleteFichaClinicaSecaoCampoAsync(Guid id)
        {
            try
            {
                FichaClinicaSecaoCampo? entity =
                    await _repository.RemoveByIdAsync<FichaClinicaSecaoCampo, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoCampoAsync(
            IEnumerable<Guid> ids
        )
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
                        FichaClinicaSecaoCampo? entity =
                            await _repository.GetByIdAsync<FichaClinicaSecaoCampo, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add(
                                $"Campo de separador com ID {id} não encontrado."
                            );
                            continue;
                        }

                        FichaClinicaSecaoCampo? deletedEntity =
                            await _repository.RemoveByIdAsync<FichaClinicaSecaoCampo, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add(
                                $"Falha ao eliminar campo de separador com ID {id}."
                            );
                        }
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add(
                            $"Erro ao eliminar campo de separador com ID {id}."
                        );
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
                        $"Eliminados {successfullyDeletedIds.Count} de {idsList.Count} campos."
                    );
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(
                        string.Join("; ", failedDeletions)
                    );
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

