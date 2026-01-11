export interface RegisterRequest {
  userNameOrEmail: string;
  password: string;
}

export interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  userName: string;
}