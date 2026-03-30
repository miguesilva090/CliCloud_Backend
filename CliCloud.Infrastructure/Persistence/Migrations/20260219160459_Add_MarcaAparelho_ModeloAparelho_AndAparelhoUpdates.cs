using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_MarcaAparelho_ModeloAparelho_AndAparelhoUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MarcaAparelho",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcaAparelho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeloAparelho",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MarcaAparelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloAparelho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeloAparelho_MarcaAparelho_MarcaAparelhoId",
                        column: x => x.MarcaAparelhoId,
                        principalSchema: "Tratamentos",
                        principalTable: "MarcaAparelho",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoAparelho_Designacao",
                schema: "Tratamentos",
                table: "TipoAparelho",
                column: "Designacao");

            migrationBuilder.CreateIndex(
                name: "IX_Aparelho_ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho",
                column: "ModeloAparelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloAparelho_MarcaAparelhoId",
                schema: "Tratamentos",
                table: "ModeloAparelho",
                column: "MarcaAparelhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aparelho_ModeloAparelho_ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho",
                column: "ModeloAparelhoId",
                principalSchema: "Tratamentos",
                principalTable: "ModeloAparelho",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aparelho_ModeloAparelho_ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho");

            migrationBuilder.DropTable(
                name: "ModeloAparelho",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "MarcaAparelho",
                schema: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_TipoAparelho_Designacao",
                schema: "Tratamentos",
                table: "TipoAparelho");

            migrationBuilder.DropIndex(
                name: "IX_Aparelho_ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho");

            migrationBuilder.DropColumn(
                name: "ModeloAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho");
        }
    }
}
