import { authClient, isApiError } from "$lib/clients";
import type { Actions } from './$types'

export const actions: Actions = {
    default: async ({ request, cookies }) => {
        const formData = await request.formData()

        const { username, email, password } = Object.fromEntries(formData)

        const data = await authClient.loginUser({
            email: email as string,
            password: password as string
        })

        if (!isApiError(data)) {
            cookies.set('token', data.token, {
                httpOnly: true,
                path: '/',
            });

            cookies.set('refreshToken', data.refreshToken, {
                httpOnly: true,
                path: '/',
            });
        }

        return {
            ...data
        };
    }
}