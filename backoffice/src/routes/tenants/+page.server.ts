import type { PageServerLoad } from "./$types";
import TenantsApi from "./helpers/tenants_api";

export const load: PageServerLoad = async () => { 
    const api = new TenantsApi();

    const { data } = await api.getTenants();

    return {
        response: data
    };
};