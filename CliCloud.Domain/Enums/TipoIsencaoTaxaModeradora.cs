#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum TipoIsencaoTaxaModeradora
  {
    [Display(Name = "Grávida")]
    Gravida = 1,
    
    [Display(Name = "Parturiente")]
    Parturiente = 2,
    
    [Display(Name = "Menor")]
    Menor = 3,
    
    [Display(Name = "Pessoa com grau de incapacidade ≥ 60%")]
    Incapacidade60 = 4,
    
    [Display(Name = "Utente em situação de insuficiência económica")]
    InsuficienciaEconomica = 5,
    
    [Display(Name = "Agregado familiar de utente em insuficiência económica")]
    AgregadoInsuficienciaEconomica = 6,
    
    [Display(Name = "Doente transplantado")]
    Transplantado = 7,
    
    [Display(Name = "Dador de sangue")]
    DadorSangue = 8,
    
    [Display(Name = "Dador de células, tecidos e órgãos")]
    DadorCelulasTecidosOrgaos = 9,
    
    [Display(Name = "Bombeiro")]
    Bombeiro = 10,
    
    [Display(Name = "Militar com incapacidade permanente do serviço militar")]
    MilitarIncapacidadePermanente = 11,
    
    [Display(Name = "Ex-militar com incapacidade permanente do serviço militar")]
    ExMilitarIncapacidadePermanente = 12,
    
    [Display(Name = "Desempregado inscrito no Centro de Emprego")]
    DesempregadoInscrito = 13,
    
    [Display(Name = "Cônjuge de desempregado inscrito")]
    ConjugeDesempregado = 14,
    
    [Display(Name = "Dependente de desempregado inscrito")]
    DependenteDesempregado = 15,
    
    [Display(Name = "Jovem com processo de proteção em CPCJ ou tribunal")]
    JovemProcessoProtecao = 16,
    
    [Display(Name = "Jovem em medida tutelar de internamento")]
    JovemMedidaTutelarInternamento = 17,
    
    [Display(Name = "Jovem em medida cautelar de guarda em centro educativo")]
    JovemMedidaCautelarGuarda = 18,
    
    [Display(Name = "Jovem integrado em respostas sociais de acolhimento")]
    JovemAcolhimento = 19,
    
    [Display(Name = "Requerente de asilo")]
    RequerenteAsilo = 20,
    
    [Display(Name = "Refugiado")]
    Refugiado = 21,
    
    [Display(Name = "Cônjuge de requerente de asilo/refugiado")]
    ConjugeRequerenteAsiloRefugiado = 22,
    
    [Display(Name = "Descendente direto de requerente de asilo/refugiado")]
    DescendenteRequerenteAsiloRefugiado = 23,
    
    [Display(Name = "Pessoa no âmbito de Interrupção Voluntária da Gravidez (IVG)")]
    IVG = 24,
    
    // ========== CAMPOS LEGADOS (para compatibilidade/migração) ==========
    [Display(Name = "Tipo O (Legado)")]
    TipoO = 100,
    
    [Display(Name = "Tipo R - Reformado (Legado)")]
    TipoR = 101,
    
    [Display(Name = "Tipo S - Segurado (Legado)")]
    TipoS = 102,
    
    [Display(Name = "Tipo T - Trabalhador (Legado)")]
    TipoT = 103
  }
}
