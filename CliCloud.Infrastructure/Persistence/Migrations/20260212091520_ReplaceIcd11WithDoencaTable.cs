using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIcd11WithDoencaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icd11",
                schema: "Doencas");

            migrationBuilder.CreateTable(
                name: "Doenca",
                schema: "Doencas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IcdId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    ClassKind = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doenca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doenca_Doenca_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doenca_Code",
                schema: "Doencas",
                table: "Doenca",
                column: "Code",
                filter: "[Code] IS NOT NULL AND [Code] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Doenca_IcdId",
                schema: "Doencas",
                table: "Doenca",
                column: "IcdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doenca_ParentId",
                schema: "Doencas",
                table: "Doenca",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doenca",
                schema: "Doencas");

            migrationBuilder.CreateTable(
                name: "Icd11",
                schema: "Doencas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ClassKind = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    IcdId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icd11", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Icd11_Icd11_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Doencas",
                        principalTable: "Icd11",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Icd11_Code",
                schema: "Doencas",
                table: "Icd11",
                column: "Code",
                filter: "[Code] IS NOT NULL AND [Code] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Icd11_IcdId",
                schema: "Doencas",
                table: "Icd11",
                column: "IcdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icd11_ParentId",
                schema: "Doencas",
                table: "Icd11",
                column: "ParentId");
        }
    }
}
