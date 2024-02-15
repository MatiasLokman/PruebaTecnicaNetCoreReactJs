## 💾 Database
### 🔄 Creación de la Base de Datos (PostgreSQL)

Para crear la Base de Datos en pgAdmin, siga estos pasos detallados:

**1) Crear la Base de Datos en pgAdmin:**
- Abra pgAdmin y cree una nueva base de datos llamada `controlglobal`
**2) Ubicar el Archivo SQL:**
- Encuentre el archivo `controlglobal.sql` en la carpeta `database`
**3) Copiar el Archivo SQL:**
- Copie el archivo `controlglobal.sql` en la siguiente ubicación de su computadora: `C:\Program Files\PostgreSQL\15\bin`
 **4) Ejecutar Comandos en la Terminal (CMD):**
- Abra una terminal como administrador (`Símbolo del sistema`).
- Navegue hasta la ubicación donde se encuentra el archivo controlglobal.sql usando los siguientes comandos:
```
  cd ..
```
```
  cd ..
```
```
  cd "Program Files"
```
```
  cd PostgreSQL
```
```
  cd bin
```
```
  psql -h localhost -p 5432 -U postgres -f controlglobal.sql controlglobal
```


‎ 

‎ 


‎ 


## 📊 Backend
### 🌐 API Server (Visual Studio)

Para correr la API debe seguir los siguientes pasos:

**1)** Abrir el archivo `backend.sln` que se encuentra dentro de la carpeta `backend`

**2)** Ejecutar en la terminal de Visual Studio el siguiente comando: 
```
  dotnet run
```
Este corre en el puerto `7069`.
Abre [https://localhost:7069/swagger/index.html](https://localhost:7069/swagger/index.html) para verlo con su documentación de Swagger en su navegador.


‎ 
### 🛠️ Configuración de la Base de Datos en el Backend (appsettings.json)

Para modificar la configuración de la base de datos, como la contraseña o el usuario, sigue estos pasos:

**1)** Abre el archivo `appsettings.json` dentro de la carpeta `API` en el backend.

**2)** Ubica la sección `"ConnectionStrings"` y encuentra la cadena de conexión denominada `"DefaultConnection"`:
```
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost; Database=controlglobal; Username=postgres; Password=password"
}
```
**3)** Cambia el usuario y contraseña si es necesario.


‎ 

‎ 


‎ 


## 📂 Frontend
### ⬇️ Instalacion de dependencias (Visual Studio Code)

Para instalar las dependencias necesarias debe ejecutar el comando: 
```
  npm install
```


‎ 
### 🚀 Aplicación (Visual Studio Code)

Para correr la aplicacion debe ejecutar el comando:
```
  npm run start
```
Esta corre en el puerto `3000`.
Abre [http://localhost:3000](http://localhost:3000) para verlo en su navegador.


‎ 

‎ 
![App Screenshot](https://yourfiles.cloud/uploads/ab5d3a7a7b32c15c934d865b626d6179/imagen.png)