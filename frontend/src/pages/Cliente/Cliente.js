import React, { useState, useEffect } from 'react';
import { GetClients, PostClient, PutClient, DeleteClient } from '../../services/ClientService';

import * as signalR from "@microsoft/signalr";

import { Link } from 'react-router-dom';

import Swal from "sweetalert2";

import { ReactComponent as PostSvg } from '../../assets/svgs/post.svg';
import { ReactComponent as PutSvg } from '../../assets/svgs/put.svg';
import { ReactComponent as PendienteSvg } from '../../assets/svgs/pendiente.svg';
import { ReactComponent as VerificarSvg } from '../../assets/svgs/verificar.svg';
import { ReactComponent as MovimientoSvg } from '../../assets/svgs/movimientos.svg';
import { ReactComponent as BackSvg } from '../../assets/svgs/back.svg';

import './Cliente.css';

const Cliente = () => {

  const [clients, setClients] = useState([]);

  const [id, setId] = useState("");
  const [nombre, setNombre] = useState("");
  const [apellido, setApellido] = useState("");
  const [identificacion, setIdentificacion] = useState("");
  const [saldo, setSaldo] = useState("");
  const [estado, setEstado] = useState("");
  const checkbox = document.getElementById("estado");

  const [modalTitle, setModalTitle] = useState("");

  //#region Función para cargar los clientes
  async function loadClients() {
    try {
      const clientsData = await GetClients();
      setClients(clientsData.clientes);
    } catch (error) {
      console.error('Error fetching clients:', error);
    }
  };
  //#endregion

  //#region Función para dar de baja a un cliente
  async function handleDeleteClient(id) {
    try {
      const deletionInfo = {
        Estado: false
      };

      const response = await DeleteClient(id, deletionInfo);

      console.log(response)


      if (response.errorMessage === "No se puede dar de baja al cliente porque el saldo es mayor a 0") {
        Swal.fire({
          title: 'No se puede dar de baja al cliente porque su saldo es mayor a 0',
          icon: 'error',
          confirmButtonText: 'Aceptar',
          confirmButtonColor: '#f27474',
        });
      } else {
        Swal.fire({
          icon: 'success',
          title: 'Cliente dado de baja exitosamente!',
          showConfirmButton: false,
          timer: 4000
        })

        await loadClients();
      }

    } catch (error) {
      console.error('Error deleting client:', error);
      Swal.fire('Error', 'Ocurrió un error al dar de baja el cliente', 'error');
    }
  };
  //#endregion

  //#region Función para dar de alta a un cliente
  async function handleDischargeClient(id) {
    try {
      const dischargeInfo = {
        Estado: true
      };

      await DeleteClient(id, dischargeInfo);

      Swal.fire({
        icon: 'success',
        title: 'Cliente dado de alta exitosamente!',
        showConfirmButton: false,
        timer: 4000
      })

      await loadClients();
    } catch (error) {
      console.error('Error deleting client:', error);
      Swal.fire('Error', 'Ocurrió un error al dar de alta el cliente', 'error');
    }
  };
  //#endregion

  //#region Función para manejar la inserción de un cliente
  async function handlePostClient(event) {
    event.preventDefault();

    const isValid = IsValid();

    if (isValid) {
      try {
        const response = await PostClient({
          nombre: nombre.charAt(0).toUpperCase() + nombre.slice(1),
          apellido: apellido.charAt(0).toUpperCase() + apellido.slice(1),
          identificacion: identificacion,
          saldo: saldo,
          estado: estado
        });


        if (response.errorMessage === "Este cliente ya se encuentra registrado") {
          Swal.fire({
            title: 'Ya existe un cliente con la identificacacion ingresada',
            text: 'Cambie la identificacion',
            icon: 'error',
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#f27474',
          });
        } else {
          Swal.fire({
            icon: 'success',
            title: 'Cliente registrado exitosamente!',
            showConfirmButton: false,
            timer: 4000
          });
          CloseModal();
          loadClients();
        }

      } catch (error) {
        Swal.fire({
          title: 'Error',
          text: 'Ocurrió un error al registrar el cliente',
          icon: 'error',
          confirmButtonText: 'Aceptar',
          confirmButtonColor: '#f27474',
        });
      }
    }
  }
  //#endregion

  //#region Función para manejar la actualización de un cliente
  async function handlePutClient(event) {
    event.preventDefault();

    const isValid = IsValid();

    if (isValid) {
      try {
        await PutClient(id, {
          nombre: nombre.charAt(0).toUpperCase() + nombre.slice(1),
          apellido: apellido.charAt(0).toUpperCase() + apellido.slice(1),
          identificacion: identificacion,
          saldo: saldo,
          estado: estado
        });

        Swal.fire({
          icon: 'success',
          title: 'Cliente actualizado exitosamente!',
          showConfirmButton: false,
          timer: 4000
        });
        CloseModal();
        loadClients();
      } catch (error) {
        Swal.fire({
          title: 'Error',
          text: 'Ocurrió un error al actualizar el cliente',
          icon: 'error',
          confirmButtonText: 'Aceptar',
          confirmButtonColor: '#f27474',
        });
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
  }
  //#endregion

  //#region Función para limpiar todos los valores de los inputs del formulario
  function ClearClientInputs() {
    setId("");
    setNombre("");
    setApellido("");
    setIdentificacion("");
    setSaldo("")
    setEstado("");
  }
  //#endregion

  //#region Función para obtener los valores almacenados de un cliente y cargar cada uno de ellos en su input correspondiente
  function RetrieveClientInputs(client) {
    setId(client.idCliente);
    setNombre(client.nombre);
    setApellido(client.apellido);
    setIdentificacion(client.identificacion);
    setSaldo(client.saldo)
    setEstado(client.estado);
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a través de los inputs son correctos
  function IsValid() {
    if (nombre === "") {
      Swal.fire({
        icon: 'error',
        title: 'El nombre es requerido',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('nombre').focus();
        }, 500);
      });
      return false;
    } else if (apellido === "") {
      Swal.fire({
        icon: 'error',
        title: 'El apellido es requerido',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('apellido').focus();
        }, 500);
      });
      return false;
    } else if (identificacion === "") {
      Swal.fire({
        icon: 'error',
        title: 'La identificacion es requerida',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('identificacion').focus();
        }, 500);
      });
      return false;
    } else if (saldo === "") {
      Swal.fire({
        icon: 'error',
        title: 'El saldo es requerido, si no tiene ingrese 0',
        text: 'Complete el campo',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      }).then(function () {
        setTimeout(function () {
          document.getElementById('saldo').focus();
        }, 500);
      });
      return false;
    } else if (estado === "") {
      Swal.fire({
        icon: 'error',
        title: 'Debe indicar si se encuentra activo',
        text: 'Clickeé el botón en caso de que la mismo se encuentre dado de baja',
        confirmButtonText: 'Aceptar',
        confirmButtonColor: '#f27474',
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si los valores de los inputs están vacíos
  function IsEmpty() {
    if (nombre !== "") {
      return false
    } else if (apellido !== "") {
      return false
    } else if (identificacion !== "") {
      return false
    } else if (saldo !== "") {
      return false
    } else if (estado !== false) {
      return false
    }
    return true
  }
  //#endregion

  useEffect(() => {
    loadClients();
  }, []);

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
        loadClients();
      } catch (error) {
        console.error("Error al obtener los clientes: " + error);
      }
    });

    connection.on("RecibirSaldoTransferido", async () => {
      try {
        loadClients();
      } catch (error) {
        console.error("Error al obtener los clientes: " + error);
      }
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div className='global-container'>
      <div className='title-btn-container'>

        <Link to="/">
          <button title="Volver atras" className="btn svg-btn svg-btn-back">
            <BackSvg className="svg" />
          </button>
        </Link>

        <h1 className="text-white">Gestión de clientes</h1>

        <button title="Crear" type="button" className="btn btn-success svg-btn" data-bs-toggle="modal" data-bs-target="#modal" onClick={() => { ClearClientInputs(); setModalTitle("Registrar Cliente"); setTimeout(function () { document.getElementById('nombre').focus(); }, 500); setEstado(true) }}>
          <PostSvg className="svg" />
        </button>

      </div>

      <table className="table table-striped table-bordered table-hover table-custom" align="center">
        <thead className="thead-dark">
          <tr className="table-header">
            <th className="table-title" scope="col">#</th>
            <th className="table-title" scope="col">Nombre</th>
            <th className="table-title" scope="col">Apellido</th>
            <th className="table-title" scope="col">Identificacion</th>
            <th className="table-title" scope="col">Saldo</th>
            <th className="table-title" scope="col">Activo</th>
            <th className="table-title" scope="col">Movimientos</th>
            <th className="table-title" scope="col">Acciones</th>
          </tr>
        </thead>

        {clients?.length > 0 ? (
          clients.map(function fn(client, index) {
            return (
              <tbody key={1 + client.idCliente}>
                <tr>
                  <th scope="row" className="table-name">{(index + 1)}</th>
                  <td className="table-name">{client.nombre}</td>
                  <td className="table-name">{client.apellido}</td>
                  <td className="table-name">{client.identificacion}</td>
                  <td className="table-name">${client.saldo?.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }).replace(',', '.')}</td>
                  {client.estado ? (
                    <td className="table-name activo">Si</td>
                  ) : (
                    <td className="table-name no-activo">No</td>
                  )}

                  <td className="table-name">
                    <Link to={`/movimientos/${client.idCliente}`} title="Ver movimientos" type="button" className="btn btn-info svg-btn">
                      <MovimientoSvg className="svg" />
                    </Link>
                  </td>

                  <td className="table-name">
                    <button title="Editar" type="button" className="btn btn-warning svg-btn" data-bs-toggle="modal" data-bs-target="#modal" onClick={() => { RetrieveClientInputs(client); setModalTitle("Actualizar Cliente") }}>
                      <PutSvg className="svg" />
                    </button>


                    {client.estado ? (
                      <button
                        title="Dar de baja"
                        type="button"
                        className="btn btn-danger svg-btn"
                        onClick={() => Swal.fire({
                          title: 'Esta seguro de que desea dar de baja al siguiente cliente: ' + (client.nombre) + " " + (client.apellido) + '?',
                          icon: 'warning',
                          showCancelButton: true,
                          confirmButtonColor: '#F8BB86',
                          cancelButtonColor: '#6c757d',
                          confirmButtonText: 'Aceptar',
                          cancelButtonText: 'Cancelar',
                          focusCancel: true
                        }).then((result) => {
                          if (result.isConfirmed) {
                            handleDeleteClient(client.idCliente)
                          }
                        })
                        }
                      >
                        <PendienteSvg className="svg" />
                      </button>

                    ) : (
                      <button
                        title="Dar de alta"
                        type="button"
                        className="btn btn-success svg-btn"
                        onClick={() => Swal.fire({
                          title: 'Esta seguro de que desea dar de alta al siguiente cliente: ' + (client.nombre) + " " + (client.apellido) + '?',
                          icon: 'warning',
                          showCancelButton: true,
                          confirmButtonColor: '#F8BB86',
                          cancelButtonColor: '#6c757d',
                          confirmButtonText: 'Aceptar',
                          cancelButtonText: 'Cancelar',
                          focusCancel: true
                        }).then((result) => {
                          if (result.isConfirmed) {
                            handleDischargeClient(client.idCliente)
                          }
                        })
                        }
                      >
                        <VerificarSvg className="svg" />
                      </button>
                    )
                    }

                  </td>
                </tr>
              </tbody>
            );
          })
        ) : (
          <tbody>
            <tr>
              <td className="table-name" colSpan={8}>Sin registros</td>
            </tr>
          </tbody>

        )}
      </table>


      {/* Modal con el formulario para registrar/actualizar un cliente */}
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
                    <input
                      type="text"
                      className="form-control mb-3"
                      id="id"
                      hidden
                      value={id}
                      onChange={(event) => {
                        setId(event.target.value);
                      }}
                    />

                    <label className="label">Nombre:</label>
                    <div className="form-group-input">
                      <input
                        type="text"
                        className="form-control mb-3"
                        id="nombre"
                        value={nombre}
                        onChange={(event) => {
                          setNombre(event.target.value);
                        }}
                      />
                    </div>

                    <label className="label">Apellido:</label>
                    <div className="form-group-input">
                      <input
                        type="text"
                        className="form-control mb-3"
                        id="apellido"
                        value={apellido}
                        onChange={(event) => {
                          setApellido(event.target.value);
                        }}
                      />
                    </div>

                    <label className="label">Identificacion:</label>
                    <div className="form-group-input">
                      <input
                        type="text"
                        className={`form-control mb-3 ${modalTitle === "Actualizar Cliente" ? "disabled" : ""}`}
                        id="identificacion"
                        value={identificacion}
                        onChange={(event) => {
                          setIdentificacion(event.target.value);
                        }}
                        readOnly={modalTitle === "Actualizar Cliente"}
                      />
                    </div>

                    <label className="label">Saldo:</label>
                    <div className="form-group-input">
                      <input
                        type="number"
                        className="form-control mb-3"
                        id="saldo"
                        value={saldo}
                        onChange={(event) => {
                          setSaldo(event.target.value);
                        }}
                      />
                    </div>
                  </div>

                  <div className="form-group d-flex flex-column align-items-start gap-1 mb-3">
                    <label className="form-check-label">Activo:</label>
                    <input
                      type="checkbox"
                      className="form-check-label"
                      id="estado"
                      checked={estado}
                      onChange={(event) => {
                        setEstado(checkbox.checked);

                        const isChecked = event.target.checked;
                        // Si el checkbox se desmarca, establecer el saldo en 0
                        if (!isChecked) {
                          setSaldo(0);
                        }
                      }}
                    />
                  </div>

                  <div>
                    {modalTitle === "Registrar Cliente" ? (
                      <div id="div-btn-save">
                        <button className="btn btn-success" id="btn-save" onClick={handlePostClient}>
                          <div>
                            <p className="fw-semibold">Guardar</p>
                          </div>
                        </button>
                      </div>
                    ) : (
                      <div id="div-btn-update">
                        <button className="btn btn-warning" id="btn-update" onClick={handlePutClient}>
                          <div>

                            <p className="fw-semibold text-white">Actualizar</p>
                          </div>
                        </button>
                      </div>
                    )}
                  </div>

                </form>
              </div>
            </div>
            <div className="modal-footer">

              <button type="button" className="btn btn-secondary"
                onClick={() => {
                  if (modalTitle === 'Registrar Cliente') {
                    if (IsEmpty() === true) {
                      ClearClientInputs();
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
                          ClearClientInputs();
                          CloseModal();
                        }
                      })
                    }
                  } else if (modalTitle === 'Actualizar Cliente') {
                    Swal.fire({
                      icon: 'warning',
                      title: '¿Está seguro de que desea cerrar el formulario?',
                      text: "Se perderán todos los datos modificados",
                      confirmButtonText: 'Aceptar',
                      showCancelButton: true,
                      cancelButtonText: 'Cancelar',
                      confirmButtonColor: '#f8bb86',
                      cancelButtonColor: '#6c757d',
                    }).then((result) => {
                      if (result.isConfirmed) {
                        ClearClientInputs();
                        CloseModal();
                      }
                    })
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


    </div>
  );
}

export default Cliente;