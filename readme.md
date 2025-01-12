# Gu�a de Configuraci�n del Proyecto

## Configuraci�n del Backend

1. **Modificar el archivo `appsettings.json`:**
   Ajusta la cadena de conexi�n para apuntar al m�todo de base de datos:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=sqlserver-estadocuenta,1433;Database=EstadoCuenta;User Id=sa;Password=prueba123!;Encrypt=False;"
   }
   ```

2. **Uso de Docker Compose:**
   - Ingresa con las credenciales proporcionadas.
   - Ejecuta el script ubicado en la carpeta `script`.

---

## Configuraci�n del Frontend

1. **Modificar el archivo `appsettings.json`:**
   - Especifica si utilizas Docker Compose.
   - Ajusta el `ApiBaseUrl` seg�n sea necesario:
     ```json
     "ApiBaseUrl": "http://localhost:5141",
     "IsCompose": true,
     "ApiBaseUrlCompose": "http://backend:5141"
     ```

2. **Pruebas en el Frontend:**
   - Selecciona un usuario del 1 al 2 para realizar las pruebas indicadas en el documento.

---

## Documentaci�n de Endpoints

### Controlador: `EstadoCuentaController`

#### Endpoint: `ConsultarEstadoCuenta`
- **M�todo HTTP:** GET
- **Ruta:** `/api/EstadoCuenta/ConsultarEstadoCuenta`
- **Descripci�n:** Permite consultar el estado de cuenta de un usuario.
- **Par�metros de Consulta:**
  - `EstadoCuentaQuery`: Modelo que contiene los par�metros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve el estado de cuenta del usuario.
  - **404 Not Found:** No se encontr� el estado de cuenta.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

### Controlador: `TransaccionesController`

#### Endpoint: `InsertarCompra`
- **M�todo HTTP:** POST
- **Ruta:** `/api/Transacciones/InsertarCompra`
- **Descripci�n:** Permite insertar una nueva compra.
- **Cuerpo de la Solicitud:**
  - `InsertarCompraCommandDTO`: Modelo que contiene los datos de la compra a insertar.
- **Respuestas:**
  - **200 OK:** Compra insertada correctamente.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `InsertarPago`
- **M�todo HTTP:** POST
- **Ruta:** `/api/Transacciones/InsertarPago`
- **Descripci�n:** Permite insertar un nuevo pago.
- **Cuerpo de la Solicitud:**
  - `InsertarPagoCommandDTO`: Modelo que contiene los datos del pago a insertar.
- **Respuestas:**
  - **200 OK:** Pago insertado correctamente.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `ConsultarTransaccionesMensuales`
- **M�todo HTTP:** GET
- **Ruta:** `/api/Transacciones/ConsultarTransaccionesMensuales`
- **Descripci�n:** Permite consultar las transacciones mensuales de un usuario.
- **Par�metros de Consulta:**
  - `TransaccionesMensualesQuery`: Modelo que contiene los par�metros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve las transacciones mensuales del usuario.
  - **404 Not Found:** No se encontraron transacciones.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

#### Endpoint: `ConsultarCompras`
- **M�todo HTTP:** GET
- **Ruta:** `/api/Transacciones/ConsultarCompras`
- **Descripci�n:** Permite consultar las compras de un usuario.
- **Par�metros de Consulta:**
  - `ComprasQuery`: Modelo que contiene los par�metros necesarios para la consulta.
- **Respuestas:**
  - **200 OK:** Devuelve las compras del usuario.
  - **404 Not Found:** No se encontraron compras.
  - **500 Internal Server Error:** Error en la base de datos o error inesperado.

---

## Ejecuci�n del Proyecto

1. **Requisitos Previos:**
   - Aseg�rate de tener Docker instalado y en ejecuci�n en tu m�quina.

2. **Iniciar los Servicios:**
   - Navega al directorio del proyecto.
   - Ejecuta el comando:
     ```bash
     docker-compose up
     ```

3. **Acceso a la Aplicaci�n:**
   - Frontend: `http://localhost:5140`
   - API Backend: `http://localhost:7058`

---

## Notas Adicionales
la forma de realizar pruebas dependera de como se ejecute si directamente el proyecto o levantarlo 
por medio de docker compose 