import React from 'react'
import { api } from './client'

export const askQuestion = async (question: string) => {
  const res = await api.post("/Chat/ask", { question }, {
    headers: {
                "Content-Type": "application/json",
                "X-Tenant-Name": "test-company" // For now including test-compay for teneant name
              },
  });
  return res.data;
}
