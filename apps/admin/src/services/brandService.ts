import api from "./api";

export interface Brand {
  id: string;
  name: string;
  slug: string;
  logoUrl: string | null;
  description: string | null;
  displayOrder: number;
  status: boolean;
  vehicleModelCount: number;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenantId: string;
}

export interface BrandListData {
  Items: Brand[];
  Count: number;
}

export interface CreateBrandRequest {
  name: string;
  logoUrl?: string;
  description?: string;
  displayOrder: number;
}

export interface UpdateBrandRequest {
  id: string;
  name: string;
  logoUrl?: string;
  description?: string;
  displayOrder: number;
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const brandService = {
  async getAll(): Promise<ApiResponse<BrandListData>> {
    const response = await api.post<ApiResponse<BrandListData>>("/brand/list");
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<Brand>> {
    const response = await api.post<ApiResponse<Brand>>("/brand/get", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" },
    });
    return response.data;
  },

  async create(request: CreateBrandRequest): Promise<ApiResponse<Brand>> {
    const response = await api.post<ApiResponse<Brand>>("/brand/add", request);
    return response.data;
  },

  async update(request: UpdateBrandRequest): Promise<ApiResponse<Brand>> {
    const response = await api.post<ApiResponse<Brand>>("/brand/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/brand/delete", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" },
    });
    return response.data;
  },
};