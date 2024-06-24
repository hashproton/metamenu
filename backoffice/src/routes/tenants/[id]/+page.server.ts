import { fail } from "@sveltejs/kit";
import { isApiError, tenantsClient } from '$lib/clients';
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({
    params: { id },
    locals: { auth }
}) => {
    tenantsClient.auth = {
        ...auth
    }

    const response = await tenantsClient.getTenantById(id)

    console.log(response);

    return {
        ...response
    }
}

export const actions = {
    update: async (e) => {
        const formData = await e.request.formData()

        const { name, status } = Object.fromEntries(formData)

        const test = parseInt(status as string)

        const response = await tenantsClient.updateTenant(e.params.id, {
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
        const response = await tenantsClient.deleteTenant(e.params.id)

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