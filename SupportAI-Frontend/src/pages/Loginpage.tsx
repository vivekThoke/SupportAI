import React, { useState } from 'react';
import { login } from '../api/auth';

export const Loginpage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async () => {
        const res = await login({ email, password });
        localStorage.setItem("token", res);
        window.location.href = "/chat";
    };

    return (
        <div className="flex h-screen items-center justify-center bg-[#0a0a0a] text-white">
            {/* Soft background glow for that SaaS feel */}
            <div className="absolute top-1/4 left-1/2 -translate-x-1/2 w-64 h-64 bg-violet-600/20 blur-[120px] rounded-full"></div>

            <div className="relative z-10 w-full max-w-sm p-8 bg-[#121212] border border-white/10 rounded-2xl shadow-2xl">
                <div className="mb-8 text-center">
                    <h1 className="text-2xl font-bold tracking-tight">Welcome back</h1>
                    <p className="text-sm text-gray-400 mt-2">Enter your credentials to access your account</p>
                </div>

                <div className="space-y-4">
                    <div>
                        <label className="block text-xs font-medium text-gray-400 uppercase tracking-wider mb-1 ml-1">Email Address</label>
                        <input 
                            className="w-full p-3 bg-[#1a1a1a] border border-white/5 rounded-lg focus:outline-none focus:border-violet-500 transition-colors placeholder:text-gray-600"
                            placeholder="name@company.com"
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>

                    <div>
                        <label className="block text-xs font-medium text-gray-400 uppercase tracking-wider mb-1 ml-1">Password</label>
                        <input 
                            className="w-full p-3 bg-[#1a1a1a] border border-white/5 rounded-lg focus:outline-none focus:border-violet-500 transition-colors placeholder:text-gray-600"
                            placeholder="••••••••"
                            type="password"
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>

                    <button 
                        className="w-full bg-white text-black font-semibold p-3 rounded-lg hover:bg-gray-200 transition-all active:scale-[0.98] mt-2"
                        onClick={handleLogin}>
                        Sign In
                    </button>
                </div>

                <p className="text-center mt-6 text-sm text-gray-400">
                    Don't have an account? 
                    <a href="/register" className="ml-1 text-violet-400 hover:text-violet-300 font-medium transition-colors">
                        Create one
                    </a>
                </p>
            </div>
        </div>
    );
}