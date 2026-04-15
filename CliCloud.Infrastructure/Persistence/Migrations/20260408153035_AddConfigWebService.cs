using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigWebService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigWebService",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlRnu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlAcss = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoginAcss = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordAcss = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UsarProxy = table.Column<bool>(type: "bit", nullable: false),
                    UserProxy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordProxy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DominioProxy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UrlAcssRsp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoginAcssRsp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordAcssRsp = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UsarProxyRsp = table.Column<bool>(type: "bit", nullable: false),
                    UserProxyRsp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordProxyRsp = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DominioProxyRsp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProxyAutenticacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TokenAutenticacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoginAutenticacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordAutenticacao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VersaoPrescricao = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigWebService", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigWebService_ClinicaId",
                schema: "Core",
                table: "ConfigWebService",
                column: "ClinicaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigWebService",
                schema: "Core");
        }
    }
}
