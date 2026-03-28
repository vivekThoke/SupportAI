import React from 'react'
import { api } from './client';

const uploadDocument = async (file: File) => {
    const form = new FormData();
    form.append("file", file);

    const res = await api.post("/document/upload", form);
    return res.data;
}

export default uploadDocument