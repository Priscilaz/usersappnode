# UsersApp

Este proyecto es una aplicación CRUD de usuarios con un **backend en ASP.NET Core** y un **frontend servido con Node.js**.

## 📌 Clonar el Repositorio

```sh
git clone https://github.com/Priscilaz/usersappnode.git
cd UsersApp
```

## ⚙️ Configurar el Backend (ASP.NET Core)

1. Asegúrate de tener instalado **.NET SDK** 
2. Abre la terminal en la carpeta del backend y ejecuta:

   ```sh
   dotnet run
   ```

3. El servidor se ejecutará en `http://localhost:5166/api/usersapp`.

## 🌍 Configurar el Frontend (Node.js)

1. Asegúrate de tener instalado **Node.js** 
2. Abre la terminal en la carpeta del frontend (`AppFront`) y ejecuta:

   ```sh
   node app.js
   ```

3. Accede a la aplicación en `http://localhost:3000`.

## 🚀 Uso de la Aplicación

1. Ejecuta el backend con `dotnet run`.
2. Ejecuta el frontend con `node app.js`.
3. Abre `http://localhost:3000` en tu navegador.
4. Usa los botones para realizar operaciones CRUD sobre los usuarios.

## 🔧 API Endpoints

| Método | Endpoint                          | Descripción                |
|--------|----------------------------------|----------------------------|
| GET    | `/api/usersapp/list`            | Obtiene la lista de usuarios |
| POST   | `/api/usersapp/new`             | Agrega un usuario          |
| PUT    | `/api/usersapp/update/{id}`     | Actualiza un usuario por ID |
| DELETE | `/api/usersapp/delete/{id}`     | Elimina un usuario por ID   |

## 📌 Notas

- Asegúrate de que el **backend y el frontend están corriendo al mismo tiempo**.


---
🛠️ *Desarrollado con ASP.NET Core & Node.js*
