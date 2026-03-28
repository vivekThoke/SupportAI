import { api } from "./client";

export const login = async(data: {
    email: string;
    password: string;
}) => {
    const res = await api.post("/auth/login", data);
    return res.data;
}