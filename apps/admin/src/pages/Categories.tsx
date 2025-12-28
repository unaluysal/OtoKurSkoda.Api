import { useState, useEffect } from "react";
import type { Category, CategoryTree, CreateCategoryRequest, UpdateCategoryRequest } from "../services/categoryService";
import { categoryService } from "../services/categoryService";

export default function Categories() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [categoryTree, setCategoryTree] = useState<CategoryTree[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Görünüm modu
  const [viewMode, setViewMode] = useState<"list" | "tree">("tree");

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const [formData, setFormData] = useState({
    parentId: "" as string | "",
    name: "",
    description: "",
    imageUrl: "",
    iconClass: "",
    displayOrder: 0,
    metaTitle: "",
    metaDescription: "",
    metaKeywords: "",
  });
  const [saving, setSaving] = useState(false);
  const [showSeoFields, setShowSeoFields] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [listResult, treeResult] = await Promise.all([
        categoryService.getAll(),
        categoryService.getTree(),
      ]);

      if (listResult.Status) {
        setCategories(listResult.Data?.Items || []);
      }
      if (treeResult.Status) {
        setCategoryTree(treeResult.Data?.Items || []);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Veriler yüklenirken hata oluştu");
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = (parentId?: string) => {
    setEditingCategory(null);
    setFormData({
      parentId: parentId || "",
      name: "",
      description: "",
      imageUrl: "",
      iconClass: "",
      displayOrder: 0,
      metaTitle: "",
      metaDescription: "",
      metaKeywords: "",
    });
    setShowSeoFields(false);
    setShowModal(true);
  };

  const openEditModal = async (category: Category) => {
    try {
      const result = await categoryService.getById(category.id);
      if (result.Status) {
        const data = result.Data;
        setEditingCategory(data);
        setFormData({
          parentId: data.parentId || "",
          name: data.name,
          description: data.description || "",
          imageUrl: data.imageUrl || "",
          iconClass: data.iconClass || "",
          displayOrder: data.displayOrder,
          metaTitle: data.metaTitle || "",
          metaDescription: data.metaDescription || "",
          metaKeywords: data.metaKeywords || "",
        });
        setShowSeoFields(!!(data.metaTitle || data.metaDescription || data.metaKeywords));
        setShowModal(true);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Kategori detayı yüklenemedi");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);

    try {
      if (editingCategory) {
        const request: UpdateCategoryRequest = {
          id: editingCategory.id,
          parentId: formData.parentId || undefined,
          name: formData.name,
          description: formData.description || undefined,
          imageUrl: formData.imageUrl || undefined,
          iconClass: formData.iconClass || undefined,
          displayOrder: formData.displayOrder,
          metaTitle: formData.metaTitle || undefined,
          metaDescription: formData.metaDescription || undefined,
          metaKeywords: formData.metaKeywords || undefined,
        };
        const result = await categoryService.update(request);
        if (result.Status) {
          setShowModal(false);
          loadData();
        } else {
          setError(result.Message);
        }
      } else {
        const request: CreateCategoryRequest = {
          parentId: formData.parentId || undefined,
          name: formData.name,
          description: formData.description || undefined,
          imageUrl: formData.imageUrl || undefined,
          iconClass: formData.iconClass || undefined,
          displayOrder: formData.displayOrder,
          metaTitle: formData.metaTitle || undefined,
          metaDescription: formData.metaDescription || undefined,
          metaKeywords: formData.metaKeywords || undefined,
        };
        const result = await categoryService.create(request);
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
    if (!confirm("Bu kategoriyi silmek istediğinizden emin misiniz? Alt kategoriler de silinecektir.")) return;

    try {
      const result = await categoryService.delete(id);
      if (result.Status) {
        loadData();
      } else {
        setError(result.Message);
      }
    } catch (err: any) {
      setError(err.response?.data?.Message || "Silme işlemi sırasında hata oluştu");
    }
  };

  // Ağaç görünümü için recursive component
  const TreeNode = ({ node, level = 0 }: { node: CategoryTree; level?: number }) => {
    const [expanded, setExpanded] = useState(true);
    const category = categories.find((c) => c.id === node.id);

    return (
      <div>
        <div
          className={`flex items-center gap-3 py-3 px-4 hover:bg-gray-50 border-b border-gray-100`}
          style={{ paddingLeft: `${level * 24 + 16}px` }}
        >
          {node.children.length > 0 ? (
            <button
              onClick={() => setExpanded(!expanded)}
              className="w-6 h-6 flex items-center justify-center text-gray-400 hover:text-gray-600"
            >
              {expanded ? "▼" : "▶"}
            </button>
          ) : (
            <span className="w-6 h-6 flex items-center justify-center text-gray-300">•</span>
          )}

          {node.iconClass && (
            <span className="text-lg">{node.iconClass}</span>
          )}

          <div className="flex-1">
            <span className="font-medium text-gray-900">{node.name}</span>
            <span className="text-gray-400 text-sm ml-2">/{node.slug}</span>
          </div>

          <span className="text-xs text-gray-500 bg-gray-100 px-2 py-1 rounded">
            {node.productCount} ürün
          </span>

          {node.children.length > 0 && (
            <span className="text-xs text-blue-600 bg-blue-50 px-2 py-1 rounded">
              {node.children.length} alt kategori
            </span>
          )}

          <span
            className={`px-2 py-1 rounded-full text-xs font-medium ${
              category?.status
                ? "bg-green-100 text-green-700"
                : "bg-gray-100 text-gray-600"
            }`}
          >
            {category?.status ? "Aktif" : "Pasif"}
          </span>

          <div className="flex items-center gap-2">
            <button
              onClick={() => openCreateModal(node.id)}
              className="text-green-600 hover:text-green-800 text-sm"
              title="Alt kategori ekle"
            >
              + Alt
            </button>
            <button
              onClick={() => category && openEditModal(category)}
              className="text-blue-600 hover:text-blue-800 text-sm"
            >
              Düzenle
            </button>
            <button
              onClick={() => handleDelete(node.id)}
              className="text-red-600 hover:text-red-800 text-sm"
            >
              Sil
            </button>
          </div>
        </div>

        {expanded && node.children.length > 0 && (
          <div>
            {node.children
              .sort((a, b) => a.displayOrder - b.displayOrder)
              .map((child) => (
                <TreeNode key={child.id} node={child} level={level + 1} />
              ))}
          </div>
        )}
      </div>
    );
  };

  // Üst kategori seçenekleri (kendisi ve alt kategorileri hariç)
  const getParentOptions = () => {
    if (!editingCategory) return categories;
    
    const excludeIds = new Set<string>();
    const addChildIds = (id: string) => {
      excludeIds.add(id);
      categories.filter((c) => c.parentId === id).forEach((c) => addChildIds(c.id));
    };
    addChildIds(editingCategory.id);
    
    return categories.filter((c) => !excludeIds.has(c.id));
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Kategoriler</h1>
        <p className="text-gray-600">Ürün kategorilerini yönetin</p>
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
            <h2 className="font-semibold">Kategori Listesi</h2>
            <div className="flex bg-gray-100 rounded-lg p-1">
              <button
                onClick={() => setViewMode("tree")}
                className={`px-3 py-1 rounded text-sm transition ${
                  viewMode === "tree"
                    ? "bg-white shadow text-gray-900"
                    : "text-gray-600 hover:text-gray-900"
                }`}
              >
                🌳 Ağaç
              </button>
              <button
                onClick={() => setViewMode("list")}
                className={`px-3 py-1 rounded text-sm transition ${
                  viewMode === "list"
                    ? "bg-white shadow text-gray-900"
                    : "text-gray-600 hover:text-gray-900"
                }`}
              >
                📋 Liste
              </button>
            </div>
          </div>
          <button
            onClick={() => openCreateModal()}
            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg font-medium transition"
          >
            + Yeni Kategori
          </button>
        </div>

        <div>
          {loading ? (
            <div className="text-center py-8 text-gray-500">Yükleniyor...</div>
          ) : viewMode === "tree" ? (
            categoryTree.length === 0 ? (
              <div className="text-center py-8 text-gray-500">Henüz kategori bulunmuyor</div>
            ) : (
              <div>
                {categoryTree
                  .sort((a, b) => a.displayOrder - b.displayOrder)
                  .map((node) => (
                    <TreeNode key={node.id} node={node} />
                  ))}
              </div>
            )
          ) : (
            <div className="p-6">
              {categories.length === 0 ? (
                <div className="text-center py-8 text-gray-500">Henüz kategori bulunmuyor</div>
              ) : (
                <table className="w-full">
                  <thead>
                    <tr className="text-left text-gray-500 text-sm border-b">
                      <th className="pb-3 font-medium w-16">Sıra</th>
                      <th className="pb-3 font-medium">Kategori Adı</th>
                      <th className="pb-3 font-medium">Üst Kategori</th>
                      <th className="pb-3 font-medium">Seviye</th>
                      <th className="pb-3 font-medium">Ürün</th>
                      <th className="pb-3 font-medium">Durum</th>
                      <th className="pb-3 font-medium">İşlemler</th>
                    </tr>
                  </thead>
                  <tbody className="text-sm">
                    {categories
                      .sort((a, b) => a.level - b.level || a.displayOrder - b.displayOrder)
                      .map((category) => (
                        <tr key={category.id} className="border-b border-gray-100">
                          <td className="py-4 font-medium">{category.displayOrder}</td>
                          <td className="py-4">
                            <span style={{ paddingLeft: `${category.level * 16}px` }}>
                              {category.level > 0 && <span className="text-gray-400 mr-2">└</span>}
                              <span className="font-medium">{category.name}</span>
                            </span>
                          </td>
                          <td className="py-4 text-gray-500">{category.parentName || "—"}</td>
                          <td className="py-4">
                            <span className="px-2 py-1 bg-gray-100 rounded text-xs">
                              Seviye {category.level}
                            </span>
                          </td>
                          <td className="py-4">{category.productCount}</td>
                          <td className="py-4">
                            <span
                              className={`px-2 py-1 rounded-full text-xs font-medium ${
                                category.status
                                  ? "bg-green-100 text-green-700"
                                  : "bg-gray-100 text-gray-600"
                              }`}
                            >
                              {category.status ? "Aktif" : "Pasif"}
                            </span>
                          </td>
                          <td className="py-4">
                            <button
                              onClick={() => openCreateModal(category.id)}
                              className="text-green-600 hover:text-green-800 mr-3"
                            >
                              + Alt
                            </button>
                            <button
                              onClick={() => openEditModal(category)}
                              className="text-blue-600 hover:text-blue-800 mr-3"
                            >
                              Düzenle
                            </button>
                            <button
                              onClick={() => handleDelete(category.id)}
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
          )}
        </div>
      </div>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-lg max-h-[90vh] overflow-auto">
            <h3 className="text-xl font-bold mb-4">
              {editingCategory ? "Kategori Düzenle" : "Yeni Kategori Oluştur"}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Üst Kategori
                </label>
                <select
                  value={formData.parentId}
                  onChange={(e) => setFormData({ ...formData, parentId: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                >
                  <option value="">Ana Kategori (Üst kategori yok)</option>
                  {getParentOptions().map((cat) => (
                    <option key={cat.id} value={cat.id}>
                      {"—".repeat(cat.level)} {cat.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Kategori Adı *
                  </label>
                  <input
                    type="text"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Örn: Motor Parçaları"
                    required
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
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    İkon (Emoji/Class)
                  </label>
                  <input
                    type="text"
                    value={formData.iconClass}
                    onChange={(e) => setFormData({ ...formData, iconClass: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Örn: 🔧 veya fa-cog"
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
                    placeholder="https://..."
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Açıklama
                </label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  placeholder="Kategori açıklaması"
                  rows={2}
                />
              </div>

              {/* SEO Alanları */}
              <div>
                <button
                  type="button"
                  onClick={() => setShowSeoFields(!showSeoFields)}
                  className="text-sm text-blue-600 hover:text-blue-800 flex items-center gap-1"
                >
                  {showSeoFields ? "▼" : "▶"} SEO Ayarları
                </button>

                {showSeoFields && (
                  <div className="mt-3 space-y-3 p-4 bg-gray-50 rounded-lg">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Meta Başlık
                      </label>
                      <input
                        type="text"
                        value={formData.metaTitle}
                        onChange={(e) => setFormData({ ...formData, metaTitle: e.target.value })}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                        placeholder="SEO başlığı"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Meta Açıklama
                      </label>
                      <textarea
                        value={formData.metaDescription}
                        onChange={(e) =>
                          setFormData({ ...formData, metaDescription: e.target.value })
                        }
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                        placeholder="SEO açıklaması"
                        rows={2}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-1">
                        Meta Anahtar Kelimeler
                      </label>
                      <input
                        type="text"
                        value={formData.metaKeywords}
                        onChange={(e) => setFormData({ ...formData, metaKeywords: e.target.value })}
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                        placeholder="anahtar, kelime, seo"
                      />
                    </div>
                  </div>
                )}
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