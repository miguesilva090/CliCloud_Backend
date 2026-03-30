using AutoMapper;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications;
using CliCloud.Domain.Entities.Utentes;

// After creating this service:
// -- 1. Create a EvolucaoTratamento domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<EvolucaoTratamento> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a EvolucaoTratamento api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService
{
    public class EvolucaoTratamentoService : IEvolucaoTratamentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public EvolucaoTratamentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<EvolucaoTratamentoDTO>>> GetEvolucaoTratamentoAsync(string keyword = "")
        {
            EvolucaoTratamentoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EvolucaoTratamentoDTO> list = await _repository.GetListAsync<EvolucaoTratamento, EvolucaoTratamentoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EvolucaoTratamentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<EvolucaoTratamentoLightDTO>>> GetEvolucaoTratamentoLightAsync(string keyword = "")
        {
            EvolucaoTratamentoSearchList specification = new(keyword);
            IEnumerable<EvolucaoTratamentoLightDTO> list = await _repository.GetListAsync<EvolucaoTratamento, EvolucaoTratamentoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EvolucaoTratamentoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<EvolucaoTratamentoTableDTO>> GetEvolucaoTratamentoPaginatedAsync(EvolucaoTratamentoTableFilter filter)
        {
            filter ??= new EvolucaoTratamentoTableFilter();
            List<TableFilter> filters = filter.Filters ?? [];

            if (filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            EvolucaoTratamentoSearchTable specification = new(filters, dynamicOrder); // ardalis specification
            PaginatedResponse<EvolucaoTratamentoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EvolucaoTratamento, EvolucaoTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all EvolucaoTratamentos (non-paginated)
        public async Task<Response<IEnumerable<EvolucaoTratamentoTableDTO>>> GetAllEvolucaoTratamentoAsync(EvolucaoTratamentoAllFilter filter)
        {
            try
            {
                filter ??= new EvolucaoTratamentoAllFilter();
                string order = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EvolucaoTratamentoSearchTable specification = new(tableFilters, order);
                IEnumerable<EvolucaoTratamentoTableDTO> list = await _repository.GetListAsync<EvolucaoTratamento, EvolucaoTratamentoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EvolucaoTratamentoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EvolucaoTratamentoTableDTO>>(ex.Message);
            }
        }

        // get single EvolucaoTratamento by Id 
        public async Task<Response<EvolucaoTratamentoDTO>> GetEvolucaoTratamentoAsync(Guid id)
        {
            try
            {
                EvolucaoTratamentoDTO dto = await _repository.GetByIdAsync<EvolucaoTratamento, EvolucaoTratamentoDTO, Guid>(id);
                return ResponseFactory.Success<EvolucaoTratamentoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EvolucaoTratamentoDTO>(ex.Message);
            }
        }

        //get single EvolucaoTratamento by Descricao (exact match)
        public async Task<Response<EvolucaoTratamentoDTO>> GetEvolucaoTratamentoByDescricaoAsync(string descricao)
        {
            try
            {
                if( string.IsNullOrWhiteSpace(descricao))
                {
                    return ResponseFactory.Fail<EvolucaoTratamentoDTO>("Descricao não pode ser vazia");
                }

                EvolucaoTratamentoMatchDescricao specification = new(descricao);
                IEnumerable<EvolucaoTratamentoDTO> results = await _repository.GetListAsync<EvolucaoTratamento, EvolucaoTratamentoDTO, Guid>(specification);

                EvolucaoTratamentoDTO? evolucaoTratamento = results.FirstOrDefault();
                if(evolucaoTratamento == null)
                {
                    return ResponseFactory.Fail<EvolucaoTratamentoDTO>("Não foi encontrada nenhuma Evolucao de Tratamento com a descrição fornecida");
                }

                return ResponseFactory.Success<EvolucaoTratamentoDTO>(evolucaoTratamento);
            }
            catch( Exception ex)
            {
                return ResponseFactory.Fail<EvolucaoTratamentoDTO>(ex.Message);
            }
        }

        // create new EvolucaoTratamento
        public async Task<Response<Guid>> CreateEvolucaoTratamentoAsync(CreateEvolucaoTratamentoRequest request)
        {
            EvolucaoTratamento newEvolucaoTratamento = _mapper.Map(request, new EvolucaoTratamento()); // map dto to domain entity

            try
            {
                EvolucaoTratamento response = await _repository.CreateAsync<EvolucaoTratamento, Guid>(newEvolucaoTratamento); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update EvolucaoTratamento
        public async Task<Response<Guid>> UpdateEvolucaoTratamentoAsync(UpdateEvolucaoTratamentoRequest request, Guid id)
        {
            EvolucaoTratamento EvolucaoTratamentoInDb = await _repository.GetByIdAsync<EvolucaoTratamento, Guid>(id); // get existing entity
            if (EvolucaoTratamentoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            EvolucaoTratamento updatedEvolucaoTratamento = _mapper.Map(request, EvolucaoTratamentoInDb); // map dto to domain entity

            try
            {
                EvolucaoTratamento response = await _repository.UpdateAsync<EvolucaoTratamento, Guid>(updatedEvolucaoTratamento);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete EvolucaoTratamento
        public async Task<Response<Guid>> DeleteEvolucaoTratamentoAsync(Guid id)
        {
            try
            {
                EvolucaoTratamento? EvolucaoTratamento = await _repository.RemoveByIdAsync<EvolucaoTratamento, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(EvolucaoTratamento.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple EvolucaoTratamentos 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEvolucaoTratamentoAsync(IEnumerable<Guid> ids)
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
                        EvolucaoTratamento? entity = await _repository.GetByIdAsync<EvolucaoTratamento, Guid>(id);
                        if(entity == null)
                        {
                            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                            continue;
                        }

                        EvolucaoTratamento? deletedEntity = await _repository.RemoveByIdAsync<EvolucaoTratamento, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else 
                        {
                            failedDeletions.Add($"EvolucaoTratamento com ID {id} não encontrado.");
                        }
                    }
                    catch(Exception)
                    {
                        return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                        _repository.ClearChangeTracker();
                    }
                }
                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} Evoluções de Tratamento.");
                }

                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }

        public async Task<Response<EvolucaoTratamentoReportDTO>> GetEvolucaoTratamentoReportAsync(Guid id)
        {
            try
            {
                EvolucaoTratamento? evolucao = await _repository.GetByIdAsync<EvolucaoTratamento, Guid>(id);

                if(evolucao == null)
                {
                    return ResponseFactory.Fail<EvolucaoTratamentoReportDTO>($"Nenhum registo de Evolução de Tratamento encontrado para o ID {id}");
                }

                Tratamento? tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(evolucao.TratamentoId);
                Utente? utente = await _repository.GetByIdAsync<Utente,Guid>(evolucao.UtenteId);

                EvolucaoTratamentoReportDTO dto = new()
                {
                    Id = evolucao.Id,
                    TratamentoId = evolucao.TratamentoId,
                    UtenteId = evolucao.UtenteId,
                    UtenteNome = utente?.Nome,
                    UtenteNumero = utente?.NumeroUtente,
                    TratamentoDesignacao = tratamento?.NomePatologia,
                    OrganismoNome = tratamento?.Organismo?.Nome,
                    NumeroSessoes = tratamento?.NumSessao,
                    PacienteInformadoInicial = evolucao.PacienteInformadoInicial,
                    PacienteMotivadoInicial = evolucao.PacienteMotivadoInicial,
                    PacienteColaboranteInicial = evolucao.PacienteColaboranteInicial,
                    AvaliacaoSubjetivaInicial = evolucao.AvaliacaoSubjetivaInicial,
                    ObservacoesAvaliacaoInicial = evolucao.ObservacoesAvaliacaoInicial,
                    TipoInicioDorInicial = evolucao.TipoInicioDorInicial,
                    ValorDorInicial = evolucao.ValorDorInicial,
                    TipoDorInicial = evolucao.TipoDorInicial,
                    ExameFisicoRegiaoInicial = evolucao.ExameFisicoRegiaoInicial,
                    PatologiaInicial = evolucao.PatologiaInicial,
                    EdemaInicial = evolucao.EdemaInicial,
                    ObservacoesEdemaInicial = evolucao.ObservacoesEdemaInicial,
                    ElasticidadeInicial = evolucao.ElasticidadeInicial,
                    ObservacoesElasticidadeInicial = evolucao.ObservacoesElasticidadeInicial,
                    ParestesiasInicial = evolucao.ParestesiasInicial,
                    DorIrradiadaInicial = evolucao.DorIrradiadaInicial,
                    CicatrizInicial = evolucao.CicatrizInicial,
                    FraquezaMuscularInicial = evolucao.FraquezaMuscularInicial,
                    ZonaFraquezaInicialDescricao = evolucao.ZonaFraquezaInicialDescricao,
                    MarchaAutonomaInicial = evolucao.MarchaAutonomaInicial,
                    ObservacoesMarchaAutonomaInicial = evolucao.ObservacoesMarchaAutonomaInicial,
                    GoniometriaInicial = evolucao.GoniometriaInicial,
                    TesteMuscularInicial = evolucao.TesteMuscularInicial,
                    AutonomiaInicial = evolucao.AutonomiaInicial,
                    ObjetivosEspecificosInicial = evolucao.ObjetivosEspecificosInicial,
                    SessoesPropostasInicial = evolucao.SessoesPropostasInicial,
                    TempoNecessarioInicial = evolucao.TempoNecessarioInicial,
                    PacienteInformadoFinal = evolucao.PacienteInformadoFinal,
                    PacienteMotivadoFinal = evolucao.PacienteMotivadoFinal,
                    PacienteColaboranteFinal = evolucao.PacienteColaboranteFinal,
                    AvaliacaoSubjetivaFinal = evolucao.AvaliacaoSubjetivaFinal,
                    ObservacoesAvaliacaoFinal = evolucao.ObservacoesAvaliacaoFinal,
                    TipoInicioDorFinal = evolucao.TipoInicioDorFinal,
                    ValorDorFinal = evolucao.ValorDorFinal,
                    TipoDorFinal = evolucao.TipoDorFinal,
                    ExameFisicoRegiaoFinalId = evolucao.ExameFisicoRegiaoFinalId,
                    PatologiaFinalId = evolucao.PatologiaFinalId,
                    EdemaFinal = evolucao.EdemaFinal,
                    ObservacoesEdemaFinal = evolucao.ObservacoesEdemaFinal,
                    ElasticidadeFinal = evolucao.ElasticidadeFinal,
                    ObservacoesElasticidadeFinal = evolucao.ObservacoesElasticidadeFinal,
                    ParestesiasFinal = evolucao.ParestesiasFinal,
                    DorIrradiadaFinal = evolucao.DorIrradiadaFinal,
                    CicatrizFinal = evolucao.CicatrizFinal,
                    FraquezaMuscularFinal = evolucao.FraquezaMuscularFinal,
                    ZonaFraquezaFinalDescricao = evolucao.ZonaFraquezaFinalDescricao,
                    MarchaAutonomaFinal = evolucao.MarchaAutonomaFinal,
                    ObservacoesMarchaAutonomaFinal = evolucao.ObservacoesMarchaAutonomaFinal,
                    GoniometriaFinal = evolucao.GoniometriaFinal,
                    TesteMuscularFinal = evolucao.TesteMuscularFinal,
                    AutonomiaFinal = evolucao.AutonomiaFinal,
                    ObjetivosAlcancados = evolucao.ObjetivosAlcancados,
                    NovosObjetivos = evolucao.NovosObjetivos,
                    SessoesPropostasFinal = evolucao.SessoesPropostasFinal,
                    TempoNecessarioFinal = evolucao.TempoNecessarioFinal,
                    DataAlta = evolucao.DataAlta,
                    MotivoAltaId = evolucao.MotivoAltaId,
                    EscalaDorAlta = evolucao.EscalaDorAlta,
                    IndicacoesParaUtenteAlta = evolucao.IndicacoesParaUtenteAlta,
                    ObservacaoClinica = evolucao.ObservacaoClinica,

                };

                return ResponseFactory.Success(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<EvolucaoTratamentoReportDTO>(ex.Message);
            }
        }
    }
}

