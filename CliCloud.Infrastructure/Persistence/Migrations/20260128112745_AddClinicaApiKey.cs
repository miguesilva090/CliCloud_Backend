using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicaApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClinicaAPIKey",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaAPIKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicaAPIKey_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaAPIKey_ApiKey",
                schema: "Core",
                table: "ClinicaAPIKey",
                column: "ApiKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaAPIKey_ClinicaId_ApiKey",
                schema: "Core",
                table: "ClinicaAPIKey",
                columns: new[] { "ClinicaId", "ApiKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicaAPIKey",
                schema: "Core");
        }
    }
}
