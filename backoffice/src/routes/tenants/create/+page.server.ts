import { isApiError, tenantsClient } from '$lib/clients';
import type { Actions } from "./$types";
import { fail } from "@sveltejs/kit";

export const actions: Actions  = {
    default: async (event) => {
        const formData = await event.request.formData()
        const tenantName = formData.get('name') as string;
            
        const response = await tenantsClient.createTenant(tenantName);

        if (isApiError(response)) {
            return fail(422, {
                ...response
            });
        }

        return {
            success: {
                message: 'Tenant created successfully',
                data: {
                    id: response
                }
            }
        }
    }
}