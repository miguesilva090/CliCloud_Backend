using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Utente_IdUtilizador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdUtilizador",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_IdUtilizador",
                schema: "Utentes",
                table: "Utente",
                column: "IdUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Utente_IdUtilizador",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "IdUtilizador",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
