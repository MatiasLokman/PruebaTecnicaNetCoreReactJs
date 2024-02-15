//#region Función para obtener todos los clientes
async function GetClients() {
  return fetch("https://localhost:7069/cliente").then(response => response.json());
}
//#endregion

//#region Función para obtener un cliente por su ID
async function GetClientById(id) {
  return fetch(`https://localhost:7069/cliente/${id}`).then(response => response.json());
}
//#endregion

//#region Función para guardar un cliente
async function PostClient(body) {
  return fetch("https://localhost:7069/cliente", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  }).then(response => response.json());
}
//#endregion

//#region Función para actualizar un cliente
async function PutClient(id, body) {
  return fetch(`https://localhost:7069/cliente/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  }).then(response => response.json());
}
//#endregion

//#region Función para dar de baja a un cliente (baja logica)
async function DeleteClient(id, body) {
  return fetch(`https://localhost:7069/cliente/${id}`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  }).then(response => response.json());
}
//#endregion

export {
  GetClientById,
  GetClients,
  PostClient,
  PutClient,
  DeleteClient
}