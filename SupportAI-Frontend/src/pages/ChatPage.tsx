import React, { useState, useRef, useEffect } from 'react';
import { askQuestion } from '../api/chat';

const ChatPage = () => {
    const [messages, setMessages] = useState<any[]>([]);
    const [input, setInput] = useState("");
    const scrollContainerRef = useRef<HTMLDivElement>(null);

    // Smooth scroll to the right when new messages appear
    useEffect(() => {
        if (scrollContainerRef.current) {
            scrollContainerRef.current.scrollTo({
                left: scrollContainerRef.current.scrollWidth,
                behavior: 'smooth'
            });
        }
    }, [messages]);

    const send = async () => {
        if (!input) return;
        const userMsg = { role: "user", text: input };
        setMessages((prev) => [...prev, userMsg]);
        setInput("");

        const res = await askQuestion(input);
        const botMsg = { role: "bot", text: res.answer };
        setMessages((prev) => [...prev, botMsg]);
    };

    return (
        <div className="h-screen bg-[#030303] text-white font-sans overflow-hidden relative selection:bg-violet-600/30">
            
            {/* Global Ambient Glows */}
            <div className="absolute top-0 left-1/4 w-full h-[500px] bg-violet-900/10 blur-[150px] pointer-events-none"></div>

            {/* Header - Minimalist & Floating */}
            <header className="absolute top-0 left-0 w-full p-8 flex justify-between items-center z-50 pointer-events-none">
                <div className="flex items-center gap-3">
                    <div className="h-3 w-3 rounded-full bg-violet-500 shadow-[0_0_15px_rgba(139,92,246,0.8)] animate-pulse"></div>
                    <h2 className="font-bold tracking-widest uppercase text-xs text-white/80">SupportAI // Pathway</h2>
                </div>
                <div className="text-[10px] uppercase tracking-[0.2em] text-white/40 border border-white/10 px-4 py-2 rounded-full backdrop-blur-md">
                    Secure Session
                </div>
            </header>

            {/* The Horizontal Timeline Canvas */}
            <div className="relative h-full w-full flex items-center">
                
                {/* Central Axis Line */}
                <div className="absolute left-0 right-0 h-[1px] bg-gradient-to-r from-transparent via-white/10 to-transparent z-0"></div>

                <div 
                    ref={scrollContainerRef}
                    className="flex items-center h-[70vh] w-full overflow-x-auto overflow-y-hidden px-[10vw] gap-12 z-10 [&::-webkit-scrollbar]:hidden"
                    style={{ scrollbarWidth: 'none' }}
                >
                    {messages.length === 0 && (
                        <div className="w-full flex justify-center absolute left-0 text-white/20 text-sm uppercase tracking-[0.5em]">
                            Awaiting Data Input
                        </div>
                    )}

                    {messages.map((m, i) => (
                        <div key={i} className="relative flex-shrink-0 w-[340px] h-full flex flex-col group animate-in slide-in-from-right-8 fade-in duration-500">
                            
                            {m.role === 'user' ? (
                                /* User Message: Branches UP */
                                <div className="flex-1 flex flex-col justify-end pb-8">
                                    <div className="bg-[#0a0a0a] border border-white/10 p-6 rounded-2xl shadow-2xl z-10 hover:border-white/30 transition-colors">
                                        <div className="text-[10px] text-white/40 uppercase tracking-widest mb-3">Query</div>
                                        <p className="text-sm leading-relaxed text-white/90">{m.text}</p>
                                    </div>
                                    {/* Connection Line Down to Axis */}
                                    <div className="w-[1px] h-8 bg-gradient-to-b from-white/20 to-transparent mx-auto mt-4"></div>
                                    {/* Axis Node */}
                                    <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-2 h-2 rounded-full bg-white/20"></div>
                                </div>
                            ) : (
                                /* AI Message: Branches DOWN */
                                <div className="flex-1 flex flex-col justify-start pt-8 mt-auto">
                                    {/* Axis Node */}
                                    <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-3 h-3 rounded-full bg-violet-500 shadow-[0_0_15px_rgba(139,92,246,0.6)]"></div>
                                    {/* Connection Line Up to Axis */}
                                    <div className="w-[1px] h-8 bg-gradient-to-t from-violet-500/50 to-transparent mx-auto mb-4"></div>
                                    <div className="bg-violet-950/20 backdrop-blur-xl border border-violet-500/30 p-6 rounded-2xl shadow-[0_20px_40px_rgba(139,92,246,0.1)] z-10">
                                        <div className="text-[10px] text-violet-400 uppercase tracking-widest mb-3">Synthesis</div>
                                        <p className="text-sm leading-relaxed text-violet-50">{m.text}</p>
                                    </div>
                                </div>
                            )}
                        </div>
                    ))}
                    
                    {/* Padding block so the last item isn't flush against the right edge */}
                    {messages.length > 0 && <div className="w-[10vw] flex-shrink-0"></div>}
                </div>
            </div>

            {/* Floating Command Center (Input) */}
            <div className="absolute bottom-12 left-1/2 -translate-x-1/2 w-[90%] max-w-2xl z-50">
                <div className="bg-[#0f0f0f]/80 backdrop-blur-2xl border border-white/10 p-2 rounded-full flex items-center gap-2 shadow-[0_20px_50px_rgba(0,0,0,0.5)] focus-within:border-violet-500/50 transition-all duration-300">
                    
                    {/* Document Upload Pill */}
                    <label className="cursor-pointer h-12 px-5 flex items-center justify-center bg-white/5 hover:bg-white/10 rounded-full transition-colors text-white/60 hover:text-white group border border-transparent hover:border-white/10">
                        <input type="file" className="hidden" onChange={(e) => console.log(e.target.files?.[0])} />
                        <svg className="w-5 h-5 group-hover:-translate-y-0.5 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M15.172 7l-6.586 6.586a2 2 0 102.828 2.828l6.414-6.586a4 4 0 00-5.656-5.656l-6.415 6.585a6 6 0 108.486 8.486L20.5 13" />
                        </svg>
                        <span className="text-xs font-medium ml-2 hidden sm:block">Attach</span>
                    </label>

                    <input
                        className="flex-1 bg-transparent border-none px-4 text-sm focus:outline-none placeholder-white/30 text-white"
                        placeholder="Inject query into the neural stream..."
                        value={input}
                        onChange={(e) => setInput(e.target.value)}
                        onKeyDown={(e) => e.key === 'Enter' && send()}
                    />
                    
                    <button 
                        className="h-12 px-8 bg-white text-black rounded-full font-bold text-sm hover:bg-violet-100 hover:text-violet-900 transition-all active:scale-95 shadow-[0_0_20px_rgba(255,255,255,0.2)]"
                        onClick={send}
                    >
                        Initialize
                    </button>
                </div>
            </div>

        </div>
    );
};

export default ChatPage;