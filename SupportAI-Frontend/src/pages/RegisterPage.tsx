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
        <div className="relative min-h-screen bg-[#0a0a0a] text-white flex items-center justify-center p-4 overflow-hidden">
            {/* Background Glow */}
            <div className="absolute top-1/4 left-1/2 -translate-x-1/2 w-72 h-72 bg-violet-600/15 blur-[120px] rounded-full"></div>

            <div className="relative z-10 w-full max-w-sm p-8 bg-[#121212] border border-white/10 rounded-2xl shadow-2xl">
                <div className="mb-8 text-center">
                    <h1 className="text-2xl font-bold tracking-tight">Create Account</h1>
                    <p className="text-sm text-gray-400 mt-2">Start your 14-day free trial today.</p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-5">
                    {/* Company Name */}
                    <div>
                        <label className="block text-xs font-medium text-gray-400 uppercase tracking-wider mb-1.5 ml-1">Company Name</label>
                        <input
                            type="text"
                            required
                            placeholder="Your Workspace"
                            className="w-full p-3 bg-[#1a1a1a] border border-white/5 rounded-lg focus:outline-none focus:border-violet-500 transition-colors placeholder:text-gray-600"
                            onChange={(e) => setFormData({ ...formData, tenantName: e.target.value })}
                        />
                    </div>

                    {/* Email */}
                    <div>
                        <label className="block text-xs font-medium text-gray-400 uppercase tracking-wider mb-1.5 ml-1">Email Address</label>
                        <input
                            type="email"
                            required
                            placeholder="name@company.com"
                            className="w-full p-3 bg-[#1a1a1a] border border-white/5 rounded-lg focus:outline-none focus:border-violet-500 transition-colors placeholder:text-gray-600"
                            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        />
                    </div>

                    {/* Password */}
                    <div>
                        <label className="block text-xs font-medium text-gray-400 uppercase tracking-wider mb-1.5 ml-1">Password</label>
                        <input
                            type="password"
                            required
                            placeholder="••••••••"
                            className="w-full p-3 bg-[#1a1a1a] border border-white/5 rounded-lg focus:outline-none focus:border-violet-500 transition-colors placeholder:text-gray-600"
                            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        />
                    </div>

                    {/* Submit Button */}
                    <button
                        type="submit"
                        className="w-full bg-white text-black font-semibold py-3 rounded-lg hover:bg-gray-200 transition-all active:scale-[0.98] mt-2"
                    >
                        Get Started
                    </button>
                </form>

                <div className="mt-8 pt-6 border-t border-white/5 text-center">
                    <p className="text-xs text-gray-500 leading-relaxed">
                        By signing up, you agree to our <br />
                        <a href="#" className="underline hover:text-gray-300">Terms of Service</a> and <a href="#" className="underline hover:text-gray-300">Privacy Policy</a>.
                    </p>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;