import api from "./api";

export interface User {
  id: string;
  email: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  emailConfirmed: boolean;
  phoneConfirmed: boolean;
  lastLoginAt: string | null;
  createTime: string;
  createUserId: string;
  updateTime: string | null;
  updateUserId: string | null;
  tenatId: string;
  status: boolean;
  roles: string[];
}

export interface UserListData {
  Items: User[];
  Count: number;
}

export interface CreateUserRequest {
  email: string;
  password: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  roleGroupIds: string[];
}

export interface UpdateUserRequest {
  id: string;
  email: string;
  password?: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  emailConfirmed: boolean;
  phoneConfirmed: boolean;
  roleGroupIds: string[];
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const userService = {
  async getAll(): Promise<ApiResponse<UserListData>> {
    const response = await api.post<ApiResponse<UserListData>>("/user/list");
    return response.data;
  },

  async getById(id: string): Promise<ApiResponse<User>> {
    const response = await api.post<ApiResponse<User>>("/user/get", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async getWithRoles(id: string): Promise<ApiResponse<User>> {
    const response = await api.post<ApiResponse<User>>("/user/get-with-roles", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },

  async create(request: CreateUserRequest): Promise<ApiResponse<User>> {
    const response = await api.post<ApiResponse<User>>("/user/add", request);
    return response.data;
  },

  async update(request: UpdateUserRequest): Promise<ApiResponse<User>> {
    const response = await api.post<ApiResponse<User>>("/user/update", request);
    return response.data;
  },

  async delete(id: string): Promise<ApiResponse<null>> {
    const response = await api.post<ApiResponse<null>>("/user/delete", JSON.stringify(id), {
      headers: { "Content-Type": "application/json" }
    });
    return response.data;
  },
};