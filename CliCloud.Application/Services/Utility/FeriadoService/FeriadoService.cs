using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.FeriadoService.DTOs;
using CliCloud.Application.Services.Utility.FeriadoService.Filters;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.FeriadoService;

public class FeriadoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService
) : IFeriadoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    private async Task<Guid> ObterClinicaAtualIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if (
            Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId)
            && clinicaId != Guid.Empty
        )
        {
            return clinicaId;
        }

        throw new InvalidOperationException("Clínica atual inválida.");
    }

    public async Task<Response<IEnumerable<FeriadoDTO>>> GetTodosAsync(string keyword = "")
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
            IEnumerable<Feriado> query = todos.Where(x => x.ClinicaId == clinicaId);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string k = keyword.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    x.Designacao.ToLower().Contains(k) || x.Data.ToString("dd/MM/yyyy").Contains(k)
                );
            }

            List<FeriadoDTO> resultado = query
                .OrderBy(x => x.Data)
                .Select(_mapper.Map<FeriadoDTO>)
                .ToList();

            return ResponseFactory.Success<IEnumerable<FeriadoDTO>>(resultado);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<FeriadoDTO>>(ex.Message);
        }
    }

    public async Task<PaginatedResponse<FeriadoDTO>> GetPaginadoAsync(FeriadoTableFilter filter)
    {
        Guid clinicaId = await ObterClinicaAtualIdAsync();

        IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
        IQueryable<Feriado> query = todos.Where(x => x.ClinicaId == clinicaId).AsQueryable();

        if (filter.DataDe.HasValue)
        {
            query = query.Where(x => x.Data.Date >= filter.DataDe.Value.Date);
        }

        if (filter.DataAte.HasValue)
        {
            query = query.Where(x => x.Data.Date <= filter.DataAte.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(filter.Designacao))
        {
            string d = filter.Designacao.Trim().ToLowerInvariant();
            query = query.Where(x => x.Designacao.ToLower().Contains(d));
        }

        if (filter.Ativo.HasValue)
        {
            query = query.Where(x => x.Ativo == filter.Ativo.Value);
        }

        int total = query.Count();

        List<FeriadoDTO> itens = query
            .OrderBy(x => x.Data)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(_mapper.Map<FeriadoDTO>)
            .ToList();

        return new PaginatedResponse<FeriadoDTO>(itens, filter.PageNumber, filter.PageSize, total);
    }

    public async Task<Response<FeriadoDTO>> GetPorIdAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            Feriado? entity = await _repository.GetByIdAsync<Feriado, Guid>(id);
            if (entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<FeriadoDTO>("Feriado não encontrado.");
            }

            return ResponseFactory.Success(_mapper.Map<FeriadoDTO>(entity));
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<FeriadoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CriarAsync(CreateFeriadoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
            bool existeData = todos.Any(x => x.ClinicaId == clinicaId && x.Data.Date == request.Data.Date);

            if (existeData)
            {
                return ResponseFactory.Fail<Guid>("Já existe um feriado para essa data.");
            }

            Feriado novo = new()
            {
                Id = Guid.NewGuid(),
                ClinicaId = clinicaId,
                Data = request.Data.Date,
                Designacao = request.Designacao.Trim(),
                Ativo = true
            };

            _ = await _repository.CreateAsync<Feriado, Guid>(novo);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(novo.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> AtualizarAsync(UpdateFeriadoRequest request, Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
            Feriado? entity = todos.FirstOrDefault(x => x.Id == id && x.ClinicaId == clinicaId);

            if (entity == null)
            {
                return ResponseFactory.Fail<Guid>("Feriado não encontrado.");
            }

            bool existeData = todos.Any(x =>
                x.ClinicaId == clinicaId && x.Id != id && x.Data.Date == request.Data.Date.Date
            );

            if (existeData)
            {
                return ResponseFactory.Fail<Guid>("Já existe um feriado para essa data.");
            }

            entity.Data = request.Data.Date;
            entity.Designacao = request.Designacao.Trim();
            entity.Ativo = request.Ativo;

            _ = await _repository.UpdateAsync<Feriado, Guid>(entity);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> ApagarAsync(Guid id)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            Feriado? entity = await _repository.GetByIdAsync<Feriado, Guid>(id);
            if (entity == null || entity.ClinicaId != clinicaId)
            {
                return ResponseFactory.Fail<Guid>("Feriado não encontrado.");
            }

            _ = await _repository.RemoveByIdAsync<Feriado, Guid>(id);
            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success(id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Guid>>> ApagarEmLoteAsync(IEnumerable<Guid> ids)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();
            HashSet<Guid> idsSet = ids.Distinct().ToHashSet();

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
            List<Feriado> apagar = todos
                .Where(x => x.ClinicaId == clinicaId && idsSet.Contains(x.Id))
                .ToList();

            foreach (Feriado item in apagar)
            {
                _ = await _repository.RemoveByIdAsync<Feriado, Guid>(item.Id);
            }

            _ = await _repository.SaveChangesAsync();

            return ResponseFactory.Success<IEnumerable<Guid>>(apagar.Select(x => x.Id).ToList());
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
        }
    }

    public async Task<Response<int>> InserirAnoAsync(InsertFeriadosAnoRequest request)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            List<(DateTime Data, string Nome)> feriadosAno = ObterFeriadosDoAno(request.Ano);
            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();

            int criados = 0;

            foreach ((DateTime data, string nome) in feriadosAno)
            {
                bool existe = todos.Any(x => x.ClinicaId == clinicaId && x.Data.Date == data.Date);
                if (existe)
                {
                    continue;
                }

                Feriado novo = new()
                {
                    Id = Guid.NewGuid(),
                    ClinicaId = clinicaId,
                    Data = data.Date,
                    Designacao = nome,
                    Ativo = true
                };

                _ = await _repository.CreateAsync<Feriado, Guid>(novo);
                todos = todos.Append(novo).ToList();
                criados++;
            }

            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(criados);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<int>(ex.Message);
        }
    }

    public async Task<Response<int>> ImportarAsync(ImportFeriadosRequest request)
    {
        try
        {
            Guid clinicaDestinoId = await ObterClinicaAtualIdAsync();

            if (request.ClinicaOrigemId == clinicaDestinoId)
            {
                return ResponseFactory.Fail<int>(
                    "A clínica de origem não pode ser a mesma da clínica atual."
                );
            }

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();

            List<Feriado> origem = todos.Where(x => x.ClinicaId == request.ClinicaOrigemId).ToList();
            HashSet<DateTime> datasDestino = todos
                .Where(x => x.ClinicaId == clinicaDestinoId)
                .Select(x => x.Data.Date)
                .ToHashSet();

            int criados = 0;

            foreach (Feriado src in origem)
            {
                if (datasDestino.Contains(src.Data.Date))
                {
                    continue;
                }

                Feriado novo = new()
                {
                    Id = Guid.NewGuid(),
                    ClinicaId = clinicaDestinoId,
                    Data = src.Data.Date,
                    Designacao = src.Designacao,
                    Ativo = src.Ativo
                };

                _ = await _repository.CreateAsync<Feriado, Guid>(novo);
                datasDestino.Add(novo.Data.Date);
                criados++;
            }

            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(criados);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<int>(ex.Message);
        }
    }

    public async Task<Response<bool>> VerificarSeEFeriadoAsync(DateTime data)
    {
        try
        {
            Guid clinicaId = await ObterClinicaAtualIdAsync();

            IEnumerable<Feriado> todos = await _repository.GetListAsync<Feriado, Guid>();
            bool eFeriado = todos.Any(x => x.ClinicaId == clinicaId && x.Data.Date == data.Date);

            return ResponseFactory.Success(eFeriado);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<bool>(ex.Message);
        }
    }

    private static List<(DateTime Data, string Nome)> ObterFeriadosDoAno(int ano)
    {
        List<(DateTime, string)> lista =
        [
            (new DateTime(ano, 1, 1), "Ano Novo"),
            (new DateTime(ano, 4, 25), "Dia da Liberdade"),
            (new DateTime(ano, 5, 1), "Dia do Trabalhador"),
            (new DateTime(ano, 6, 10), "Dia de Portugal"),
            (new DateTime(ano, 8, 15), "Assunção de Nossa Senhora"),
            (new DateTime(ano, 10, 5), "Implantação da República"),
            (new DateTime(ano, 11, 1), "Dia de Todos os Santos"),
            (new DateTime(ano, 12, 1), "Restauração da Independência"),
            (new DateTime(ano, 12, 8), "Imaculada Conceição"),
            (new DateTime(ano, 12, 25), "Natal")
        ];

        DateTime pascoa = EasterCalculator.ObterDomingoPascoa(ano);
        lista.Add((pascoa.AddDays(-2), "Sexta-Feira Santa"));
        lista.Add((pascoa.AddDays(60), "Corpo de Deus"));

        return lista;
    }
}
