import axios, { type AxiosInstance } from 'axios';
import { isApiError, type ApiError, type ApiResponse, type PaginatedResponse, type VoidApiResponse } from '.';



export enum TenantStatus {
    Active = 0,
    Inactive = 1,
    Demo = 2
}

export const mapTenantStatus = (status: number) => {
    switch (status) {
        case TenantStatus.Active:
            return 'Active';
        case TenantStatus.Inactive:
            return 'Inactive';
        case TenantStatus.Demo:
            return 'Demo';
        default:
            return 'Unknown';
    }
}

function handleApiResponse<T>(data: ApiResponse<T>, transform?: (data: T) => any): ApiError | T {
    if (isApiError(data)) {
      return data;
    }
  
    return transform ? transform(data) : data;
}

interface Tenant {
    id: string;
    name: string;
    status: number;
}

interface TenantInfo {
    active: number;
    inactive: number;
    demo: number;
    total: number;
}

class TenantsClient {
    private http: AxiosInstance;

    public auth?: {
        token: string;
        refreshToken: string;
    }

    constructor() {
        this.http = axios.create({
            baseURL: 'http://localhost:5105/api',
            validateStatus: () => true,
        });
    }

    async setAuth(token: string, refreshToken: string) {
        this.auth = {
            token,
            refreshToken
        }

        this.http.defaults.headers.Authorization = `Bearer ${token}`;
        this.http.defaults.headers.RefreshToken = refreshToken;
    }

    async createTenant(name: string): Promise<ApiResponse<number>> {
        const { data } = await this.http.post<ApiResponse<number>>('/tenants', { name });

        return data;
    }

    async getTenants(pageNumber = 1, pageSize = 5) {
        const { data } = await this.http.get<ApiResponse<PaginatedResponse<Tenant>>>('/tenants', {
            params: {
                pageNumber,
                pageSize
            }
        });

        return data
    }

    async updateTenant(id: string, tenant: Partial<Pick<Tenant, "name" | 'status'>>) {
        const { data } = await this.http.put<VoidApiResponse>(`/tenants/${id}`, tenant);

        return data
    }

    async getTenantById(id: string) {
        const { data } = await this.http.get<ApiResponse<Tenant>>(`/tenants/${id}`);

        return data;
    }

    async getTenantsInfo() {
        const { data } = await this.http.get<ApiResponse<TenantInfo>>(`/tenants/info`);

        return data
    }

    async deleteTenant(id: string) {
        const { data } = await this.http.delete<VoidApiResponse>(`/tenants/${id}`);

        return data
    }
}

export default TenantsClient;
