import { NavLink } from 'react-router-dom';
import controlGlobalLogo from '../../assets/svgs/controlglobal.svg';

import './Header.css';

const Header = () => {
  return (
    <div className="global-header-footer-container">
      <div className='navbar-container'>

        <div className='navbar-left'>
          <img src={controlGlobalLogo} className="logo" alt="Control Global logo" />
        </div>

        <nav className='navbar-right'>
          <ul className='navbar-ul'>
            <li className='navbar-li'><NavLink className="navbar-li-text" to="/" aria-label="Gestión">Gestión</NavLink></li>
            {/* <li className='navbar-li'><NavLink className="navbar-li-text" to="/clientes" aria-label="Clientes">Clientes</NavLink></li>
            <li className='navbar-li'><NavLink className="navbar-li-text" to="/movimientos" aria-label="Movimientos">Movimientos</NavLink></li> */}
          </ul>
        </nav >
      </div>

    </div>
  );
}

export default Header;