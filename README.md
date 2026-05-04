Documentación de la Base de Datos
El diseño de la base de datos se basa en una arquitectura relacional normalizada, diseñada para soportar la gestión multifuncional de consorcios, unidades funcionales y liquidación de expensas.

Entidades Principales
1. Usuarios y Seguridad
User: Almacena las credenciales de acceso. Soporta diferentes roles (Admin, Administrador de Propiedades, Propietario).

Role: Define los permisos dentro del sistema (RBAC - Role Based Access Control).

Owner: Representa a la persona física dueña de una o más unidades. Puede o no tener un usuario asociado para acceder a la plataforma.

2. Estructura Edilicia
Building (Consorcio): Representa el edificio o complejo. Contiene metadatos como la fecha de creación, día de vencimiento de expensas y superficie total.

Address: Entidad separada para normalizar la ubicación. Incluye soporte para coordenadas geográficas (Latitude, Longitude) con precisión de 10 decimales para integración con mapas.

Province: Tabla maestra que contiene las 24 jurisdicciones de Argentina.

Unit: Unidades funcionales pertenecientes a un edificio. Incluye superficie propia en metros cuadrados, necesaria para el cálculo proporcional de expensas.

3. Gestión de Gastos y Expensas
Expense (Gasto): Registro de los egresos del edificio.

Posee un ExpenseType (Ordinario/Extraordinario).

Incluye un campo IsGlobal para distinguir gastos que afectan a todo el edificio de aquellos específicos.

Almacena la ruta del comprobante físico (ReceiptPath).

ExpenseDistribution: Tabla de resolución muchos-a-muchos que permite asignar un gasto a unidades específicas cuando no es un gasto global del consorcio.

ExpenseType: Clasificación maestra de los gastos.