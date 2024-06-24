import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { isApiError, tenantsClient } from "$lib/clients";

export const load: PageServerLoad = async ({ url, locals }) => {
    console.log(locals.auth.token);
    let page = parseInt(url.searchParams.get('page') || '1')

    if (page < 1) {
        redirect(302, '/tenants?page=1');
    }

    const data = await tenantsClient.getTenants(page, 5);
    if (!isApiError(data)) {
        if (data.items.length === 0) {
            redirect(302, `/tenants?page=${data.totalPages}`);
        }
    }

    const info = await tenantsClient.getTenantsInfo();

    return {
        ...data,
        info
    };
};