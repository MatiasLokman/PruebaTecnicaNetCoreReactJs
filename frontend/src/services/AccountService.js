//#region Función para obtener depositos por ID de cliente
async function GetAccountsById(clientId) {
  const url = `https://localhost:7069/cuenta/${clientId}`;
  return fetch(url).then(response => response.json());
}
//#endregion

//#region Función para guardar un deposito
async function PostOwnAccount(body) {
  return fetch("https://localhost:7069/cuenta", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  }).then(response => response.json());
}
//#endregion

//#region Función para guardar un deposito
async function PostOtherAccount(body) {
  return fetch("https://localhost:7069/cuenta/transferencia", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  }).then(response => response.json());
}
//#endregion

export {
  GetAccountsById,
  PostOwnAccount,
  PostOtherAccount
}