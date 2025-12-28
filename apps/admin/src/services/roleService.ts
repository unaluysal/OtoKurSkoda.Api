import api from "./api";

export interface Role {
  id: string;
  name: string;
  description: string;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenatId: string;
  status: boolean;
}

export interface RoleListData {
  Items: Role[];
  Count: number;
}

export interface CreateRoleRequest {
  name: string;
  description: string;
}

export interface UpdateRoleRequest {
  id: string;
  name: string;
  description: string;
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const roleService = {
  async getAll(): Promise<ApiResponse<RoleListData>> {
    const response = await api.post<ApiResponse<RoleListData>>("/role/list");
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<Role>> {
    const response = await api.post<ApiResponse<Role>>("/role/get", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async create(request: CreateRoleRequest): Promise<ApiResponse<Role>> {
    const response = await api.post<ApiResponse<Role>>("/role/add", request);
    return response.data;
  },

  async update(request: UpdateRoleRequest): Promise<ApiResponse<Role>> {
    const response = await api.post<ApiResponse<Role>>("/role/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/role/delete", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },
};