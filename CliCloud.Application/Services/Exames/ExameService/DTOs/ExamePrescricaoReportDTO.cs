using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
    public class ExamePrescricaoReportLinhaDTO : IDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Designacao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }

    public class ExamePrescricaoReportDTO : IDto
    {
        public Guid Id { get; set; }
        public string NumeroPrescricao { get; set; } = string.Empty;
        public DateTime DataPrescricao { get; set; }
        public string UtenteNome { get; set; } = string.Empty;
        public string? UtenteNumero { get; set; }
        public string? OrganismoNome { get; set; }
        public string? MedicoNome { get; set; }
        public List<ExamePrescricaoReportLinhaDTO> Linhas { get; set; } = new();
    }
}

