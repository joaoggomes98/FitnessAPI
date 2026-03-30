# 🏋️ Fitness Platform

Plataforma fullstack para gestão de personal trainers, clientes, planos de treino e planos de nutrição.

Projeto desenvolvido com **ASP.NET Core + Angular**, com autenticação, roles e dashboard completo.

---

## 🚀 Funcionalidades

### 🔐 Autenticação

* Login com JWT
* Gestão de utilizadores com Identity
* Roles:

  * SuperAdmin
  * PT (Personal Trainer)
  * Client

---

### 👨‍💼 SuperAdmin

* Criar / editar / remover PTs
* Gestão de utilizadores
* Atribuição de roles

---

### 🧑‍🏫 PT (Personal Trainer)

* Criar e gerir clientes
* Criar planos de treino
* Criar planos de nutrição
* Visualizar dados dos clientes

---

### 🧑‍💻 Client

* Visualizar plano de treino
* Visualizar plano de nutrição

---

## 🧱 Tecnologias

### Backend (.NET)

* ASP.NET Core
* Entity Framework Core
* ASP.NET Identity
* JWT Authentication

### Frontend (Angular)

* Angular (Standalone Components)
* Signals (state management)
* Tailwind CSS
* DaisyUI

---

## 🏗️ Arquitetura

```text
Frontend (Angular)
        ↓
API (ASP.NET Core)
        ↓
Entity Framework Core
        ↓
Base de Dados
```

---

## 🔑 Autenticação

1. Utilizador faz login
2. API valida credenciais
3. Token JWT é gerado
4. Token é enviado no header:

```bash
Authorization: Bearer {token}
```

---

## 📡 Endpoints principais

### 🔹 Auth

* `POST /api/account/login`

---

### 🔹 Admin

* `GET /api/admin/users`
* `POST /api/admin/users`
* `PUT /api/admin/users/{id}`
* `DELETE /api/admin/users/{id}`

---

### 🔹 PT

* `GET /api/PT/clients`

* `POST /api/PT/clients`

* `PUT /api/PT/clients/{id}`

* `DELETE /api/PT/clients/{id}`

* `GET /api/PT/training/{clientId}`

* `GET /api/PT/nutrition/{clientId}`

---

### 🔹 Client

* `GET /api/client/training/my-plans`
* `GET /api/client/nutrition/my-plans`

---

## 🗂️ Estrutura do Projeto

```text
src/app
├── core
│   ├── services
│   ├── models
│
├── features
│   ├── auth
│   ├── pt
│   ├── client
│   ├── super-admin
│
├── layout
│   ├── public-layout
│   ├── private-layout
│
└── shared
    ├── components
```

---

## ⚙️ Setup do Projeto

### Backend

```bash
cd API
dotnet restore
dotnet run
```

---

### Frontend

```bash
cd client
npm install
ng serve
```

---

## 🌐 Acesso

Frontend:

```
http://localhost:4200
```

API:

```
https://localhost:5001
```

---

## 📌 Roadmap

* [ ] Dashboard com métricas avançadas
* [ ] Estrutura de treino por dias/exercícios
* [ ] Estrutura de nutrição por refeições
* [ ] Notificações
* [ ] Melhorias de UX/UI

---

## 🧠 Autor

Desenvolvido por [Teu Nome]

---

## 📄 Licença

Este projeto é open-source e pode ser utilizado para fins educativos ou comerciais.
