using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_UnidadesLocaisSaude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UnidadesLocaisSaude");

            migrationBuilder.CreateTable(
                name: "UnidadesLocaisSaude",
                schema: "UnidadesLocaisSaude",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesLocaisSaude", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesLocaisSaude_Codigo",
                schema: "UnidadesLocaisSaude",
                table: "UnidadesLocaisSaude",
                column: "Codigo",
                unique: true);

            // Seed da "Nova ULS" (equivalente a dbo.UlsNovas no legado - Código/Nome/NIF).
            migrationBuilder.Sql(@"
INSERT INTO [UnidadesLocaisSaude].[UnidadesLocaisSaude] ([Id],[Codigo],[Nome],[Nif],[CreatedBy],[CreatedOn])
VALUES
(NEWID(), 1, N'Unidade Local de Saúde Alentejo Central', N'508085888', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 2, N'Unidade Local de Saúde Algarve', N'510745997', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 3, N'Unidade Local de Saúde Almada / Seixal', N'506361470', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 4, N'Unidade Local de Saúde Alto Alentejo', N'508094461', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 5, N'Unidade Local de Saúde Alto Ave', N'508080827', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 6, N'Unidade Local de Saúde Amadora / Sintra', N'503035416', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 7, N'Unidade Local de Saúde Arco Ribeirinho', N'509186998', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 8, N'Unidade Local de Saúde Arrábida', N'507606787', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 9, N'Unidade Local de Saúde Baixo Mondego', N'506361527', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 10, N'Unidade Local de Saúde Barcelos / Esposende', N'506361381', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 11, N'Unidade Local de Saúde Coimbra', N'510103448', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 12, N'Unidade Local de Saúde Cova da Beira', N'506361659', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 13, N'Unidade Local de Saúde da Guarda', N'508752000', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 14, N'Unidade Local de Saúde de Braga', N'515545180', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 15, N'Unidade Local de Saúde de Castelo Branco', N'509309844', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 16, N'Unidade Local de Saúde de Loures / Odivelas', N'516726862', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 17, N'Unidade Local de Saúde de Matosinhos', N'506361390', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 18, N'Unidade Local de Saúde de Santo António', N'517392259', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 19, N'Unidade Local de Saúde do Alto Minho', N'508786193', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 20, N'Unidade Local de Saúde do Baixo Alentejo', N'508754275', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 21, N'Unidade Local de Saúde do Estuário do Tejo', N'516487493', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 22, N'Unidade Local de Saúde do Litoral Alentejano', N'510445152', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 23, N'Unidade Local de Saúde do Nordeste', N'509932584', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 24, N'Unidade Local de Saúde Entre Douro e Vouga', N'508878462', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 25, N'Unidade Local de Saúde Gaia / Espinho', N'508142156', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 26, N'Unidade Local de Saúde Lezíria', N'506361462', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 27, N'Unidade Local de Saúde Lisboa Ocidental', N'507618319', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 28, N'Unidade Local de Saúde Médio Ave', N'508093937', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 29, N'Unidade Local de Saúde Médio Tejo', N'506361608', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 30, N'Unidade Local de Saúde Oeste', N'514993871', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 31, N'Unidade Local de Saúde Póvoa Varzim / Vila Conde', N'508741823', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 32, N'Unidade Local de Saúde Região de Aveiro', N'510123210', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 33, N'Unidade Local de Saúde Região de Leiria', N'509822932', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 34, N'Unidade Local de Saúde Santa Maria', N'508481287', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 35, N'Unidade Local de Saúde São João', N'509821197', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 36, N'Unidade Local de Saúde São José', N'508080142', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 37, N'Unidade Local de Saúde Tâmega e Sousa', N'508318262', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 38, N'Unidade Local de Saúde Trás-os-Montes Alto Douro', N'508100496', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 39, N'Unidade Local de Saúde Viseu Dão-Lafões', N'509822940', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 40, N'Instituto para os Comportamentos Aditivos e as Dependências (ICAD)', N'517839539', '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
(NEWID(), 41, N'Hospital de Cascais, PPP', N'517091402', '00000000-0000-0000-0000-000000000000', GETUTCDATE());
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnidadesLocaisSaude",
                schema: "UnidadesLocaisSaude");
        }
    }
}
