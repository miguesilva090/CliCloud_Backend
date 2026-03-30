using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Outros_Parametros_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaPrestacaoDefeitoAreaZ",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvisoInqueritoSessoesDiarias",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalendarioMarcacoesRadio",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaminhoSaft",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CccCodLocalEmissao",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CccDescLocalEmissao",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cid",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodSb",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codcentral",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codfact",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codintern",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ControlarAparelhos",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ControlarPlafond",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CredencialExternaAdse",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Credito",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CtrlPlafond",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Descarga",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Diasvalid",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiretoriaDocumentos",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntidadeUtilizadora",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnvioEmail",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExportContabilidadeFa",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExportContabilidadeFr",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExportPredUtenteFa",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExportPredUtenteFr",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExportTipoContaFa",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExportTipoContaFr",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaltasConsecutivasMax",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaltasMax",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GestaoSalas",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KqueueAvisoAtraso",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KqueueMensagemAvisoAtraso",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KqueueTempoAvisoAtraso",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelAuxiliares",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ligacb",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalPrescricao",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MovimentosInternos",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MsgCredenciais",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MsgFaltaPagamento",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Netiquetas",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeEtiqueta",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NovaPrescricao",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NovoEstadoPaginaAtendimento",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortaLeitorCartoes",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Regiao",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ruptura",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Segundos",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StocksColunaStockReal",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Taxamoderadora",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tcamas",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoAdmissPorDefeito",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoPagamento",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoSrvTratamentos",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tiporecal",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Validade",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Valorart",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Valorecal",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaPrestacaoDefeitoAreaZ",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "AvisoInqueritoSessoesDiarias",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CalendarioMarcacoesRadio",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CaminhoSaft",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CccCodLocalEmissao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CccDescLocalEmissao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Cid",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CodSb",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Codcentral",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Codfact",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Codintern",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ControlarAparelhos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ControlarPlafond",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CredencialExternaAdse",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Credito",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CtrlPlafond",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Descarga",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Diasvalid",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "DiretoriaDocumentos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EntidadeUtilizadora",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EnvioEmail",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportContabilidadeFa",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportContabilidadeFr",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportPredUtenteFa",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportPredUtenteFr",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportTipoContaFa",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ExportTipoContaFr",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FaltasConsecutivasMax",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FaltasMax",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "GestaoSalas",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "KqueueAvisoAtraso",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "KqueueMensagemAvisoAtraso",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "KqueueTempoAvisoAtraso",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "LabelAuxiliares",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Ligacb",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "LocalPrescricao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "MovimentosInternos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "MsgCredenciais",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "MsgFaltaPagamento",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Netiquetas",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "NomeEtiqueta",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "NovaPrescricao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "NovoEstadoPaginaAtendimento",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "PortaLeitorCartoes",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Regiao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Ruptura",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Segundos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "StocksColunaStockReal",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Taxamoderadora",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Tcamas",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "TipoAdmissPorDefeito",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "TipoPagamento",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "TipoSrvTratamentos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Tiporecal",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Validade",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Valorart",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Valorecal",
                schema: "Core",
                table: "Clinica");
        }
    }
}
