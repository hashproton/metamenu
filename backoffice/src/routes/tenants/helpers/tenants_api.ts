import type { AxiosInstance } from 'axios';
import axios from 'axios';

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

class TenantsApi {
    // Generate http client with base URL

    private http: AxiosInstance;
    constructor() {
        this.http = axios.create({
            baseURL: 'http://localhost:5105/api'
        });
    }

    async createTenant(name: string): Promise<number> {
        const response = await this.http.post('/tenants', {
            name: name
        });

        return response.data;
    }

    async getTenants(): Promise<GetTenantsResponse> {
        const response = await this.http.get('/tenants', {
            params: {
                pageNumber: 1,
                pageSize: 10
            }
        })

        return response.data;
    }
}

export default TenantsApi;