using CliCloud.Application.Common.Marker;

    namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs
    {
        public class FraquezasMuscularesTableDTO : IDto
        {
            public Guid Id {get;set;}
            public string? Descricao {get;set;}
            public DateTime CreatedOn {get;set;}
        }
    }
