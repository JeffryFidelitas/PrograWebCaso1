## Migraciones en Entity Framework Core

Para manejar las migraciones en este proyecto, primero debemos estar en el directorio principal del proyecto. Es decir, debemos abrir la terminal en la solución misma. A continuación, seguimos los siguientes pasos:

### 1. Crear una nueva migración

Desde la terminal, ejecuta el siguiente comando en la raíz del proyecto:
```
dotnet ef migrations add NombreDeLaMigracion --project Core --startup-project Caso1API
```
💡 Reemplaza `NombreDeLaMigracion` con un nombre descriptivo, por ejemplo:
```
dotnet ef migrations add InitialCreate --project Core --startup-project Caso1API
```
### 2. Aplicar las migraciones a la base de datos

Para actualizar la base de datos con la última migración, usa:
```
dotnet ef database update --project Core --startup-project Caso1API
```
### 3. Eliminar la última migración

Si cometes un error o necesitas rehacer una migración antes de aplicarla, usa:
```
dotnet ef migrations remove --project Core --startup-project Caso1API
```
Esto deshará la última migración creada.

4. Solución a advertencias sobre listas


### 💡 Nota: Asegúrate de que la cadena de conexión en appsettings.json esté configurada correctamente antes de ejecutar los comandos. 🚀