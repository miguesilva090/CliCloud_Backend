using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtenteSubsistemaLinha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UtenteSubsistemaLinha",
                schema: "Utentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sigla = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCartao = table.Column<DateOnly>(type: "date", nullable: true),
                    NumeroApolice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtenteSubsistemaLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtenteSubsistemaLinha_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalSchema: "Empresas",
                        principalTable: "Empresa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UtenteSubsistemaLinha_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UtenteSubsistemaLinha_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtenteSubsistemaLinha_EmpresaId",
                schema: "Utentes",
                table: "UtenteSubsistemaLinha",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_UtenteSubsistemaLinha_OrganismoId",
                schema: "Utentes",
                table: "UtenteSubsistemaLinha",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_UtenteSubsistemaLinha_UtenteId",
                schema: "Utentes",
                table: "UtenteSubsistemaLinha",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtenteSubsistemaLinha",
                schema: "Utentes");
        }
    }
}
