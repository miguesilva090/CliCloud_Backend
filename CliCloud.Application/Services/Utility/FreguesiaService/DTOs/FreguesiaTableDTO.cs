using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FreguesiaService.DTOs
{
    public class FreguesiaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? ConcelhoId { get; set; }
        public FreguesiaTableConcelhoDTO? Concelho { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class FreguesiaTableConcelhoDTO : IDto
    {
      public Guid Id { get; set; }
      public string? Nome { get; set; }
      public Guid? DistritoId { get; set; }
      public FreguesiaTableDistritoDTO? Distrito { get; set; }
    }

    public class FreguesiaTableDistritoDTO : IDto
    {
      public Guid Id { get; set; }
      public string? Nome { get; set; }
      public Guid? PaisId { get; set; }
      public FreguesiaTablePaisDTO? Pais { get; set; }
    }

    public class FreguesiaTablePaisDTO : IDto
    {
      public Guid Id { get; set; }
      public string? Nome { get; set; }
      public string? Codigo { get; set; }
    }
}

