✅ MotoTrackAPI
API RESTful desenvolvida em ASP.NET Core para gerenciar motos, filiais, eventos, agendamentos e usuários.
________________________________________
✅ Tecnologias Utilizadas
•	ASP.NET Core 8
•	Entity Framework Core
•	Banco de Dados: Oracle (FIAP)
•	Swagger (OpenAPI)
________________________________________
✅ Como rodar o projeto
📥 Clone o repositório:
bash
CopiarEditar
git clone https://github.com/seu-usuario/MotoTrackAPI.git
cd MotoTrackAPI
⚙️ Configure a string de conexão em appsettings.json:
json
CopiarEditar
"ConnectionStrings": {
  "OracleConnection": "User Id=rmXXXXX;Password=SUASENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
}
________________________________________
⚙️ Crie as migrations e atualize o banco:
bash
CopiarEditar
dotnet ef migrations add InitialCreate
dotnet ef database update
________________________________________
▶️ Execute a aplicação:
bash
CopiarEditar
dotnet run
➡️ Acesse:
https://localhost:{porta}/swagger
________________________________________
✅ Endpoints Disponíveis e como testar no Swagger
Para todos os testes:
➡️ Acesse Swagger UI: https://localhost:{porta}/swagger
➡️ Clique em "Try it out"
➡️ Preencha os campos e clique em "Execute".
________________________________________
✅ 1. Motos
➡️ GET /api/motos
•	Lista todas as motos.
➡️ GET /api/motos/{id}
•	Busca uma moto por ID.
➡️ POST /api/motos
•	Exemplo de corpo JSON:
json
CopiarEditar
{
  "placa": "ABC1234",
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2023,
  "status": "Disponível"
}
➡️ PUT /api/motos/{id}
•	Exemplo de atualização:
json
CopiarEditar
{
  "id": 1,
  "placa": "XYZ5678",
  "modelo": "Fan 160",
  "marca": "Honda",
  "ano": 2022,
  "status": "Manutenção"
}
➡️ DELETE /api/motos/{id}
•	Remove a moto pelo ID.
________________________________________
✅ 2. Filiais
➡️ GET /api/filiais
•	Lista todas as filiais.
➡️ POST /api/filiais
json
CopiarEditar
{
  "nome": "Filial Centro",
  "endereco": "Rua A, 123",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01000-000",
  "latitude": -23.5505,
  "longitude": -46.6333
}
________________________________________
✅ 3. Eventos
➡️ GET /api/eventos
•	Lista todos os eventos.
➡️ POST /api/eventos
json
CopiarEditar
{
  "tipo": "Manutenção",
  "dataHora": "2025-05-22T10:00:00",
  "motivo": "Troca de óleo",
  "localizacao": "Filial Centro"
}
________________________________________
✅ 4. Agendamentos
➡️ GET /api/agendamentos
•	Lista todos os agendamentos.
➡️ POST /api/agendamentos
json
CopiarEditar
{
  "dataHora": "2025-05-23T15:00:00",
  "status": "Confirmado"
}
________________________________________
✅ 5. Usuários
➡️ GET /api/usuarios
•	Lista todos os usuários.
➡️ POST /api/usuarios
json
CopiarEditar
{
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "perfil": "Administrador"
}
________________________________________
✅ Resumo de Testes no Swagger:
Entidade	Métodos
Motos	GET, GET/{id}, POST, PUT, DELETE
Filiais	GET, POST, PUT, DELETE
Eventos	GET, POST, PUT, DELETE
Agendamentos	GET, POST, PUT, DELETE
Usuários	GET, POST, PUT, DELETE
________________________________________
✅ Observações
•	A aplicação cria automaticamente as tabelas no banco Oracle.
•	É necessário garantir que o usuário Oracle tenha permissão de CREATE TABLE.
•	A porta do Swagger pode variar → veja no console ao rodar:
Now listening on: https://localhost:{porta}
________________________________________
✅ Desenvolvido por
Seu Nome
FIAP — Advanced Business Development with .NET

