import { useState, useEffect } from "react";
import type { Role } from "../services/roleService";
import { roleService } from "../services/roleService";
import type { RoleGroup, CreateRoleGroupRequest, UpdateRoleGroupRequest } from "../services/roleGroupService";
import { roleGroupService } from "../services/roleGroupService";

export default function RoleGroups() {
  const [roleGroups, setRoleGroups] = useState<RoleGroup[]>([]);
  const [allRoles, setAllRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [showModal, setShowModal] = useState(false);
  const [showRolesModal, setShowRolesModal] = useState(false);
  const [editingGroup, setEditingGroup] = useState<RoleGroup | null>(null);
  const [selectedGroup, setSelectedGroup] = useState<RoleGroup | null>(null);
  const [formData, setFormData] = useState({ name: "", description: "" });
  const [selectedRoles, setSelectedRoles] = useState<string[]>([]);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

const loadData = async () => {
  try {
    setLoading(true);
    const [groupsResult, rolesResult] = await Promise.all([
      roleGroupService.getAll(),
      roleService.getAll(),
    ]);

    if (groupsResult.Status) {
      setRoleGroups(groupsResult.Data?.Items || []);
    }
    if (rolesResult.Status) {
      setAllRoles(rolesResult.Data?.Items || []);
    }
  } catch (err: any) {
    setError(err.response?.data?.Message || "Veriler yüklenirken hata oluştu");
  } finally {
    setLoading(false);
  }
};

  const openCreateModal = () => {
    setEditingGroup(null);
    setFormData({ name: "", description: "" });
    setShowModal(true);
  };

  const openEditModal = (group: RoleGroup) => {
    setEditingGroup(group);
    setFormData({ name: group.name, description: group.description });
    setShowModal(true);
  };

  const openRolesModal = async (group: RoleGroup) => {
    try {
      const result = await roleGroupService.getWithRoles(group.id);
      if (result.Status) {
        setSelectedGroup(result.Data);
        setSelectedRoles(result.Data.roles?.map((r) => r.id) || []);
        setShowRolesModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Rol grubu detayı yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingGroup) {
        const request: UpdateRoleGroupRequest = {
          id: editingGroup.id,
          name: formData.name,
          description: formData.description,
        };
        const result = await roleGroupService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadData();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateRoleGroupRequest = {
          name: formData.name,
          description: formData.description,
        };
        const result = await roleGroupService.create(request);
        if (result.Status) {
          setShowModal(false);
          loadData();
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

  const handleSaveRoles = async () => {
    if (!selectedGroup) return;
    setSaving(true);

    try {
      const result = await roleGroupService.assignMultipleRoles({
        roleGroupId: selectedGroup.id,
        roleIds: selectedRoles,
      });

      if (result.Status) {
        setShowRolesModal(false);
        loadData();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Roller kaydedilirken hata oluştu");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Bu rol grubunu silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await roleGroupService.delete(id);
      if (result.Status) {
        loadData();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme işlemi sırasında hata oluştu");
    }
  };

  const toggleRole = (roleId: string) => {
    setSelectedRoles((prev) =>
      prev.includes(roleId) ? prev.filter((id) => id !== roleId) : [...prev, roleId]
    );
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Rol Grupları</h1>
        <p className="text-gray-600">Kullanıcı rol gruplarını yönetin</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">×</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <h2 className="font-semibold">Rol Grubu Listesi</h2>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Grup
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : roleGroups.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz rol grubu bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium">Grup Adı</th>
                  <th className="pb-3 font-medium">Açıklama</th>
                  <th className="pb-3 font-medium">Oluşturulma</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {roleGroups.map((group) => (
                  <tr key={group.id} className="border-b border-gray-100">
                    <td className="py-4 font-medium">{group.name}</td>
                    <td className="py-4 text-gray-600">{group.description}</td>
                    <td className="py-4 text-gray-600">
                      {new Date(group.createTime).toLocaleDateString("tr-TR")}
                    </td>
                    <td className="py-4">
                      <button
                        onClick={() => openRolesModal(group)}
                        className="text-green-600 hover:text-green-800 mr-3"
                      >
                        Roller
                      </button>
                      <button
                        onClick={() => openEditModal(group)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDelete(group.id)}
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

      {/* Create/Edit Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-md">
            <h3 className="text-xl font-bold mb-4">
              {editingGroup ? "Rol Grubu Düzenle" : "Yeni Rol Grubu Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Grup Adı</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Örn: Admin"
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
                  placeholder="Grup açıklaması"
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

      {/* Roles Modal */}
      {showRolesModal && selectedGroup && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-lg max-h-[80vh] overflow-auto">
            <h3 className="text-xl font-bold mb-2">{selectedGroup.name} - Roller</h3>
            <p className="text-gray-600 text-sm mb-4">Bu gruba atanacak rolleri seçin</p>

            <div className="space-y-2 max-h-64 overflow-auto border rounded-lg p-3">
              {allRoles.length === 0 ? (
                <p className="text-gray-500 text-center py-4">Henüz rol tanımlanmamış</p>
              ) : (
                allRoles.map((role) => (
                  <label
                    key={role.id}
                    className="flex items-center gap-3 p-2 hover:bg-gray-50 rounded cursor-pointer"
                  >
                    <input
                      type="checkbox"
                      checked={selectedRoles.includes(role.id)}
                      onChange={() => toggleRole(role.id)}
                      className="w-4 h-4 text-green-600 rounded focus:ring-green-500"
                    />
                    <div>
                      <p className="font-medium text-sm">{role.name}</p>
                      <p className="text-gray-500 text-xs">{role.description}</p>
                    </div>
                  </label>
                ))
              )}
            </div>

            <div className="flex gap-3 pt-4">
              <button
                type="button"
                onClick={() => setShowRolesModal(false)}
                className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition"
              >
                İptal
              </button>
              <button
                onClick={handleSaveRoles}
                disabled={saving}
                className="flex-1 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:bg-green-400 transition"
              >
                {saving ? "Kaydediliyor..." : "Rolleri Kaydet"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}