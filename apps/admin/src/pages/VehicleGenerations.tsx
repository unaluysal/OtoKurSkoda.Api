import { useState, useEffect } from "react";
import type { VehicleGeneration, CreateVehicleGenerationRequest, UpdateVehicleGenerationRequest } from "../services/vehicleGenerationService";
import { vehicleGenerationService } from "../services/vehicleGenerationService";
import type { VehicleModel } from "../services/vehicleModelService";
import { vehicleModelService } from "../services/vehicleModelService";
import type { Brand } from "../services/brandService";
import { brandService } from "../services/brandService";

export default function VehicleGenerations() {
  const [generations, setGenerations] = useState<VehicleGeneration[]>([]);
  const [models, setModels] = useState<VehicleModel[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Filtreler
  const [selectedBrandId, setSelectedBrandId] = useState<string>("");
  const [selectedModelId, setSelectedModelId] = useState<string>("");
  const [filteredModels, setFilteredModels] = useState<VehicleModel[]>([]);

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editingGeneration, setEditingGeneration] = useState<VehicleGeneration | null>(null);
  const [formData, setFormData] = useState({
    vehicleModelId: "",
    name: "",
    code: "",
    startYear: new Date().getFullYear(),
    endYear: "" as number | "",
    imageUrl: "",
    description: "",
  });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  // Marka değiştiğinde modelleri filtrele
  useEffect(() => {
    if (selectedBrandId) {
      const filtered = models.filter((m) => m.brandId === selectedBrandId);
      setFilteredModels(filtered);
      setSelectedModelId("");
    } else {
      setFilteredModels(models);
      setSelectedModelId("");
    }
  }, [selectedBrandId, models]);

  // Model değiştiğinde nesilleri yükle
  useEffect(() => {
    if (selectedModelId) {
      loadGenerationsByModel(selectedModelId);
    } else if (selectedBrandId) {
      // Markaya göre tüm nesilleri filtrele
      loadAllGenerations();
    } else {
      loadAllGenerations();
    }
  }, [selectedModelId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [generationsResult, modelsResult, brandsResult] = await Promise.all([
        vehicleGenerationService.getAll(),
        vehicleModelService.getAll(),
        brandService.getAll(),
      ]);

      if (generationsResult.Status) {
        setGenerations(generationsResult.Data?.Items || []);
      }
      if (modelsResult.Status) {
        setModels(modelsResult.Data?.Items || []);
        setFilteredModels(modelsResult.Data?.Items || []);
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

  const loadAllGenerations = async () => {
    try {
      const result = await vehicleGenerationService.getAll();
      if (result.Status) {
        let items = result.Data?.Items || [];
        // Marka filtresi varsa uygula
        if (selectedBrandId) {
          items = items.filter((g) => {
            const model = models.find((m) => m.id === g.vehicleModelId);
            return model?.brandId === selectedBrandId;
          });
        }
        setGenerations(items);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Nesiller yüklenirken hata oluştu");
    }
  };

  const loadGenerationsByModel = async (modelId: string) => {
    try {
      setLoading(true);
      const result = await vehicleGenerationService.getByModelId(modelId);
      if (result.Status) {
        setGenerations(result.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Nesiller yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setEditingGeneration(null);
    setFormData({
      vehicleModelId: selectedModelId || (filteredModels.length > 0 ? filteredModels[0].id : ""),
      name: "",
      code: "",
      startYear: new Date().getFullYear(),
      endYear: "",
      imageUrl: "",
      description: "",
    });
    setShowModal(true);
  };

  const openEditModal = async (generation: VehicleGeneration) => {
    try {
      const result = await vehicleGenerationService.getById(generation.id);
      if (result.Status) {
        const data = result.Data;
        setEditingGeneration(data);
        setFormData({
          vehicleModelId: data.vehicleModelId,
          name: data.name,
          code: data.code || "",
          startYear: data.startYear,
          endYear: data.endYear || "",
          imageUrl: data.imageUrl || "",
          description: data.description || "",
        });
        setShowModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Nesil detayı yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingGeneration) {
        const request: UpdateVehicleGenerationRequest = {
          id: editingGeneration.id,
          vehicleModelId: formData.vehicleModelId,
          name: formData.name,
          code: formData.code || undefined,
          startYear: formData.startYear,
          endYear: formData.endYear || undefined,
          imageUrl: formData.imageUrl || undefined,
          description: formData.description || undefined,
        };
        const result = await vehicleGenerationService.update(request);
        if (result.Status) {
          setShowModal(false);
          selectedModelId ? loadGenerationsByModel(selectedModelId) : loadAllGenerations();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateVehicleGenerationRequest = {
          vehicleModelId: formData.vehicleModelId,
          name: formData.name,
          code: formData.code || undefined,
          startYear: formData.startYear,
          endYear: formData.endYear || undefined,
          imageUrl: formData.imageUrl || undefined,
          description: formData.description || undefined,
        };
        const result = await vehicleGenerationService.create(request);
        if (result.Status) {
          setShowModal(false);
          selectedModelId ? loadGenerationsByModel(selectedModelId) : loadAllGenerations();
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
    if (!confirm("Bu nesili silmek istediğinizden emin misiniz?")) return;

    try {
      const result = await vehicleGenerationService.delete(id);
      if (result.Status) {
        selectedModelId ? loadGenerationsByModel(selectedModelId) : loadAllGenerations();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme işlemi sırasında hata oluştu");
    }
  };

  // Görüntülenecek nesilleri filtrele
  const displayedGenerations = selectedBrandId && !selectedModelId
    ? generations.filter((g) => {
        const model = models.find((m) => m.id === g.vehicleModelId);
        return model?.brandId === selectedBrandId;
      })
    : generations;

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Araç Nesilleri</h1>
        <p className="text-gray-600">Araç nesillerini (kasa tiplerini) yönetin</p>
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
            <h2 className="font-semibold">Nesil Listesi</h2>
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
            <select
              value={selectedModelId}
              onChange={(e) => setSelectedModelId(e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-green-500 focus:border-transparent"
              disabled={filteredModels.length === 0}
            >
              <option value="">Tüm Modeller</option>
              {filteredModels.map((model) => (
                <option key={model.id} value={model.id}>
                  {model.name}
                </option>
              ))}
            </select>
          </div>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Nesil
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : displayedGenerations.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henüz nesil bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium w-20">Görsel</th>
                  <th className="pb-3 font-medium">Nesil Adı</th>
                  <th className="pb-3 font-medium">Kasa Kodu</th>
                  <th className="pb-3 font-medium">Model</th>
                  <th className="pb-3 font-medium">Marka</th>
                  <th className="pb-3 font-medium">Yıl Aralığı</th>
                  <th className="pb-3 font-medium">Durum</th>
                  <th className="pb-3 font-medium">İşlemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {displayedGenerations.map((generation) => (
                  <tr key={generation.id} className="border-b border-gray-100">
                    <td className="py-4">
                      {generation.imageUrl ? (
                        <img
                          src={generation.imageUrl}
                          alt={generation.name}
                          className="h-10 w-16 object-cover rounded"
                        />
                      ) : (
                        <div className="h-10 w-16 bg-gray-100 rounded flex items-center justify-center text-gray-400 text-xs">
                          —
                        </div>
                      )}
                    </td>
                    <td className="py-4 font-medium">{generation.name}</td>
                    <td className="py-4">
                      {generation.code ? (
                        <span className="px-2 py-1 bg-gray-100 rounded text-xs font-mono">
                          {generation.code}
                        </span>
                      ) : (
                        <span className="text-gray-400">—</span>
                      )}
                    </td>
                    <td className="py-4 text-gray-600">{generation.vehicleModelName}</td>
                    <td className="py-4 text-gray-600">{generation.brandName}</td>
                    <td className="py-4">
                      <span className="text-gray-700">{generation.yearRange}</span>
                    </td>
                    <td className="py-4">
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-medium ${
                          generation.status
                            ? "bg-green-100 text-green-700"
                            : "bg-gray-100 text-gray-600"
                        }`}
                      >
                        {generation.status ? "Aktif" : "Pasif"}
                      </span>
                    </td>
                    <td className="py-4">
                      <button
                        onClick={() => openEditModal(generation)}
                        className="text-blue-600 hover:text-blue-800 mr-3"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDelete(generation.id)}
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
          <div className="bg-white rounded-xl p-6 w-full max-w-lg">
            <h3 className="text-xl font-bold mb-4">
              {editingGeneration ? "Nesil Düzenle" : "Yeni Nesil Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Araç Modeli *
                </label>
                <select
                  value={formData.vehicleModelId}
                  onChange={(e) => setFormData({ ...formData, vehicleModelId: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  required
                >
                  <option value="">Model Seçin</option>
                  {models.map((model) => (
                    <option key={model.id} value={model.id}>
                      {model.brandName} - {model.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Nesil Adı *
                  </label>
                  <input
                    type="text"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Örn: Octavia 3"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Kasa Kodu
                  </label>
                  <input
                    type="text"
                    value={formData.code}
                    onChange={(e) => setFormData({ ...formData, code: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Örn: 5E"
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Başlangıç Yılı *
                  </label>
                  <input
                    type="number"
                    value={formData.startYear}
                    onChange={(e) =>
                      setFormData({ ...formData, startYear: parseInt(e.target.value) || 2000 })
                    }
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    min={1950}
                    max={2030}
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Bitiş Yılı
                  </label>
                  <input
                    type="number"
                    value={formData.endYear}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        endYear: e.target.value ? parseInt(e.target.value) : "",
                      })
                    }
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Devam ediyorsa boş bırakın"
                    min={1950}
                    max={2030}
                  />
                </div>
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
                  placeholder="https://example.com/generation.jpg"
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
                  placeholder="Nesil hakkında kısa açıklama"
                  rows={3}
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