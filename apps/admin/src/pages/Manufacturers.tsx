import { useState, useEffect } from "react";
import type { Manufacturer, CreateManufacturerRequest, UpdateManufacturerRequest } from "../services/manufacturerService";
import { manufacturerService } from "../services/manufacturerService";

export default function Manufacturers() {
  const [manufacturers, setManufacturers] = useState<Manufacturer[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [showModal, setShowModal] = useState(false);
  const [editingManufacturer, setEditingManufacturer] = useState<Manufacturer | null>(null);
  const [formData, setFormData] = useState({
    name: "",
    logoUrl: "",
    website: "",
    description: "",
    displayOrder: 0,
  });
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadManufacturers();
  }, []);

  const loadManufacturers = async () => {
    try {
      setLoading(true);
      const result = await manufacturerService.getAll();
      if (result.Status) {
        setManufacturers(result.Data?.Items || []);
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Ureticiler yuklenirken hata olustu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setEditingManufacturer(null);
    setFormData({ name: "", logoUrl: "", website: "", description: "", displayOrder: 0 });
    setShowModal(true);
  };

  const openEditModal = async (manufacturer: Manufacturer) => {
    try {
      const result = await manufacturerService.getById(manufacturer.id);
      if (result.Status) {
        const data = result.Data;
        setEditingManufacturer(data);
        setFormData({
          name: data.name,
          logoUrl: data.logoUrl || "",
          website: data.website || "",
          description: data.description || "",
          displayOrder: data.displayOrder,
        });
        setShowModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Uretici detayi yuklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingManufacturer) {
        const request: UpdateManufacturerRequest = {
          id: editingManufacturer.id,
          name: formData.name,
          logoUrl: formData.logoUrl || undefined,
          website: formData.website || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await manufacturerService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadManufacturers();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateManufacturerRequest = {
          name: formData.name,
          logoUrl: formData.logoUrl || undefined,
          website: formData.website || undefined,
          description: formData.description || undefined,
          displayOrder: formData.displayOrder,
        };
        const result = await manufacturerService.create(request);
        if (result.Status) {
          setShowModal(false);
          loadManufacturers();
        } else {
          setError(result.Message);
        }
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Islem sirasinda hata olustu");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Bu ureticiyi silmek istediginizden emin misiniz?")) return;

    try {
      const result = await manufacturerService.delete(id);
      if (result.Status) {
        loadManufacturers();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme islemi sirasinda hata olustu");
    }
  };

  const formatWebsite = (url: string) => {
    return url.replace(/^https?:\/\//, "").replace(/\/$/, "");
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Ureticiler</h1>
        <p className="text-gray-600">Yedek parca ureticilerini yonetin (Bosch, Mann, Valeo vb.)</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm">
          {error}
          <button onClick={() => setError("")} className="float-right font-bold">X</button>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200 flex justify-between items-center">
          <h2 className="font-semibold">Uretici Listesi</h2>
          <button
            onClick={openCreateModal}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Uretici
          </button>
        </div>
        <div className="p-6">
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yukleniyor...</div>
          ) : manufacturers.length === 0 ? (
            <div className="text-center py-8 text-gray-500">Henuz uretici bulunmuyor</div>
          ) : (
            <table className="w-full">
              <thead>
                <tr className="text-left text-gray-500 text-sm border-b">
                  <th className="pb-3 font-medium w-16">Sira</th>
                  <th className="pb-3 font-medium w-20">Logo</th>
                  <th className="pb-3 font-medium">Uretici Adi</th>
                  <th className="pb-3 font-medium">Website</th>
                  <th className="pb-3 font-medium">Urun Sayisi</th>
                  <th className="pb-3 font-medium">Durum</th>
                  <th className="pb-3 font-medium">Islemler</th>
                </tr>
              </thead>
              <tbody className="text-sm">
                {manufacturers.map((manufacturer) => (
                  <tr key={manufacturer.id} className="border-b border-gray-100">
                    <td className="py-4 font-medium">{manufacturer.displayOrder}</td>
                    <td className="py-4">
                      {manufacturer.logoUrl ? (
                        <img
                          src={manufacturer.logoUrl}
                          alt={manufacturer.name}
                          className="h-10 w-10 object-contain rounded"
                        />
                      ) : (
                        <div className="h-10 w-10 bg-gray-100 rounded flex items-center justify-center text-gray-400 text-xs">
                          -
                        </div>
                      )}
                    </td>
                    <td className="py-4 font-medium">{manufacturer.name}</td>
                    <td className="py-4">
                      {manufacturer.website ? (
                        <a href={manufacturer.website} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:text-blue-800 text-sm">
                          {formatWebsite(manufacturer.website)}
                        </a>
                      ) : (
                        <span className="text-gray-400">-</span>
                      )}
                    </td>
                    <td className="py-4">{manufacturer.productCount}</td>
                    <td className="py-4">
                      <span className={`px-2 py-1 rounded-full text-xs font-medium ${manufacturer.status ? "bg-green-100 text-green-700" : "bg-gray-100 text-gray-600"}`}>
                        {manufacturer.status ? "Aktif" : "Pasif"}
                      </span>
                    </td>
                    <td className="py-4">
                      <button onClick={() => openEditModal(manufacturer)} className="text-blue-600 hover:text-blue-800 mr-3">
                        Duzenle
                      </button>
                      <button onClick={() => handleDelete(manufacturer.id)} className="text-red-600 hover:text-red-800">
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
          <div className="bg-white rounded-xl p-6 w-full max-w-md">
            <h3 className="text-xl font-bold mb-4">
              {editingManufacturer ? "Uretici Duzenle" : "Yeni Uretici Olustur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Uretici Adi *</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Orn: Bosch"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Logo URL</label>
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
                      alt="Logo onizleme"
                      className="h-12 w-12 object-contain border rounded"
                      onError={(e) => {
                        (e.target as HTMLImageElement).style.display = "none";
                      }}
                    />
                  </div>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Website</label>
                <input
                  type="url"
                  value={formData.website}
                  onChange={(e) => setFormData({ ...formData, website: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="https://www.bosch.com"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Aciklama</label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Uretici hakkinda kisa aciklama"
                  rows={3}
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Siralama</label>
                <input
                  type="number"
                  value={formData.displayOrder}
                  onChange={(e) => setFormData({ ...formData, displayOrder: parseInt(e.target.value) || 0 })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                />
              </div>

              <div className="flex gap-3 pt-4">
                <button
                  type="button"
                  onClick={() => setShowModal(false)}
                  className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition"
                >
                  Iptal
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