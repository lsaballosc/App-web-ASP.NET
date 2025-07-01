Sistema de Carrito de Compras en ASP.NET MVC (C#)

Este es un sistema completo de comercio electrónico desarrollado en ASP.NET MVC (C#) utilizando una arquitectura orientada a objetos y en capas (Entidad, Datos, Negocio, admin, tienda). Incluye funcionalidades administrativas y de tienda, integración con API de PayPal, manejo de sesiones, autenticación segura, y almacenamiento en la nube con Firebase.

---

 Arquitectura del Proyecto

El proyecto está diseñado con una arquitectura en capas:

- Entidad: Define los modelos de negocio (Producto, Usuario, Categoria, etc.).
- Datos: Acceso a datos con SQL Server.
- Negocio: Lógica del negocio y validaciones.
- Presentación: Sitio web MVC dividido en dos partes: administración y tienda.

  Sitios Web Incluidos

  Panel de Administración

- CRUD de usuarios administradores, categorías, marcas y productos.
- Carga de imágenes de productos a Firebase Storage.
- Generación de reportes en Excel.
- Seguridad:
  - Autenticación obligatoria.
  - Bloqueo de rutas sin sesión iniciada.
  - Contraseñas encriptadas en base de datos.
  - Envío automático de claves por correo para nuevos administradores.
  - Cambio obligatorio de contraseña en primer ingreso.

🛍️ Sitio de Tienda

- Visualización y filtrado de productos por categoría y marca.
- Carrito persistente por usuario (productos se mantienen al iniciar sesión).
- Registro y login de usuarios.
- Proceso de compra:
  - Conversión automática de colones a dólares con una API de tipo de cambio en tiempo real.
  - Pago con API oficial de PayPal.
  - Redirección a pantalla de éxito con el ID de transacción.
  - Envío automático de factura por correo al usuario y a la tienda.

---
 Funcionalidades 

 Facturación

- Generación de factura con:
  - Productos, cantidades, precios, importe total.
  - Datos del comprador: nombre, contacto, ubicación (provincia, cantón, distrito), dirección y teléfono.
  - Datos de la tienda.
- Factura enviada automáticamente al usuario y administrador por correo.

 Seguridad y Autenticación

- Contraseñas almacenadas con SHA256.
- Clave temporal enviada por correo (admin) y cambio obligatorio en primer ingreso.
- Recuperación de contraseñas por correo para admins y usuarios.
- Validaciones de acceso por sesión iniciada.

 Cascada Dinámica de Ubicación

- Combo box encadenado: Provincia → Cantón → Distrito.
- Carga dinámica según la selección anterior usando jQuery AJAX.

---

 Tecnologías y Herramientas

| Categoría     | Herramientas / Tecnologías |
|---------------|----------------------------|
| Lenguaje      | C#, ASP.NET MVC            |
| Frontend      | Bootstrap, jQuery, AJAX, FontAwesome, SweetAlert |
| Backend       | ADO.NET, SQL Server, Firebase, PayPal API |
| API           | PayPal Checkout, Tipo de Cambio (real-time) |
| Seguridad     | Encriptación de contraseñas, validaciones, sesiones |
| Otros         | Firebase Storage, envío de correos automáticos |

---


---

Cómo Ejecutar el Proyecto

1. Clona este repositorio.
2. Configura tu cadena de conexión en Web.config.(en mi caso no están subidos porque hay datos sensibles)
3. Registra un proyecto en Firebase y configura las credenciales.
4. Configura las credenciales de la API de PayPal.
5. Configura la API de tipo de cambio que estés utilizando.
6. Ejecuta desde Visual Studio.

---

 Requisitos Previos

- Visual Studio 2019 o superior
- SQL Server (Express o full)
- Cuenta PayPal Developer
- Proyecto Firebase con acceso a Storage
- SMTP válido para envío de correos

---

 Funcionalidades de Correo

- Envío de contraseña temporal a nuevos administradores.
- Recuperación de contraseña para usuarios y admins.
- Envío de facturas automáticas luego de una compra exitosa.

---

 Estado del Proyecto

✔️ Funcional  
✔️ Completado  
✔️ Listo para producción e integración en negocio pequeño

---
Desarrollado en C#, ASP.NET MVC, y  por Leonel Saballos Cordón.


