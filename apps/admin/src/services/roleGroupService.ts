import api from "./api";
import type { Role } from "./roleService";

export interface RoleGroup {
  id: string;
  name: string;
  description: string;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenatId: string;
  status: boolean;
  roles?: Role[];
}

export interface RoleGroupListData {
  Items: RoleGroup[];
  Count: number;
}

export interface CreateRoleGroupRequest {
  name: string;
  description: string;
}

export interface UpdateRoleGroupRequest {
  id: string;
  name: string;
  description: string;
}

export interface AssignRoleRequest {
  roleGroupId: string;
  roleId: string;
}

export interface AssignMultipleRolesRequest {
  roleGroupId: string;
  roleIds: string[];
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const roleGroupService = {
  async getAll(): Promise<ApiResponse<RoleGroupListData>> {
    const response = await api.post<ApiResponse<RoleGroupListData>>("/rolegroup/list");
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<RoleGroup>> {
    const response = await api.post<ApiResponse<RoleGroup>>("/rolegroup/get", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async getWithRoles(id: string): Promise<ApiResponse<RoleGroup>> {
    const response = await api.post<ApiResponse<RoleGroup>>("/rolegroup/get-with-roles", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async create(request: CreateRoleGroupRequest): Promise<ApiResponse<RoleGroup>> {
    const response = await api.post<ApiResponse<RoleGroup>>("/rolegroup/add", request);
    return response.data;
  },

  async update(request: UpdateRoleGroupRequest): Promise<ApiResponse<RoleGroup>> {
    const response = await api.post<ApiResponse<RoleGroup>>("/rolegroup/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/rolegroup/delete", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async assignRole(request: AssignRoleRequest): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/rolegrouprole/assign", request);
    return response.data;
  },

  async assignMultipleRoles(request: AssignMultipleRolesRequest): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/rolegrouprole/assign-multiple", request);
    return response.data;
  },

  async removeRole(roleGroupId: string, roleId: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/rolegrouprole/remove", { roleGroupId, roleId });
    return response.data;
  },
};