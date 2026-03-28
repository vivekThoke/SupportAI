import React, { useState } from 'react'
import { login } from '../api/auth';

export const Loginpage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async () => {
        const res = await login({email, password});
        localStorage.setItem("token", res);
        window.location.href = "/chat";
    };

  return (
    <div className="flex h-screen items-center justify-center">
        <div className="p-6 border rounded w-80">
            <input 
                className="w-full mb-2 p-2 border"
                placeholder="Email"
                onChange={(e) => setEmail(e.target.value)}/>

            <input 
                className="w-full mb-2 p-2 border"
                placeholder="Password"
                type="password"
                onChange={(e) => setPassword(e.target.value)}/>

            <button 
                className="w-full bg-black text-white p-2"
                onClick={handleLogin}>
                    Login
            </button>
        </div>
    </div>
  )
}
