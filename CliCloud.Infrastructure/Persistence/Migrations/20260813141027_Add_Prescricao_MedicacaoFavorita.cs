using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Prescricao_MedicacaoFavorita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicacaoFavorita",
                schema: "Prescricao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cnpem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmbId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Dosagem = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    DescricaoEmbalagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FormaFarmaceutica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrincipioAtivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Posologia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TipoLinha = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicacaoFavorita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicacaoFavorita_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicacaoFavorita_MedicoId_Cnpem",
                schema: "Prescricao",
                table: "MedicacaoFavorita",
                columns: new[] { "MedicoId", "Cnpem" },
                unique: true,
                filter: "[DeletedOn] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicacaoFavorita",
                schema: "Prescricao");
        }
    }
}
