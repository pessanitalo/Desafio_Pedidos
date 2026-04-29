#  Guia Rápido - Como Subir o Projeto
 
##  Pré-requisitos (Cumprir ANTES de tudo)
 
Você precisa ter instalado no seu computador:
 
1. **.NET 8 SDK** → [Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0)
   ```bash
   # Verificar se já tem instalado
   dotnet --version
   ```
 
2. **SQL Server** (escolha uma opção):
   - **SQL Server 2022 Express** → [Download](https://www.microsoft.com/pt-br/sql-server/sql-server-editions-express)
   - **SQL Server Developer Edition** → [Download](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
   - **Docker com SQL Server** (mais fácil):
     ```bash
     docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Numsey#2021" \
       -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
     ```
 
3. **Visual Studio ou VS Code** (opcional):
   - Visual Studio Community 2022 (recomendado)
   - VS Code + C# Extension
---
 
##  Passos para Subir o Projeto (Rápido)
 
### Extrair o Projeto
```bash
# Descompacte o arquivo DesafioPedido_Application.zip
unzip DesafioPedido_Application.zip
 
# Entre na pasta
cd DesafioPedido
```
 
###  Criar o Banco de Dados
 
#### Opção A: Usando SQL Server Management Studio (SSMS)
1. Abra **SQL Server Management Studio**
2. Conecte-se ao servidor
3. Clique com botão direito em **Databases** → **New Database**
4. Abra o arquivo `script_database.txt`
5. Cole todo o conteúdo na janela de query
6. Pressione **F5** ou clique em **Execute**
####  Opção B: Usando Linha de Comando
```bash
# No diretório do projeto, execute:
sqlcmd -S localhost,1433 -U sa -P "Numsey#2021" -i script_database.txt
```
 
#### Opção C: Automaticamente com PowerShell (Windows)
```powershell
$scriptPath = ".\script_database.txt"
$serverInstance = "localhost,1433"
$username = "sa"
$password = "Numsey#2021"
 
Invoke-Sqlcmd -InputFile $scriptPath -ServerInstance $serverInstance `
  -Username $username -Password $password -Encrypt NotSupported
```
 
** Se vir mensagens de sucesso no final, o banco foi criado corretamente!**
 
###  Restaurar Dependências
```bash
# Na raiz do projeto
dotnet restore
```
 
### Compilar
```bash
dotnet build
```
 
### 5️ Executar a Aplicação
```bash
cd DesafioPedido.Web
dotnet run
```
 
** Pronto! A aplicação estará rodando em:**
- `http://localhost:5000`
- `https://localhost:5001`
---
 
## 🌐 Acessar a Aplicação
 
Abra seu navegador e acesse:
```
http://localhost:5000
```
 
Você verá a página inicial com os links para:
-  **Clientes** - Gerenciar clientes
-  **Produtos** - Gerenciar produtos  
-  **Pedidos** - Gerenciar pedidos
---
 
## 🔧 Solução de Problemas
 
### Erro: "Cannot connect to database"
 
**Causa:** SQL Server não está rodando ou string de conexão errada
 
**Solução:**
```bash
# Verifique se SQL Server está rodando
# Windows:
Get-Service | grep MSSQL
net start "SQL Server (SQLEXPRESS)"
 
# Linux:
sudo systemctl start mssql-server
 
# Verifique appsettings.json - certifique-se que Server, User Id e Password estão corretos
```
 
### Erro: "The specified table does not exist"
 
**Causa:** O script do banco de dados não foi executado corretamente
 
**Solução:**
1. Abra o SQL Server Management Studio
2. Verifique se existe banco de dados chamado `desafio_pedidos`
3. Se não existir, crie manualmente e execute o script
4. Se existir mas está vazio, delete e execute o script novamente
```sql
-- Para limpar e recomeçar:
DROP DATABASE desafio_pedidos;
-- Depois execute novamente o script_database.txt
```
 
### Erro: "Port 5000 is already in use"
 
**Causa:** Outra aplicação está usando a porta 5000
 
**Solução:**
```bash
# Mudar a porta
dotnet run --urls "http://localhost:6000;https://localhost:6001"
```
 
###  "The framework version ... is not available"
 
**Causa:** .NET 8 não está instalado
 
**Solução:**
```bash
# Instale .NET 8
# Windows/Mac: Faça o download do SDK em https://dotnet.microsoft.com/download
# Linux: 
sudo apt-get update
sudo apt-get install dotnet-sdk-8.0
```
 
---
 
## Estrutura de Pastas
 
```
DesafioPedido/
├── DesafioPedido.Web/              👈 Aqui roda a aplicação
├── DesafioPedido.Application/       Lógica de negócio
├── DesafioPedido.Domain/            Modelos e regras
├── DesafioPedido.Infrastructure/    Banco de dados
└── script_database.txt              Script SQL
```
 
---
 
## Funcionalidades Disponíveis
 
Após subir o projeto, você poderá:
 
###  Clientes
- ✅ Listar todos os clientes
- ✅ Criar novo cliente
- ✅ Editar dados do cliente
- ✅ Ver detalhes do cliente
### Produtos
- ✅ Listar todos os produtos
- ✅ Criar novo produto
- ✅ Editar produto
- ✅ Ver detalhes e estoque
###  Pedidos
- ✅ Listar todos os pedidos
- ✅ Criar novo pedido
- ✅ Adicionar itens ao pedido
- ✅ Editar pedido
- ✅ Ver detalhes completos do pedido
---
 
##  Dados de Teste Inclusos
 
O script do banco inclui dados prontos para testar:
 
**5 Clientes:**
- João Silva, Maria Oliveira, Carlos Souza, Ana Pereira, Lucas Fernandes
**5 Produtos:**
- Notebook Dell, Mouse Logitech, Teclado Mecânico, Monitor 24", Headset Gamer
**4 Pedidos prontos** com itens associados
 
---

- 📂 Use **Visual Studio Community** para melhor experiência (gratuita)
- 🔄 Cada vez que modifica código, precisa compilar com `dotnet build`
- 🧹 Para limpar artefatos de build: `dotnet clean`
- 📝 Logs aparecem no console - útil para debugar problemas
- 🔐 **Não esqueça**: Altere as senhas em produção!
---
 
1. Verifique os erros no console
2. Leia as mensagens de erro com atenção
3. Confirme que os pré-requisitos estão instalados
4. Tente limpar e restaurar: `dotnet clean && dotnet restore`
5. Reinicie a aplicação
---
 
Se tudo correr bem, você verá:
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```
 
Abra seu navegador em `http://localhost:5000` 