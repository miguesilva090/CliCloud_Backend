using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Utility");

            migrationBuilder.EnsureSchema(
                name: "Utentes");

            migrationBuilder.CreateTable(
                name: "CodigoPostal",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoPostal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prefixo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Distrito",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distrito_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "Utility",
                        principalTable: "Pais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Concelho",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistritoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concelho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concelho_Distrito_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "Utility",
                        principalTable: "Distrito",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Freguesia",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Freguesia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Freguesia_Concelho_ConcelhoId",
                        column: x => x.ConcelhoId,
                        principalSchema: "Utility",
                        principalTable: "Concelho",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rua",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreguesiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rua", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rua_CodigoPostal_CodigoPostalId",
                        column: x => x.CodigoPostalId,
                        principalSchema: "Utility",
                        principalTable: "CodigoPostal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rua_Freguesia_FreguesiaId",
                        column: x => x.FreguesiaId,
                        principalSchema: "Utility",
                        principalTable: "Freguesia",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entidade",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoEntidade = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroContribuinte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CodigoPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreguesiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DistritoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaisId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroPorta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AndarRua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UrlFoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entidade_CodigoPostal_CodigoPostalId",
                        column: x => x.CodigoPostalId,
                        principalSchema: "Utility",
                        principalTable: "CodigoPostal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entidade_Concelho_ConcelhoId",
                        column: x => x.ConcelhoId,
                        principalSchema: "Utility",
                        principalTable: "Concelho",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entidade_Distrito_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "Utility",
                        principalTable: "Distrito",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entidade_Freguesia_FreguesiaId",
                        column: x => x.FreguesiaId,
                        principalSchema: "Utility",
                        principalTable: "Freguesia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entidade_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "Utility",
                        principalTable: "Pais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entidade_Rua_RuaId",
                        column: x => x.RuaId,
                        principalSchema: "Utility",
                        principalTable: "Rua",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntidadeContacto",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntidadeContactoTipoId = table.Column<int>(type: "int", nullable: false),
                    EntidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Indicativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Principal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadeContacto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntidadeContacto_Entidade_EntidadeId",
                        column: x => x.EntidadeId,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntidadePessoa",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: true),
                    Sexo = table.Column<int>(type: "int", nullable: true),
                    EstadoCivil = table.Column<int>(type: "int", nullable: true),
                    Nacionalidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naturalidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCartaoIdentificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEmissaoCartaoIdentificacao = table.Column<DateOnly>(type: "date", nullable: true),
                    DataValidadeCartaoIdentificacao = table.Column<DateOnly>(type: "date", nullable: true),
                    Arquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carteira = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlFotoAssinatura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetencaoImposto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupoFuncional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroIdentificacaoBancaria = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadePessoa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntidadePessoa_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utente",
                schema: "Utentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomePai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeMae = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroSegurancaSocial = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Profissao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupoSanguineo = table.Column<int>(type: "int", nullable: true),
                    NumeroUtente = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Aviso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desistencia = table.Column<bool>(type: "bit", nullable: false),
                    Cronico = table.Column<bool>(type: "bit", nullable: false),
                    TipoConsulta = table.Column<int>(type: "int", nullable: false),
                    Migrante = table.Column<bool>(type: "bit", nullable: false),
                    MarkConsentimento = table.Column<int>(type: "int", nullable: false),
                    RgpdConsentimento = table.Column<int>(type: "int", nullable: false),
                    DataConsentimentoRgpd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataRevogacaoRgpd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataConsentimentoMark = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataRevogacaoMark = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkTratamentoDados = table.Column<bool>(type: "bit", nullable: false),
                    CCValidado = table.Column<int>(type: "int", nullable: true),
                    CCDataValidacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataValidadeCU = table.Column<DateOnly>(type: "date", nullable: true),
                    NDocMigrante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoTaxaModeradora = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utente_EntidadePessoa_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "EntidadePessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concelho_DistritoId",
                schema: "Utility",
                table: "Concelho",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Distrito_PaisId",
                schema: "Utility",
                table: "Distrito",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_CodigoPostalId",
                schema: "Utility",
                table: "Entidade",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_ConcelhoId",
                schema: "Utility",
                table: "Entidade",
                column: "ConcelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_DistritoId",
                schema: "Utility",
                table: "Entidade",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_FreguesiaId",
                schema: "Utility",
                table: "Entidade",
                column: "FreguesiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_PaisId",
                schema: "Utility",
                table: "Entidade",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidade_RuaId",
                schema: "Utility",
                table: "Entidade",
                column: "RuaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeContacto_EntidadeId",
                schema: "Utility",
                table: "EntidadeContacto",
                column: "EntidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Freguesia_ConcelhoId",
                schema: "Utility",
                table: "Freguesia",
                column: "ConcelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rua_CodigoPostalId",
                schema: "Utility",
                table: "Rua",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Rua_FreguesiaId",
                schema: "Utility",
                table: "Rua",
                column: "FreguesiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_NumeroSegurancaSocial",
                schema: "Utentes",
                table: "Utente",
                column: "NumeroSegurancaSocial");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_NumeroUtente",
                schema: "Utentes",
                table: "Utente",
                column: "NumeroUtente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntidadeContacto",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Utente",
                schema: "Utentes");

            migrationBuilder.DropTable(
                name: "EntidadePessoa",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Entidade",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Rua",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "CodigoPostal",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Freguesia",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Concelho",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Distrito",
                schema: "Utility");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "Utility");
        }
    }
}
