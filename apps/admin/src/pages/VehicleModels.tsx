import { useState, useEffect } from "react";
import type { VehicleModel, CreateVehicleModelRequest, UpdateVehicleModelRequest } from "../services/vehicleModelService";
import { vehicleModelService } from "../services/vehicleModelService";
import type { Brand } from "../services/brandService";
import { brandService } from "../services/brandService";

export default function VehicleModels() {
  const [models, setModels] = useState<VehicleModel[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Filtre
  const [selectedBrandId, setSelectedBrandId] = useState<string>("");

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editingModel, setEditingModel] = useState<VehicleModel | null>(null);
  const [formData, setFormData] = useState({
    brandId: "",
    name: "",
    imageUrl: "",
    description: "",
    displayOrder: 0,
  });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (selectedBrandId) {
      loadModelsByBrand(selectedBrandId);
    } else {
      loadAllModels();
    }
  }, [selectedBrandId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [modelsResult, brandsResult] = await Promise.all([
        vehicleModelService.getAll(),
        brandService.getAll(),
      ]);

      if (modelsResult.Status) {
        setModels(modelsResult.Data?.Items || []);
      }
      if (brandsResult.Status) {
        setBrands(brandsResult.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Veriler yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const loadAllModels = async () => {
    try {
      const result = await vehicleModelService.getAll();
      if (result.Status) {
        setModels(result.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Modeller yüklenirken hata oluştu");
    }
  };

  const loadModelsByBrand = async (brandId: string) => {
    try {
      setLoading(true);
      const result = await vehicleModelService.getByBrandId(brandId);
      if (result.Status) {
        setModels(result.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Modeller yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setEditingModel(null);
    setFormData({
      brandId: selectedBrandId || (brands.length > 0 ? brands[0].id : ""),
      name: "",
      imageUrl: "",
      description: "",
      displayOrder: 0,
    });
    setShowModal(true);
  };

  const openEditModal = async (model: VehicleModel) => {
    try {
      const result = await vehicleModelService.getById(model.id);
      if (result.Status) {
        const data = result.Data;
        setEditingModel(data);
        setFormData({
          brandId: data.brandId,
          name: data.name,
          imageUrl: data.imageUrl || "",
          description: data.description || "",
          displayOrder: data.displayOrder,
        });
        setShowModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Model detayı yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingModel) {
        const request: UpdateVehicleModelRequest = {
          id: editingModel.id,
          brandId: formData.brandId,
          name: formData.name,
          imageUrl: formData.imageUrl || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await vehicleModelService.update(request);
        if (result.Status) {
          setShowModal(false);
          selectedBrandId ? loadModelsByBrand(selectedBrandId) : loadAllModels();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateVehicleModelRequest = {
          brandId: formData.brandId,
          name: formData.name,
          imageUrl: formData.imageUrl || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await vehicleModelService.create(request);
        if (result.Status) {
          setShowModal(false);
          selectedBrandId ? loadModelsByBrand(selectedBrandId) : loadAllModels();
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
    if (!confirm("Bu modeli silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await vehicleModelService.delete(id);
      if (result.Status) {
        selectedBrandId ? loadModelsByBrand(selectedBrandId) : loadAllModels();
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
        <h1 className="text-2xl font-bold text-gray-900">Araç Modelleri</h1>
        <p className="text-gray-600">Araç modellerini yönetin</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">×</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <div className="flex items-center gap-4">
            <h2 className="font-semibold">Model Listesi</h2>
            <select
              value={selectedBrandId}
              onChange={(e) => setSelectedBrandId(e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-green-500 focus:border-transparent"
            >
              <option value="">Tüm Markalar</option>
              {brands.map((brand) => (
                <option key={brand.id} value={brand.id}>
                  {brand.name}
                </option>
              ))}
            </select>
          </div>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Model
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : models.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz model bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium w-16">Sıra</th>
                  <th className="pb-3 font-medium w-20">Görsel</th>
                  <th className="pb-3 font-medium">Model Adı</th>
                  <th className="pb-3 font-medium">Marka</th>
                  <th className="pb-3 font-medium">Slug</th>
                  <th className="pb-3 font-medium">Nesil Sayısı</th>
                  <th className="pb-3 font-medium">Durum</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {models.map((model) => (
                  <tr key={model.id} className="border-b border-gray-100">
                    <td className="py-4 font-medium">{model.displayOrder}</td>
                    <td className="py-4">
                      {model.imageUrl ? (
                        <img
                          src={model.imageUrl}
                          alt={model.name}
                          className="h-10 w-14 object-cover rounded"
                        />
                      ) : (
                        <div className="h-10 w-14 bg-gray-100 rounded flex items-center justify-center text-gray-400 text-xs">
                          —
                        </div>
                      )}
                    </td>
                    <td className="py-4 font-medium">{model.name}</td>
                    <td className="py-4 text-gray-600">{model.brandName}</td>
                    <td className="py-4 text-gray-500">{model.slug}</td>
                    <td className="py-4">{model.generationCount}</td>
                    <td className="py-4">
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-medium ${
                          model.status
                            ? "bg-green-100 text-green-700"
                            : "bg-gray-100 text-gray-600"
                        }`}
                      >
                        {model.status ? "Aktif" : "Pasif"}
                      </span>
                    </td>
                    <td className="py-4">
                      <button
                        onClick={() => openEditModal(model)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDelete(model.id)}
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
              {editingModel ? "Model Düzenle" : "Yeni Model Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Marka *
                </label>
                <select
                  value={formData.brandId}
                  onChange={(e) => setFormData({ ...formData, brandId: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                >
                  <option value="">Marka Seçin</option>
                  {brands.map((brand) => (
                    <option key={brand.id} value={brand.id}>
                      {brand.name}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Model Adı *
                </label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Örn: Octavia"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Görsel URL
                </label>
                <input
                  type="text"
                  value={formData.imageUrl}
                  onChange={(e) => setFormData({ ...formData, imageUrl: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="https://example.com/model.jpg"
                />
                {formData.imageUrl && (
                  <div className="mt-2">
                    <img
                      src={formData.imageUrl}
                      alt="Görsel önizleme"
                      className="h-16 w-24 object-cover border rounded"
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
                  placeholder="Model hakkında kısa açıklama"
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