using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Horario_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FolgaDom",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaQua",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaQui",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaSab",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaSeg",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaSex",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FolgaTer",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraFimManha",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraFimTarde",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraInicManha",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraInicTarde",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Interrupcao",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolgaDom",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaQua",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaQui",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaSab",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaSeg",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaSex",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FolgaTer",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "HoraFimManha",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "HoraFimTarde",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "HoraInicManha",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "HoraInicTarde",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Interrupcao",
                schema: "Core",
                table: "Clinica");
        }
    }
}
