using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sinistrados_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sinistros");

            migrationBuilder.CreateTable(
                name: "EstadoSinistro",
                schema: "Sinistros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoSinistro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sinistrado",
                schema: "Sinistros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoSinistro = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DataAcidente = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataParticipacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataPrimeiraObservacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataUltimoTratamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataAlta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoSinistroId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoAcidente = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Responsabilidade = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    NumeroProcesso = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Historico = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relatorio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinistrado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sinistrado_EstadoSinistro_EstadoSinistroId",
                        column: x => x.EstadoSinistroId,
                        principalSchema: "Sinistros",
                        principalTable: "EstadoSinistro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinistradoLinhaServico",
                schema: "Sinistros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SinistradoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoServico = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DesignacaoServico = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorContratado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataServico = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroFaturaInterno = table.Column<int>(type: "int", nullable: true),
                    NumeroTFatura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFatura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdmissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinistradoLinhaServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinistradoLinhaServico_Sinistrado_SinistradoId",
                        column: x => x.SinistradoId,
                        principalSchema: "Sinistros",
                        principalTable: "Sinistrado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSinistro_Designacao",
                schema: "Sinistros",
                table: "EstadoSinistro",
                column: "Designacao");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistrado_CodigoSinistro",
                schema: "Sinistros",
                table: "Sinistrado",
                column: "CodigoSinistro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sinistrado_EstadoSinistroId",
                schema: "Sinistros",
                table: "Sinistrado",
                column: "EstadoSinistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistrado_Historico",
                schema: "Sinistros",
                table: "Sinistrado",
                column: "Historico");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistrado_UtenteId",
                schema: "Sinistros",
                table: "Sinistrado",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_SinistradoLinhaServico_CodigoServico",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                column: "CodigoServico");

            migrationBuilder.CreateIndex(
                name: "IX_SinistradoLinhaServico_SinistradoId",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                column: "SinistradoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SinistradoLinhaServico",
                schema: "Sinistros");

            migrationBuilder.DropTable(
                name: "Sinistrado",
                schema: "Sinistros");

            migrationBuilder.DropTable(
                name: "EstadoSinistro",
                schema: "Sinistros");
        }
    }
}
