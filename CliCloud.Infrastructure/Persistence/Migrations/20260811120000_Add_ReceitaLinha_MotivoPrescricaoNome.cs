using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260811120000_Add_ReceitaLinha_MotivoPrescricaoNome")]
    public class Add_ReceitaLinha_MotivoPrescricaoNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodIndicacaoTerapeutica",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CodMotivo",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CodTipoPrescricao",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodIndicacaoTerapeutica",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "CodMotivo",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "CodTipoPrescricao",
                schema: "Prescricao",
                table: "ReceitaLinha");
        }
    }
}
