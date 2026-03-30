using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Empresa_OrganismoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganismoId",
                schema: "Empresas",
                table: "Empresa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_OrganismoId",
                schema: "Empresas",
                table: "Empresa",
                column: "OrganismoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_Organismo_OrganismoId",
                schema: "Empresas",
                table: "Empresa",
                column: "OrganismoId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_Organismo_OrganismoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_OrganismoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                schema: "Empresas",
                table: "Empresa");
        }
    }
}
