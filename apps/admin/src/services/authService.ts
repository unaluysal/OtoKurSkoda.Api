import api from "./api";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface User {
  id: string;
  email: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  roles: string[];
  createTime: string;
  createUserId: string;
  updateTime: string;
  updateUserId: string;
  tenatId: string;
  status: boolean;
}

export interface AuthData {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiration: string;
  user: User;
}

export interface ApiResponse<T> {
  Data: T;
  Status: boolean;
  StatusCode: number;
  Message: string;
  MessageCode: string;
}

export const authService = {
  async login(request: LoginRequest): Promise<ApiResponse<AuthData>> {
    const response = await api.post<ApiResponse<AuthData>>("/auth/login", request);
    
    if (response.data.Status && response.data.Data) {
      localStorage.setItem("accessToken", response.data.Data.accessToken);
      localStorage.setItem("refreshToken", response.data.Data.refreshToken);
      localStorage.setItem("user", JSON.stringify(response.data.Data.user));
    }
    
    return response.data;
  },

  async refreshToken(): Promise<ApiResponse<AuthData>> {
    const refreshToken = localStorage.getItem("refreshToken");
    const response = await api.post<ApiResponse<AuthData>>("/auth/refresh-token", {
      refreshToken,
    });

    if (response.data.Status && response.data.Data) {
      localStorage.setItem("accessToken", response.data.Data.accessToken);
      localStorage.setItem("refreshToken", response.data.Data.refreshToken);
      localStorage.setItem("user", JSON.stringify(response.data.Data.user));
    }

    return response.data;
  },

  async logout(): Promise<void> {
    const refreshToken = localStorage.getItem("refreshToken");
    try {
      await api.post("/auth/revoke-token", { refreshToken });
    } catch (error) {
      // Hata olsa bile local storage'ı temizle
    }
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
  },

  isLoggedIn(): boolean {
    return !!localStorage.getItem("accessToken");
  },

  getUser(): User | null {
    const userStr = localStorage.getItem("user");
    if (userStr) {
      return JSON.parse(userStr);
    }
    return null;
  },
};