using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ThirdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Consultas");

            migrationBuilder.EnsureSchema(
                name: "Servicos");

            migrationBuilder.EnsureSchema(
                name: "Tratamentos");

            migrationBuilder.CreateTable(
                name: "Consulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraFim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraChegada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraGdh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sala = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirmado = table.Column<bool>(type: "bit", nullable: true),
                    Efectuado = table.Column<bool>(type: "bit", nullable: true),
                    Faltou = table.Column<bool>(type: "bit", nullable: true),
                    EmTratamento = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmaConsulta = table.Column<bool>(type: "bit", nullable: false),
                    ReciboId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataRecibo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Desconto = table.Column<double>(type: "float", nullable: true),
                    Pago = table.Column<int>(type: "int", nullable: true),
                    Faturado = table.Column<int>(type: "int", nullable: true),
                    NumDevolucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumDestacavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstadoU = table.Column<int>(type: "int", nullable: true),
                    EstadoI = table.Column<int>(type: "int", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredencialExterna = table.Column<int>(type: "int", nullable: true),
                    Apolice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumBenif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeguradoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sinistrado = table.Column<int>(type: "int", nullable: true),
                    Justificacao = table.Column<int>(type: "int", nullable: true),
                    DescricaoJust = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isencao = table.Column<int>(type: "int", nullable: true),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstituicaoEmpregadoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Utilizador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    NumLinhas = table.Column<int>(type: "int", nullable: true),
                    NaoDiscriminar = table.Column<int>(type: "int", nullable: true),
                    TipoCambio = table.Column<int>(type: "int", nullable: true),
                    Movimento = table.Column<int>(type: "int", nullable: true),
                    ProdAplic = table.Column<double>(type: "float", nullable: true),
                    Pic = table.Column<double>(type: "float", nullable: true),
                    TipoAdmiss = table.Column<int>(type: "int", nullable: true),
                    TipoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CExtramed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcessoKioskSemPag = table.Column<int>(type: "int", nullable: true),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraMarcacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CodigoFatura = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataFatura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CodigoTipoDocumentoBase = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroTFatura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoFaturaOrganismo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroTFaturaOrganismo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspecialidadeCodigo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CodMotivoConsulta = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DescMotivoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ICD91 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ICD92 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificadorQueue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consulta_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Consulta_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Consulta_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Consulta_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TipoServico",
                schema: "Servicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VTaxa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorK = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mad = table.Column<int>(type: "int", nullable: true),
                    PartilhaSemRequisicao = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FisioterapeutaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuxiliarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OutroTecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TratamentoPredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalOrigemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumSessao = table.Column<int>(type: "int", nullable: true),
                    DataInic = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfDfim = table.Column<int>(type: "int", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NFaltMax = table.Column<int>(type: "int", nullable: true),
                    NFaltComax = table.Column<int>(type: "int", nullable: true),
                    NFalta = table.Column<int>(type: "int", nullable: true),
                    NFaltaCons = table.Column<int>(type: "int", nullable: true),
                    NAltSess = table.Column<int>(type: "int", nullable: true),
                    Preco = table.Column<double>(type: "float", nullable: true),
                    DescInst = table.Column<double>(type: "float", nullable: true),
                    DescCli = table.Column<double>(type: "float", nullable: true),
                    ValorDesc = table.Column<double>(type: "float", nullable: true),
                    ReciboId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataRecibo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pago = table.Column<int>(type: "int", nullable: true),
                    Faturado = table.Column<int>(type: "int", nullable: true),
                    NumDevolucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumDestacavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoU = table.Column<int>(type: "int", nullable: true),
                    EstadoI = table.Column<int>(type: "int", nullable: true),
                    Suspenso = table.Column<int>(type: "int", nullable: true),
                    DataSuspensao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Provisorio = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TecObs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isencao = table.Column<int>(type: "int", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredencialExterna = table.Column<int>(type: "int", nullable: true),
                    DestacavelCredencial = table.Column<int>(type: "int", nullable: true),
                    TaxaMod = table.Column<int>(type: "int", nullable: true),
                    Inisess = table.Column<int>(type: "int", nullable: true),
                    HoraFisio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraAux = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraOutro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuracaoTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelOutro = table.Column<int>(type: "int", nullable: true),
                    NumCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orespons = table.Column<bool>(type: "bit", nullable: true),
                    ConfirmaLoc = table.Column<int>(type: "int", nullable: true),
                    SinistroId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeguradoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SemanaCompleta = table.Column<int>(type: "int", nullable: true),
                    VemListEsp = table.Column<int>(type: "int", nullable: true),
                    CartaoDevolv = table.Column<int>(type: "int", nullable: true),
                    TerapiaFala = table.Column<int>(type: "int", nullable: false),
                    NumBenif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apolice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomePatologia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frespons = table.Column<bool>(type: "bit", nullable: true),
                    Arespons = table.Column<bool>(type: "bit", nullable: true),
                    Lotes = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tratamento_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tratamento_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tratamento_Tecnico_AuxiliarId",
                        column: x => x.AuxiliarId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tratamento_Tecnico_FisioterapeutaId",
                        column: x => x.FisioterapeutaId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tratamento_Tecnico_OutroTecnicoId",
                        column: x => x.OutroTecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tratamento_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MarcacaoConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraFim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoDigital = table.Column<int>(type: "int", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstadosCTH = table.Column<int>(type: "int", nullable: true),
                    TipoAdmiss = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcacaoConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Consulta_ConsultaId",
                        column: x => x.ConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "Consulta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servico",
                schema: "Servicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TipoServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Duracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxaIvaId = table.Column<int>(type: "int", nullable: true),
                    EAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TipoAparelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TratDentario = table.Column<bool>(type: "bit", nullable: false),
                    UsaFisioter = table.Column<bool>(type: "bit", nullable: true),
                    UsaAuxiliar = table.Column<bool>(type: "bit", nullable: true),
                    CodigoMotivoIsencao = table.Column<int>(type: "int", nullable: true),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servico_TipoServico_TipoServicoId",
                        column: x => x.TipoServicoId,
                        principalSchema: "Servicos",
                        principalTable: "TipoServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SessaoTratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumSessao = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IHoraIni = table.Column<int>(type: "int", nullable: true),
                    Duracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDuraca = table.Column<int>(type: "int", nullable: true),
                    FisioterapeutaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuxiliarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OutroTecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HoraFisio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraAux = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraOutro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuracaoFisio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuracaoAux = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuracaoOutro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReciboId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataRecibo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pago = table.Column<int>(type: "int", nullable: true),
                    Faturado = table.Column<int>(type: "int", nullable: true),
                    NumDevolucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumDestacavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumTransacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoU = table.Column<int>(type: "int", nullable: true),
                    EstadoI = table.Column<int>(type: "int", nullable: true),
                    Faltou = table.Column<int>(type: "int", nullable: true),
                    CompensaFalta = table.Column<int>(type: "int", nullable: true),
                    ObsFalta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desmarcado = table.Column<int>(type: "int", nullable: true),
                    Destino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfFact = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservSessao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistSess = table.Column<int>(type: "int", nullable: true),
                    TipoCambio = table.Column<int>(type: "int", nullable: true),
                    TipoDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataApagar = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessaoTratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessaoTratamento_Tecnico_AuxiliarId",
                        column: x => x.AuxiliarId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SessaoTratamento_Tecnico_FisioterapeutaId",
                        column: x => x.FisioterapeutaId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SessaoTratamento_Tecnico_OutroTecnicoId",
                        column: x => x.OutroTecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SessaoTratamento_Tratamento_TratamentoId",
                        column: x => x.TratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "Tratamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServicoConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CodigoArtigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeArtigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorArtigo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MargemMed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MargemIns = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RecMed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RecInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescCli = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorDesc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    Dente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Linha = table.Column<int>(type: "int", nullable: false),
                    NCheque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Electrocardiograma = table.Column<int>(type: "int", nullable: true),
                    ValorUt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoConsulta_Consulta_ConsultaId",
                        column: x => x.ConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "Consulta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicoConsulta_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServicoTratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Duracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDuraca = table.Column<int>(type: "int", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    UsaFisioter = table.Column<int>(type: "int", nullable: true),
                    UsaAuxiliar = table.Column<int>(type: "int", nullable: true),
                    UsaOutro = table.Column<int>(type: "int", nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorDesc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorUt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessaoTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoTratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoTratamento_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServicoTratamento_Tratamento_TratamentoId",
                        column: x => x.TratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "Tratamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServicoSessao",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessaoTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FisioterapeutaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuxiliarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HoraInic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IHoraIni = table.Column<int>(type: "int", nullable: true),
                    HoraFim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IHoraFim = table.Column<int>(type: "int", nullable: true),
                    Duracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDuraca = table.Column<int>(type: "int", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    AparelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorDesc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorUt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoSessao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoSessao_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServicoSessao_SessaoTratamento_SessaoTratamentoId",
                        column: x => x.SessaoTratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "SessaoTratamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ServicoSessao_Tecnico_AuxiliarId",
                        column: x => x.AuxiliarId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ServicoSessao_Tecnico_FisioterapeutaId",
                        column: x => x.FisioterapeutaId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_EspecialidadeId",
                schema: "Consultas",
                table: "Consulta",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_MedicoId",
                schema: "Consultas",
                table: "Consulta",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_TecnicoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_UtenteId",
                schema: "Consultas",
                table: "Consulta",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_ConsultaId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_EspecialidadeId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_MedicoId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_UtenteId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_Codigo",
                schema: "Servicos",
                table: "Servico",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servico_TipoServicoId",
                schema: "Servicos",
                table: "Servico",
                column: "TipoServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoConsulta_ConsultaId",
                schema: "Consultas",
                table: "ServicoConsulta",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoConsulta_ServicoId",
                schema: "Consultas",
                table: "ServicoConsulta",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_AuxiliarId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "AuxiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_FisioterapeutaId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_ServicoId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_SessaoTratamentoId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "SessaoTratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoTratamento_ServicoId",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoTratamento_TratamentoId",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                column: "TratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_AuxiliarId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "AuxiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_FisioterapeutaId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_OutroTecnicoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "OutroTecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_TratamentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "TratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoServico_Nome",
                schema: "Servicos",
                table: "TipoServico",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_AuxiliarId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "AuxiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_FisioterapeutaId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_MedicoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_OrganismoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_OutroTecnicoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "OutroTecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_UtenteId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarcacaoConsulta",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "ServicoConsulta",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "ServicoSessao",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "ServicoTratamento",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "Consulta",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "SessaoTratamento",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "Servico",
                schema: "Servicos");

            migrationBuilder.DropTable(
                name: "Tratamento",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "TipoServico",
                schema: "Servicos");
        }
    }
}
