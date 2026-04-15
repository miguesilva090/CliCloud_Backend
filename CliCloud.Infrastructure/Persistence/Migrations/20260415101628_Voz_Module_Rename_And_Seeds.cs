using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Voz_Module_Rename_And_Seeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('Core.ConfiguracaoVoz', 'U') IS NULL AND OBJECT_ID('Core.ConfiguracaoChamadaVoz', 'U') IS NOT NULL
BEGIN
  EXEC sp_rename 'Core.ConfiguracaoChamadaVoz', 'ConfiguracaoVoz';
END
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('Core.ConfiguracaoVozOpcao', 'U') IS NULL AND OBJECT_ID('Core.ConfiguracaoChamadaVozOpcao', 'U') IS NOT NULL
BEGIN
  EXEC sp_rename 'Core.ConfiguracaoChamadaVozOpcao', 'ConfiguracaoVozOpcao';
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Core.ConfiguracaoVoz', 'Provider') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD Provider NVARCHAR(50) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_Provider DEFAULT 'web-speech';
IF COL_LENGTH('Core.ConfiguracaoVoz', 'IdiomaPadrao') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD IdiomaPadrao NVARCHAR(10) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_IdiomaPadrao DEFAULT 'pt-PT';
IF COL_LENGTH('Core.ConfiguracaoVoz', 'SttAtivo') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD SttAtivo BIT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_SttAtivo DEFAULT(1);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'SttInterimResults') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD SttInterimResults BIT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_SttInterimResults DEFAULT(1);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'SttContinuous') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD SttContinuous BIT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_SttContinuous DEFAULT(1);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'SttAutoPontuacao') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD SttAutoPontuacao BIT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_SttAutoPontuacao DEFAULT(1);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'SttConfidenceMin') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD SttConfidenceMin DECIMAL(5,4) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_SttConfidenceMin DEFAULT(0.5000);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TtsAtivo') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TtsAtivo BIT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_TtsAtivo DEFAULT(1);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TtsVoice') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TtsVoice NVARCHAR(100) NULL;
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TtsRate') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TtsRate DECIMAL(5,2) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_TtsRate DEFAULT(1.00);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TtsPitch') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TtsPitch DECIMAL(5,2) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_TtsPitch DEFAULT(1.00);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TtsVolume') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TtsVolume DECIMAL(5,2) NOT NULL CONSTRAINT DF_ConfiguracaoVoz_TtsVolume DEFAULT(1.00);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'TimeoutMs') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD TimeoutMs INT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_TimeoutMs DEFAULT(15000);
IF COL_LENGTH('Core.ConfiguracaoVoz', 'MaxDuracaoCapturaSegundos') IS NULL
  ALTER TABLE Core.ConfiguracaoVoz ADD MaxDuracaoCapturaSegundos INT NOT NULL CONSTRAINT DF_ConfiguracaoVoz_MaxDuracaoCapturaSegundos DEFAULT(90);
");

            migrationBuilder.Sql(@"
UPDATE Core.ConfiguracaoVoz
SET IdiomaPadrao = CASE
  WHEN IdiomaPadrao = 'pt' THEN 'pt-PT'
  WHEN IdiomaPadrao = 'en' THEN 'en-US'
  WHEN IdiomaPadrao = 'fr' THEN 'fr-FR'
  WHEN IdiomaPadrao = 'es' THEN 'es-ES'
  WHEN IdiomaPadrao IS NULL OR LTRIM(RTRIM(IdiomaPadrao)) = '' THEN 'pt-PT'
  ELSE IdiomaPadrao
END;

UPDATE Core.ConfiguracaoVoz
SET TtsVoice = NULL
WHERE TtsVoice IN ('pt','com.br','com.au','co.uk','com','ca','co.in','ie','co.za','fr','es','com.mx');
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('Core.ConfiguracaoVozOpcao', 'U') IS NOT NULL
BEGIN
  DELETE FROM Core.ConfiguracaoVozOpcao
  WHERE Id IN (
    'f1a10000-0000-0000-0000-000000000001','f1a10000-0000-0000-0000-000000000002','f1a10000-0000-0000-0000-000000000003',
    'f1a10000-0000-0000-0000-000000000004','f1a10000-0000-0000-0000-000000000005','f1a10000-0000-0000-0000-000000000006',
    'f1a10000-0000-0000-0000-000000000101','f1a10000-0000-0000-0000-000000000102','f1a10000-0000-0000-0000-000000000103',
    'f1a10000-0000-0000-0000-000000000104','f1a10000-0000-0000-0000-000000000105','f1a10000-0000-0000-0000-000000000106',
    'f1a10000-0000-0000-0000-000000000107','f1a10000-0000-0000-0000-000000000108','f1a10000-0000-0000-0000-000000000109',
    'f1a10000-0000-0000-0000-000000000110','f1a10000-0000-0000-0000-000000000111','f1a10000-0000-0000-0000-000000000112'
  );

  INSERT INTO Core.ConfiguracaoVozOpcao (Id, Tipo, Codigo, Descricao, Ordem, Ativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedOn, DeletedBy)
  VALUES
  ('f1a10000-0000-0000-0000-000000000001','Language','pt-PT','Português (PT)',1,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000002','Language','pt-BR','Português (BR)',2,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000003','Language','en-US','Inglês (US)',3,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000004','Language','es-ES','Espanhol (ES)',4,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000005','Language','zh-CN','Mandarim (China)',5,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000006','Language','zh-TW','Mandarim (Taiwan)',6,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000101','Voice','pt-PT-Female','Português (PT) - Feminina',1,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000102','Voice','pt-PT-Male','Português (PT) - Masculina',2,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000103','Voice','pt-BR-Female','Português (BR) - Feminina',3,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000104','Voice','pt-BR-Male','Português (BR) - Masculina',4,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000105','Voice','en-US-Female','Inglês (US) - Feminina',5,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000106','Voice','en-US-Male','Inglês (US) - Masculina',6,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000107','Voice','es-ES-Female','Espanhol (ES) - Feminina',7,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000108','Voice','es-ES-Male','Espanhol (ES) - Masculina',8,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000109','Voice','fr-FR-Female','Francês (FR) - Feminina',9,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000110','Voice','fr-FR-Male','Francês (FR) - Masculina',10,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000111','Voice','zh-CN-Female','Mandarim (CN) - Feminina',11,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL),
  ('f1a10000-0000-0000-0000-000000000112','Voice','zh-CN-Male','Mandarim (CN) - Masculina',12,1,'00000000-0000-0000-0000-000000000000','0001-01-01T00:00:00',NULL,NULL,NULL,NULL);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('Core.ConfiguracaoVoz', 'U') IS NOT NULL AND OBJECT_ID('Core.ConfiguracaoChamadaVoz', 'U') IS NULL
BEGIN
  EXEC sp_rename 'Core.ConfiguracaoVoz', 'ConfiguracaoChamadaVoz';
END
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('Core.ConfiguracaoVozOpcao', 'U') IS NOT NULL AND OBJECT_ID('Core.ConfiguracaoChamadaVozOpcao', 'U') IS NULL
BEGIN
  EXEC sp_rename 'Core.ConfiguracaoVozOpcao', 'ConfiguracaoChamadaVozOpcao';
END
");
        }
    }
}
