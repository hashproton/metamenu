import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import TenantsApi, { isApiError } from "./helpers/tenants_api";

export const load: PageServerLoad = async ({ url }) => { 
    let page = parseInt(url.searchParams.get('page') || '1')

    if (page < 1) {
        redirect(302, '/tenants?page=1');
    }

    const api = new TenantsApi();

    const data = await api.getTenants(page, 5);
    if (!isApiError(data)) {
        if (data.items.length === 0) {
            redirect(302, `/tenants?page=${data.totalPages}`);
        }
    }

    const info = await api.getTenantsInfo();

    return {
        ...data,
        info
    };
};