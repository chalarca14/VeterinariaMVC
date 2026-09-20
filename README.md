
# Sistema de Gestión Veterinaria (VeterinariaMVC)

Proyecto web desarrollado como prueba técnica backend en **.NET 8 Core** bajo una arquitectura en capas, incorporando persistencia relacional en **SQL Server**, Entity Framework Core y una interfaz dinámica tipo **SPA (Single Page Application)** construida con Bootstrap 5, DataTables y jQuery AJAX.

---

##Decisiones de Arquitectura y Diseño

- **Arquitectura en Capas**: Separación clara de responsabilidades en tres capas principales:
  - `MVC.Data`: Modelos de datos (`VeterinariaContext`, entidades) y Objetos de Transferencia de Datos (DTOs).
  - `MVC.Domain`: Servicios de lógica de negocio (`MascotaServices`, `PropietarioServices`) desacoplados de los controladores.
  - `VeterinariaMVC`: Capa de presentación Web que expone los controladores y vistas Razor.
- **Inyección Directa y Desacoplamiento**: Se inyectan las interfaces de servicios en los controladores, respetando la arquitectura MVC sin sobrecargas innecesarias.
- **Gestión de Archivos de Imagen**: Las imágenes de las mascotas no se guardan como BLOBs en la base de datos. Se almacenan físicamente en el servidor dentro de la carpeta `wwwroot/images/mascotas/` con nombres únicos (`Guid`), guardando en la base de datos únicamente la ruta relativa (`/images/mascotas/foto1.png`).
- **Normalización de Base de Datos (3FN)**: Se ajustó el modelo para cumplir con la Tercera Forma Normal (3FN), eliminando el campo directo `IdEspecie` de la tabla `Mascotas`. La mascota se relaciona únicamente con `IdRaza`, y la **Especie** se resuelve mediante navegación relacional (`Mascota -> Raza -> Especie`).

---

## Modelo de Base de Datos y Script DDL

### Tablas y Relaciones
1. **`Propietarios`**: Información básica de los clientes (`Id`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Direccion`).
2. **`Especies`**: Catálogo principal (`Id`, `Nombre`).
3. **`Razas`**: Catálogo dependiente (`Id`, `IdEspecie`, `Nombre`) con FK a `Especies`.
4. **`Mascotas`**: Registro de mascotas (`Id`, `Nombre`, `IdPropietario`, `IdRaza`, `FechaNacimiento`, `Peso`, `RutaFoto`) con FK a `Propietarios` y `Razas`.

```sql
-- 1. Crear la base de datos
CREATE DATABASE VeterinariaDb;
GO
USE VeterinariaDb;
GO

-- 2. Tabla Propietarios
CREATE TABLE Propietarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Email VARCHAR(100) NULL,
    Direccion VARCHAR(150) NULL
);

-- 3. Tabla Especies
CREATE TABLE Especies (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);

-- 4. Tabla Razas
CREATE TABLE Razas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEspecie INT NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Razas_Especies FOREIGN KEY (IdEspecie) REFERENCES Especies(Id) ON DELETE CASCADE
);

-- 5. Tabla Mascotas (Normalizada sin IdEspecie directo)
CREATE TABLE Mascotas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    IdPropietario INT NOT NULL,
    IdRaza INT NOT NULL,
    FechaNacimiento DATE NULL,
    Peso DECIMAL(5,2) NULL,
    RutaFoto VARCHAR(255) NULL,
    CONSTRAINT FK_Mascotas_Propietarios FOREIGN KEY (IdPropietario) REFERENCES Propietarios(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Mascotas_Razas FOREIGN KEY (IdRaza) REFERENCES Razas(Id)
);
GO

-- INSERCIÓN DE DATOS DE PRUEBA (Seed Data)
INSERT INTO Especies (Nombre) VALUES ('Canino'), ('Felino'), ('Ave'), ('Roedor');

INSERT INTO Razas (IdEspecie, Nombre) VALUES 
(1, 'Labrador Retriever'), (1, 'Pastor Alemán'), (1, 'Golden Retriever'), (1, 'Bulldog'), (1, 'Poodle (Caniche)'), (1, 'Criollo / Mestizo'),
(2, 'Siamés'), (2, 'Persa'), (2, 'Maine Coon'), (2, 'Bengalí'), (2, 'Mestizo / Criollo'),
(3, 'Perico Australiano'), (3, 'Ninfa (Calopsita)'), (3, 'Canario'),
(4, 'Hámster Sirio'), (4, 'Cuy / Cobaya');

INSERT INTO Propietarios (Nombre, Apellido, Telefono, Email, Direccion) VALUES 
('Carlos', 'Mendoza', '3001234567', 'carlos.mendoza@email.com', 'Calle 10 # 45-12'),
('Laura', 'Gómez', '3159876543', 'laura.gomez@email.com', 'Carrera 70 # 23-89');
GO

## Funcionalidades Destacadas

* **Dashboard de Bienvenida**:
  * Página de inicio (`Views/Home/Index.cshtml`) con métricas, accesos rápidos y presentación del proyecto.
  * Hoja de estilos global (`site.css`) con gradientes, sombras suaves y tarjetas interactivas.

* **Módulo Propietarios (CRUD SPA)**:
  * Gestión completa de propietarios mediante peticiones asíncronas con jQuery AJAX y DataTables.

* **Módulo Mascotas (SPA con Carga de Imágenes)**:
  * Registro, edición y eliminación de mascotas sin recarga de página.
  * Carga y reemplazo de fotos recibidas mediante `IFormFile` y enviadas vía JavaScript `FormData`.
  * Validaciones en servidor: extensión permitida (`.jpg`, `.jpeg`, `.png`, `.webp`) y peso máximo de 5 MB.
  * Visualización de miniatura circular (`rounded-circle`), apertura en pantalla completa con visor **GLightbox** y opción para remover la imagen actual.

* **Combos en Cascada y Filtros Dinámicos (HU04)**:
  * Selector relacional de **Raza** alimentado en cascada según la **Especie** seleccionada en los modales.
  * Panel de filtros superior para filtrar la tabla de mascotas en tiempo real por Especie y Raza.

* **UI / UX Mejorada**:
  * DataTables traducido globalmente al español (`i18n/es-ES.json`).
  * Alertas estandarizadas y notificaciones flotantes tipo Toast con **SweetAlert2**.
  * Spinner / Overlay de carga visual automático durante solicitudes AJAX en segundo plano.

---

## Tecnologías Utilizadas

* **Backend**: .NET 8 Core (C#), Entity Framework Core 8, SQL Server.
* **Frontend**: Razor Views, JavaScript / jQuery AJAX, DataTables 2.3.8, Bootstrap 5, SweetAlert2, GLightbox.
* **Gestión de Proyecto**: Git / GitHub, Tablero Kanban en Trello.