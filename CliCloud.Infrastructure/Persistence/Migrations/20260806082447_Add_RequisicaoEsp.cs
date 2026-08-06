using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_RequisicaoEsp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoMcdt",
                schema: "Servicos",
                table: "SubsistemaServico",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EstadoExameEsp",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoExameEsp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisicaoEsp",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    NumeroRequisicao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CodigoMedico = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EnpAssinado = table.Column<bool>(type: "bit", nullable: false),
                    Historico = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataCativacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAgendamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataServico = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataRealizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UltimaData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoEsp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisicaoEspEfetuadoNaoPrescrito",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    RequisicaoEspId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoMcdt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAmostras = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoEspEfetuadoNaoPrescrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisicaoEspEfetuadoNaoPrescrito_RequisicaoEsp_RequisicaoEspId",
                        column: x => x.RequisicaoEspId,
                        principalSchema: "Consultas",
                        principalTable: "RequisicaoEsp",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequisicaoEspLinha",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    RequisicaoEspId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoMcdt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoEspLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisicaoEspLinha_RequisicaoEsp_RequisicaoEspId",
                        column: x => x.RequisicaoEspId,
                        principalSchema: "Consultas",
                        principalTable: "RequisicaoEsp",
                        principalColumn: "Id");
                });

            // One-shot: copiar dados históricos de dbo.* para schemas novos (não usado em runtime).
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.EstadoExameESP', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO Consultas.EstadoExameEsp
                    (
                        Id, Codigo, Abreviatura, Descricao,
                        CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn
                    )
                    SELECT
                        NEWID(),
                        e.Codigo,
                        LTRIM(RTRIM(e.Abreviatura)),
                        LTRIM(RTRIM(e.Descricao)),
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL
                    FROM dbo.EstadoExameESP e
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Consultas.EstadoExameEsp x
                        WHERE x.Codigo = e.Codigo
                    );
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.RequisicoesEsp', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO Consultas.RequisicaoEsp
                    (
                        Id, Codigo, NumeroRequisicao, CodigoMedico, EnpAssinado, Historico,
                        Estado, DataCativacao, DataAgendamento, DataServico, DataRealizacao, UltimaData,
                        CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedBy, DeletedOn
                    )
                    SELECT
                        NEWID(),
                        r.Codigo,
                        LTRIM(RTRIM(r.NumeroRequisicao)),
                        NULLIF(LTRIM(RTRIM(r.CodigoMedico)), N''),
                        CAST(ISNULL(r.ENP_Assinado, 0) AS bit),
                        CAST(ISNULL(r.Historico, 0) AS bit),
                        ISNULL(r.Estado, 0),
                        ISNULL(r.DataCativacao, SYSUTCDATETIME()),
                        r.DataAgendamento,
                        r.DataServico,
                        r.DataRealizacao,
                        ISNULL(r.UltimaData, SYSUTCDATETIME()),
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL,
                        NULL,
                        CASE WHEN ISNULL(r.Apagado, 0) = 1 THEN SYSUTCDATETIME() ELSE NULL END
                    FROM dbo.RequisicoesEsp r
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Consultas.RequisicaoEsp x
                        WHERE x.Codigo = r.Codigo
                    );
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.RequisicoesESPLinha', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO Consultas.RequisicaoEspLinha
                    (
                        Id, Codigo, RequisicaoEspId, CodigoMcdt,
                        CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedBy, DeletedOn
                    )
                    SELECT
                        NEWID(),
                        rl.Codigo,
                        req.Id,
                        LTRIM(RTRIM(rl.CodMCDT)),
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL,
                        NULL,
                        NULL
                    FROM dbo.RequisicoesESPLinha rl
                    INNER JOIN Consultas.RequisicaoEsp req ON req.Codigo = rl.CodigoRequisicaoESP
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Consultas.RequisicaoEspLinha x
                        WHERE x.Codigo = rl.Codigo
                    );
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.EFETUADOS_N_PRESC', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO Consultas.RequisicaoEspEfetuadoNaoPrescrito
                    (
                        Id, Codigo, RequisicaoEspId, CodigoMcdt, NAmostras,
                        CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedBy, DeletedOn
                    )
                    SELECT
                        NEWID(),
                        e.Codigo,
                        req.Id,
                        LTRIM(RTRIM(e.CodigoMCDT)),
                        ISNULL(e.NAmostras, 0),
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL,
                        NULL,
                        NULL
                    FROM dbo.EFETUADOS_N_PRESC e
                    INNER JOIN Consultas.RequisicaoEsp req ON req.Codigo = e.CodigoRequisicaoESP
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Consultas.RequisicaoEspEfetuadoNaoPrescrito x
                        WHERE x.Codigo = e.Codigo
                    );
                END
                """);

            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.ACOR_INS', N'U') IS NOT NULL
                BEGIN
                    UPDATE ss
                    SET ss.CodigoMcdt = LTRIM(RTRIM(a.cservinst))
                    FROM Servicos.SubsistemaServico ss
                    INNER JOIN Servicos.Servico s ON s.Id = ss.ServicoId
                    INNER JOIN Organismos.Organismo o ON o.Id = ss.OrganismoId
                    INNER JOIN dbo.ACOR_INS a
                        ON a.c_instit = o.CodigoULSNova
                       AND LTRIM(RTRIM(a.c_servico)) = LTRIM(RTRIM(s.EAN))
                    WHERE o.CodigoULSNova IS NOT NULL
                      AND s.EAN IS NOT NULL
                      AND NULLIF(LTRIM(RTRIM(a.cservinst)), N'') IS NOT NULL;
                END
                """);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoExameEsp_Abreviatura",
                schema: "Consultas",
                table: "EstadoExameEsp",
                column: "Abreviatura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoExameEsp_Codigo",
                schema: "Consultas",
                table: "EstadoExameEsp",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEsp_Codigo",
                schema: "Consultas",
                table: "RequisicaoEsp",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEsp_NumeroRequisicao",
                schema: "Consultas",
                table: "RequisicaoEsp",
                column: "NumeroRequisicao");

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEspEfetuadoNaoPrescrito_Codigo",
                schema: "Consultas",
                table: "RequisicaoEspEfetuadoNaoPrescrito",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEspEfetuadoNaoPrescrito_RequisicaoEspId",
                schema: "Consultas",
                table: "RequisicaoEspEfetuadoNaoPrescrito",
                column: "RequisicaoEspId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEspLinha_Codigo",
                schema: "Consultas",
                table: "RequisicaoEspLinha",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoEspLinha_RequisicaoEspId",
                schema: "Consultas",
                table: "RequisicaoEspLinha",
                column: "RequisicaoEspId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoExameEsp",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "RequisicaoEspEfetuadoNaoPrescrito",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "RequisicaoEspLinha",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "RequisicaoEsp",
                schema: "Consultas");

            migrationBuilder.DropColumn(
                name: "CodigoMcdt",
                schema: "Servicos",
                table: "SubsistemaServico");
        }
    }
}
