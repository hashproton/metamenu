import AuthClient from "./auth_client";
import TenantsClient from "./tenants_client";

interface PaginatedResponse<T> {
    items: T[];
    totalItems: number;
    totalPages: number;
    pageNumber: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

interface ApiError {
    errors: {
        code: string;
        message: string;
        type: number;
    }[];
}

function isApiError(response: any): response is ApiError {
    // 
    return response && typeof response === 'object' && ('errors' in response) && 'status' in response;
}

type ApiResponse<T> = T | ApiError;
type VoidApiResponse = ApiError | void;

const tenantsClient = new TenantsClient();
const authClient = new AuthClient();
export {
    tenantsClient,
    authClient
}


export { isApiError };
export type { PaginatedResponse, ApiError, ApiResponse, VoidApiResponse };

