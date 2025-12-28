import api from "./api";

export interface Category {
  id: string;
  parentId: string | null;
  parentName: string | null;
  name: string;
  slug: string;
  description: string | null;
  imageUrl: string | null;
  iconClass: string | null;
  displayOrder: number;
  level: number;
  metaTitle: string | null;
  metaDescription: string | null;
  metaKeywords: string | null;
  status: boolean;
  childCount: number;
  productCount: number;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenantId: string;
}

export interface CategoryTree {
  id: string;
  name: string;
  slug: string;
  iconClass: string | null;
  displayOrder: number;
  level: number;
  productCount: number;
  children: CategoryTree[];
}

export interface CategoryListData {
  Items: Category[];
  Count: number;
}

export interface CategoryTreeData {
  Items: CategoryTree[];
  Count: number;
}

export interface CreateCategoryRequest {
  parentId?: string;
  name: string;
  description?: string;
  imageUrl?: string;
  iconClass?: string;
  displayOrder: number;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
}

export interface UpdateCategoryRequest {
  id: string;
  parentId?: string;
  name: string;
  description?: string;
  imageUrl?: string;
  iconClass?: string;
  displayOrder: number;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const categoryService = {
  async getAll(): Promise<ApiResponse<CategoryListData>> {
    const response = await api.post<ApiResponse<CategoryListData>>("/category/list");
    return response.data;
  },

  async getTree(): Promise<ApiResponse<CategoryTreeData>> {
    const response = await api.post<ApiResponse<CategoryTreeData>>("/category/tree");
    return response.data;
  },

  async getRootCategories(): Promise<ApiResponse<CategoryListData>> {
    const response = await api.post<ApiResponse<CategoryListData>>("/category/root");
    return response.data;
  },

  async getChildren(parentId: string): Promise<ApiResponse<CategoryListData>> {
    const response = await api.post<ApiResponse<CategoryListData>>(
      "/category/children",
      JSON.stringify(parentId),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<Category>> {
    const response = await api.post<ApiResponse<Category>>(
      "/category/get",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async create(request: CreateCategoryRequest): Promise<ApiResponse<Category>> {
    const response = await api.post<ApiResponse<Category>>("/category/add", request);
    return response.data;
  },

  async update(request: UpdateCategoryRequest): Promise<ApiResponse<Category>> {
    const response = await api.post<ApiResponse<Category>>("/category/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>(
      "/category/delete",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },
};