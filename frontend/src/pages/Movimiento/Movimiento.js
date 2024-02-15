import React, { useState, useEffect } from 'react';
import { GetAccountsById, PostOwnAccount, PostOtherAccount } from '../../services/AccountService';
import { GetClientById } from '../../services/ClientService';

import { useParams } from 'react-router-dom';

import { Link } from 'react-router-dom';

import * as signalR from "@microsoft/signalr";

import Swal from "sweetalert2";

import { ReactComponent as PostSvg } from '../../assets/svgs/post.svg';
import { ReactComponent as BackSvg } from '../../assets/svgs/back.svg';

import './Movimiento.css';

const Movimiento = () => {

  const { id } = useParams();

  const [accounts, setAccounts] = useState([]);
  const [client, setClient] = useState({});

  const [accion, setAccion] = useState("");

  const [importe, setImporte] = useState("");
  const [descripcion, setDescripcion] = useState("");

  const [modalTitle, setModalTitle] = useState("");

  const [identificacionReceptor, setIdentificacionReceptor] = useState("");

  //#region Función para cargar las movimientos
  async function fetchAccounts() {
    try {
      const accountsData = await GetAccountsById(id);
      setAccounts(accountsData.cuentas);
    } catch (error) {
      console.error('Error fetching accounts:', error);
    }
  };
  //#endregion

  //#region Función para manejar la inserción de un movimiento
  async function handlePostAccount(event) {
    event.preventDefault();

    if (accion === "Depositar dinero") {
      if (IsValidDeposito() === true) {
        try {
          const response = await PostOwnAccount({
            importe: importe,
            descripcion: "Deposito de dinero",
            idCliente: id
          });

          Swal.fire({
            icon: 'success',
            title: 'Deposito registrado exitosamente!',
            showConfirmButton: false,
            timer: 4000
          }).then(() => {
            Swal.fire({
              toast: true,
              position: 'top-end',
              title: `+ $${response.importe}`,
              showConfirmButton: false,
              timer: 5000,
              timerProgressBar: true,
              customClass: {
                popup: 'swal2-toast',
                title: 'swal2-toast-title',
                container: 'swal2-toast-container'
              },
              background: '#6ede73',
              grow: 'column'
            });
          });

          CloseModal();
          fetchAccounts();
        } catch (error) {
          Swal.fire({
            title: 'Error',
            text: 'Ocurrió un error al registrar el deposito',
            icon: 'error',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#f27474',
          });
        }
      }
    } else {
      if (IsValid() === true) {

        try {
          const response = await PostOtherAccount({
            importe: importe,
            descripcion: "Transferencia: " + descripcion,
            idCliente: id,
            identificacion: identificacionReceptor
          });

          console.log(response)

          if (response.errorMessage.includes("ingrese un id de un receptor existente")) {
            Swal.fire({
              icon: 'error',
              title: 'No existe la identificacion del receptor!',
              text: 'Cambie la identificacion por una valida',
              showConfirmButton: false,
              timer: 4000
            })
          } else if (response.errorMessage.includes("Saldo insuficiente para realizar la transferencia")) {
            Swal.fire({
              icon: 'error',
              title: 'Saldo insuficiente para realizar la transferencia!',
              text: 'Ingrese un importe menor o igual a su saldo actual',
              showConfirmButton: false,
              timer: 4000
            })
          }
          else {
            Swal.fire({
              icon: 'success',
              title: 'Transferencia registrada exitosamente!',
              showConfirmButton: false,
              timer: 4000
            }).then(() => {
              Swal.fire({
                toast: true,
                position: 'top-end',
                title: `- $${response.importe}`,
                showConfirmButton: false,
                timer: 5000,
                timerProgressBar: true,
                customClass: {
                  popup: 'swal2-toast',
                  title: 'swal2-toast-title',
                  container: 'swal2-toast-container'
                },
                background: '#de6e6e',
                grow: 'column'
              });
            });

            CloseModal();
            fetchAccounts();
          }
        } catch (error) {
          Swal.fire({
            title: 'Error',
            text: 'Ocurrió un error al registrar la transferencia',
            icon: 'error',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#f27474',
          });
        }
      }

    }
  }
  //#endregion

  //#region Función para cerrar el modal 
  function CloseModal() {
    const closeButton = document.getElementById("btn-close-modal");
    if (closeButton) {
      closeButton.click();
    }

    setAccion("");
  }
  //#endregion

  //#region Función para limpiar todos los valores de los inputs del formulario
  function ClearAccountInputs() {
    setImporte("");
    setDescripcion("");
    setIdentificacionReceptor("");
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a través de los inputs son correctos
  function IsValid() {
    if (importe === "") {
      Swal.fire({
        icon: 'error',
        title: 'El importe no puede estar vacío',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('importe').focus();
        }, 500);
      });
      return false;
    } else if (descripcion === "") {
      Swal.fire({
        icon: 'error',
        title: 'La descripcion no puede estar vacía',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('descripcion').focus();
        }, 500);
      });
      return false;
    } else if (identificacionReceptor === "") {
      Swal.fire({
        icon: 'error',
        title: 'La identificacion del receptor no puede estar vacía',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('identificacion').focus();
        }, 500);
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si los valores ingresados (en el deposito) a través de los inputs son correctos
  function IsValidDeposito() {
    if (importe === "") {
      Swal.fire({
        icon: 'error',
        title: 'El importe no puede estar vacío',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('importe').focus();
        }, 500);
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si los valores de los inputs están vacíos
  function IsEmpty() {
    if (importe !== "") {
      return false
    } else if (descripcion !== "") {
      return false
    }
    return true
  }
  //#endregion

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7069/cuentaHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.start().then(() => {
      // console.log("Conexión establecida con el servidor SignalR");
    }).catch(err => console.error(err.toString()));

    connection.on("RecibirSaldoDepositado", async () => {
      try {
        const clientData = await GetClientById(id);
        setClient(clientData);
      } catch (error) {
        console.error("Error al obtener el cliente: " + error);
      }
    });

    connection.on("RecibirSaldoTransferido", async () => {
      try {
        const clientData = await GetClientById(id);
        setClient(clientData);
      } catch (error) {
        console.error("Error al obtener el cliente: " + error);
      }
    });

    return () => {
      connection.stop();
    };
  }, [id]);

  useEffect(() => {
    async function fetchAccounts() {
      try {
        const accountsData = await GetAccountsById(id);
        setAccounts(accountsData.cuentas);
      } catch (error) {
        console.error('Error fetching accounts:', error);
      }
    }
    fetchAccounts();
  }, [id]);

  useEffect(() => {
    async function fetchClient() {
      try {
        const clientData = await GetClientById(id);
        setClient(clientData);
      } catch (error) {
        console.error('Error fetching accounts:', error);
      }
    }
    fetchClient();
  }, [id]);

  return (
    <div className='global-container'>

      <div className='info-container'>
        <p className="text-white normal">{client.nombre} {client.apellido} - <b className="bold">{client.identificacion}</b></p>
        <p className="text-white normal">Saldo: <b className="bold">${client.saldo?.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }).replace(',', '.')}</b></p>
      </div>

      <div className='title-btn-container'>
        <Link to="/clientes">
          <button title="Volver atras" className="btn svg-btn svg-btn-back">
            <BackSvg className="svg" />
          </button>
        </Link>

        <h1 className="text-white">Gestión de movimientos</h1>

        <button type="button" className="btn btn-success svg-btn" data-bs-toggle="modal" data-bs-target="#modal" title={!client.estado ? "Usuario no habilitado" : "Registrar movimiento"} disabled={!client.estado} onClick={() => { ClearAccountInputs(); setModalTitle("Registrar Movimiento") }}>
          <PostSvg className="svg" />
        </button>

      </div>

      <table className="table table-striped table-bordered table-hover table-custom" align="center">
        <thead className="thead-dark">
          <tr className="table-header">
            <th className="table-title" scope="col">#</th>
            <th className="table-title" scope="col">Fecha Movimiento</th>
            <th className="table-title" scope="col">Importe</th>
            <th className="table-title" scope="col">descripcion</th>
          </tr>
        </thead>

        {accounts && accounts.length > 0 ? (
          accounts.map(function fn(account, index) {
            return (
              <tbody key={1 + account.idCuenta}>
                <tr>
                  <th scope="row" className="table-name">{(index + 1)}</th>
                  <td className="table-name">
                    {new Date(account.fechaMovimiento).toLocaleDateString('es-ES', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                      hour: '2-digit',
                      minute: '2-digit'
                    })}
                  </td>
                  <td className="table-name">
                    {account.descripcion === 'Deposito de dinero' ? (
                      <span className="activo">+ ${account.importe?.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }).replace(',', '.')}</span>
                    ) : (
                      <span className="no-activo">- ${account.importe?.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }).replace(',', '.')}</span>
                    )}
                  </td>
                  <td className="table-name">{account.descripcion}</td>
                </tr>
              </tbody>
            );
          })
        ) : (
          <tbody>
            <tr>
              <td className="table-name" colSpan={4}>Sin registros</td>
            </tr>
          </tbody>

        )}
      </table>


      {/* Modal con el formulario para registrar un movimiento */}
      <div className="modal fade" id="modal" data-bs-backdrop="static" data-bs-keyboard="false" tabIndex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header justify-content-center">
              <h1 className="modal-title" id="exampleModalLabel">{modalTitle}</h1>
            </div>
            <div className="modal-body">
              <div className="container">
                <form>
                  <div className="form-group">
                    <label className="label">Accion:</label>
                    <div className="form-group-input">
                      <select
                        className="form-control mb-3"
                        value={accion}
                        onChange={(event) => {
                          setAccion(event.target.value);
                        }}
                      >
                        <option value="">Seleccione una acción</option>
                        <option value="Depositar dinero">Depositar dinero</option>
                        {client.saldo > 0 && <option value="Transferir dinero">Transferir dinero</option>}
                      </select>
                    </div>
                  </div>

                  {accion && (
                    <div className="form-group">
                      <label className="label">Importe:</label>
                      <div className="form-group-input">
                        <input
                          type="number"
                          className="form-control mb-3"
                          id="importe"
                          value={importe}
                          onChange={(event) => {
                            setImporte(event.target.value);
                          }}
                        />
                      </div>
                    </div>
                  )}

                  {accion === 'Transferir dinero' && (
                    <div className="form-group">
                      <label id="descripcion" className="label">Descripción:</label>
                      <div id="descripcion">
                        <input
                          type="text"
                          className="form-control mb-3"
                          id="descripcion"
                          value={descripcion}
                          onChange={(event) => {
                            setDescripcion(event.target.value);
                          }}
                        />
                      </div>
                    </div>
                  )}

                  {accion === 'Transferir dinero' && (
                    <div className="form-group">
                      <label id="identificacion" className="label">Identificacion del receptor:</label>
                      <div id="identificacion">
                        <input
                          type="text"
                          className="form-control mb-3"
                          id="identificacion"
                          value={identificacionReceptor}
                          onChange={(event) => {
                            setIdentificacionReceptor(event.target.value);
                          }}
                        />
                      </div>
                    </div>
                  )}

                  <div>
                    <div id="div-btn-save">
                      <button className="btn btn-success" id="btn-save" onClick={handlePostAccount}>
                        <div>
                          <p className="fw-semibold">Guardar</p>
                        </div>
                      </button>
                    </div>
                  </div>
                </form>
              </div>
            </div>
            <div className="modal-footer">

              <button type="button" className="btn btn-secondary"
                onClick={() => {
                  if (modalTitle === 'Registrar Movimiento') {
                    if (IsEmpty() === true) {
                      ClearAccountInputs();
                      CloseModal()
                    } else {
                      Swal.fire({
                        icon: 'warning',
                        title: '¿Está seguro de que desea cerrar el formulario?',
                        text: "Se perderán todos los datos cargados",
                        confirmButtonText: 'Aceptar',
                        showCancelButton: true,
                        cancelButtonText: 'Cancelar',
                        confirmButtonColor: '#f8bb86',
                        cancelButtonColor: '#6c757d',
                      }).then((result) => {
                        if (result.isConfirmed) {
                          ClearAccountInputs();
                          CloseModal();
                        }
                      })
                    }
                  }
                }}
              >
                <div>
                  <p className="fw-semibold">X Cerrar</p>
                </div>
              </button>

              <button type="button" hidden className="btn-close-modal" id="btn-close-modal" data-bs-dismiss="modal"></button>

            </div>
          </div>
        </div>
      </div>


    </div >
  );
}

export default Movimiento;
