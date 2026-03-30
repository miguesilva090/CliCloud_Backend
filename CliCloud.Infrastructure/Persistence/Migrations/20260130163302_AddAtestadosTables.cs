using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAtestadosTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Atestados");

            migrationBuilder.CreateTable(
                name: "Atestado",
                schema: "Atestados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataAtestado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroSPMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoEnvio = table.Column<int>(type: "int", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroSNS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atestado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atestado_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Atestado_CodigoPostal_CodigoPostalId",
                        column: x => x.CodigoPostalId,
                        principalSchema: "Utility",
                        principalTable: "CodigoPostal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Atestado_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Atestado_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AtestadoCategoria",
                schema: "Atestados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaConducaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Apto = table.Column<int>(type: "int", nullable: false),
                    AptoGrupo2 = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtestadoCategoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtestadoCategoria_Atestado_AtestadoId",
                        column: x => x.AtestadoId,
                        principalSchema: "Atestados",
                        principalTable: "Atestado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtestadoCategoria_CartaConducao_CartaConducaoId",
                        column: x => x.CartaConducaoId,
                        principalSchema: "CartaConducao",
                        principalTable: "CartaConducao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AtestadoRestricao",
                schema: "Atestados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaConducaoRestricaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaConducaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Anotacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtestadoRestricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtestadoRestricao_Atestado_AtestadoId",
                        column: x => x.AtestadoId,
                        principalSchema: "Atestados",
                        principalTable: "Atestado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtestadoRestricao_CartaConducaoRestricoes_CartaConducaoRestricaoId",
                        column: x => x.CartaConducaoRestricaoId,
                        principalSchema: "CartaConducao",
                        principalTable: "CartaConducaoRestricoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtestadoRestricao_CartaConducao_CartaConducaoId",
                        column: x => x.CartaConducaoId,
                        principalSchema: "CartaConducao",
                        principalTable: "CartaConducao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AtestadoRestricaoAnterior",
                schema: "Atestados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaConducaoRestricaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Anotacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtestadoRestricaoAnterior", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtestadoRestricaoAnterior_Atestado_AtestadoId",
                        column: x => x.AtestadoId,
                        principalSchema: "Atestados",
                        principalTable: "Atestado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AtestadoRestricaoAnterior_CartaConducaoRestricoes_CartaConducaoRestricaoId",
                        column: x => x.CartaConducaoRestricaoId,
                        principalSchema: "CartaConducao",
                        principalTable: "CartaConducaoRestricoes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atestado_ClinicaId",
                schema: "Atestados",
                table: "Atestado",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Atestado_CodigoPostalId",
                schema: "Atestados",
                table: "Atestado",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Atestado_MedicoId",
                schema: "Atestados",
                table: "Atestado",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Atestado_UtenteId",
                schema: "Atestados",
                table: "Atestado",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoCategoria_AtestadoId",
                schema: "Atestados",
                table: "AtestadoCategoria",
                column: "AtestadoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoCategoria_CartaConducaoId",
                schema: "Atestados",
                table: "AtestadoCategoria",
                column: "CartaConducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoRestricao_AtestadoId",
                schema: "Atestados",
                table: "AtestadoRestricao",
                column: "AtestadoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoRestricao_CartaConducaoId",
                schema: "Atestados",
                table: "AtestadoRestricao",
                column: "CartaConducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoRestricao_CartaConducaoRestricaoId",
                schema: "Atestados",
                table: "AtestadoRestricao",
                column: "CartaConducaoRestricaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoRestricaoAnterior_AtestadoId",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior",
                column: "AtestadoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtestadoRestricaoAnterior_CartaConducaoRestricaoId",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior",
                column: "CartaConducaoRestricaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtestadoCategoria",
                schema: "Atestados");

            migrationBuilder.DropTable(
                name: "AtestadoRestricao",
                schema: "Atestados");

            migrationBuilder.DropTable(
                name: "AtestadoRestricaoAnterior",
                schema: "Atestados");

            migrationBuilder.DropTable(
                name: "Atestado",
                schema: "Atestados");
        }
    }
}
