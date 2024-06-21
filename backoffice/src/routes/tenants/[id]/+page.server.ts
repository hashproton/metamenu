import { fail } from "@sveltejs/kit";
import TenantsApi, { isApiError, tenantApi } from "../helpers/tenants_api";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({
    params: { id }
}) => {
    const response = await tenantApi.getTenantById(id)

    return {
        ...response
    }
}

export const actions = {
    update: async (e) => {
        const formData = await e.request.formData()

        const { name, status } = Object.fromEntries(formData)

        const test = parseInt(status as string)

        const response = await tenantApi.updateTenant(e.params.id, {
            name: name as string,
            status: test
        })

        if (isApiError(response)) {
            return fail(422, {
                ...response
            });
        }

        return {
            success: 'Tenant updated successfully'
        }
    },
    delete: async (e) => {
        const response = await tenantApi.deleteTenant(e.params.id)

        console.log(response);

        if (isApiError(response)) {
            return fail(422, {
                ...response
            });
        }

        return {
            success: 'Tenant deleted successfully'
        }
    }
}