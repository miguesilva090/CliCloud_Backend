using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedSeparadoresBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[ProcessoClinico].[Separador]', N'U') IS NOT NULL
                BEGIN
                    DECLARE @seed TABLE
                    (
                        Ordem INT NOT NULL,
                        Nome NVARCHAR(150) NOT NULL,
                        Ativo BIT NOT NULL
                    );

                    INSERT INTO @seed (Ordem, Nome, Ativo)
                    VALUES
                    (1,  N'Antecedentes', 1),
                    (2,  N'Sinais Vitais', 1),
                    (3,  N'História Clínica', 1),
                    (4,  N'Tratamentos', 1),
                    (5,  N'Prescrição de Exames', 1),
                    (6,  N'Dentária', 1),
                    (7,  N'Laboratório', 0),
                    (8,  N'Bloco Operatório', 0),
                    (9,  N'Internamento', 0),
                    (10, N'Relatório/Atestado', 1),
                    (11, N'Medicação', 1),
                    (12, N'Documentos', 1),
                    (13, N'Tensão Arterial', 1),
                    (14, N'Glicemia Capilar', 1),
                    (15, N'Temperatura Corporal', 1),
                    (16, N'IMC (Índice Massa Corporal)', 1),
                    (19, N'Processo Clínico', 1),
                    (20, N'Antecedentes Pessoais', 1),
                    (21, N'Antecedentes Familiares', 1),
                    (22, N'Alergias', 1),
                    (23, N'Questionário', 1),
                    (24, N'Hábitos e Vícios', 1),
                    (25, N'Ficha Tratamento', 1),
                    (26, N'Evolução Tratamento', 1),
                    (28, N'Gordura/Massa Muscular', 1),
                    (29, N'Avaliação Antropometrica', 1),
                    (30, N'Avaliação Postural', 1),
                    (31, N'Escala de Dor', 1);

                    UPDATE s
                    SET
                        s.Nome = x.Nome,
                        s.Ativo = x.Ativo,
                        s.LastModifiedOn = SYSUTCDATETIME(),
                        s.DeletedOn = NULL,
                        s.DeletedBy = NULL
                    FROM [ProcessoClinico].[Separador] s
                    INNER JOIN @seed x ON x.Ordem = s.Ordem;

                    INSERT INTO [ProcessoClinico].[Separador]
                        ([Id], [Nome], [Ordem], [Ativo], [CreatedBy], [CreatedOn], [LastModifiedBy], [LastModifiedOn], [DeletedOn], [DeletedBy])
                    SELECT
                        NEWID(), x.Nome, x.Ordem, x.Ativo,
                        '00000000-0000-0000-0000-000000000000', SYSUTCDATETIME(), NULL, NULL, NULL, NULL
                    FROM @seed x
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM [ProcessoClinico].[Separador] s
                        WHERE s.Ordem = x.Ordem
                    );
                END
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[ProcessoClinico].[Separador]', N'U') IS NOT NULL
                BEGIN
                    DELETE FROM [ProcessoClinico].[Separador]
                    WHERE [Ordem] IN (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,19,20,21,22,23,24,25,26,28,29,30,31);
                END
                """
            );
        }
    }
}
