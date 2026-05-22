/*
  Dados de teste — apenas na BD NOVA (não copia legado).
  Substituir @ClinicaId pelo Id (Guid) da clínica da sessão:

  SELECT Id, Nome FROM Core.Clinica;
*/

DECLARE @ClinicaId uniqueidentifier = '00000000-0000-0000-0000-000000000000'; -- TODO: Guid real

SET NOCOUNT ON;
BEGIN TRANSACTION;

INSERT INTO Consultas.PedidoConsultaUtente (Nome, Codinst, Email, Telemovel, NIF)
VALUES (N'Utente Teste GB', 1, N'teste.gb@example.com', N'910000000', N'123456789');

DECLARE @UtenteCodigo int = SCOPE_IDENTITY();

INSERT INTO Consultas.PedidoConsulta (
  ClinicaId,
  CodigoPedidosConsultaUtente,
  CodigoEspecialidade,
  Data,
  Hora,
  CodigoMedico,
  Agendado,
  EmailPedido,
  SmsPedido,
  EmailAgendado,
  SmsAgendado,
  Recusado
)
VALUES (
  @ClinicaId,
  @UtenteCodigo,
  1,
  CAST(GETDATE() AS date),
  N'10:00',
  NULL,
  0,
  0,
  0,
  0,
  0,
  0
);

COMMIT;

SELECT COUNT(*) AS Total FROM Consultas.PedidoConsulta WHERE ClinicaId = @ClinicaId;
