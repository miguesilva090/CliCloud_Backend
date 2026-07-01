using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F22_Faturacao_WebserviceAdse_OrganismoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoInstituicaoAdse",
                schema: "Faturacao",
                table: "WebserviceAdse");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WebserviceAdse_OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse",
                column: "OrganismoId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebserviceAdse_Organismo_OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse",
                column: "OrganismoId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebserviceAdse_Organismo_OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse");

            migrationBuilder.DropIndex(
                name: "IX_WebserviceAdse_OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                schema: "Faturacao",
                table: "WebserviceAdse");

            migrationBuilder.AddColumn<int>(
                name: "CodigoInstituicaoAdse",
                schema: "Faturacao",
                table: "WebserviceAdse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
