using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSexoTableAndSexoIdToEntidadePessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Sexo table
            migrationBuilder.CreateTable(
                name: "Sexo",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sexo_Codigo",
                schema: "Utility",
                table: "Sexo",
                column: "Codigo",
                unique: true);

            // 2. Seed Sexo: 1=Masculino, 2=Feminino, 3=Indefinido (enum: Masculino=0, Feminino=1, Outro=2)
            Guid masculinoId = Guid.NewGuid();
            Guid femininoId = Guid.NewGuid();
            Guid indefinidoId = Guid.NewGuid();
            migrationBuilder.Sql($@"
                INSERT INTO Utility.Sexo (Id, Codigo, Descricao, CreatedBy, CreatedOn)
                VALUES
                    ('{masculinoId}', '1', 'Masculino', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    ('{femininoId}', '2', 'Feminino', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    ('{indefinidoId}', '3', 'Indefinido', '00000000-0000-0000-0000-000000000000', GETUTCDATE());
            ");

            // 3. Add SexoId column
            migrationBuilder.AddColumn<Guid>(
                name: "SexoId",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "uniqueidentifier",
                nullable: true);

            // 4. Migrate data: Sexo=0 -> Masculino, Sexo=1 -> Feminino, Sexo=2 -> Indefinido
            migrationBuilder.Sql($@"
                UPDATE Utility.EntidadePessoa SET SexoId = '{masculinoId}' WHERE Sexo = 0;
                UPDATE Utility.EntidadePessoa SET SexoId = '{femininoId}' WHERE Sexo = 1;
                UPDATE Utility.EntidadePessoa SET SexoId = '{indefinidoId}' WHERE Sexo = 2;
            ");

            // 5. Drop old Sexo column
            migrationBuilder.DropColumn(
                name: "Sexo",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadePessoa_SexoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "SexoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntidadePessoa_Sexo_SexoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "SexoId",
                principalSchema: "Utility",
                principalTable: "Sexo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntidadePessoa_Sexo_SexoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropTable(
                name: "Sexo",
                schema: "Utility");

            migrationBuilder.DropIndex(
                name: "IX_EntidadePessoa_SexoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropColumn(
                name: "SexoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "int",
                nullable: true);
        }
    }
}
