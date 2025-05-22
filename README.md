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
