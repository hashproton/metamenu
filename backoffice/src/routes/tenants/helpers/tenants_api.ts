import axios, { AxiosError, type AxiosInstance } from 'axios';

interface Tenant {
    id: string;
    name: string;
    isActive: boolean;
}

interface GetTenantsResponse {
    items: Tenant[];
    totalItems: number;
    totalPages: number;
    pageNumber: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

interface ApiError {
    code: string;
    message: string;
}

interface ApiResponse<T> {
    errors?: ApiError[];
    data?: T;
}

class TenantsApi {
    private http: AxiosInstance;

    constructor() {
        this.http = axios.create({
            validateStatus(status) {
                return status < 500; // Accept all responses, we'll handle status codes manually
            },
            baseURL: 'http://localhost:5105/api'
        });
    }

    async createTenant(name: string): Promise<ApiResponse<number>> {
        try {
            const response = await this.http.post<ApiResponse<number>>('/tenants', { name });

            if (response.status >= 500) {
                return {
                    errors: [{ code: response.status.toString(), message: response.statusText }]
                };
            }

            return response.data;
        } catch (error: unknown) {
            return this.handleAxiosError(error);
        }
    }

    async getTenants(pageNumber = 1, pageSize = 10): Promise<ApiResponse<GetTenantsResponse>> {
        try {
            const response = await this.http.get<GetTenantsResponse>('/tenants', {
                params: {
                    pageNumber,
                    pageSize
                }
            });

            if (response.status >= 500) {
                return {
                    errors: [{ code: response.status.toString(), message: response.statusText }]
                };
            }

            return { data: response.data };
        } catch (error: unknown) {
            return this.handleAxiosError(error);
        }
    }

    async deleteTenant(id: string): Promise<ApiResponse<void>> {
        try {
            const response = await this.http.delete<void>(`/tenants/${id}`);

            if (response.status >= 500) {
                return {
                    errors: [{ code: response.status.toString(), message: response.statusText }]
                };
            }

            return { data: response.data };
        } catch (error: unknown) {
            return this.handleAxiosError(error);
        }
    }

    private handleAxiosError(error: unknown): ApiResponse<any> {
        if (error instanceof AxiosError && error.response) {
            const statusCode = error.response.status;
            if (statusCode >= 500) {
                return {
                    errors: [{ code: statusCode.toString(), message: error.response.statusText }]
                };
            }

            return {
                errors: error.response.data.errors
            };
        }

        throw error;
    }
}

export default TenantsApi;
