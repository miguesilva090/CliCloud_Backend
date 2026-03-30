using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIcd11Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Doencas");

            migrationBuilder.CreateTable(
                name: "Icd11",
                schema: "Doencas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IcdId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    ClassKind = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icd11",
                schema: "Doencas");
        }
    }
}
