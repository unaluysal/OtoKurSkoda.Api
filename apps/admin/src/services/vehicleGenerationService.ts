import api from "./api";

export interface VehicleGeneration {
  id: string;
  vehicleModelId: string;
  vehicleModelName: string;
  brandName: string;
  name: string;
  code: string | null;
  startYear: number;
  endYear: number | null;
  imageUrl: string | null;
  description: string | null;
  status: boolean;
  yearRange: string;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenantId: string;
}

export interface VehicleGenerationListData {
  Items: VehicleGeneration[];
  Count: number;
}

export interface CreateVehicleGenerationRequest {
  vehicleModelId: string;
  name: string;
  code?: string;
  startYear: number;
  endYear?: number;
  imageUrl?: string;
  description?: string;
}

export interface UpdateVehicleGenerationRequest {
  id: string;
  vehicleModelId: string;
  name: string;
  code?: string;
  startYear: number;
  endYear?: number;
  imageUrl?: string;
  description?: string;
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const vehicleGenerationService = {
  async getAll(): Promise<ApiResponse<VehicleGenerationListData>> {
    const response = await api.post<ApiResponse<VehicleGenerationListData>>("/vehiclegeneration/list");
    return response.data;
  },

  async getByModelId(modelId: string): Promise<ApiResponse<VehicleGenerationListData>> {
    const response = await api.post<ApiResponse<VehicleGenerationListData>>(
      "/vehiclegeneration/list-by-model",
      JSON.stringify(modelId),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<VehicleGeneration>> {
    const response = await api.post<ApiResponse<VehicleGeneration>>(
      "/vehiclegeneration/get",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },

  async create(request: CreateVehicleGenerationRequest): Promise<ApiResponse<VehicleGeneration>> {
    const response = await api.post<ApiResponse<VehicleGeneration>>("/vehiclegeneration/add", request);
    return response.data;
  },

  async update(request: UpdateVehicleGenerationRequest): Promise<ApiResponse<VehicleGeneration>> {
    const response = await api.post<ApiResponse<VehicleGeneration>>("/vehiclegeneration/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>(
      "/vehiclegeneration/delete",
      JSON.stringify(id),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  },
};