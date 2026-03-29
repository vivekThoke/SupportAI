import { api } from "./client";

export const login = async(data: {
    email: string;
    password: string;
}) => {
    const res = await api.post("/Auth/login", data);
    return res.data;
}

export const register = async(data: {tenantName: string; email: string; password: string;}) => {
    const res =  await api.post("/Auth/register", data);
    return res.data;
}