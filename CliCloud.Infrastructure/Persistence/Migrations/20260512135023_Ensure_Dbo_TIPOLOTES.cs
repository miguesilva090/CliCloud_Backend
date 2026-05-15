using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Ensure_Dbo_TIPOLOTES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.TIPOLOTES', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.TIPOLOTES
                    (
                        codigo INT NOT NULL,
                        valor INT NULL,
                        designa NVARCHAR(50) NULL,
                        CONSTRAINT PK_TIPOLOTES PRIMARY KEY (codigo)
                    );
                END
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
