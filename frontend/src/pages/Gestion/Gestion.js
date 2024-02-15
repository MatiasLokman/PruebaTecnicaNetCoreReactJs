import { ReactComponent as Clientes } from "../../assets/svgs/clientes.svg";
// import { ReactComponent as Movimientos } from "../../assets/svgs/movimientos.svg";

import { Link } from 'react-router-dom';

import './Gestion.css';

const Gestion = () => {

  return (
    <div className='global-container'>
      <h1 className="text-white">Gestión de cuentas</h1>

      <div className='secciones'>
        <Link to="/clientes" className='btn btn-dark gestion-btn'>
          <Clientes className='gestion-svg' />
          <p className='gestion-title'>Clientes</p>
        </Link>

        {/* <Link to="/movimientos" className='btn btn-dark gestion-btn'>
          <Movimientos className='gestion-svg' />
          <p className='gestion-title'>Movimientos</p>
        </Link> */}
      </div>
    </div>

  );
}

export default Gestion;
