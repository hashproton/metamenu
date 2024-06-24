import { tenantsClient } from "$lib/clients";
import { redirect, type Handle } from "@sveltejs/kit";

export const handle: Handle = async ({ event, resolve }) => {
    const { get } = event.cookies;

    const token = get("token");
    const refreshToken = get("refreshToken");

    if (token && refreshToken && !isJwtExpired(token)) {
        event.locals.auth = {
            token,
            refreshToken
        }

        if (event.route.id?.includes("/auth")) {
            redirect(302, "/");
        }
    } else {
        event.locals.auth = {
            token: "",
            refreshToken: ""
        }

        if (event.route.id !== "/auth/login") {
            redirect(302, "/auth/login");
        }
    }

    tenantsClient.setAuth(event.locals.auth.token, event.locals.auth.refreshToken);

    return await resolve(event);
};

function isJwtExpired(jwt: string): boolean {
    const [, payload] = jwt.split(".");
    const decoded = JSON.parse(atob(payload));

    return decoded.exp < Date.now() / 1000;
}