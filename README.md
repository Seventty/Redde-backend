# Redde prueba técnica

# ERD:

![redde prueba diagrama](https://github.com/user-attachments/assets/80f9c22b-6915-4699-952c-7228411cdec7)

# 🏛️ Redde prueba técnica - Gestión de Empresas y Usuarios

Redde es una plataforma de gestión de compañías orientada a entidades gubernamentales y privadas. Permite a los usuarios autenticarse, crear compañías, gestionarlas y supervisar usuarios, con control de roles (Admin / Owner).

---

## Tecnologías Utilizadas / Ficha técnica

### Frontend
- Angular 19 (standalone components)
- PrimeNG + Tailwind CSS
- JWT + Guards + Interceptors
- Atlantis Template (Licenciada)

### Backend
- .NET 8 + ASP.NET Core
- Entity Framework Core + PostgreSQL
- Autenticación JWT + OAuth2 (Google, GitHub)
- Arquitectura en capas: Application / Domain / Infrastructure

---

## Features del proyecto:

- **Login y Registro clásico + OAuth (Google y GitHub)**  
  Autenticación segura usando JWT. Se guarda el token en `localStorage` y se usa un interceptor para enviarlo en cada request.

- **Roles y Permisos**  
  Hay tres roles:
  - `Admin`: puede ver todos los usuarios y empresas.
  - `Owner`: puede crear y administrar sus empresas.
  - `Empleado`: reservado para el futuro.

- **Gestión de Empresas**  
  CRUD completo. Cada empresa tiene dueño y metadatos como categoría, estado, actividad económica, etc. Todo viene con datos iniciales (seeders).

- **Arquitectura limpia**  
  Usé `clean architecture` con separación clara de responsabilidades:
  - Lógica de negocio en servicios.
  - Acceso a datos a través de repositorios y `UnitOfWork`.
  - Frontend modular con guards y componentes standalone (Angular + PrimeNG).

## ¿Por qué así?

Bajo la necesidad de un código que sea:
- Fácil de mantener.
- Listo para escalar.
