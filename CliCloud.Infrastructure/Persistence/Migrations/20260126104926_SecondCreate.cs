using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Bancos");

            migrationBuilder.EnsureSchema(
                name: "Especialidades");

            migrationBuilder.EnsureSchema(
                name: "CentroSaude");

            migrationBuilder.EnsureSchema(
                name: "Documentos");

            migrationBuilder.EnsureSchema(
                name: "Fornecedores");

            migrationBuilder.EnsureSchema(
                name: "Funcionarios");

            migrationBuilder.EnsureSchema(
                name: "Medicos");

            migrationBuilder.EnsureSchema(
                name: "Tecnicos");

            migrationBuilder.EnsureSchema(
                name: "Organismos");

            migrationBuilder.EnsureSchema(
                name: "TipoEntidadeFinanceira");

            migrationBuilder.CreateTable(
                name: "Banco",
                schema: "Bancos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banco_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaEspecialidade",
                schema: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaEspecialidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentroSaude",
                schema: "CentroSaude",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoLocalCS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentroSaude", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentroSaude_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                schema: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstituicaoFinanceiraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroConta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plafond = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CondicaoPagamento = table.Column<int>(type: "int", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Moeda = table.Column<int>(type: "int", nullable: true),
                    TotalDebito = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Origem = table.Column<int>(type: "int", nullable: true),
                    TipoFornecedor = table.Column<int>(type: "int", nullable: true),
                    TipoModoPagamento = table.Column<int>(type: "int", nullable: true),
                    NumeroNib = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aprovado = table.Column<int>(type: "int", nullable: true),
                    DataAprovacao = table.Column<DateOnly>(type: "date", nullable: true),
                    EnderecoWeb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiasPrevEntrega = table.Column<int>(type: "int", nullable: true),
                    DiasEfectiEntrega = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedor_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                schema: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionario_EntidadePessoa_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "EntidadePessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MedicoExterno",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoExterno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicoExterno_EntidadePessoa_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "EntidadePessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Natureza = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    TipoMovimento = table.Column<int>(type: "int", nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    TipoSerie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NumeroDocumento = table.Column<int>(type: "int", nullable: true),
                    NumVias = table.Column<int>(type: "int", nullable: true),
                    TemCabecalho = table.Column<int>(type: "int", nullable: true),
                    ImprimirEmtodasAsVias = table.Column<int>(type: "int", nullable: true),
                    PermiteMovimento = table.Column<int>(type: "int", nullable: true),
                    Config = table.Column<int>(type: "int", nullable: true),
                    AtualizaStock = table.Column<int>(type: "int", nullable: true),
                    Inactivo = table.Column<bool>(type: "bit", nullable: false),
                    MostraFaturacao = table.Column<bool>(type: "bit", nullable: false),
                    DescarregarTesouraria = table.Column<bool>(type: "bit", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false),
                    Cae = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodigoATCUD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ATCUDEstado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ATCUDData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContabContaConsulta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabContaTratamento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabContaOutros = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabTipoServicoConsulta = table.Column<int>(type: "int", nullable: true),
                    ContabTipoServicoTratamento = table.Column<int>(type: "int", nullable: true),
                    ContabTipoConta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContabDiario = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabSeccao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabSerieSeccao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContabDimensaoCCDebito = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContabDimensaoCCCredito = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContabValorCCDebito = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContabValorCCCredito = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PocalTipoDocumento = table.Column<int>(type: "int", nullable: true),
                    PocalGuiaFatAutartica = table.Column<int>(type: "int", nullable: true),
                    PocalCodigoServico = table.Column<int>(type: "int", nullable: true),
                    PocalTipoDocPocalEmitido = table.Column<int>(type: "int", nullable: true),
                    PocalTipoDocPocalCobrado = table.Column<int>(type: "int", nullable: true),
                    ReportPersonalizado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEntidadeFinanceira",
                schema: "TipoEntidadeFinanceira",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Dominio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DescricaoDominio = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEntidadeFinanceira", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organismo",
                schema: "Organismos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeComercial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Abreviatura = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PrazoPagamento = table.Column<int>(type: "int", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescontoUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CondicaoPagamento = table.Column<int>(type: "int", nullable: true),
                    TipoModoPagamento = table.Column<int>(type: "int", nullable: true),
                    BancoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroIdentificacaoBancaria = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: true),
                    Apolice = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Avenca = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataInicioContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    DataFimContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    NumeroPagamentos = table.Column<int>(type: "int", nullable: true),
                    CodigoClinica = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Faltas = table.Column<int>(type: "int", nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ars = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subregiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Regiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FraseADM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bloqueio = table.Column<int>(type: "int", nullable: true),
                    LimitarConsultas = table.Column<bool>(type: "bit", nullable: false),
                    NumeroConsultas = table.Column<int>(type: "int", nullable: true),
                    ContabilizarFaltas = table.Column<bool>(type: "bit", nullable: false),
                    AssinarPagaDocumento = table.Column<int>(type: "int", nullable: true),
                    AdmissaoCC = table.Column<int>(type: "int", nullable: true),
                    FaturaCredencial = table.Column<int>(type: "int", nullable: true),
                    DiscriminaServicos = table.Column<bool>(type: "bit", nullable: false),
                    DesignaTratamentos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApresentarCredenciaisPrimeiraSessaoTratamento = table.Column<bool>(type: "bit", nullable: false),
                    ApresentarCredenciaisPrimeiraConsulta = table.Column<bool>(type: "bit", nullable: false),
                    CodigoFaturacao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiltroFaturacao = table.Column<int>(type: "int", nullable: true),
                    CServicoFaturaResumo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FaturarPorDatas = table.Column<int>(type: "int", nullable: false),
                    TRUST = table.Column<bool>(type: "bit", nullable: false),
                    ADM = table.Column<bool>(type: "bit", nullable: false),
                    SADGNR = table.Column<bool>(type: "bit", nullable: false),
                    SADPSP = table.Column<bool>(type: "bit", nullable: false),
                    Globalbooking = table.Column<bool>(type: "bit", nullable: false),
                    AlterarPrecoTratamento = table.Column<bool>(type: "bit", nullable: false),
                    ContabContaFA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabContaFR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContabTipoContaFA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContabTipoContaFR = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CodigoULSNova = table.Column<int>(type: "int", nullable: true),
                    TratamentoCred = table.Column<int>(type: "int", nullable: true),
                    Nacional = table.Column<int>(type: "int", nullable: false),
                    CodigoRegiaoAtestadoCC = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organismo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organismo_Banco_BancoId",
                        column: x => x.BancoId,
                        principalSchema: "Bancos",
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Organismo_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Especialidade",
                schema: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CategoriaEspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Fisioterapia = table.Column<bool>(type: "bit", nullable: false),
                    Atendimento = table.Column<bool>(type: "bit", nullable: false),
                    Globalbooking = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Especialidade_CategoriaEspecialidade_CategoriaEspecialidadeId",
                        column: x => x.CategoriaEspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "CategoriaEspecialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroDocumento = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DescontoCliente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDocumento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIva = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLiquido = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Outros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CondicaoPagamento = table.Column<int>(type: "int", nullable: true),
                    TipoModoPagamento = table.Column<int>(type: "int", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: true),
                    Liquidado = table.Column<bool>(type: "bit", nullable: false),
                    Rectificado = table.Column<bool>(type: "bit", nullable: false),
                    Exportado = table.Column<bool>(type: "bit", nullable: false),
                    IsentoIva = table.Column<bool>(type: "bit", nullable: false),
                    NomeCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoradaCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalidadeCliente = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    NumeroContribuinteCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NumVias = table.Column<int>(type: "int", nullable: true),
                    Emitido = table.Column<int>(type: "int", nullable: true),
                    Origem = table.Column<int>(type: "int", nullable: true),
                    GlobalHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    VersaoChave = table.Column<int>(type: "int", nullable: true),
                    DataSistemaRegisto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documento_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalSchema: "Funcionarios",
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Documento_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Documento_TipoDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "TipoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documento_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Medico",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Director = table.Column<bool>(type: "bit", nullable: false),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Margem = table.Column<double>(type: "float", nullable: true),
                    LoginPRVR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComunicacaoNif = table.Column<bool>(type: "bit", nullable: false),
                    ComunicacaoNifAdse = table.Column<bool>(type: "bit", nullable: true),
                    GrupoFuncional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Letra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CartaoCidadaoMedico = table.Column<int>(type: "int", nullable: false),
                    IdUtilizador = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Globalbooking = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medico_EntidadePessoa_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "EntidadePessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Medico_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tecnico",
                schema: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Margem = table.Column<double>(type: "float", nullable: true),
                    IdUtilizador = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tecnico_EntidadePessoa_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "EntidadePessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tecnico_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HorarioMedico",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoHorario = table.Column<int>(type: "int", nullable: true),
                    MinMarcacao = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraComp = table.Column<bool>(type: "bit", nullable: false),
                    PrimeiraConsulta = table.Column<TimeSpan>(type: "time", nullable: true),
                    HorarioFlexivel = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioMedico_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioTecnico",
                schema: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoHorario = table.Column<int>(type: "int", nullable: true),
                    MinMarcacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraComp = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioTecnico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioTecnico_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioMedicoDia",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorarioMedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    Periodo = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    Fim = table.Column<TimeSpan>(type: "time", nullable: true),
                    Sala = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vagas = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioMedicoDia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioMedicoDia_HorarioMedico_HorarioMedicoId",
                        column: x => x.HorarioMedicoId,
                        principalSchema: "Medicos",
                        principalTable: "HorarioMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioTecnicoDia",
                schema: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorarioTecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    Periodo = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sala = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumMarcacoesPeriodo = table.Column<int>(type: "int", nullable: true),
                    NumMarcacoesOutro = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioTecnicoDia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioTecnicoDia_HorarioTecnico_HorarioTecnicoId",
                        column: x => x.HorarioTecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "HorarioTecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaEspecialidade_Codigo",
                schema: "Especialidades",
                table: "CategoriaEspecialidade",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_Data",
                schema: "Documentos",
                table: "Documento",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_Estado",
                schema: "Documentos",
                table: "Documento",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_FuncionarioId",
                schema: "Documentos",
                table: "Documento",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_OrganismoId",
                schema: "Documentos",
                table: "Documento",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDocumentoId_NumeroDocumento",
                schema: "Documentos",
                table: "Documento",
                columns: new[] { "TipoDocumentoId", "NumeroDocumento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_UtenteId",
                schema: "Documentos",
                table: "Documento",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Especialidade_CategoriaEspecialidadeId",
                schema: "Especialidades",
                table: "Especialidade",
                column: "CategoriaEspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioMedico_MedicoId",
                schema: "Medicos",
                table: "HorarioMedico",
                column: "MedicoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorarioMedicoDia_HorarioMedicoId_DiaSemana_Periodo",
                schema: "Medicos",
                table: "HorarioMedicoDia",
                columns: new[] { "HorarioMedicoId", "DiaSemana", "Periodo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorarioTecnico_TecnicoId",
                schema: "Tecnicos",
                table: "HorarioTecnico",
                column: "TecnicoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorarioTecnicoDia_HorarioTecnicoId_DiaSemana_Periodo",
                schema: "Tecnicos",
                table: "HorarioTecnicoDia",
                columns: new[] { "HorarioTecnicoId", "DiaSemana", "Periodo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medico_EspecialidadeId",
                schema: "Medicos",
                table: "Medico",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organismo_BancoId",
                schema: "Organismos",
                table: "Organismo",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tecnico_EspecialidadeId",
                schema: "Tecnicos",
                table: "Tecnico",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento",
                column: "Abreviatura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoEntidadeFinanceira_Codigo",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentroSaude",
                schema: "CentroSaude");

            migrationBuilder.DropTable(
                name: "Documento",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "Fornecedor",
                schema: "Fornecedores");

            migrationBuilder.DropTable(
                name: "HorarioMedicoDia",
                schema: "Medicos");

            migrationBuilder.DropTable(
                name: "HorarioTecnicoDia",
                schema: "Tecnicos");

            migrationBuilder.DropTable(
                name: "MedicoExterno",
                schema: "Medicos");

            migrationBuilder.DropTable(
                name: "TipoEntidadeFinanceira",
                schema: "TipoEntidadeFinanceira");

            migrationBuilder.DropTable(
                name: "Funcionario",
                schema: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Organismo",
                schema: "Organismos");

            migrationBuilder.DropTable(
                name: "TipoDocumento",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "HorarioMedico",
                schema: "Medicos");

            migrationBuilder.DropTable(
                name: "HorarioTecnico",
                schema: "Tecnicos");

            migrationBuilder.DropTable(
                name: "Banco",
                schema: "Bancos");

            migrationBuilder.DropTable(
                name: "Medico",
                schema: "Medicos");

            migrationBuilder.DropTable(
                name: "Tecnico",
                schema: "Tecnicos");

            migrationBuilder.DropTable(
                name: "Especialidade",
                schema: "Especialidades");

            migrationBuilder.DropTable(
                name: "CategoriaEspecialidade",
                schema: "Especialidades");
        }
    }
}
