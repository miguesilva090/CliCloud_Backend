using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SessaoTratamento_Confirmado_Efetuado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Confirmado",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Efetuado",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmado",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropColumn(
                name: "Efetuado",
                schema: "Tratamentos",
                table: "SessaoTratamento");
        }
    }
}
