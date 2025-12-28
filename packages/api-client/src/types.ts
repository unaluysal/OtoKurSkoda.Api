// User & Auth
export interface User {
  id: string;
  email: string;
  phone: string;
  firstName: string;
  lastName: string;
  billingAddress?: Address;
  shippingAddress?: Address;
  roleGroups: RoleGroup[];
  createdAt: string;
  updatedAt: string;
}

export interface Address {
  id: string;
  street: string;
  city: string;
  district: string;
  postalCode: string;
  country: string;
}

export interface Role {
  id: string;
  name: string;
  description?: string;
}

export interface RoleGroup {
  id: string;
  name: string;
  roles: Role[];
}

// API Response
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}