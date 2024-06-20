import axios, { type AxiosInstance } from 'axios';

interface GetTenantsResponse {
    items: Tenant[];
    totalItems: number;
    totalPages: number;
    pageNumber: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

interface Tenant {
    id: string;
    name: string;
    isActive: boolean;
}

interface ApiError {
    errors: {
        code: string;
        message: string;
        type: number;
    }[];
}

export function isApiError(response: any): response is ApiError {
    return response && typeof response === 'object' && 'errors' in response;
}

type ApiResponse<T> = T | ApiError;
type VoidApiResponse = ApiError | void;

class TenantsApi {
    private http: AxiosInstance;

    constructor() {
        this.http = axios.create({
            baseURL: 'http://localhost:5105/api',
            validateStatus: () => true
        });
    }

    async createTenant(name: string): Promise<ApiResponse<number>> {
        const { data } = await this.http.post<ApiResponse<number>>('/tenants', { name });

        return data;
    }

    async getTenants(pageNumber = 1, pageSize = 10): Promise<ApiResponse<GetTenantsResponse>> {
        const { data } = await this.http.get<GetTenantsResponse>('/tenants', {
            params: {
                pageNumber,
                pageSize
            }
        });

        return { data }
    }

    async updateTenant(id: string, tenant: Partial<Pick<Tenant, "name">>) {
        const { data } = await this.http.put<VoidApiResponse>(`/tenants/${id}`, tenant);
        
        return data
    }

    async getTenantById(id: string) {
        const { data } = await this.http.get<ApiResponse<Tenant>>(`/tenants/${id}`);

        return data;
    }

    async deleteTenant(id: string) {
        const { data } = await this.http.delete<VoidApiResponse>(`/tenants/${id}`);

        return data
    }
}

export default TenantsApi;
