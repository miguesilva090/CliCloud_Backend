using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Filters;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Helpers;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Specifications;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService
{
  public class ReceitaMedicaService : IReceitaMedicaService
  {
    /// <summary>Código de sucesso SPMS (igual a SpmsPrescricaoSoapService.OperacaoSucesso).</summary>
    private const string SpmsOperacaoSucesso = "100006010001";

    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly ISpmsPrescricaoSoapService _spms;

    public ReceitaMedicaService(
      IRepositoryAsync repository,
      IMapper mapper,
      ISpmsPrescricaoSoapService spms)
    {
      _repository = repository;
      _mapper = mapper;
      _spms = spms;
    }

    public async Task<PaginatedResponse<ReceitaMedicaTableDTO>> GetPaginatedAsync(ReceitaMedicaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ReceitaMedicaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ReceitaMedica, ReceitaMedicaTableDTO, Guid>(
        filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<ReceitaMedicaDTO>> GetByIdAsync(Guid id)
    {
      try
      {
        var entity = await _repository.GetByIdAsync<ReceitaMedica, Guid>(id, new ReceitaMedicaByIdWithLinhas(id));
        return ResponseFactory.Success(_mapper.Map<ReceitaMedicaDTO>(entity));
      }
      catch (InvalidOperationException)
      {
        return ResponseFactory.Fail<ReceitaMedicaDTO>("A receita médica não existe");
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ReceitaMedicaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAsync(CreateReceitaMedicaRequest request)
    {
      try
      {
        var entity = _mapper.Map<ReceitaMedica>(request);
        entity.NumeroReceitaLocal = $"LOC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6]}";
        entity.Enviada = 0;
        entity.Anulada = 0;
        entity.EstadoEnvio = 0;
        entity.Desmaterializada = 1;

        if (string.IsNullOrWhiteSpace(entity.LocalPrescricao))
        {
          try
          {
            var clinica = await _repository.GetByIdAsync<Clinica, Guid>(request.ClinicaId);
            entity.LocalPrescricao = clinica.LocalPrescricao;
          }
          catch
          {
            /* ignore */
          }
        }

        foreach (var linha in entity.Linhas)
          linha.DataValidade = CalcularValidade(request.DataPrescricao, linha.CodValidade);

        SyncPrescricaoPorNomeHeader(entity);

        var created = await _repository.CreateAsync<ReceitaMedica, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateReceitaMedicaRequest request)
    {
      try
      {
        var existing = await _repository.GetByIdAsync<ReceitaMedica, Guid>(id, new ReceitaMedicaByIdWithLinhas(id));
        if (existing.Anulada == 1)
          return ResponseFactory.Fail<Guid>("Atenção, a sua receita já foi anulada!");
        if (existing.Enviada == 1)
          return ResponseFactory.Fail<Guid>("A receita já foi gravada");

        _mapper.Map(request, existing);

        // Soft-delete das linhas antigas (Clear() com FK obrigatória lança InvalidOperationException)
        foreach (var antiga in existing.Linhas.ToList())
          await _repository.RemoveAsync<ReceitaLinha, Guid>(antiga);

        foreach (var req in request.Linhas)
        {
          existing.Linhas.Add(new ReceitaLinha
          {
            ReceitaMedicaId = id,
            Ordem = req.Ordem,
            TipoLinha = req.TipoLinha,
            EmbId = req.EmbId,
            Cnpem = req.Cnpem,
            Designacao = req.Designacao,
            DescricaoEmbalagem = req.DescricaoEmbalagem,
            Quantidade = req.Quantidade,
            Pvp = req.Pvp,
            Comparticipacao = req.Comparticipacao,
            ValorUtente = req.ValorUtente,
            Posologia = req.Posologia,
            PosologiaQuantidadeUnidade = req.PosologiaQuantidadeUnidade,
            PosologiaQuantidadeValor = req.PosologiaQuantidadeValor,
            PosologiaFrequenciaUnidade = req.PosologiaFrequenciaUnidade,
            PosologiaFrequenciaValor = req.PosologiaFrequenciaValor,
            PosologiaDuracaoUnidade = req.PosologiaDuracaoUnidade,
            PosologiaDuracaoValor = req.PosologiaDuracaoValor,
            PosologiaInstrucoes = req.PosologiaInstrucoes,
            CodValidade = req.CodValidade,
            DataValidade = CalcularValidade(request.DataPrescricao, req.CodValidade),
            CodJustificacaoQuantidade = req.CodJustificacaoQuantidade,
            JustificacaoQuantidade = req.JustificacaoQuantidade,
            CodTipoPrescricao = req.CodTipoPrescricao,
            CodMotivo = req.CodMotivo,
            CodIndicacaoTerapeutica = req.CodIndicacaoTerapeutica,
            Diploma = req.Diploma,
          });
        }

        SyncPrescricaoPorNomeHeader(existing);

        var updated = await _repository.UpdateAsync<ReceitaMedica, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (InvalidOperationException ex) when (ex.Message.Contains("Não encontrado", StringComparison.OrdinalIgnoreCase))
      {
        return ResponseFactory.Fail<Guid>("A receita médica não existe");
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> EnviarAsync(Guid id, Guid clinicaId, EnviarReceitaMedicaRequest request)
    {
      try
      {
        var receita = await _repository.GetByIdAsync<ReceitaMedica, Guid>(
          id, new ReceitaMedicaByIdWithLinhas(id));

        if (receita.ClinicaId != clinicaId)
          return ResponseFactory.Fail<Guid>("A receita médica não pertence à clínica atual.");
        if (receita.Anulada == 1)
          return ResponseFactory.Fail<Guid>("Atenção, a sua receita já foi anulada!");
        // Legado GuardarDesmaterializada / send: já enviada
        if (receita.Enviada == 1)
          return ResponseFactory.Fail<Guid>("A receita já foi gravada");
        if (receita.Desmaterializada != 1)
          return ResponseFactory.Fail<Guid>("A receita não é desmaterializada");
        if (receita.Linhas == null || receita.Linhas.Count == 0)
          return ResponseFactory.Fail<Guid>("Atenção, a receita tem de conter pelo menos um medicamento.");

        if (string.IsNullOrWhiteSpace(receita.Medico?.GrupoFuncional) ||
            string.IsNullOrWhiteSpace(receita.Medico?.Carteira))
          return ResponseFactory.Fail<Guid>("Número da ordem do médico inválido.");

        if (string.IsNullOrWhiteSpace(receita.LocalPrescricao) &&
            string.IsNullOrWhiteSpace(receita.Clinica?.LocalPrescricao))
          return ResponseFactory.Fail<Guid>("Local de prescrição inválido.");

        if (string.IsNullOrWhiteSpace(request.TokenPrescritor))
          return ResponseFactory.Fail<Guid>("Autenticação do prescritor é obrigatória");

        // Paridade: GuardarDesmaterializada gera XML → EnviarDesmaterializadas envia
        var xml = ReceitaPemXmlBuilder.Build(receita);
        if (string.IsNullOrWhiteSpace(xml))
        {
          // Legado PrescricaoRSPSend sem XML → ApagarReceita
          _ = await _repository.RemoveByIdAsync<ReceitaMedica, Guid>(id);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Fail<Guid>(
            $"A receita com o n.º '{receita.NumeroReceitaLocal}' foi apagada devido ao erro: 'Falha de assinatura da receita'.");
        }

        var spmsResult = await _spms.ExecutarRegistoPrescricaoRspAsync(
          clinicaId,
          new RegistoPrescricaoRspRequest
          {
            CodigoOperacao = "REG",
            EnviadoEmUtc = DateTime.UtcNow,
            AtivadoEmUtc = DateTime.UtcNow,
            ChavePedido = $"RegistoPrescricaoMedicamentos-{receita.Id:D}",
            CorpoXml = xml,
            TokenPrescritor = request.TokenPrescritor.Trim(),
          });

        if (!IsSpmsOk(spmsResult))
        {
          var msg = FirstMessage(spmsResult)
            ?? spmsResult.Data?.Descricao
            ?? "Falha de assinatura da receita.";
          receita.EstadoEnvio = 3;
          receita.MensagemErro = msg;
          _ = await _repository.UpdateAsync<ReceitaMedica, Guid>(receita);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Fail<Guid>(
            $"A receita com o n.º '{receita.NumeroReceitaLocal}' foi marcada com erro: '{msg}'.");
        }

        // UpdateReceitaEnviada (legado)
        receita.Enviada = 1;
        receita.EstadoEnvio = 2;
        receita.MensagemErro = null;
        if (string.IsNullOrWhiteSpace(receita.NumeroReceita))
          receita.NumeroReceita = receita.NumeroReceitaLocal;

        _ = await _repository.UpdateAsync<ReceitaMedica, Guid>(receita);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(receita.Id);
      }
      catch (InvalidOperationException)
      {
        return ResponseFactory.Fail<Guid>("A receita médica não existe");
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> AnularAsync(Guid id, AnularReceitaMedicaRequest request, Guid clinicaId)
    {
      try
      {
        var existing = await _repository.GetByIdAsync<ReceitaMedica, Guid>(
          id, new ReceitaMedicaByIdWithLinhas(id));

        if (existing.ClinicaId != clinicaId)
          return ResponseFactory.Fail<Guid>("A receita médica não pertence à clínica atual.");
        if (existing.Anulada == 1)
          return ResponseFactory.Fail<Guid>("Atenção, a sua receita já foi anulada!");
        if (string.IsNullOrWhiteSpace(request.MotivoCodigo))
          return ResponseFactory.Fail<Guid>("Não existe o motivo de anulação.");

        // ValidarReceitaAnulacao — 30 dias
        if (existing.DataPrescricao.Date.AddDays(30) < DateTime.Today)
          return ResponseFactory.Fail<Guid>("Não pode anular… ultrapassou os 30 dias.");

        // Ainda não enviada → anulação só local (não há nº oficial SPMS)
        if (existing.Enviada == 0)
        {
          existing.Anulada = 1;
          existing.DataAnulacao = DateTime.UtcNow;
          existing.MotivoAnulacaoCodigo = request.MotivoCodigo;
          existing.MotivoAnulacaoDescricao = request.MotivoDescricao;
          _ = await _repository.UpdateAsync<ReceitaMedica, Guid>(existing);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(existing.Id);
        }

        existing.DataAnulacao = DateTime.UtcNow;
        existing.MotivoAnulacaoCodigo = request.MotivoCodigo;
        existing.MotivoAnulacaoDescricao = request.MotivoDescricao;

        var xmlAnulacao = ReceitaPemXmlBuilder.BuildAnulacao(existing, request);
        var spms = await _spms.ExecutarRegistoPrescricaoRspAsync(clinicaId, new RegistoPrescricaoRspRequest
        {
          CodigoOperacao = "ANU",
          EnviadoEmUtc = DateTime.UtcNow,
          AtivadoEmUtc = DateTime.UtcNow,
          ChavePedido = $"AnulacaoPrescricaoMedicamentos-{existing.Id:D}",
          CorpoXml = xmlAnulacao,
        });

        if (!IsSpmsOk(spms))
        {
          var msg = FirstMessage(spms) ?? spms.Data?.Descricao ?? "A receita não foi anulada.";
          // Reverter campos de anulação locais (legado em falha parcial)
          existing.Anulada = 0;
          existing.DataAnulacao = null;
          existing.MotivoAnulacaoCodigo = null;
          existing.MotivoAnulacaoDescricao = null;
          existing.MensagemErro = msg;
          _ = await _repository.UpdateAsync<ReceitaMedica, Guid>(existing);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Fail<Guid>(
            $"A receita com o n.º '{existing.NumeroReceita ?? existing.NumeroReceitaLocal}' não foi anulada devido ao seguinte motivo: '{msg}'.");
        }

        existing.Anulada = 1;
        _ = await _repository.UpdateAsync<ReceitaMedica, Guid>(existing);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(existing.Id);
      }
      catch (InvalidOperationException)
      {
        return ResponseFactory.Fail<Guid>("A receita médica não existe");
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static bool IsSpmsOk(Response<SpmsSoapOperationResultDTO> result)
    {
      if (result.Status != ResponseStatus.Success || result.Data is null)
        return false;
      // Alguns ambientes devolvem sucesso sem código; aceitar Success + Data
      if (string.IsNullOrWhiteSpace(result.Data.Codigo))
        return true;
      return string.Equals(result.Data.Codigo, SpmsOperacaoSucesso, StringComparison.Ordinal);
    }

    private static string? FirstMessage(Response result) =>
      result.Messages?.Values.SelectMany(x => x).FirstOrDefault();

    private static DateTime? CalcularValidade(DateTime dataPrescricao, int? codValidade) =>
      codValidade switch
      {
        1 => dataPrescricao.Date.AddDays(30),
        2 => dataPrescricao.Date.AddMonths(6),
        _ => dataPrescricao.Date.AddMonths(12),
      };

    /// <summary>
    /// Agrega no cabeçalho (legado PrescricaoPorNome / MotivoPrescricaoNome)
    /// a partir das linhas por nome comercial.
    /// </summary>
    private static void SyncPrescricaoPorNomeHeader(ReceitaMedica entity)
    {
      var porNome = (entity.Linhas ?? [])
        .Where(l => l.TipoLinha <= 3 && l.CodTipoPrescricao != 2)
        .ToList();
      entity.PrescricaoPorNome = porNome.Count > 0 ? 1 : 0;
      entity.MotivoPrescricaoNome = porNome.FirstOrDefault()?.CodMotivo;
    }
  }
}
