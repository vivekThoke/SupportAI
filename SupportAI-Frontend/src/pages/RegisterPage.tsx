import React, { useState } from 'react';
import { register } from '../api/auth';

const RegisterPage = () => {
    const [formData, setFormData] = useState({
        tenantName: '',
        email: '',
        password: '',
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const res = await register(formData);
        localStorage.setItem("token", res);
        window.location.href = "/chat";
    };

    return (
        <div className="min-h-screen bg-black text-white flex items-center justify-center p-4">
            <div className="w-full max-w-sm bg-neutral-900 p-8 rounded-2xl border border-neutral-800">
                <h1 className="text-2xl font-bold mb-6 text-center">Create Account</h1>

                <form onSubmit={handleSubmit} className="space-y-4">
                    {/* Tenant Name */}
                    <div>
                        <label className="block text-sm mb-1 text-neutral-400">Company Name</label>
                        <input
                            type="text"
                            required
                            className="w-full bg-black border border-neutral-700 rounded-lg p-2.5 focus:outline-none focus:border-blue-500"
                            placeholder="Your Workspace"
                            onChange={(e) => setFormData({ ...formData, tenantName: e.target.value })}
                        />
                    </div>

                    {/* Email */}
                    <div>
                        <label className="block text-sm mb-1 text-neutral-400">Email Address</label>
                        <input
                            type="email"
                            required
                            className="w-full bg-black border border-neutral-700 rounded-lg p-2.5 focus:outline-none focus:border-blue-500"
                            placeholder="name@company.com"
                            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        />
                    </div>

                    {/* Password */}
                    <div>
                        <label className="block text-sm mb-1 text-neutral-400">Password</label>
                        <input
                            type="password"
                            required
                            className="w-full bg-black border border-neutral-700 rounded-lg p-2.5 focus:outline-none focus:border-blue-500"
                            placeholder="••••••••"
                            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        />
                    </div>

                    {/* Submit Button */}
                    <button
                        type="submit"
                        className="w-full bg-white text-black font-bold py-2.5 rounded-lg mt-4 hover:bg-neutral-200 transition-colors"
                    >
                        Register
                    </button>
                </form>

                <p className="text-center text-xs text-neutral-500 mt-6">
                    By signing up, you agree to our Terms of Service.
                </p>
            </div>
        </div>
    );
};

export default RegisterPage;