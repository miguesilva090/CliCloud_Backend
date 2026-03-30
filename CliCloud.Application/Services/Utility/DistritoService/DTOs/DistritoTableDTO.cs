using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.DistritoService.DTOs
{
    public class DistritoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? PaisId { get; set; }
        public DistritoTablePaisDTO? Pais { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class DistritoTablePaisDTO : IDto
    {
      public Guid Id { get; set; }
      public string? Codigo { get; set; }
      public string? Nome { get; set; }
    }
}

