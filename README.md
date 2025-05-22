# 🏍️ MotoTrackAPI - API REST

API RESTful desenvolvida com **ASP.NET Core** para gerenciamento de motos, filiais, eventos, agendamentos e usuários.

---

## 📌 Tecnologias Utilizadas

* **ASP.NET Core 8**
* **Entity Framework Core**
* **Banco de Dados: Oracle (FIAP)**
* **Swagger (OpenAPI)**
* **Insomnia/Postman** (para testes)

---

## 🚀 Como Rodar o Projeto

### ✅ Pré-requisitos

* .NET 8 SDK instalado
* Banco Oracle configurado

### ▶️ Comandos

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

API disponível em: `https://localhost:{porta}/swagger`

---

## 🔻 Endpoints por Entidade

### 🛵 /api/motos

#### ✅ Cadastrar Moto

`POST /api/motos`

```json
{
  "placa": "ABC1234",
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2023,
  "status": "Disponível"
}
```

**Response:**

```json
{
  "id": 1,
  "placa": "ABC1234",
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2023,
  "status": "Disponível"
}
```

#### 🔎 Listar Motos

`GET /api/motos`
**Response:**

```json
[
  {
    "id": 1,
    "placa": "ABC1234",
    "modelo": "CG 160",
    "marca": "Honda",
    "ano": 2023,
    "status": "Disponível"
  }
]
```

#### 🔍 Buscar Moto por ID

`GET /api/motos/{id}`
**Response:**

```json
{
  "id": 1,
  "placa": "ABC1234",
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2023,
  "status": "Disponível"
}
```

#### ✏️ Atualizar Moto

`PUT /api/motos/{id}`

```json
{
  "placa": "XYZ5678",
  "modelo": "Fan 160",
  "marca": "Honda",
  "ano": 2022,
  "status": "Manutenção"
}
```

**Response:**

```json
{
  "id": 1,
  "placa": "XYZ5678",
  "modelo": "Fan 160",
  "marca": "Honda",
  "ano": 2022,
  "status": "Manutenção"
}
```

#### 🗑️ Excluir Moto

`DELETE /api/motos/{id}`
**Response:**

```
204 No Content
```
### 🏢 /api/filiais

#### ✅ Cadastrar Filial
`POST /api/filiais`

```json
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
```

**Response:**

```json
{
  "id": 1,
  "nome": "Filial Centro",
  "endereco": "Rua A, 123",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01000-000",
  "latitude": -23.5505,
  "longitude": -46.6333
}
```

#### 🔎 Listar Filiais
`GET /api/filiais`

**Response:**

```json
[
  {
    "id": 1,
    "nome": "Filial Centro",
    "endereco": "Rua A, 123",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "estado": "SP",
    "cep": "01000-000",
    "latitude": -23.5505,
    "longitude": -46.6333
  }
]
```

#### 🔍 Buscar Filial por ID
`GET /api/filiais/{id}`

**Response:**

```json
{
  "id": 1,
  "nome": "Filial Centro",
  "endereco": "Rua A, 123",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01000-000",
  "latitude": -23.5505,
  "longitude": -46.6333
}
```

#### ✏️ Atualizar Filial
`PUT /api/filiais/{id}`

```json
{
  "nome": "Filial Norte",
  "endereco": "Rua B, 456",
  "bairro": "Norte",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "02000-000",
  "latitude": -23.5200,
  "longitude": -46.6200
}
```

**Response:**

```json
{
  "id": 1,
  "nome": "Filial Norte",
  "endereco": "Rua B, 456",
  "bairro": "Norte",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "02000-000",
  "latitude": -23.5200,
  "longitude": -46.6200
}
```

#### 🗑️ Excluir Filial
`DELETE /api/filiais/{id}`

**Response:**

```
204 No Content
```

---

### 📅 /api/eventos

#### ✅ Cadastrar Evento
`POST /api/eventos`

```json
{
  "tipo": "Manutenção",
  "dataHora": "2025-05-22T10:00:00",
  "motivo": "Troca de óleo",
  "localizacao": "Filial Centro"
}
```

**Response:**

```json
{
  "id": 1,
  "tipo": "Manutenção",
  "dataHora": "2025-05-22T10:00:00",
  "motivo": "Troca de óleo",
  "localizacao": "Filial Centro"
}
```

#### 🔎 Listar Eventos
`GET /api/eventos`

**Response:**

```json
[
  {
    "id": 1,
    "tipo": "Manutenção",
    "dataHora": "2025-05-22T10:00:00",
    "motivo": "Troca de óleo",
    "localizacao": "Filial Centro"
  }
]
```

#### 🔍 Buscar Evento por ID
`GET /api/eventos/{id}`

**Response:**

```json
{
  "id": 1,
  "tipo": "Manutenção",
  "dataHora": "2025-05-22T10:00:00",
  "motivo": "Troca de óleo",
  "localizacao": "Filial Centro"
}
```

#### ✏️ Atualizar Evento
`PUT /api/eventos/{id}`

```json
{
  "tipo": "Revisão",
  "dataHora": "2025-06-15T09:00:00",
  "motivo": "Revisão geral",
  "localizacao": "Filial Norte"
}
```

**Response:**

```json
{
  "id": 1,
  "tipo": "Revisão",
  "dataHora": "2025-06-15T09:00:00",
  "motivo": "Revisão geral",
  "localizacao": "Filial Norte"
}
```

#### 🗑️ Excluir Evento
`DELETE /api/eventos/{id}`

**Response:**

```
204 No Content
```

---

### 📆 /api/agendamentos

#### ✅ Cadastrar Agendamento
`POST /api/agendamentos`

```json
{
  "dataHora": "2025-05-23T15:00:00",
  "status": "Confirmado"
}
```

**Response:**

```json
{
  "id": 1,
  "dataHora": "2025-05-23T15:00:00",
  "status": "Confirmado"
}
```

#### 🔎 Listar Agendamentos
`GET /api/agendamentos`

**Response:**

```json
[
  {
    "id": 1,
    "dataHora": "2025-05-23T15:00:00",
    "status": "Confirmado"
  }
]
```

#### 🔍 Buscar Agendamento por ID
`GET /api/agendamentos/{id}`

**Response:**

```json
{
  "id": 1,
  "dataHora": "2025-05-23T15:00:00",
  "status": "Confirmado"
}
```

#### ✏️ Atualizar Agendamento
`PUT /api/agendamentos/{id}`

```json
{
  "dataHora": "2025-06-01T10:00:00",
  "status": "Reagendado"
}
```

**Response:**

```json
{
  "id": 1,
  "dataHora": "2025-06-01T10:00:00",
  "status": "Reagendado"
}
```

#### 🗑️ Excluir Agendamento
`DELETE /api/agendamentos/{id}`

**Response:**

```
204 No Content
```

---

### 👤 /api/usuarios

#### ✅ Cadastrar Usuário
`POST /api/usuarios`

```json
{
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "perfil": "Administrador"
}
```

**Response:**

```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "perfil": "Administrador"
}
```

#### 🔎 Listar Usuários
`GET /api/usuarios`

**Response:**

```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao.silva@example.com",
    "perfil": "Administrador"
  }
]
```

#### 🔍 Buscar Usuário por ID
`GET /api/usuarios/{id}`

**Response:**

```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "perfil": "Administrador"
}
```

#### ✏️ Atualizar Usuário
`PUT /api/usuarios/{id}`

```json
{
  "nome": "João da Silva",
  "email": "joao.silva@example.com",
  "perfil": "Gestor"
}
```

**Response:**

```json
{
  "id": 1,
  "nome": "João da Silva",
  "email": "joao.silva@example.com",
  "perfil": "Gestor"
}
```

#### 🗑️ Excluir Usuário
`DELETE /api/usuarios/{id}`

**Response:**

```
204 No Content
```

---

## 📌 Como Contribuir

```bash
git clone https://github.com/seu-usuario/MotoTrackAPI.git
cd MotoTrackAPI
```

1. Crie uma branch: `git checkout -b minha-feature`
2. Faça suas alterações
3. Commit: `git commit -m "Minha contribuição"`
4. Push: `git push origin minha-feature`
5. Crie um Pull Request no GitHub

---

## 🪪 Licença

Projeto sob licença MIT. Criado por **Seu Nome** 🏍️
