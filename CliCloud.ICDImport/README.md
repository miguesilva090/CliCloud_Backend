# CliCloud.ICDImport

Importa a hierarquia ICD-11 MMS (Estatísticas de Mortalidade e Morbidade) da API WHO para a base de dados, em **português**.

## Pré-requisitos

1. **Credenciais OAuth2 da OMS** – Regista-te em https://icd.who.int/icdapi e obtém Client Id e Client Secret
2. **Migração aplicada** – A tabela `Doencas.Doenca` deve existir (`dotnet ef database update` no projeto WebApi)
3. **Connection string** – Configura em `appsettings.json` ou variáveis de ambiente

## Configuração

Edita `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  },
  "WhoIcd": {
    "ClientId": "TEU_CLIENT_ID",
    "ClientSecret": "TEU_CLIENT_SECRET",
    "DelayMs": 500,
    "BatchSize": 500
  }
}
```

## Execução

**Importação completa** (todos os capítulos e blocos):
```bash
dotnet run --project CliCloud.ICDImport
```

**Por capítulo** (ex.: apenas capítulo 0):
```bash
dotnet run --project CliCloud.ICDImport -- --chapter 0
```

**Por bloco** (ex.: capítulo 0, bloco 2):
```bash
dotnet run --project CliCloud.ICDImport -- --chapter 0 --block 2
```

**Apagar tudo e recomeçar:**
```bash
dotnet run --project CliCloud.ICDImport -- --reset --chapter 0
```

**Apagar apenas um capítulo e reimportá-lo** (ex.: capítulo 1 falhou, queres repetir):
```bash
dotnet run --project CliCloud.ICDImport -- --reset-chapter 1 --chapter 1
```

Cada bloco ≈ 500 entidades. Uma fase de ~10–15 min.

## Estrutura

- `Doenca` em `Doencas.Doenca` – hierarquia auto-referenciada (ParentId), herda de AuditableEntity
- `ClassKind`: chapter, block, category
- `Level`: profundidade na árvore
- `Code`: código ICD-11 (ex: 1A00) para categorias
