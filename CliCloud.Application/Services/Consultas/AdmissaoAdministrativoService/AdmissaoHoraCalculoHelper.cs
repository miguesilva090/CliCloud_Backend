using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

/// <summary>
/// Calcula HoraFim a partir de HoraInicio + intervalo do médico (legado ADMISS / Prim_Conslt vs min_marc).
/// </summary>
public static class AdmissaoHoraCalculoHelper
{
  private static readonly TimeSpan DefaultSlot = TimeSpan.FromMinutes(15);

  public static async Task AplicarHoraFimAsync(Admissao admissao, IRepositoryAsync repository)
  {
    if (!admissao.HoraInicio.HasValue || !admissao.MedicoId.HasValue)
    {
      return;
    }

    List<HorarioMedico> horarios = (
      await repository.GetListAsync<HorarioMedico, Guid>(
        new HorarioMedicoPorMedicoIdSpec(admissao.MedicoId.Value)
      )
    ).ToList();

    HorarioMedico? horario = horarios.FirstOrDefault();

    if (horario?.HorarioFlexivel == true && admissao.HoraFim.HasValue)
    {
      return;
    }

    bool primeiraConsulta = await ResolverPrimeiraConsultaAsync(admissao, repository);
    TimeSpan duracao = ResolverDuracaoSlot(horario, primeiraConsulta);
    admissao.HoraFim = admissao.HoraInicio.Value.Add(duracao);
  }

  private static async Task<bool> ResolverPrimeiraConsultaAsync(
    Admissao admissao,
    IRepositoryAsync repository
  )
  {
    if (!admissao.TipoConsultaId.HasValue)
    {
      return false;
    }

    TipoConsultaItem? tipo = await repository.GetByIdAsync<TipoConsultaItem, Guid>(
      admissao.TipoConsultaId.Value
    );
    return AdmissaoTipoConsultaHelper.EhPrimeiraConsulta(tipo);
  }

  private static TimeSpan ResolverDuracaoSlot(HorarioMedico? horario, bool primeiraConsulta)
  {
    if (horario == null)
    {
      return DefaultSlot;
    }

    TimeSpan? slot = primeiraConsulta
      ? horario.PrimeiraConsulta ?? horario.MinMarcacao
      : horario.MinMarcacao ?? horario.PrimeiraConsulta;

    return slot ?? DefaultSlot;
  }
}
