using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService.DTOs;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigExamesSemPapelService;

public class ConfigExamesSemPapelService(IRepositoryAsync repository) : IConfigExamesSemPapelService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfigExamesSemPapelDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        var spec = new ConfigExamesSemPapelPorClinicaSpec(clinicaId);
        var entity = ( await _repository.GetListAsync<ConfigExamesSemPapel, Guid>(spec)).FirstOrDefault();

        if(entity is null)
            return ResponseFactory.Fail<ConfigExamesSemPapelDTO>("Configuração Exames Sem Papel não encontrada");

        return ResponseFactory.Success(new ConfigExamesSemPapelDTO
        {
            Id = entity.Id,
            ClinicaId = entity.ClinicaId,
            CodigoEntidade = entity.CodigoEntidade,
            Username = entity.Username,
            Password = entity.Password,
            PesquisaPrestacao = entity.PesquisaPrestacao,
            Agendamento = entity.Agendamento,
            Efetivacao = entity.Efetivacao,
            Anulacao = entity.Anulacao,
            ConsultaCancelados = entity.ConsultaCancelados,
            EfetuadosNaoPrescritos = entity.EfetuadosNaoPrescritos,
            TaxasModeradoras = entity.TaxasModeradoras,
            RelatorioResultados = entity.RelatorioResultados,
            UsernamePartilhaResultados = entity.UsernamePartilhaResultados,
            PasswordPartilhaResultados = entity.PasswordPartilhaResultados,
            RelatorioResultadosSemRequisicao = entity.RelatorioResultadosSemRequisicao,
            UsernamePartilhaResultadosSemRequisicao = entity.UsernamePartilhaResultadosSemRequisicao,
            PasswordPartilhaResultadosSemRequisicao = entity.PasswordPartilhaResultadosSemRequisicao,
            AreaPrestacao = entity.AreaPrestacao
        });
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigExamesSemPapelRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.PesquisaPrestacao))
            return ResponseFactory.Fail<Guid>("Endpoint Pesquisa Prestação é obrigatorio");
        
        var spec = new ConfigExamesSemPapelPorClinicaSpec(clinicaId);
        var entity = (await _repository.GetListAsync<ConfigExamesSemPapel, Guid>(spec)).FirstOrDefault()
            ?? new ConfigExamesSemPapel { ClinicaId = clinicaId };

        entity.CodigoEntidade = request.CodigoEntidade;
        entity.Username = request.Username?.Trim();
        entity.Password = request.Password;
        entity.PesquisaPrestacao = request.PesquisaPrestacao?.Trim();
        entity.Agendamento = request.Agendamento?.Trim();
        entity.Efetivacao = request.Efetivacao?.Trim();
        entity.Anulacao = request.Anulacao?.Trim();
        entity.ConsultaCancelados = request.ConsultaCancelados?.Trim();
        entity.EfetuadosNaoPrescritos = request.EfetuadosNaoPrescritos?.Trim();
        entity.TaxasModeradoras = request.TaxasModeradoras?.Trim();
        entity.RelatorioResultados = request.RelatorioResultados?.Trim();
        entity.UsernamePartilhaResultados = request.UsernamePartilhaResultados?.Trim();
        entity.PasswordPartilhaResultados = request.PasswordPartilhaResultados;
        entity.RelatorioResultadosSemRequisicao = request.RelatorioResultadosSemRequisicao?.Trim();
        entity.UsernamePartilhaResultadosSemRequisicao = request.UsernamePartilhaResultadosSemRequisicao?.Trim();
        entity.PasswordPartilhaResultadosSemRequisicao = request.PasswordPartilhaResultadosSemRequisicao;
        entity.AreaPrestacao = request.AreaPrestacao?.Trim();

        if(entity.Id == Guid.Empty)
            await _repository.CreateAsync<ConfigExamesSemPapel, Guid>(entity);
        else
            await _repository.UpdateAsync<ConfigExamesSemPapel, Guid>(entity);

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);

    }
}
