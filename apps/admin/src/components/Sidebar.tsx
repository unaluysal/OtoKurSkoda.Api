import { authService } from "../services/authService";

interface SidebarProps {
  activeMenu: string;
  setActiveMenu: (menu: string) => void;
  onLogout: () => void;
}

export default function Sidebar({ activeMenu, setActiveMenu, onLogout }: SidebarProps) {
  const user = authService.getUser();

  return (
    <aside className="w-64 bg-gray-900 text-white flex flex-col">
      {/* Logo */}
      <div className="p-6 border-b border-gray-800">
        <h1 className="text-xl font-bold text-green-400">OtoKur Admin</h1>
        <p className="text-gray-500 text-sm">Yönetim Paneli</p>
      </div>

      {/* Menu */}
      <nav className="flex-1 p-4">
        <p className="text-gray-500 text-xs font-semibold uppercase tracking-wider mb-4">
          Kullanıcı Yönetimi
        </p>
        <ul className="space-y-2">
          <li>
            <button
              onClick={() => setActiveMenu("users")}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-lg transition ${
                activeMenu === "users"
                  ? "bg-green-600 text-white"
                  : "text-gray-400 hover:bg-gray-800 hover:text-white"
              }`}
            >
              <span className="text-lg">👤</span>
              <span>Kullanıcılar</span>
            </button>
          </li>
          <li>
            <button
              onClick={() => setActiveMenu("roles")}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-lg transition ${
                activeMenu === "roles"
                  ? "bg-green-600 text-white"
                  : "text-gray-400 hover:bg-gray-800 hover:text-white"
              }`}
            >
              <span className="text-lg">🔑</span>
              <span>Roller</span>
            </button>
          </li>
          <li>
            <button
              onClick={() => setActiveMenu("role-groups")}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-lg transition ${
                activeMenu === "role-groups"
                  ? "bg-green-600 text-white"
                  : "text-gray-400 hover:bg-gray-800 hover:text-white"
              }`}
            >
              <span className="text-lg">👥</span>
              <span>Rol Grupları</span>
            </button>
          </li>
        </ul>
      </nav>

      {/* User */}
      <div className="p-4 border-t border-gray-800">
        <div className="flex items-center gap-3 mb-4">
          <div className="w-10 h-10 bg-green-600 rounded-full flex items-center justify-center font-bold">
            {user?.firstName?.charAt(0) || "U"}
          </div>
          <div>
            <p className="font-medium text-sm">{user?.fullName || "Kullanıcı"}</p>
            <p className="text-gray-500 text-xs">{user?.email || ""}</p>
          </div>
        </div>
        <button
          onClick={onLogout}
          className="w-full flex items-center justify-center gap-2 px-4 py-2 bg-red-600 hover:bg-red-700 rounded-lg transition text-sm"
        >
          <span>🚪</span>
          <span>Çıkış Yap</span>
        </button>
      </div>
    </aside>
  );
}