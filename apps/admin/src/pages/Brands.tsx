import { useState, useEffect } from "react";
import type { Brand, CreateBrandRequest, UpdateBrandRequest } from "../services/brandService";
import { brandService } from "../services/brandService";

export default function Brands() {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [showModal, setShowModal] = useState(false);
  const [editingBrand, setEditingBrand] = useState<Brand | null>(null);
  const [formData, setFormData] = useState({
    name: "",
    logoUrl: "",
    description: "",
    displayOrder: 0,
  });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadBrands();
  }, []);

  const loadBrands = async () => {
    try {
      setLoading(true);
      const result = await brandService.getAll();
      if (result.Status) {
        setBrands(result.Data?.Items || []);
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Markalar yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setEditingBrand(null);
    setFormData({ name: "", logoUrl: "", description: "", displayOrder: 0 });
    setShowModal(true);
  };

  const openEditModal = async (brand: Brand) => {
    try {
      const result = await brandService.getById(brand.id);
      if (result.Status) {
        const data = result.Data;
        setEditingBrand(data);
        setFormData({
          name: data.name,
          logoUrl: data.logoUrl || "",
          description: data.description || "",
          displayOrder: data.displayOrder,
        });
        setShowModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Marka detayı yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingBrand) {
        const request: UpdateBrandRequest = {
          id: editingBrand.id,
          name: formData.name,
          logoUrl: formData.logoUrl || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await brandService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadBrands();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateBrandRequest = {
          name: formData.name,
          logoUrl: formData.logoUrl || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await brandService.create(request);
        if (result.Status) {
          setShowModal(false);
          loadBrands();
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
    if (!confirm("Bu markayı silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await brandService.delete(id);
      if (result.Status) {
        loadBrands();
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
        <h1 className="text-2xl font-bold text-gray-900">Markalar</h1>
        <p className="text-gray-600">Araç markalarını yönetin</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">×</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <h2 className="font-semibold">Marka Listesi</h2>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Marka
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : brands.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz marka bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium w-16">Sıra</th>
                  <th className="pb-3 font-medium w-20">Logo</th>
                  <th className="pb-3 font-medium">Marka Adı</th>
                  <th className="pb-3 font-medium">Slug</th>
                  <th className="pb-3 font-medium">Model Sayısı</th>
                  <th className="pb-3 font-medium">Durum</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {brands.map((brand) => (
                  <tr key={brand.id} className="border-b border-gray-100">
                    <td className="py-4 font-medium">{brand.displayOrder}</td>
                    <td className="py-4">
                      {brand.logoUrl ? (
                        <img
                          src={brand.logoUrl}
                          alt={brand.name}
                          className="h-10 w-10 object-contain rounded"
                        />
                      ) : (
                        <div className="h-10 w-10 bg-gray-100 rounded flex items-center justify-center text-gray-400 text-xs">
                          —
                        </div>
                      )}
                    </td>
                    <td className="py-4 font-medium">{brand.name}</td>
                    <td className="py-4 text-gray-500">{brand.slug}</td>
                    <td className="py-4">{brand.vehicleModelCount}</td>
                    <td className="py-4">
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-medium ${
                          brand.status
                            ? "bg-green-100 text-green-700"
                            : "bg-gray-100 text-gray-600"
                        }`}
                      >
                        {brand.status ? "Aktif" : "Pasif"}
                      </span>
                    </td>
                    <td className="py-4">
                      <button
                        onClick={() => openEditModal(brand)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDelete(brand.id)}
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
              {editingBrand ? "Marka Düzenle" : "Yeni Marka Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Marka Adı *
                </label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Örn: Skoda"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Logo URL
                </label>
                <input
                  type="text"
                  value={formData.logoUrl}
                  onChange={(e) => setFormData({ ...formData, logoUrl: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="https://example.com/logo.png"
                />
                {formData.logoUrl && (
                  <div className="mt-2">
                    <img
                      src={formData.logoUrl}
                      alt="Logo önizleme"
                      className="h-12 w-12 object-contain border rounded"
                      onError={(e) => {
                        (e.target as HTMLImageElement).style.display = "none";
                      }}
                    />
                  </div>
                )}
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Açıklama
                </label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Marka hakkında kısa açıklama"
                  rows={3}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Sıralama
                </label>
                <input
                  type="number"
                  value={formData.displayOrder}
                  onChange={(e) =>
                    setFormData({ ...formData, displayOrder: parseInt(e.target.value) || 0 })
                  }
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
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