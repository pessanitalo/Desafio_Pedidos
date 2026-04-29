# DesafioPedido Application
 
Uma aplicação web completa para gerenciamento de pedidos, clientes e produtos desenvolvida em **ASP.NET Core 8** com arquitetura em camadas.
 
## 📋 Visão Geral do Projeto
 
Este projeto implementa um sistema de gerenciamento de pedidos com as seguintes funcionalidades:
 
- ✅ **Gestão de Clientes**: Criar, listar, editar e visualizar clientes
- ✅ **Gestão de Produtos**: Controle de estoque, preço e descrição de produtos
- ✅ **Gestão de Pedidos**: Criar, editar, listar e visualizar detalhes de pedidos
- ✅ **Itens de Pedido**: Adicionar itens aos pedidos com rastreamento de quantidade e preço
## 🏗️ Arquitetura
 
O projeto segue a arquitetura em **4 camadas**:
 
```
DesafioPedido.Web (Apresentação)
    ↓
DesafioPedido.Application (Aplicação)
    ↓
DesafioPedido.Domain (Domínio)
    ↓
DesafioPedido.Infrastructure (Infraestrutura/Dados)
```
 
### Estrutura de Camadas
 
| Camada | Responsabilidade |
|--------|-----------------|
| **Web (MVC)** | Controllers, Views, Models - Apresentação para o usuário |
| **Application** | Services, DTOs, Interfaces - Lógica de negócio |
| **Domain** | Entities, Interfaces, Validações - Regras de domínio |
| **Infrastructure** | Repositories, DataContext - Acesso aos dados |
 
## 🔧 Requisitos de Sistema
 
### Software Necessário
 
- **Sistema Operacional**: Windows, Linux ou macOS
- **SDK .NET**: [.NET 8.0 SDK](https://dotnet.microsoft.com/download) ou superior
- **SQL Server**: SQL Server 2019+ ou SQL Server Express
  - Alternativa: SQL Server em Docker
- **IDE/Editor**: 
  - Visual Studio 2022 Community (recomendado)
  - Visual Studio Code + C# Extension
  - JetBrains Rider
### Dependências do Projeto
 
- **Microsoft.NET.Sdk.Web**: Framework web ASP.NET Core
- **Microsoft.Data.SqlClient 7.0.0**: Driver para conexão com SQL Server
## 📦 Instalação e Setup
 
### 1️⃣ Pré-requisitos
 
#### Opção A: SQL Server Local
```bash
# Certifique-se de que o SQL Server está instalado e rodando
# Verifique a porta (padrão: 1433)
# Usuário padrão: sa
# Você pode usar SQL Server Management Studio (SSMS) para verificar
```
 
#### Opção B: SQL Server em Docker
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Numsey#2021" \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```
 
### 2️⃣ Clonar/Extrair o Repositório
 
```bash
# Se em um repositório git
git clone https://seu-repositorio.git
cd DesafioPedido
 
# Ou se extraído como ZIP
unzip DesafioPedido_Application.zip
cd DesafioPedido
```
 
### 3️⃣ Configurar o Banco de Dados
 
#### Opção A: SQL Server Management Studio (SSMS)
1. Abra o SQL Server Management Studio
2. Conecte-se ao servidor SQL
3. Clique com botão direito em "Databases" → "New Database"
4. Configure as informações de conexão em `appsettings.json`
5. Abra o arquivo `script_database.txt`
6. Execute todo o script no Query Editor
#### Opção B: Linha de Comando
```bash
# No diretório do projeto, execute:
sqlcmd -S localhost,1433 -U sa -P "Numsey#2021" -i script_database.txt
```
 
#### Verificar a String de Conexão
Edite o arquivo `DesafioPedido.Web/appsettings.json`:
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=desafio_pedidos;User Id=sa;Password=Numsey#2021;TrustServerCertificate=True"
  }
}
```
 
**Ajuste conforme necessário:**
- `Server`: Endereço do SQL Server (ex: `.\SQLEXPRESS` para Express)
- `User Id`: Usuário SQL Server
- `Password`: Senha do usuário
### 4️⃣ Restaurar Dependências e Compilar
 
```bash
# Restaurar pacotes NuGet
dotnet restore
 
# Compilar a solução
dotnet build
 
# Verificar se compilou sem erros
dotnet build --configuration Release
```
 
## 🚀 Como Executar o Projeto
 
### Opção 1: Usando o dotnet CLI
 
```bash
# Navegar para o projeto Web
cd DesafioPedido.Web
 
# Executar a aplicação
dotnet run
 
# A aplicação estará disponível em:
# http://localhost:5000
# https://localhost:5001
```
 
### Opção 2: Usando Visual Studio
 
1. Abra a solução em Visual Studio
2. Defina `DesafioPedido.Web` como projeto de inicialização
3. Pressione `F5` ou clique em "Start Debugging"
4. A aplicação abrirá automaticamente no navegador padrão
### Opção 3: Publicar para Produção
 
```bash
# Criar um build de release
dotnet publish -c Release -o ./publish
 
# Executar a versão publicada
dotnet DesafioPedido.Web.dll
```
 
## 📍 Rotas Disponíveis
 
| Rota | Descrição |
|------|-----------|
| `/` | Home - Página inicial |
| `/Cliente` | Listar clientes |
| `/Cliente/Create` | Criar novo cliente |
| `/Cliente/Edit/{id}` | Editar cliente |
| `/Cliente/Details/{id}` | Detalhes do cliente |
| `/Produto` | Listar produtos |
| `/Produto/Create` | Criar novo produto |
| `/Produto/Edit/{id}` | Editar produto |
| `/Produto/Details/{id}` | Detalhes do produto |
| `/Pedido` | Listar pedidos |
| `/Pedido/Create` | Criar novo pedido |
| `/Pedido/Edit/{id}` | Editar pedido |
| `/Pedido/Details/{id}` | Detalhes do pedido |
 
## 🎨 Interface
 
A aplicação utiliza:
- **Frontend**: HTML5, CSS3, Bootstrap
- **Backend**: ASP.NET Core MVC
- **Views**: Razor Engine (`.cshtml`)
## 📊 Modelo de Dados
 
### Tabelas Principais
 
#### **Clientes**
```sql
- ClienteId (PK)
- Nome
- Email
- Telefone
- DataCadastro
```
 
#### **Produtos**
```sql
- ProdutoId (PK)
- Nome
- Descricao
- Preco
- QuantidadeEstoque
```
 
#### **Pedidos**
```sql
- PedidoId (PK)
- ClienteId (FK)
- DataPedido
- ValorTotal
- Status
```
 
#### **ItensPedido**
```sql
- ItemId (PK)
- PedidoId (FK)
- ProdutoId (FK)
- Quantidade
- PrecoUnitario
```
 
## 🔍 Dados de Teste
 
O script de banco de dados inclui dados iniciais:
 
**Clientes:**
- João Silva
- Maria Oliveira
- Carlos Souza
- Ana Pereira
- Lucas Fernandes
**Produtos:**
- Notebook Dell (R$ 3.500,00)
- Mouse Logitech (R$ 120,00)
- Teclado Mecânico (R$ 250,00)
- Monitor 24" (R$ 900,00)
- Headset Gamer (R$ 300,00)
**Pedidos:**
- 4 pedidos iniciais com itens associados
## 🐛 Troubleshooting
 
### Erro: "Cannot connect to database"
```bash
# Verificar se SQL Server está rodando
# Windows:
Get-Service -Name "MSSQLSERVER" | Start-Service
 
# Linux:
sudo systemctl start mssql-server
 
# Verificar string de conexão em appsettings.json
```
 
### Erro: "The specified table/database does not exist"
```bash
# Certificar-se de que o script_database.txt foi executado completamente
# Reexecutar o script no SQL Server Management Studio ou via sqlcmd
```
 
### Porta 5000/5001 já está em uso
```bash
# Mudar a porta em Properties/launchSettings.json
# Ou usar:
dotnet run --urls "http://localhost:6000"
```
 
### Erro ao restaurar dependências
```bash
# Limpar cache e restaurar novamente
dotnet nuget locals all --clear
dotnet restore
```
 
## 📚 Estrutura de Pastas
 
```
DesafioPedido/
├── DesafioPedido.Web/              # Camada de Apresentação
│   ├── Controllers/                 # Controladores MVC
│   ├── Views/                       # Interfaces de usuário (Razor)
│   ├── Models/                      # View Models
│   ├── Properties/                  # Configurações do projeto
│   ├── Program.cs                   # Configuração da aplicação
│   ├── appsettings.json            # Configurações (produção)
│   └── appsettings.Development.json # Configurações (desenvolvimento)
│
├── DesafioPedido.Application/       # Camada de Aplicação
│   ├── Services/                    # Serviços de negócio
│   ├── Interfaces/                  # Contratos de serviços
│   └── DTOs/                        # Data Transfer Objects
│
├── DesafioPedido.Domain/            # Camada de Domínio
│   ├── Entities/                    # Modelos de domínio
│   ├── Interfaces/                  # Contratos de repositório
│   ├── DTOs/                        # DTOs do domínio
│   └── Validation/                  # Validações customizadas
│
├── DesafioPedido.Infrastructure/    # Camada de Infraestrutura
│   ├── Context/                     # DbContext (acesso a dados)
│   └── Repositories/                # Implementação dos repositórios
│
└── script_database.txt              # Script de criação de DB
```
 
## 💡 Padrões de Design Utilizados
 
- **Repository Pattern**: Abstração do acesso a dados
- **Dependency Injection**: Injeção de dependências via container
- **DTO (Data Transfer Object)**: Transferência de dados entre camadas
- **Service Layer**: Camada de serviços para lógica de negócio
- **MVC Pattern**: Separação entre Model, View e Controller
## 🔐 Segurança
 
⚠️ **Importante para Produção:**
 
1. **Credenciais**: Altere a senha padrão em `appsettings.json`
2. **Connection String**: Armazene em variáveis de ambiente
3. **SSL/TLS**: Ative HTTPS em produção
4. **CORS**: Configure conforme necessário
5. **Validação**: Implemente validações adicionais conforme necessário
Exemplo de variáveis de ambiente:
```bash
set ConnectionStrings__DefaultConnection="Server=seu-servidor;Database=seu-db;User Id=seu-usuario;Password=sua-senha"
```
 
## 📝 Desenvolvimento
 
### Adicionar um Nova Funcionalidade
 
1. **Domain**: Criar a entidade em `Entities/`
2. **Domain**: Criar interface em `Interfaces/`
3. **Infrastructure**: Implementar repositório em `Repositories/`
4. **Application**: Criar serviço em `Services/`
5. **Web**: Criar controller e views
### Compilação em Diferentes Modos
 
```bash
# Debug
dotnet build -c Debug
 
# Release
dotnet build -c Release
 
# Com análise
dotnet build --analyze
```
 
## 📞 Suporte
 
Para dúvidas ou problemas:
- Verifique as mensagens de erro no console
- Consulte os logs da aplicação
- Revise o script do banco de dados
## 📄 Licença
 
Este projeto é fornecido como desafio de desenvolvimento.
 
## ✨ Versão
 
- **Versão**: 1.0.0
- **Framework**: .NET 8.0
- **Última Atualização**: 2024
---
