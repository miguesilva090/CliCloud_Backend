using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_PedidoConsulta_GlobalBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoConsultaUtente",
                schema: "Consultas",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Codinst = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoConsultaUtente", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "PedidoConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPedidosConsultaUtente = table.Column<int>(type: "int", nullable: false),
                    CodigoEspecialidade = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CodigoMedico = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Filtro = table.Column<int>(type: "int", nullable: true),
                    Agendado = table.Column<bool>(type: "bit", nullable: false),
                    EmailPedido = table.Column<bool>(type: "bit", nullable: false),
                    SmsPedido = table.Column<bool>(type: "bit", nullable: false),
                    EmailAgendado = table.Column<bool>(type: "bit", nullable: false),
                    SmsAgendado = table.Column<bool>(type: "bit", nullable: false),
                    Recusado = table.Column<bool>(type: "bit", nullable: false),
                    CodigoAdmissao = table.Column<int>(type: "int", nullable: true),
                    Ficheiro = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoConsulta", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_PedidoConsulta_PedidoConsultaUtente_CodigoPedidosConsultaUtente",
                        column: x => x.CodigoPedidosConsultaUtente,
                        principalSchema: "Consultas",
                        principalTable: "PedidoConsultaUtente",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsulta_CodigoPedidosConsultaUtente",
                schema: "Consultas",
                table: "PedidoConsulta",
                column: "CodigoPedidosConsultaUtente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoConsulta",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "PedidoConsultaUtente",
                schema: "Consultas");
        }
    }
}
