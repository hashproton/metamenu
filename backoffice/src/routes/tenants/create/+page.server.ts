import TenantsApi from "../helpers/tenants_api";
import type { Actions } from "./$types";
import { fail } from "@sveltejs/kit";

export const actions: Actions  = {
    default: async (event) => {
        const api = new TenantsApi();

        const formData = await event.request.formData()
        const tenantName = formData.get('name') as string;
            
        try {
            const newTenantId = await api.createTenant(tenantName);

            console.log('New tenant ID:', newTenantId);
            return {
                id: newTenantId
            }
        } catch (error: any) {
            return fail(422, {
                errors: error.message
            });
        }
    }
}