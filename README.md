# BookExchange: Plataforma de Intercambio de Libros entre Estudiantes

📕BookExchange es una plataforma integral diseñada para facilitar el intercambio y la venta de libros de texto entre estudiantes universitarios. Este proyecto está construido con un backend robusto en **ASP.NET Core Web API** y un frontend interactivo en **Blazor WebAssembly**, utilizando **Entity Framework Core** para la gestión de la base de datos SQL Server.

## 📚 Características Principales
* **Gestión de Usuarios:** Registro y login de estudiantes.
* **Gestión de Libros:** Los estudiantes pueden añadir, actualizar y gestionar sus libros.
* **Gestión de Materias:** Categorización de libros por materias académicas.
* **Ofertas de Intercambio/Venta:** Creación y gestión de ofertas para intercambiar o vender libros.
* **Sistema de Mensajería:** Comunicación directa entre estudiantes interesados en ofertas.
* **Sistema de Reseñas:** Los estudiantes pueden calificar y reseñar a otros usuarios.
* **Búsqueda Avanzada:** Funcionalidades de búsqueda para encontrar libros y ofertas específicas.

## 🚀 Tecnologías Utilizadas

### Backend
* **ASP.NET Core 8.0 Web API:** Framework para construir APIs RESTful.
* **C#:** Lenguaje de programación principal.
* **Entity Framework Core:** ORM para interactuar con la base de datos.
* **SQL Server:** Base de datos relacional.
* **Fluent Validation:** Para validación de modelos de datos.
* **AutoMapper:** Para mapeo entre entidades y DTOs.
* **BCrypt.Net:** Para el hashing seguro de contraseñas.
* **Principios SOLID y Arquitectura Limpia (Clean Architecture):** Separación de preocupaciones para un código mantenible y escalable.

### Frontend
* **Blazor WebAssembly 9.0:** Framework de UI para construir SPAs usando C#.
* **C#:** Lenguaje de programación.
* **HttpClient:** Para consumir la API RESTful del backend.
* **Componentes Blazor:** UI modular y reutilizable.

### Herramientas
* **Visual Studio Code / Visual Studio:** Entorno de desarrollo.
* **Postman:** Para probar los endpoints de la API.
* **SQL Server Management Studio (SSMS) / Azure Data Studio:** Para la gestión de la base de datos.

## 📂 Estructura del Proyecto

El repositorio está organizado de la siguiente manera:
```
BookExchange/
├── BookExchange.Api/                    # Proyecto ASP.NET Core Web API (Backend)
│   ├── Controllers/                     # Endpoints de la API
│   ├── Program.cs                       # Configuración de servicios y middleware
│   └── appsettings.json                 # Configuración de la aplicación
├── BookExchange.Application/            # Capa de Aplicación (Lógica de negocio, DTOs, Interfaces de Servicios)
│   ├── DTOs/                            # Objetos de Transferencia de Datos
│   ├── Exceptions/                      # Excepciones personalizadas
│   ├── Interfaces/                      # Interfaces de repositorios y servicios
│   ├── Mappers/                         # Perfiles de AutoMapper
│   └── Services/                        # Implementaciones de servicios
├── BookExchange.Client/                 # Proyecto Blazor WebAssembly (Frontend)
│   ├── Pages/                           # Componentes de página (.razor)
│   ├── Shared/                          # Componentes compartidos
│   ├── Program.cs                       # Configuración del HttpClient y otros servicios del cliente
│   └── App.razor                        # Componente raíz de la aplicación Blazor
├── BookExchange.Domain/                 # Capa de Dominio (Entidades de negocio, Enums)
│   ├── Entities/                        # Clases de entidades (Student, Book, ExchangeOffer, etc.)
│   └── Enums/                           # Enumeraciones
└── BookExchange.Infrastructure/         # Capa de Infraestructura (Implementaciones de repositorios, DbContext, Migraciones)
├── Data/                            # DbContext y configuraciones de entidades
├── Migrations/                      # Migraciones de Entity Framework Core
└── Repositories/                    # Implementaciones de repositorios
```

## ⚙️ Configuración y Ejecución

Sigue estos pasos para poner en marcha el proyecto en tu máquina local.

### Prerequisitos

* .NET SDK 8.0 o superior ([Descargar aquí](https://dotnet.microsoft.com/download/dotnet/8.0))
* SQL Server (Express, LocalDB o cualquier instancia de SQL Server)
* Visual Studio Code o Visual Studio

### Pasos de Configuración

1.  **Clonar el Repositorio:**
    ```bash
    git clone [https://github.com/](https://github.com/)[sebas-gith]/BookExchange.git
    cd BookExchange
    ```

2.  **Configurar la Base de Datos (Backend):**
    * Navega a la carpeta `BookExchange.Api`.
    * Abre el archivo `appsettings.json`.
    * Actualiza la cadena de conexión `DefaultConnection` para que apunte a tu instancia de SQL Server. Ejemplo para LocalDB:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookExchangeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
        },
        ```
    * Aplica las migraciones de Entity Framework Core para crear la base de datos y sus tablas. Asegúrate de estar en la raíz del proyecto (`BookExchange/`):
        ```bash
        dotnet ef database update --project BookExchange.Infrastructure --startup-project BookExchange.Api
        ```

3.  **Ejecutar el Backend (API):**
    * Navega a la carpeta `BookExchange.Api`.
    * Ejecuta la API:
        ```bash
        dotnet run
        ```
    * La API se ejecutará en `https://localhost:XXXX` (el puerto se mostrará en la consola, anótalo). Puedes probarla accediendo a `https://localhost:XXXX/swagger` para ver la documentación de Swagger UI.

4.  **Configurar el Frontend (Blazor WebAssembly):**
    * Navega a la carpeta `BookExchange.Client`.
    * Abre `Program.cs`.
    * Actualiza la `BaseAddress` del `HttpClient` para que apunte a la URL de tu backend.
        ```csharp
        builder.Services.AddScoped(sp => new HttpClient {
            BaseAddress = new Uri("https://localhost:XXXX/") // Reemplaza XXXX con el puerto de tu API backend
        });
        ```
    * **Asegúrate de que el Backend permita CORS desde el puerto de Blazor.**
        En `BookExchange.Api/Program.cs`, verifica que la configuración `policy.WithOrigins` incluya la URL de tu aplicación Blazor (ej., `https://localhost:5001`). Si tu puerto de Blazor es diferente, cámbialo allí.

        ```csharp
        // En BookExchange.Api/Program.cs
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("https://localhost:5001") // <-- Reemplaza 5001 con el puerto de tu Blazor Client
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });
        // ...
        app.UseCors();
        ```
    * **Agregar referencia de proyecto a DTOs:** Desde la carpeta `BookExchange.Client`, ejecuta:
        ```bash
        dotnet add reference ../BookExchange.Application
        ```

5.  **Ejecutar el Frontend (Blazor):**
    * Navega a la carpeta `BookExchange.Client`.
    * Ejecuta la aplicación Blazor:
        ```bash
        dotnet watch run
        ```
    * La aplicación Blazor se ejecutará en `https://localhost:YYYY` (el puerto se mostrará en la consola, anótalo).

6.  **Acceder a la Aplicación:**
    * Abre tu navegador y navega a la URL de tu aplicación Blazor (ej., `https://localhost:YYYY`).
    * Puedes registrar un usuario, ingresar libros y probar las funcionalidades. La página "Fetch data" debería mostrar los libros obtenidos de tu API.

## 🧪 Pruebas de API con Postman

Aquí hay algunos ejemplos de solicitudes que puedes usar con Postman para interactuar directamente con tu API. Reemplaza `https://localhost:XXXX` con la URL de tu backend.

### Registrar Estudiante
* **Método:** `POST`
* **URL:** `https://localhost:XXXX/api/Auth/register`
* **Body (raw, JSON):**
    ```json
    {
        "firstName": "Test",
        "lastName": "User",
        "email": "test@example.com",
        "password": "Password123!",
        "confirmPassword": "Password123!",
        "phone": "809-555-0000",
        "address": "Calle Ficticia #1",
        "campus": "Campus Central"
    }
    ```

### Crear Libro
* **Método:** `POST`
* **URL:** `https://localhost:XXXX/api/Books`
* **Body (raw, JSON):**
    ```json
    {
        "title": "El Señor de los Anillos",
        "author": "J.R.R. Tolkien",
        "isbn": "9780618051767",
        "description": "Una épica aventura de fantasía.",
        "condition": 0,
        "subjectId": 1,   "ownerId": 1,     "edition": 1,
        "publicationYear": 1954
    }
    ```

### Obtener Todos los Libros
* **Método:** `GET`
* **URL:** `https://localhost:XXXX/api/Books`

---

## 🤝 Contribuciones

Si deseas contribuir a este proyecto, por favor sigue estos pasos:

1.  Haz un fork del repositorio.
2.  Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3.  Realiza tus cambios y haz commit (`git commit -am 'feat: Añadir nueva funcionalidad'`).
4.  Sube tus cambios a tu fork (`git push origin feature/nueva-funcionalidad`).
5.  Abre un Pull Request.

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo [LICENSE](LICENSE) para más detalles.

---

## 📞 Contacto

Enlace al Perfil de GitHub: [https://github.com/[sebas-gith]]
