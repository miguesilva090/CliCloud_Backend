using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs
{
    public class TensaoArterialLightDTO : IDto
    {
        public Guid UtenteId { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public int TensaoSistolica { get; set; }

        public int TensaoDiastolica { get; set; }


    }
}

