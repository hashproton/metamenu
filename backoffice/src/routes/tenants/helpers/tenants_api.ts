import axios, { type AxiosInstance } from 'axios';

interface PaginatedResponse<T> {
    items: T[];
    totalItems: number;
    totalPages: number;
    pageNumber: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

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

        return handleApiResponse(data);
    }

    async getTenantsInfo() {
        const { data } = await this.http.get<ApiResponse<TenantInfo>>(`/tenants/info`);

        return handleApiResponse(data);
    }

    async deleteTenant(id: string) {
        const { data } = await this.http.delete<VoidApiResponse>(`/tenants/${id}`);

        return data
    }
}

export default TenantsApi;
