import type { Handle } from "@sveltejs/kit";

export const handle: Handle = async ({ event, resolve }) => {
    const { get } = event.cookies;

    const token = get("token");

    if (token && !isJwtExpired(token)) {
        console.log("Token expired");
    }

    console.log("Token", token);

    return await resolve(event);
};

function isJwtExpired(jwt: string): boolean {
    const [, payload] = jwt.split(".");
    const decoded = JSON.parse(atob(payload));

    return decoded.exp < Date.now() / 1000;
}