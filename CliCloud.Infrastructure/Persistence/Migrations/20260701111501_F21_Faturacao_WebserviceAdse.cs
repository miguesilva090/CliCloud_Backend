using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F21_Faturacao_WebserviceAdse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebserviceAdse",
                schema: "Faturacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoInstituicaoAdse = table.Column<int>(type: "int", nullable: false),
                    ClinicaFisioterapiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlAdse = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    DominioUserAdse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserAdse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordAdse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumlocalAdse = table.Column<int>(type: "int", nullable: false),
                    NomelocalAdse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PasslocalAdse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PastaPdfAdse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebserviceAdse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebserviceAdse_ClinicaId",
                schema: "Faturacao",
                table: "WebserviceAdse",
                column: "ClinicaId",
                unique: true,
                filter: "[DeletedOn] IS NULL ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebserviceAdse",
                schema: "Faturacao");
        }
    }
}
