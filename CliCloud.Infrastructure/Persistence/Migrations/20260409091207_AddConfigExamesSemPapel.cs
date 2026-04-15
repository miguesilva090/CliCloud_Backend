using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigExamesSemPapel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigExamesSemPapel",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoEntidade = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PesquisaPrestacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Agendamento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Efetivacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Anulacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConsultaCancelados = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EfetuadosNaoPrescritos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaxasModeradoras = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelatorioResultados = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsernamePartilhaResultados = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordPartilhaResultados = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RelatorioResultadosSemRequisicao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsernamePartilhaResultadosSemRequisicao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordPartilhaResultadosSemRequisicao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AreaPrestacao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigExamesSemPapel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigExamesSemPapel_ClinicaId",
                schema: "Core",
                table: "ConfigExamesSemPapel",
                column: "ClinicaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigExamesSemPapel",
                schema: "Core");
        }
    }
}
