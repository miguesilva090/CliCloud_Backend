# Guia de Comandos para Configuração do Projeto

Este guia contém os comandos para criar e configurar o projeto utilizando o .NET.

## 1. Criar um Nano Service

Este comando cria um serviço no projeto com a solução e o nome de projeto especificados.
A pasta padrão é a seguinte Luma.API.Application/Services/[MODULE].

```bash
dotnet new nano-service -s Distrito -p Distritos -ap Luma.API
```

## 2. Criar um Nano Controller

Este comando cria um controlador no projeto com a solução e o nome de projeto especificados.
A pasta padrão é a seguinte Luma.API.WebApi/Controllers/[MODULE].

```bash
dotnet new nano-controller -s Distrito -p Distritos -ap Luma.API
```

## 3. Adicionar uma Migration ao Entity Framework

Este comando adiciona uma nova migration ao contexto de dados, com o nome especificado, para a base de dados.

```bash
dotnet ef migrations add DistritoEntityCreate -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/ -o Persistence/Migrations
```

Este comando cria uma migration chamada DistritoEntityCreate, no contexto de dados ApplicationDbContext. Especifica as pastas para o projeto de API (Luma.API.WebApi/), o projeto de infraestrutura (Luma.API.Infrastructure/) e o diretório onde as migrations serão armazenadas (Persistence/Migrations).

## 4. Remover uma Migration

Este comando remove a última migration criada no projeto.

```bash
dotnet ef migrations remove -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
```

Este comando remove a última migration criada para o contexto de dados ApplicationDbContext. Especifica as pastas para o projeto de API (Luma.API.WebApi/) e o projeto de infraestrutura (Luma.API.Infrastructure/).

## 5. Fazer publish

Este comando é usado para fazer publish do projeto.

```bash
dotnet publish -c Release -o <publish-folder-path> --framework net9.0
dotnet publish -c Release -o C:\\Projects\\licenses-project\\publish --framework net9.0
dotnet publish Luma.API.WebApi/Luma.API.WebApi.csproj -c Release
dotnet publish -c Release -o publish
```

## 6. Criar Release

Este comando faz o publish do projeto e cria um ficheiro zip na pasta `releases/` com a versão especificada.

**Windows:**
```bash
powershell -File release.ps1 1.0.12
```

**Mac/Linux:**
```bash
./release.sh 1.0.12
```

O ficheiro gerado será: `releases/luma-api-v1.0.12.zip`
