import React from 'react'
import { api } from './client'

export const askQuestion = async (question: string) => {
  const res = await api.post("/chat/ask", { question }, {
    headers: {
                "Content-Type": "application/json",
                "tenantName": "myTenant"
            },
  });
  return res.data;
}
