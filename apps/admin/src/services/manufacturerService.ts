import api from "./api";

export interface Manufacturer {
  id: string;
  name: string;
  slug: string;
  logoUrl: string | null;
  website: string | null;
  description: string | null;
  displayOrder: number;
  status: boolean;
  productCount: number;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenantId: string;
}

export interface ManufacturerListData {
  Items: Manufacturer[];
  Count: number;
}

export interface CreateManufacturerRequest {
  name: string;
  logoUrl?: string;
  website?: string;
  description?: string;
  displayOrder: number;
}

export interface UpdateManufacturerRequest {
  id: string;
  name: string;
  logoUrl?: string;
  website?: string;
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

export const manufacturerService = {
  async getAll(): Promise<ApiResponse<ManufacturerListData>> {
    const response = await api.post<ApiResponse<ManufacturerListData>>("/manufacturer/list");
    return response.data;
  },

  async getActive(): Promise<ApiResponse<ManufacturerListData>> {
    const response = await api.post<ApiResponse<ManufacturerListData>>("/manufacturer/active");
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<Manufacturer>> {
    const response = await api.post<ApiResponse<Manufacturer>>(
      "/manufacturer/get",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async create(request: CreateManufacturerRequest): Promise<ApiResponse<Manufacturer>> {
    const response = await api.post<ApiResponse<Manufacturer>>("/manufacturer/add", request);
    return response.data;
  },

  async update(request: UpdateManufacturerRequest): Promise<ApiResponse<Manufacturer>> {
    const response = await api.post<ApiResponse<Manufacturer>>("/manufacturer/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>(
      "/manufacturer/delete",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },
};