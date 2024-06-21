import axios, { type AxiosInstance } from 'axios';
import type { ApiError, ApiResponse } from '.';

interface LoginRequest {
    username?: string;
    email?: string;
    password: string;
}

interface LoginResponse {
    token: string;
    refreshToken: string;
}

class AuthClient {
    private http: AxiosInstance;

    constructor() {
        this.http = axios.create({
            baseURL: 'http://localhost:5105/api',
            validateStatus: () => true
        });
    }

    async loginUser(request: LoginRequest) {
        const { data } = await this.http.post<ApiResponse<LoginResponse>>('/auth/login', request);

        return data;
    }
}

export default AuthClient;
