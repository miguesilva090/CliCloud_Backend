using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Sinistros.SinistradoService.DTOs;
using CliCloud.Application.Services.Sinistros.SinistradoService.Filters;
using CliCloud.Application.Services.Sinistros.SinistradoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Sinistros.SinistradoService
{
    public class SinistradoService(IRepositoryAsync repository, IMapper mapper) : ISinistradoService 
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;
        private const int CodigoServicoMaxLength = 30;
        private const int DesignacaoServicoMaxLength = 160;

        public Task<PaginatedResponse<SinistradoTableDTO>> GetPaginatedAsync(SinistradoTableFilter filter)
        {
            if (filter.Filters?.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new SinistradoSearchTable(filter.Filters ?? [], order);
            return _repository.GetPaginatedResultsAsync<Sinistrado, SinistradoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<SinistradoDTO>> GetByIdAsync(Guid id)
        {
            var dto = await _repository.GetByIdAsync<Sinistrado, SinistradoDTO, Guid>(id);
            return ResponseFactory.Success(dto);
        }

        public async Task<Response<Guid>> CreateAsync(CreateSinistradoRequest request)
        {
            var entity = _mapper.Map<Sinistrado>(request);
            entity.Id = Guid.NewGuid();
            NormalizeServiceLines(entity.LinhasServico);

            foreach(var linha in entity.LinhasServico)
                linha.Id = Guid.NewGuid();

            await _repository.CreateAsync<Sinistrado, Guid>(entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateSinistradoRequest request)
        {
            var entity = await _repository.GetByIdAsync<Sinistrado, Guid>(id);
            
            _mapper.Map(request, entity);
            NormalizeServiceLines(entity.LinhasServico);
            await _repository.UpdateAsync<Sinistrado, Guid>(entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> MoveToHistoryAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync<Sinistrado, Guid>(id);
            entity.Historico = true;
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> RestoreFromHistoryAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync<Sinistrado, Guid>(id);
            entity.Historico = false;
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Sinistrado, Guid>(id);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }

        public async Task<Response<string>> GetNextCodigoSinistroAsync()
        {
            var all = await _repository.GetListAsync<Sinistrado, Guid>();
            int maxCodigo = 0;

            foreach (var item in all)
            {
                if (int.TryParse(item.CodigoSinistro?.Trim(), out var codigo) && codigo > maxCodigo)
                {
                    maxCodigo = codigo;
                }
            }

            return ResponseFactory.Success((maxCodigo + 1).ToString());
        }

        public async Task<Response<List<SinistradoLinhaServicoDTO>>> GetUnbilledServicesByUtenteIdAsync(Guid utenteId)
        {
            var consultasBase = (await _repository.GetListAsync<Consulta, ConsultaServicoBaseRowDTO, Guid>(
                new ConsultasBaseByUtenteSpec(utenteId))).ToList();

            HashSet<Guid> consultasFaturadas = [];
            foreach (var consultaId in consultasBase.Select(x => x.Id))
            {
                var faturadasConsulta = await _repository.GetListAsync<ConsultaFaturacao, Guid>(
                    new ConsultaFaturadaByConsultaIdSpec(consultaId));
                if (faturadasConsulta.Any())
                {
                    consultasFaturadas.Add(consultaId);
                }
            }

            var consultasNaoFaturadas = consultasBase
                .Where(x => !consultasFaturadas.Contains(x.Id))
                .ToList();

            var tratamentosBase = (await _repository.GetListAsync<Tratamento, TratamentoServicoBaseRowDTO, Guid>(
                new TratamentosBaseByUtenteSpec(utenteId))).ToList();

            List<SinistradoLinhaServicoDTO> linhas = [];

            foreach (var consulta in consultasNaoFaturadas)
            {
                var servicos = (await _repository.GetListAsync<ServicoConsulta, Guid>(
                    new ServicosConsultaByConsultaIdSpec(consulta.Id))).ToList();
                if (servicos.Count > 0)
                {
                    foreach (var servico in servicos)
                    {
                        linhas.Add(new SinistradoLinhaServicoDTO
                        {
                            CodigoServico = servico.ServicoId?.ToString() ?? servico.CodigoArtigo ?? $"CONS-{consulta.Id:N}",
                            DesignacaoServico = servico.Servico?.Designacao ?? servico.NomeArtigo ?? consulta.TipoConsultaDesignacao ?? "Consulta",
                            Quantidade = Math.Max(1, Convert.ToInt32(servico.Quantidade ?? 1)),
                            ValorServico = servico.ValorServico ?? servico.ValorArtigo,
                            ValorContratado = servico.ValorUt ?? servico.ValorServico ?? servico.ValorArtigo,
                            DataServico = consulta.Data,
                            AdmissaoId = consulta.AdmissaoId
                        });
                    }
                }
                else
                {
                    linhas.Add(new SinistradoLinhaServicoDTO
                    {
                        CodigoServico = $"CONS-{consulta.Id:N}",
                        DesignacaoServico = consulta.TipoConsultaDesignacao ?? "Consulta",
                        Quantidade = 1,
                        DataServico = consulta.Data,
                        AdmissaoId = consulta.AdmissaoId
                    });
                }
            }

            foreach (var tratamento in tratamentosBase)
            {
                var servicos = (await _repository.GetListAsync<ServicoTratamento, Guid>(
                    new ServicosTratamentoByTratamentoIdSpec(tratamento.Id))).ToList();
                if (servicos.Count > 0)
                {
                    foreach (var servico in servicos)
                    {
                        linhas.Add(new SinistradoLinhaServicoDTO
                        {
                            CodigoServico = servico.ServicoId?.ToString() ?? $"TRAT-{tratamento.Id:N}",
                            DesignacaoServico = servico.Servico?.Designacao ?? tratamento.Designacao ?? "Tratamento",
                            Quantidade = 1,
                            ValorServico = servico.Preco,
                            ValorContratado = servico.ValorUt ?? servico.Preco,
                            DataServico = tratamento.DataServico,
                            TratamentoId = tratamento.Id
                        });
                    }
                }
                else
                {
                    linhas.Add(new SinistradoLinhaServicoDTO
                    {
                        CodigoServico = $"TRAT-{tratamento.Id:N}",
                        DesignacaoServico = tratamento.Designacao ?? "Tratamento",
                        Quantidade = 1,
                        DataServico = tratamento.DataServico,
                        TratamentoId = tratamento.Id
                    });
                }
            }

            return ResponseFactory.Success(linhas
                .OrderByDescending(x => x.DataServico ?? DateTime.MinValue)
                .ToList());
        }

        private static void NormalizeServiceLines(IEnumerable<SinistradoLinhaServico> linhas)
        {
            foreach (var linha in linhas)
            {
                var sourceCode = string.IsNullOrWhiteSpace(linha.CodigoServico)
                    ? $"SERV-{Guid.NewGuid():N}"
                    : linha.CodigoServico.Trim();

                linha.CodigoServico = sourceCode.Length > CodigoServicoMaxLength
                    ? sourceCode[..CodigoServicoMaxLength]
                    : sourceCode;

                if (!string.IsNullOrWhiteSpace(linha.DesignacaoServico) &&
                    linha.DesignacaoServico.Length > DesignacaoServicoMaxLength)
                {
                    linha.DesignacaoServico = linha.DesignacaoServico[..DesignacaoServicoMaxLength];
                }
            }
        }
    }
}