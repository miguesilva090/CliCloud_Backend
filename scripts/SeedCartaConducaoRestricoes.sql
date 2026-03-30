-- =============================================================================
-- Seed: Restrições de Carta de Condução (Portugal - IMT)
-- Schema: CartaConducao, Tabela: CartaConducaoRestricoes
-- CodigoRestricao = inteiros normais sequenciais (1, 2, 3, ... 94). Sem pontos.
-- Descricao = apenas texto, sem códigos nem números.
-- Idempotente: WHERE NOT EXISTS (CodigoRestricao).
-- =============================================================================

DECLARE @CreatedBy UNIQUEIDENTIFIER = 'EEDEE592-CFBE-4382-B35D-7370E7886275';

-- 01.xx (visão)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 1, N'Óculos', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 1);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2, N'Lente(s) de contacto', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3, N'Cobertura ocular', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4, N'Óculos ou lentes de contacto', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 5, N'Ajuda ótica específica', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 5);

-- 02
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 6, N'Prótese auditiva/ajuda à comunicação', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 6);

-- 03.xx
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 7, N'Prótese/ortotese de um/dos membro(s) superior(es)', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 7);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 8, N'Prótese/ortotese de um/dos membro(s) inferior(es)', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 8);

-- 10.xx (transmissão)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 9, N'Seleção automática da relação de transmissão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 9);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 10, N'Dispositivo de comando de transmissão adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 10);

-- 103, 105 (3 dígitos - mantidos)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 11, N'Capacete com viseira', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 11);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 12, N'Pára-brisas inamovível', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 12);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 13, N'Avaliação médica antecipada', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 13);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 14, N'Avaliação psicológica antecipada', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 14);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 15, N'Uso de colete ortopédico', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 15);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 16, N'Avaliação psicológica obrigatória', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 16);

-- 15.xx (embraiagem)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 17, N'Pedal de embraiagem adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 17);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 18, N'Embraiagem manual', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 18);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 19, N'Embraiagem automática', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 19);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 20, N'Medida destinada a evitar a obstrução ou o acionamento do pedal de embraiagem.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 20);

-- 160 (3 dígitos)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 21, N'Isenção do cinto de segurança, sujeito à posse de atestado médico válido.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 21);

-- 20.xx (travão)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 22, N'Pedal do travão adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 22);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 23, N'Pedal do travão adequado para ser utilizado com o pé esquerdo', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 23);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 24, N'Pedal do travão com corrediça', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 24);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 25, N'Pedal do travão inclinado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 25);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 26, N'Travão de serviço', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 26);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 27, N'Funcionamento do travão com força máxima a indicar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 27);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 28, N'Travão de estacionamento adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 28);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 29, N'Medida destinada a evitar a obstrução ou o acionamento do pedal do travão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 29);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 30, N'Travão comandado pelo joelho', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 30);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 31, N'Acionamento do sistema de travagem assistido por uma força exterior.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 31);

-- 25.xx (acelerador)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 32, N'Pedal do acelerador adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 32);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2503, N'Pedal do acelerador inclinado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2503);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2504, N'Acelerador manual', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2504);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2505, N'Acelerador comandado pelo joelho', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2505);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2506, N'Acionamento do acelerador assistido por uma força exterior', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2506);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2508, N'Pedal do acelerador à esquerda', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2508);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 2509, N'Medida destinada a evitar a obstrução ou acionamento do pedal do acelerador.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 2509);

-- 31.xx (pedais)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3101, N'Conjunto suplementar de pedais paralelos', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3101);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3102, N'Pedais ao (ou quase ao) mesmo nível;', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3102);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3103, N'Medida destinada a evitar a obstrução ou acionamento dos pedais do acelerador e do travão não acionados pelo pé', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3103);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3104, N'Piso elevado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3104);

-- 32.xx, 33.xx, 35.xx
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3201, N'Acelerador e travão de serviço enquanto sistema combinado acionado com uma mão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3201);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3202, N'Acelerador e travão de serviço enquanto sistema combinado acionado por uma força exterior', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3202);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3301, N'Acelerador, travão de serviço e direção, enquanto sistema combinado acionado por uma força exterior com uma mão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3301);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3302, N'Acelerador, travão de serviço e direção, enquanto sistema combinado acionado por uma força exterior com duas mãos', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3302);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3502, N'Dispositivos de comando acionáveis sem libertar o dispositivo de direção);', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3502);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3503, N'Dispositivos de comando acionáveis sem libertar o dispositivo de direção com a mão esquerda;', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3503);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3504, N'Dispositivos de comando acionáveis sem libertar o dispositivo de direção com a mão direita', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3504);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 3505, N'Dispositivos de comando acionáveis sem libertar o dispositivo de direção e os comandos do acelerador e do travão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 3505);

-- 40.xx (direção)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4001, N'Direção com força máxima de funcionamento a indicar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4001);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4005, N'Volante adaptado (secção do volante maior e ou mais espessa, volante de diâmetro reduzido, etc.)', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4005);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4006, N'Posição adaptada do volante', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4006);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4009, N'Condução com os pés', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4009);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4011, N'Dispositivo de assistência no volante', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4011);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4014, N'Sistema de direção adaptada alternativa acionado com uma mão ou com o braço', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4014);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4015, N'Sistema de direção adaptada alternativa acionado com duas mãos ou com os dois braços', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4015);

-- 42.xx, 43.xx, 44.xx
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4201, N'Dispositivo adaptado de retrovisão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4201);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4203, N'Dispositivo interior adicional que permita uma visão lateral', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4203);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4205, N'Dispositivo de visualização para o ângulo morto', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4205);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4301, N'Banco do condutor à altura adequada para permitir uma visão normal e à distância normal do volante e dos pedais', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4301);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4302, N'Banco do condutor adaptado à forma do corpo', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4302);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4303, N'Banco do condutor com apoio lateral para uma boa estabilidade', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4303);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4304, N'Banco do condutor com braço de apoio', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4304);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4306, N'Cinto de segurança adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4306);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4307, N'Tipo de cinto de segurança com suporte para uma boa estabilidade', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4307);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4401, N'Travões de pé e de mão combinados num só', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4401);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4402, N'Travão da roda da frente adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4402);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4403, N'Travão da roda traseira adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4403);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4404, N'Acelerador adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4404);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4408, N'Altura do banco adequada para permitir ao condutor ter simultaneamente os dois pés no chão em posição sentada e equilibrar o motociclo durante a paragem e o estacionamento', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4408);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4409, N'Força máxima de funcionamento do travão da roda da frente a indicar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4409);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4410, N'Força máxima de funcionamento do travão da roda da traseira a indicar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4410);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4411, N'Apoio para pés adaptado', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4411);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 4412, N'Pega adaptada', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 4412);

-- 45, 46, 47 (códigos só número)
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 45, N'Unicamente motociclo com carro lateral', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 45);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 46, N'Unicamente triciclos', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 46);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 47, N'Restrito a veículos com mais de duas rodas que não necessitem de ser equilibrados pelo condutor para o arranque, paragem e o estacionamento', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 47);

-- 50.a a 50.g → 501 a 507
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 501, N'Esquerda', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 501);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 502, N'Direita', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 502);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 503, N'50.c - mão', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 503);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 504, N'Pé', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 504);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 505, N'Meio', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 505);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 506, N'Braço', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 506);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 507, N'Polegar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 507);

-- 61 a 69
INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 61, N'Limitada a deslocações durante o dia', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 61);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 62, N'Limitada a deslocações num raio de … km a contar da residência do titular ou apenas na cidade ou região da sua residência.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 62);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 63, N'Condução sem passageiros', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 63);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 64, N'Limitada a deslocações a velocidade máxima a indicar', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 64);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 65, N'Condução autorizada exclusivamente quando acompanhada por titular de carta de condução da categoria, no mínimo equivalente.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 65);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 66, N'Sem reboque', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 66);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 67, N'Condução não autorizada em auto-estradas', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 67);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 68, N'Proibida a ingestão de bebidas alcoólicas', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 68);

INSERT INTO CartaConducao.CartaConducaoRestricoes (Id, CodigoRestricao, Descricao, Inativo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
SELECT NEWID(), 69, N'Limitada à condução de veículos equipados com dispositivos de bloqueio da ignição em caso de ingestão de álcool, em conformidade com norma europeia aplicável, sem indicação do prazo de validade.', 0, @CreatedBy, GETUTCDATE(), NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM CartaConducao.CartaConducaoRestricoes WHERE CodigoRestricao = 69);

-- Verificação
SELECT COUNT(*) AS TotalRestricoes FROM CartaConducao.CartaConducaoRestricoes;
