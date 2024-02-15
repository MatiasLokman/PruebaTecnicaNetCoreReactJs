import './styles/global.css';

import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';

import Gestion from './pages/Gestion/Gestion';
import Cliente from './pages/Cliente/Cliente';
import Movimiento from './pages/Movimiento/Movimiento';
import NotFound from './pages/NotFound/NotFound';

function App() {
  return (
    <BrowserRouter>
      <Header />

      <Routes>
        <Route index element={<Gestion />} />
        <Route path='/' element={<Gestion />} />
        <Route path='clientes' element={<Cliente />} />
        <Route path='movimientos/:id' element={<Movimiento />} />
        <Route path='*' element={<NotFound />} />
      </Routes>

      <Footer />
    </BrowserRouter>
  );
}

export default App;