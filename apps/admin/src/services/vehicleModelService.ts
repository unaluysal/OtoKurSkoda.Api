import api from "./api";

export interface VehicleModel {
  id: string;
  brandId: string;
  brandName: string;
  name: string;
  slug: string;
  imageUrl: string | null;
  description: string | null;
  displayOrder: number;
  status: boolean;
  generationCount: number;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenantId: string;
}

export interface VehicleModelListData {
  Items: VehicleModel[];
  Count: number;
}

export interface CreateVehicleModelRequest {
  brandId: string;
  name: string;
  imageUrl?: string;
  description?: string;
  displayOrder: number;
}

export interface UpdateVehicleModelRequest {
  id: string;
  brandId: string;
  name: string;
  imageUrl?: string;
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

export const vehicleModelService = {
  async getAll(): Promise<ApiResponse<VehicleModelListData>> {
    const response = await api.post<ApiResponse<VehicleModelListData>>("/vehiclemodel/list");
    return response.data;
  },

  async getByBrandId(brandId: string): Promise<ApiResponse<VehicleModelListData>> {
    const response = await api.post<ApiResponse<VehicleModelListData>>(
      "/vehiclemodel/list-by-brand",
      JSON.stringify(brandId),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<VehicleModel>> {
    const response = await api.post<ApiResponse<VehicleModel>>(
      "/vehiclemodel/get",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async create(request: CreateVehicleModelRequest): Promise<ApiResponse<VehicleModel>> {
    const response = await api.post<ApiResponse<VehicleModel>>("/vehiclemodel/add", request);
    return response.data;
  },

  async update(request: UpdateVehicleModelRequest): Promise<ApiResponse<VehicleModel>> {
    const response = await api.post<ApiResponse<VehicleModel>>("/vehiclemodel/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>(
      "/vehiclemodel/delete",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },
};