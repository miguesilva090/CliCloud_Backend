using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProvenienciaUtenteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvenienciaUtente",
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
                    table.PrimaryKey("PK_ProvenienciaUtente", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvenienciaUtente_Codigo",
                schema: "Utility",
                table: "ProvenienciaUtente",
                column: "Codigo",
                unique: true);

            // Seed Proveniências Utente (conforme imagem de referência)
            migrationBuilder.Sql(@"
                INSERT INTO Utility.ProvenienciaUtente (Id, Codigo, Descricao, CreatedBy, CreatedOn)
                VALUES
                    (NEWID(), '1', 'Online', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '2', 'Telefone', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '3', 'Centro de Saude', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '4', 'Medico Familia', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '5', 'Medico Externo', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '6', 'Amigo', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '7', 'Internet', '00000000-0000-0000-0000-000000000000', GETUTCDATE());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvenienciaUtente",
                schema: "Utility");
        }
    }
}
