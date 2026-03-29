import React, { useState } from 'react'
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
        <div className="flex h-screen bg-black text-neutral-200 font-sans antialiased">
            {/* Simple SaaS Sidebar */}
            <div className="w-64 bg-neutral-900 border-r border-neutral-800 hidden md:flex flex-col p-4">
                <div className="flex items-center gap-2 mb-8 px-2">
                    <div className="h-6 w-6 bg-white rounded-md flex items-center justify-center">
                        <span className="text-black font-bold text-xs">AI</span>
                    </div>
                    <span className="font-bold text-white tracking-tight">SupportAI</span>
                </div>
                <button className="w-full py-2 px-4 bg-neutral-800 border border-neutral-700 rounded-lg text-sm hover:bg-neutral-700 transition-colors text-left mb-4">
                    + New Chat
                </button>
            </div>

            {/* Main Chat Area */}
            <div className="flex-1 flex flex-col max-w-4xl mx-auto w-full">
                {/* Header */}
                <header className="p-4 border-b border-neutral-800 flex justify-between items-center bg-black/50 backdrop-blur-md sticky top-0">
                    <h2 className="font-semibold text-white">General Inquiry</h2>
                    <div className="text-xs text-neutral-500 bg-neutral-900 px-2 py-1 rounded border border-neutral-800">
                        Pro Plan
                    </div>
                </header>

                {/* Messages Container */}
                <div className="flex-1 overflow-y-auto p-4 space-y-6 scrollbar-hide">
                    {messages.length === 0 && (
                        <div className="h-full flex flex-col items-center justify-center text-neutral-600">
                            <p className="text-sm">No messages yet. Start a conversation!</p>
                        </div>
                    )}
                    
                    {messages.map((m, i) => (
                        <div key={i} className={`flex ${m.role === 'user' ? 'justify-end' : 'justify-start'}`}>
                            <div className={`max-w-[80%] rounded-2xl px-4 py-3 text-sm shadow-sm ${
                                m.role === 'user' 
                                ? 'bg-white text-black rounded-tr-none' 
                                : 'bg-neutral-900 border border-neutral-800 text-neutral-200 rounded-tl-none'
                            }`}>
                                <div className="text-[10px] uppercase font-bold tracking-widest opacity-50 mb-1">
                                    {m.role}
                                </div>
                                <div className="leading-relaxed">{m.text}</div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* Input Area */}
                <div className="p-4 bg-gradient-to-t from-black via-black to-transparent">
                    <div className="relative max-w-3xl mx-auto flex gap-2 items-center bg-neutral-900 border border-neutral-800 rounded-2xl p-2 focus-within:border-neutral-600 transition-colors">
                        <input
                            className="flex-1 bg-transparent border-none p-3 text-sm focus:outline-none placeholder-neutral-600 text-white"
                            placeholder="Ask anything..."
                            value={input}
                            onChange={(e) => setInput(e.target.value)}
                            onKeyDown={(e) => e.key === 'Enter' && send()}
                        />
                        <button 
                            className="bg-white text-black h-10 px-4 rounded-xl font-bold text-sm hover:bg-neutral-200 transition-all active:scale-95"
                            onClick={send}
                        >
                            Send
                        </button>
                    </div>
                    <p className="text-[10px] text-center text-neutral-600 mt-3 uppercase tracking-tighter">
                        AI may generate inaccurate info. Check important facts.
                    </p>
                </div>
            </div>
        </div>
    )
}

export default ChatPage