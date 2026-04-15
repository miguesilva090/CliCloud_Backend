using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Atestados.SpmsCartaConducaoService;

public interface ISpmsCartaConducaoService : ITransientService
{
    Task<SpmsRegistoAtestadoResult> RegistarOnlineAsync(
        Atestado atestado,
        Utente utente, 
        Medico medico, 
        Clinica clinica,
        ConfigCartaConducao config,
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes,
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores
    );

    Task<SpmsRegistoAtestadoResult> RegistarOfflineAsync(
        Atestado atestado,
        Utente utente,
        Medico medico,
        Clinica clinica,
        ConfigCartaConducao config,
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes,
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores
    );
}