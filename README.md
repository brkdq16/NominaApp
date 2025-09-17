📌 NominaApp – Sistema de Nómina
📖 Descripción

NominaApp es un sistema de gestión de pagos de empleados desarrollado en C# y .NET 8 siguiendo los principios de Arquitectura Limpia.
El sistema permite capturar datos de empleados, calcular automáticamente pagos semanales según el tipo de contrato, y generar reportes detallados, garantizando mantenibilidad, escalabilidad y un diseño modular.

🚀 Características

📋 Gestión de empleados: Registro de asalariados, por horas, por comisión y asalariados por comisión.

💵 Cálculo automático de pagos:

Asalariado: salario semanal fijo.

Por horas: cálculo normal y horas extra.

Por comisión: basado en ventas brutas y tarifa de comisión.

Asalariado por comisión: salario base + comisión + bono.

📊 Generación de reportes: Detalle semanal de pagos por empleado.

🔄 Actualización de información: Permite modificar datos para recalcular pagos.

🛠️ Tecnologías

Lenguaje: C#

Framework: .NET 8

Arquitectura: Clean Architecture

Almacenamiento: colecciones en memoria (simulación inicial, con posibilidad de persistencia futura)

📂 Estructura del Proyecto
NominaApp/
├── NominaApp.sln             # Archivo principal de la solución
├── src/
│   ├── NominaApp.Domain/     # Reglas de negocio
│   │   ├── Entities/         # Entidades principales (Empleado, etc.)
│   │   ├── Interfaces/       # Contratos
│   │   ├── Enums/            # Tipos de empleado
│   │   └── ValueObjects/     # Objetos de valor (ej. Dinero)
│   ├── NominaApp.Application/ # Casos de uso
│   │   ├── Services/         # Servicios de aplicación
│   │   ├── DTOs/             # Objetos de transferencia
│   │   └── Interfaces/       # Interfaces de aplicación
│   ├── NominaApp.Infrastructure/ # Implementaciones
│   │   ├── Repositories/     # Manejo de datos
│   │   └── Data/             # Contexto de datos
│   └── NominaApp.Console/    # Interfaz de usuario (consola)
│       ├── Program.cs        # Punto de entrada
│       └── UI/               # Menús y pantallas
├── tests/                    # Pruebas unitarias
└── docs/                     # Documentación

▶️ Cómo ejecutar

Clonar el repositorio:

git clone https://github.com/TU_USUARIO/NominaApp.git
cd NominaApp


Compilar el proyecto:

dotnet build


Ejecutar la aplicación desde consola:

dotnet run --project src/NominaApp.Console

📊 Requisitos Funcionales (resumen)

RF-1: Captura de empleados según su tipo (asalariado, horas, comisión, mixto).

RF-2: Cálculo automático de pagos semanales.

RF-3: Actualización de datos de empleados.

RF-4: Generación de reportes semanales.

👤 Autor

Desarrollado por: Berkeley Vladimir

Proyecto académico / personal para práctica de .NET 8 y Arquitectura Limpia

📄 Licencia

Este proyecto se distribuye bajo licencia MIT.
