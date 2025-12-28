import { useState, useEffect } from "react";
import type { Role, CreateRoleRequest, UpdateRoleRequest } from "../services/roleService";
import { roleService } from "../services/roleService";

export default function Roles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  
  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editingRole, setEditingRole] = useState<Role | null>(null);
  const [formData, setFormData] = useState({ name: "", description: "" });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadRoles();
  }, []);

  const loadRoles = async () => {
  try {
    setLoading(true);
    const result = await roleService.getAll();
    if (result.Status) {
      setRoles(result.Data?.Items || []);
    } else {
      setError(result.Message);
    }
  } catch (err: any) {
    setError(err.response?.data?.Message || "Roller yüklenirken hata oluştu");
  } finally {
    setLoading(false);
  }
};

  const openCreateModal = () => {
    setEditingRole(null);
    setFormData({ name: "", description: "" });
    setShowModal(true);
  };

  const openEditModal = (role: Role) => {
    setEditingRole(role);
    setFormData({ name: role.name, description: role.description });
    setShowModal(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingRole) {
        const request: UpdateRoleRequest = {
          id: editingRole.id,
          name: formData.name,
          description: formData.description,
        };
        const result = await roleService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadRoles();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateRoleRequest = {
          name: formData.name,
          description: formData.description,
        };
        const result = await roleService.create(request);
        if (result.Status) {
          setShowModal(false);
          loadRoles();
        } else {
          setError(result.Message);
        }
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "İşlem sırasında hata oluştu");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Bu rolü silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await roleService.delete(id);
      if (result.Status) {
        loadRoles();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme işlemi sırasında hata oluştu");
    }
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Roller</h1>
        <p className="text-gray-600">Sistem rollerini yönetin</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">×</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <h2 className="font-semibold">Rol Listesi</h2>
          <button 
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Rol
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : roles.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz rol bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium">Rol Adı</th>
                  <th className="pb-3 font-medium">Açıklama</th>
                  <th className="pb-3 font-medium">Oluşturulma</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {roles.map((role) => (
                  <tr key={role.id} className="border-b border-gray-100">
                    <td className="py-4 font-medium">{role.name}</td>
                    <td className="py-4 text-gray-600">{role.description}</td>
                    <td className="py-4 text-gray-600">
                      {new Date(role.createTime).toLocaleDateString("tr-TR")}
                    </td>
                    <td className="py-4">
                      <button 
                        onClick={() => openEditModal(role)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button 
                        onClick={() => handleDelete(role.id)}
                        className="text-red-600 hover:text-red-800"
                      >
                        Sil
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-md">
            <h3 className="text-xl font-bold mb-4">
              {editingRole ? "Rol Düzenle" : "Yeni Rol Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Rol Adı</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Örn: Products.Create"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Açıklama</label>
                <input
                  type="text"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Rol açıklaması"
                  required
                />
              </div>
              <div className="flex gap-3 pt-4">
                <button
                  type="button"
                  onClick={() => setShowModal(false)}
                  className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition"
                >
                  İptal
                </button>
                <button
                  type="submit"
                  disabled={saving}
                  className="flex-1 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:bg-green-400 transition"
                >
                  {saving ? "Kaydediliyor..." : "Kaydet"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}