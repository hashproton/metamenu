import type { PageServerLoad } from "./$types";
import TenantsApi from "./helpers/tenants_api";

export const load: PageServerLoad = async () => { 
    const api = new TenantsApi();

    return {
        response: await api.getTenants()
    }
};