import { useState, useEffect } from "react";
import Sidebar from "./components/Sidebar";
import Roles from "./pages/Roles";
import RoleGroups from "./pages/RoleGroups";
import Users from "./pages/Users";
import Login from "./pages/Login";
import Brands from "./pages/Brands";
import VehicleModels from "./pages/VehicleModels";
import VehicleGenerations from "./pages/VehicleGenerations";
import Categories from "./pages/Categories";
import Manufacturers from "./pages/Manufacturers";



import { authService } from "./services/authService";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [activeMenu, setActiveMenu] = useState("users");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const checkAuth = () => {
      const loggedIn = authService.isLoggedIn();
      setIsLoggedIn(loggedIn);
      setLoading(false);
    };
    checkAuth();
  }, []);

  const handleLogout = async () => {
    await authService.logout();
    setIsLoggedIn(false);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-900 flex items-center justify-center">
        <div className="text-green-400 text-xl">Yükleniyor...</div>
      </div>
    );
  }

  if (!isLoggedIn) {
    return <Login onLogin={() => setIsLoggedIn(true)} />;
  }

  return (
    <div className="flex min-h-screen bg-gray-100">
      <Sidebar
        activeMenu={activeMenu}
        setActiveMenu={setActiveMenu}
        onLogout={handleLogout}
      />

      <main className="flex-1 p-8">
        {activeMenu === "users" && <Users />}
        {activeMenu === "roles" && <Roles />}
        {activeMenu === "role-groups" && <RoleGroups />}
        {activeMenu === "brands" && <Brands />}
        {activeMenu === "vehicle-models" && <VehicleModels />}
        {activeMenu === "vehicle-generations" && <VehicleGenerations />}
        {activeMenu === "categories" && <Categories />}
        {activeMenu === "manufacturers" && <Manufacturers />}





      </main>
    </div>
  );
}

export default App;