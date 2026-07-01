using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ConfiguracaoADSE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Faturacao");

            migrationBuilder.CreateTable(
                name: "ConfiguracaoADSE",
                schema: "Faturacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlADSE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Dominio = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Utilizador = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroLocal = table.Column<int>(type: "int", nullable: true),
                    PasswordLocal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NomeLocal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UrlPasta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoADSE", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoADSE_EmpresaId",
                schema: "Faturacao",
                table: "ConfiguracaoADSE",
                column: "EmpresaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoADSE",
                schema: "Faturacao");
        }
    }
}
