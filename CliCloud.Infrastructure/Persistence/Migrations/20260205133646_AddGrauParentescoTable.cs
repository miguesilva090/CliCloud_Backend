using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGrauParentescoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrauParentesco",
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
                    table.PrimaryKey("PK_GrauParentesco", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrauParentesco_Codigo",
                schema: "Utility",
                table: "GrauParentesco",
                column: "Codigo",
                unique: true);

            // Seed Graus Parentesco: Pai, Mãe, Avó materna (conforme imagem de referência)
            migrationBuilder.Sql(@"
                INSERT INTO Utility.GrauParentesco (Id, Codigo, Descricao, CreatedBy, CreatedOn)
                VALUES
                    (NEWID(), '1', 'Pai', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '2', 'Avó materna', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '3', 'Mãe', '00000000-0000-0000-0000-000000000000', GETUTCDATE());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrauParentesco",
                schema: "Utility");
        }
    }
}
