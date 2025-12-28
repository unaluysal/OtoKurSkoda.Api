import { useState, useEffect } from "react";
import type { User, CreateUserRequest, UpdateUserRequest } from "../services/userService";
import { userService } from "../services/userService";
import type { RoleGroup } from "../services/roleGroupService";
import { roleGroupService } from "../services/roleGroupService";

export default function Users() {
  const [users, setUsers] = useState<User[]>([]);
  const [roleGroups, setRoleGroups] = useState<RoleGroup[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [showModal, setShowModal] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    phoneNumber: "",
    firstName: "",
    lastName: "",
    emailConfirmed: false,
    phoneConfirmed: false,
    roleGroupIds: [] as string[],
  });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [usersResult, roleGroupsResult] = await Promise.all([
        userService.getAll(),
        roleGroupService.getAll(),
      ]);

      if (usersResult.Status) {
        setUsers(usersResult.Data?.Items || []);
      }
      if (roleGroupsResult.Status) {
        setRoleGroups(roleGroupsResult.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Veriler yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setEditingUser(null);
    setFormData({
      email: "",
      password: "",
      phoneNumber: "",
      firstName: "",
      lastName: "",
      emailConfirmed: false,
      phoneConfirmed: false,
      roleGroupIds: [],
    });
    setShowModal(true);
  };

  const openEditModal = async (user: User) => {
    try {
      const result = await userService.getWithRoles(user.id);
      if (result.Status && result.Data) {
        const userData = result.Data;
        setEditingUser(userData);

        const userRoleGroupIds = roleGroups
          .filter((rg) => userData.roles.includes(rg.name))
          .map((rg) => rg.id);

        setFormData({
          email: userData.email,
          password: "",
          phoneNumber: userData.phoneNumber,
          firstName: userData.firstName,
          lastName: userData.lastName,
          emailConfirmed: userData.emailConfirmed,
          phoneConfirmed: userData.phoneConfirmed,
          roleGroupIds: userRoleGroupIds,
        });
        setShowModal(true);
      } else {
        setError(result.Message || "Kullanıcı bilgileri yüklenemedi");
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Kullanıcı bilgileri yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingUser) {
        const request: UpdateUserRequest = {
          id: editingUser.id,
          email: formData.email,
          password: formData.password || undefined,
          phoneNumber: formData.phoneNumber,
          firstName: formData.firstName,
          lastName: formData.lastName,
          emailConfirmed: formData.emailConfirmed,
          phoneConfirmed: formData.phoneConfirmed,
          roleGroupIds: formData.roleGroupIds,
        };
        const result = await userService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadData();
        } else {
          setError(result.Message);
        }
      } else {
        if (!formData.password) {
          setError("Şifre zorunludur");
          setSaving(false);
          return;
        }
        const request: CreateUserRequest = {
          email: formData.email,
          password: formData.password,
          phoneNumber: formData.phoneNumber,
          firstName: formData.firstName,
          lastName: formData.lastName,
          roleGroupIds: formData.roleGroupIds,
        };
        const result = await userService.create(request);
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

  const handleDelete = async (id: string) => {
    if (!confirm("Bu kullanıcıyı silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await userService.delete(id);
      if (result.Status) {
        loadData();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme işlemi sırasında hata oluştu");
    }
  };

  const toggleRoleGroup = (roleGroupId: string) => {
    setFormData((prev) => ({
      ...prev,
      roleGroupIds: prev.roleGroupIds.includes(roleGroupId)
        ? prev.roleGroupIds.filter((id) => id !== roleGroupId)
        : [...prev.roleGroupIds, roleGroupId],
    }));
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Kullanıcılar</h1>
        <p className="text-gray-600">Sistem kullanıcılarını yönetin</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">×</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <h2 className="font-semibold">Kullanıcı Listesi</h2>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Kullanıcı
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : users.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz kullanıcı bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium">Ad Soyad</th>
                  <th className="pb-3 font-medium">E-Posta</th>
                  <th className="pb-3 font-medium">Telefon</th>
                  <th className="pb-3 font-medium">Roller</th>
                  <th className="pb-3 font-medium">Son Giriş</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {users.map((user) => (
                  <tr key={user.id} className="border-b border-gray-100">
                    <td className="py-4">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 bg-green-600 rounded-full flex items-center justify-center text-white font-medium text-xs">
                          {user.firstName?.charAt(0)}{user.lastName?.charAt(0)}
                        </div>
                        <span className="font-medium">{user.firstName} {user.lastName}</span>
                      </div>
                    </td>
                    <td className="py-4 text-gray-600">{user.email}</td>
                    <td className="py-4 text-gray-600">{user.phoneNumber}</td>
                    <td className="py-4">
                      <div className="flex flex-wrap gap-1">
                        {user.roles.length > 0 ? (
                          user.roles.map((role, i) => (
                            <span key={i} className="bg-blue-100 text-blue-700 px-2 py-0.5 rounded text-xs">
                              {role}
                            </span>
                          ))
                        ) : (
                          <span className="text-gray-400 text-xs">Rol yok</span>
                        )}
                      </div>
                    </td>
                    <td className="py-4 text-gray-600">
                      {user.lastLoginAt
                        ? new Date(user.lastLoginAt).toLocaleString("tr-TR")
                        : "-"}
                    </td>
                    <td className="py-4">
                      <button
                        onClick={() => openEditModal(user)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDelete(user.id)}
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

      {showModal && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-lg max-h-[90vh] overflow-auto">
            <h3 className="text-xl font-bold mb-4">
              {editingUser ? "Kullanıcı Düzenle" : "Yeni Kullanıcı Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Ad</label>
                  <input
                    type="text"
                    value={formData.firstName}
                    onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Soyad</label>
                  <input
                    type="text"
                    value={formData.lastName}
                    onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    required
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">E-Posta</label>
                <input
                  type="email"
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Telefon</label>
                <input
                  type="tel"
                  value={formData.phoneNumber}
                  onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Şifre {editingUser && <span className="text-gray-400">(Değiştirmek için doldurun)</span>}
                </label>
                <input
                  type="password"
                  value={formData.password}
                  onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required={!editingUser}
                  placeholder={editingUser ? "••••••••" : ""}
                />
              </div>

              {editingUser && (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={formData.emailConfirmed}
                      onChange={(e) => setFormData({ ...formData, emailConfirmed: e.target.checked })}
                      className="w-4 h-4 text-green-600 rounded focus:ring-green-500"
                    />
                    <span className="text-sm text-gray-700">E-posta onaylı</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={formData.phoneConfirmed}
                      onChange={(e) => setFormData({ ...formData, phoneConfirmed: e.target.checked })}
                      className="w-4 h-4 text-green-600 rounded focus:ring-green-500"
                    />
                    <span className="text-sm text-gray-700">Telefon onaylı</span>
                  </label>
                </div>
              )}

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">Rol Grupları</label>
                <div className="space-y-2 max-h-40 overflow-auto border rounded-lg p-3">
                  {roleGroups.length === 0 ? (
                    <p className="text-gray-500 text-center py-2 text-sm">Henüz rol grubu tanımlanmamış</p>
                  ) : (
                    roleGroups.map((group) => (
                      <label
                        key={group.id}
                        className="flex items-center gap-3 p-2 hover:bg-gray-50 rounded cursor-pointer"
                      >
                        <input
                          type="checkbox"
                          checked={formData.roleGroupIds.includes(group.id)}
                          onChange={() => toggleRoleGroup(group.id)}
                          className="w-4 h-4 text-green-600 rounded focus:ring-green-500"
                        />
                        <div>
                          <p className="font-medium text-sm">{group.name}</p>
                          <p className="text-gray-500 text-xs">{group.description}</p>
                        </div>
                      </label>
                    ))
                  )}
                </div>
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