import { Routes, Route, NavLink } from "react-router-dom";
import MenuPage from "./features/menu/pages/MenuPage";
import DishDetailPage from "./features/menu/pages/DishDetailPage";
import OrderPage from "./features/menu/pages/OrderPage";

function App() {
  return (
    <div>
      <h1>Campus Eats Dashboard</h1>
      <nav>
        <NavLink to="/">Menu</NavLink> |{" "}
        <NavLink to="/order">Place Order</NavLink>
      </nav>

      <Routes>
        <Route path="/" element={<MenuPage />} />
        <Route path="/dish/:id" element={<DishDetailPage />} />
        <Route path="/order" element={<OrderPage />} />
        <Route path="*" element={<p>404 - Page Not Found</p>} />
      </Routes>
    </div>
  );
}

export default App;