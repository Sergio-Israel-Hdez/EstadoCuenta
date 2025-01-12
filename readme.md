# Guía de Configuración del Proyecto

## Configuración del Backend

1. **Modificar el archivo `appsettings.json`:**
   Ajusta la cadena de conexión para apuntar al método de base de datos:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=sqlserver-estadocuenta,1433;Database=EstadoCuenta;User Id=sa;Password=prueba123!;Encrypt=False;"
   }
   ```

2. **Uso de Docker Compose:**
   - Ingresa con las credenciales proporcionadas.
   - Ejecuta el script ubicado en la carpeta `script`.

---

## Configuración del Frontend

1. **Modificar el archivo `appsettings.json`:**
   - Especifica si utilizas Docker Compose.
   - Ajusta el `ApiBaseUrl` según sea necesario:
     ```json
     "ApiBaseUrl": "http://localhost:5141",
     "IsCompose": true,
     "ApiBaseUrlCompose": "http://backend:5141"
     ```

2. **Pruebas en el Frontend:**
   - Selecciona un usuario del 1 al 2 para realizar las pruebas indicadas en el documento.

---

## Documentación de Endpoints

### Controlador: `EstadoCuentaController`

#### Endpoint: `ConsultarEstadoCuenta`
- **Método HTTP:** GET
- **Ruta:** `/api/EstadoCuenta/ConsultarEstadoCuenta`
- **Descripción:** Permite consultar el estado de cuenta de un usuario.
- **Parámetros de Consulta:**
  - `EstadoCuentaQuery`: Modelo que contiene los parámetros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve el estado de cuenta del usuario.
  - **404 Not Found:** No se encontró el estado de cuenta.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

### Controlador: `TransaccionesController`

#### Endpoint: `InsertarCompra`
- **Método HTTP:** POST
- **Ruta:** `/api/Transacciones/InsertarCompra`
- **Descripción:** Permite insertar una nueva compra.
- **Cuerpo de la Solicitud:**
  - `InsertarCompraCommandDTO`: Modelo que contiene los datos de la compra a insertar.
- **Respuestas:**
  - **200 OK:** Compra insertada correctamente.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `InsertarPago`
- **Método HTTP:** POST
- **Ruta:** `/api/Transacciones/InsertarPago`
- **Descripción:** Permite insertar un nuevo pago.
- **Cuerpo de la Solicitud:**
  - `InsertarPagoCommandDTO`: Modelo que contiene los datos del pago a insertar.
- **Respuestas:**
  - **200 OK:** Pago insertado correctamente.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `ConsultarTransaccionesMensuales`
- **Método HTTP:** GET
- **Ruta:** `/api/Transacciones/ConsultarTransaccionesMensuales`
- **Descripción:** Permite consultar las transacciones mensuales de un usuario.
- **Parámetros de Consulta:**
  - `TransaccionesMensualesQuery`: Modelo que contiene los parámetros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve las transacciones mensuales del usuario.
  - **404 Not Found:** No se encontraron transacciones.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `ConsultarCompras`
- **Método HTTP:** GET
- **Ruta:** `/api/Transacciones/ConsultarCompras`
- **Descripción:** Permite consultar las compras de un usuario.
- **Parámetros de Consulta:**
  - `ComprasQuery`: Modelo que contiene los parámetros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve las compras del usuario.
  - **404 Not Found:** No se encontraron compras.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

---

## Ejecución del Proyecto

1. **Requisitos Previos:**
   - Asegúrate de tener Docker instalado y en ejecución en tu máquina.

2. **Iniciar los Servicios:**
   - Navega al directorio del proyecto.
   - Ejecuta el comando:
     ```bash
     docker-compose up
     ```

3. **Acceso a la Aplicación:**
   - Frontend: `http://localhost:5140`
   - API Backend: `http://localhost:7058`

---

## Notas Adicionales
la forma de realizar pruebas dependera de como se ejecute si directamente el proyecto o levantarlo 
por medio de docker compose 