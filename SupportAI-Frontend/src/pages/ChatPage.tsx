import React, { useState } from 'react';
import { askQuestion } from '../api/chat';

const ChatPage = () => {
    const [messages, setMessages] = useState<any[]>([]);
    const [input, setInput] = useState("");

    const send = async () => {
        if (!input) return;
        const userMsg = { role: "user", text: input };
        setMessages((prev) => [...prev, userMsg]);
        const res = await askQuestion(input);
        const botMsg = { role: "bot", text: res.answer };
        setMessages((prev) => [...prev, botMsg]);
        setInput("");
    };

    return (
        <div className="relative flex h-screen bg-[#0a0a0a] text-neutral-200 font-sans antialiased overflow-hidden">
            {/* Background Aesthetic Glows */}
            <div className="absolute top-0 left-1/4 w-96 h-96 bg-violet-600/10 blur-[120px] rounded-full"></div>
            <div className="absolute bottom-0 right-1/4 w-96 h-96 bg-blue-600/5 blur-[120px] rounded-full"></div>

            {/* Main Chat Area */}
            <div className="relative z-10 flex-1 flex flex-col max-w-5xl mx-auto w-full border-x border-white/5 bg-[#0a0a0a]/50 backdrop-blur-sm">
                
                {/* Header */}
                <header className="p-5 border-b border-white/10 flex justify-between items-center bg-[#0a0a0a]/80 backdrop-blur-md sticky top-0 z-20">
                    <div className="flex items-center gap-3">
                        <div className="h-8 w-8 bg-white rounded-lg flex items-center justify-center shadow-[0_0_15px_rgba(255,255,255,0.2)]">
                            <span className="text-black font-black text-xs">AI</span>
                        </div>
                        <div>
                            <h2 className="font-bold text-white tracking-tight leading-none">SupportAI</h2>
                            <span className="text-[10px] text-violet-400 font-medium uppercase tracking-widest">Assistant Active</span>
                        </div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="text-[10px] font-bold text-neutral-400 bg-white/5 px-3 py-1.5 rounded-full border border-white/10 tracking-widest uppercase">
                            Pro Plan
                        </div>
                    </div>
                </header>

                {/* Messages Container */}
                <div className="flex-1 overflow-y-auto p-6 space-y-8 scrollbar-hide">
                    {messages.length === 0 && (
                        <div className="h-full flex flex-col items-center justify-center space-y-4 opacity-40">
                            <div className="h-12 w-12 border border-white/10 rounded-2xl flex items-center justify-center">
                                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" /></svg>
                            </div>
                            <p className="text-sm tracking-wide">Ready for your first instruction.</p>
                        </div>
                    )}
                    
                    {messages.map((m, i) => (
                        <div key={i} className={`flex ${m.role === 'user' ? 'justify-end' : 'justify-start'} animate-in fade-in slide-in-from-bottom-2 duration-300`}>
                            <div className={`max-w-[75%] rounded-2xl px-5 py-4 text-sm shadow-2xl ${
                                m.role === 'user' 
                                ? 'bg-white text-black font-medium' 
                                : 'bg-[#161616] border border-white/10 text-neutral-200'
                            }`}>
                                <div className={`text-[9px] uppercase font-bold tracking-[0.2em] mb-2 ${m.role === 'user' ? 'text-black/50' : 'text-violet-400'}`}>
                                    {m.role}
                                </div>
                                <div className="leading-relaxed text-[15px]">{m.text}</div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* Input Area */}
                <div className="p-6 bg-gradient-to-t from-[#0a0a0a] via-[#0a0a0a] to-transparent">
                    <div className="relative max-w-4xl mx-auto">
                        <div className="flex gap-3 items-center bg-[#161616] border border-white/10 rounded-2xl p-2.5 shadow-2xl focus-within:border-violet-500/50 transition-all duration-300">
                            
                            {/* Document Upload Button */}
                            <label className="cursor-pointer p-2.5 hover:bg-white/5 rounded-xl transition-colors text-neutral-400 hover:text-white group">
                                <input type="file" className="hidden" onChange={(e) => console.log(e.target.files?.[0])} />
                                <svg className="w-5 h-5 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.172 7l-6.586 6.586a2 2 0 102.828 2.828l6.414-6.586a4 4 0 00-5.656-5.656l-6.415 6.585a6 6 0 108.486 8.486L20.5 13" />
                                </svg>
                            </label>

                            <input
                                className="flex-1 bg-transparent border-none py-2 px-1 text-[15px] focus:outline-none placeholder-neutral-600 text-white"
                                placeholder="Type a message or upload a file..."
                                value={input}
                                onChange={(e) => setInput(e.target.value)}
                                onKeyDown={(e) => e.key === 'Enter' && send()}
                            />
                            
                            <button 
                                className="bg-white text-black h-11 px-6 rounded-xl font-bold text-sm hover:bg-neutral-200 transition-all active:scale-95 shadow-[0_0_20px_rgba(255,255,255,0.1)]"
                                onClick={send}
                            >
                                Send
                            </button>
                        </div>
                        <p className="text-[10px] text-center text-neutral-600 mt-4 uppercase tracking-[0.2em] font-medium">
                            Internal AI Instance • Secured with 256-bit encryption
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ChatPage;