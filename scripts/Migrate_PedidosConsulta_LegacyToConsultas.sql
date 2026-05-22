/*
  OPCIONAL — só se legado e novo partilham a MESMA BD e existem dbo.PedidosConsulta.
  No projeto novo o isolamento é por Consultas.PedidoConsulta.ClinicaId (Guid), não por Filtro int.

  Migração: dbo.PedidosConsulta + dbo.PedidosConsultaUtente
           → Consultas.PedidoConsulta + Consultas.PedidoConsultaUtente

  Schema legado (script00964 + script00965/972):    dbo.PedidosConsultaUtente: Codigo, Nome, Codinst, Email, Telemovel, NIF
    dbo.PedidosConsulta: Codigo, CodigoPedidosConsultaUtente, CodigoEspecialidade,
      Data, Hora, CodigoMedico, Filtro, Agendado, EmailPedido, SmsPedido,
      EmailAgendado, SmsAgendado, Recusado, CodigoAdmissao, Ficheiro

  Schema novo (migration Add_PedidoConsulta_GlobalBooking):
    Consultas.PedidoConsultaUtente: Codigo (PK), Nome, Codinst, Email, Telemovel, NIF
    Consultas.PedidoConsulta: Codigo (PK), CodigoPedidosConsultaUtente (FK), ...

  Executar na BD da clínica, após backup.
*/

SET NOCOUNT ON;
BEGIN TRANSACTION;

-- ---------------------------------------------------------------------------
-- 1) Utentes (FK obrigatória nos pedidos)
-- ---------------------------------------------------------------------------
SET IDENTITY_INSERT Consultas.PedidoConsultaUtente ON;

INSERT INTO Consultas.PedidoConsultaUtente (
  Codigo,
  Nome,
  Codinst,
  Email,
  Telemovel,
  NIF
)
SELECT
  u.Codigo,
  u.Nome,
  u.Codinst,
  u.Email,
  u.Telemovel,
  u.NIF
FROM dbo.PedidosConsultaUtente u
WHERE NOT EXISTS (
  SELECT 1
  FROM Consultas.PedidoConsultaUtente n
  WHERE n.Codigo = u.Codigo
);

SET IDENTITY_INSERT Consultas.PedidoConsultaUtente OFF;

-- ---------------------------------------------------------------------------
-- 2) Pedidos
-- ---------------------------------------------------------------------------
SET IDENTITY_INSERT Consultas.PedidoConsulta ON;

INSERT INTO Consultas.PedidoConsulta (
  Codigo,
  CodigoPedidosConsultaUtente,
  CodigoEspecialidade,
  Data,
  Hora,
  CodigoMedico,
  Filtro,
  Agendado,
  EmailPedido,
  SmsPedido,
  EmailAgendado,
  SmsAgendado,
  Recusado,
  CodigoAdmissao,
  Ficheiro
)
SELECT
  p.Codigo,
  p.CodigoPedidosConsultaUtente,
  p.CodigoEspecialidade,
  p.Data,
  LEFT(LTRIM(RTRIM(p.Hora)), 5),
  NULLIF(LTRIM(RTRIM(p.CodigoMedico)), ''),
  p.Filtro,
  ISNULL(p.Agendado, 0),
  ISNULL(p.EmailPedido, 0),
  ISNULL(p.SmsPedido, 0),
  ISNULL(p.EmailAgendado, 0),
  ISNULL(p.SmsAgendado, 0),
  ISNULL(p.Recusado, 0),
  p.CodigoAdmissao,
  NULLIF(LTRIM(RTRIM(p.Ficheiro)), '')
FROM dbo.PedidosConsulta p
WHERE EXISTS (
  SELECT 1
  FROM Consultas.PedidoConsultaUtente u
  WHERE u.Codigo = p.CodigoPedidosConsultaUtente
)
AND NOT EXISTS (
  SELECT 1
  FROM Consultas.PedidoConsulta n
  WHERE n.Codigo = p.Codigo
);

SET IDENTITY_INSERT Consultas.PedidoConsulta OFF;

-- ---------------------------------------------------------------------------
-- 3) Ajustar IDENTITY para próximos inserts automáticos
-- ---------------------------------------------------------------------------
DECLARE @maxUtente INT = (SELECT ISNULL(MAX(Codigo), 0) FROM Consultas.PedidoConsultaUtente);
DECLARE @maxPedido INT = (SELECT ISNULL(MAX(Codigo), 0) FROM Consultas.PedidoConsulta);

DBCC CHECKIDENT ('Consultas.PedidoConsultaUtente', RESEED, @maxUtente);
DBCC CHECKIDENT ('Consultas.PedidoConsulta', RESEED, @maxPedido);

COMMIT;

-- Verificação rápida:
-- SELECT COUNT(*) AS Legado FROM dbo.PedidosConsulta;
-- SELECT COUNT(*) AS Novo FROM Consultas.PedidoConsulta;
-- SELECT COUNT(*) AS Pendentes FROM Consultas.PedidoConsulta WHERE Agendado = 0 AND Recusado = 0;
-- SELECT Filtro, COUNT(*) FROM Consultas.PedidoConsulta GROUP BY Filtro;
