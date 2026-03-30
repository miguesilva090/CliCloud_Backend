using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodigoFromLocaisTratamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocaisTratamento_Codigo",
                schema: "Tratamentos",
                table: "LocaisTratamento");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Tratamentos",
                table: "LocaisTratamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                schema: "Tratamentos",
                table: "LocaisTratamento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LocaisTratamento_Codigo",
                schema: "Tratamentos",
                table: "LocaisTratamento",
                column: "Codigo",
                unique: true);
        }
    }
}
