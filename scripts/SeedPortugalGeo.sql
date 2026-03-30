-- =============================================================================
-- Seed: Portugal - Pais, Distritos, Concelhos e Freguesias
-- Fonte: json.geoapi.pt (CAOP)
-- Schema: Utility
-- Idempotente: WHERE NOT EXISTS
-- =============================================================================

DECLARE @CreatedBy UNIQUEIDENTIFIER = 'EEDEE592-CFBE-4382-B35D-7370E7886275';
DECLARE @PortugalId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';

-- Pais: Portugal
IF NOT EXISTS (SELECT 1 FROM Utility.Pais WHERE Id = @PortugalId)
INSERT INTO Utility.Pais (Id, Codigo, Nome, Prefixo, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES (@PortugalId, 'PT', N'Portugal', '+351', @CreatedBy, GETUTCDATE(), NULL, NULL);

-- Distritos
IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Aveiro')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('3e75e827-f9bf-4233-8251-9d33c7f529ee', N'Aveiro', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Beja')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('a6139d86-35bd-4ca9-9388-2c1102174e03', N'Beja', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Braga')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('036aaecb-722b-4286-8ef7-8ff71df613af', N'Braga', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Bragança')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('51005294-6ecf-4db3-81c7-58e5154efa62', N'Bragança', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Castelo Branco')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('d625a849-d7cc-4f67-8c3c-bab097dd3a0b', N'Castelo Branco', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Coimbra')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('ecb7ae17-2a99-4ff9-ad30-2d3a9c8d6287', N'Coimbra', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Faro')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('8a869efb-8963-4916-a993-83bc921f5381', N'Faro', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Guarda')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('e404da15-8f14-4cb3-86aa-c713edaf81c3', N'Guarda', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha Das Flores')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('779bdc9b-e669-47f5-9c14-22a1807341b5', N'Ilha Das Flores', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha Terceira')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('fb208ece-81f3-433a-98f5-85a3c3fb076a', N'Ilha Terceira', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha da Graciosa')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('6c6b4bec-2958-4972-8207-57a78d0db2ac', N'Ilha da Graciosa', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha da Madeira')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('2f31bb7a-fcda-42fe-8c23-2e6619c01739', N'Ilha da Madeira', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha de Porto Santo')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('48b494ca-8043-4d12-81a5-20875230ee25', N'Ilha de Porto Santo', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha de Santa Maria')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('09b50f81-3b3c-4c87-9da6-0e21c34c9e6f', N'Ilha de Santa Maria', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha de São Jorge')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('1036cc01-697a-447e-b752-14bb9dcfc420', N'Ilha de São Jorge', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha de São Miguel')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('60c68c26-a8ea-4ded-9028-51d3f1fa83e2', N'Ilha de São Miguel', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha do Corvo')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('25851624-187f-4ef1-8985-df89d5dcaf0b', N'Ilha do Corvo', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha do Faial')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('90241748-49bc-47ca-9fd0-4b426db4795b', N'Ilha do Faial', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Ilha do Pico')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('9c223d6e-c28f-4dba-8619-b58354a53e8c', N'Ilha do Pico', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Leiria')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('3f2a47a1-1166-451c-afe6-abf374b8c48d', N'Leiria', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Lisboa')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('a15e703b-6c2f-4f05-8aec-14ed53aeaaf7', N'Lisboa', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Portalegre')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('331931da-90f3-4caf-abbf-73a1bdaa72df', N'Portalegre', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Porto')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('58824ab2-adff-4f90-9f95-493fb7181e95', N'Porto', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Santarém')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('423b6aa6-7f97-4641-a7ce-cd5753fc0ef8', N'Santarém', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Setúbal')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('2e1be5cf-a90e-4c1c-86d6-6a75ebb41c4e', N'Setúbal', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Viana do Castelo')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('972ef4e8-2ae2-4735-b290-066894693bb0', N'Viana do Castelo', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Vila Real')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('9bef0fab-bb2b-43b2-92d9-54d7faa7703b', N'Vila Real', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Viseu')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('d93dc19d-21f4-4ac4-8808-7cc117889511', N'Viseu', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

IF NOT EXISTS (SELECT 1 FROM Utility.Distrito WHERE PaisId = @PortugalId AND Nome = N'Évora')
INSERT INTO Utility.Distrito (Id, Nome, PaisId, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn)
VALUES ('2f2b0538-0a68-4ac0-96db-7bbf3c447704', N'Évora', @PortugalId, @CreatedBy, GETUTCDATE(), NULL, NULL);

-- Concelhos
-- Freguesias