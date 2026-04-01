import React, { useState, useRef, useEffect } from 'react';
import { askQuestion } from '../api/chat';

// Defining the message structure to include optional files
interface ChatMessage {
    role: "user" | "bot";
    text: string;
    file?: { name: string; size: number } | null;
}

const ChatPage = () => {
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const [input, setInput] = useState("");
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    
    const fileInputRef = useRef<HTMLInputElement>(null);
    const scrollEndRef = useRef<HTMLDivElement>(null);

    // Smooth scroll to bottom
    const scrollToBottom = () => {
        scrollEndRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        scrollToBottom();
    }, [messages, isLoading]);

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setSelectedFile(file);
        }
        // Reset input value so the same file can be selected again if removed
        if (fileInputRef.current) {
            fileInputRef.current.value = '';
        }
    };

    const removeFile = () => {
        setSelectedFile(null);
    };

    const send = async () => {
        if ((!input.trim() && !selectedFile) || isLoading) return;
        
        const currentInput = input;
        const currentFile = selectedFile;

        const userMsg: ChatMessage = { 
            role: "user", 
            text: currentInput,
            file: currentFile ? { name: currentFile.name, size: currentFile.size } : null
        };
        
        setMessages((prev) => [...prev, userMsg]);
        setInput("");
        setSelectedFile(null);
        setIsLoading(true);

        try {
            // NOTE: You will need to update your askQuestion API to accept FormData or the file object
            // if you intend to send the actual file to the backend.
            const res = await askQuestion(currentInput /*, currentFile */);
            const botMsg: ChatMessage = { role: "bot", text: res.answer };
            setMessages((prev) => [...prev, botMsg]);
        } catch (error) {
            console.error("Failed to fetch response:", error);
            const errorMsg: ChatMessage = { role: "bot", text: "⚠️ Communication error. Please try again." };
            setMessages((prev) => [...prev, errorMsg]);
        } finally {
            setIsLoading(false);
        }
    };

    // Helper to format file sizes nicely
    const formatBytes = (bytes: number) => {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + ' ' + sizes[i];
    };

    return (
        <div className="flex flex-col h-screen bg-[#0a0a0a] text-neutral-200 font-sans selection:bg-violet-500/30">
            
            {/* Header */}
            <header className="flex-none flex items-center justify-between px-6 py-4 bg-[#0a0a0a]/80 backdrop-blur-md border-b border-white/5 z-10">
                <div className="flex items-center gap-3">
                    <div className="relative flex items-center justify-center w-8 h-8 rounded-full bg-violet-600/20 text-violet-400">
                        <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 24 24">
                            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm-1-13h2v6h-2zm0 8h2v2h-2z" />
                        </svg>
                    </div>
                    <div>
                        <h1 className="text-sm font-semibold text-neutral-100">SupportAI</h1>
                        <p className="text-xs text-neutral-500">Secure File & Data Uplink</p>
                    </div>
                </div>
            </header>

            {/* Chat Area */}
            <main className="flex-1 overflow-y-auto p-4 sm:p-6 scrollbar-hide">
                <div className="max-w-3xl mx-auto flex flex-col gap-6">
                    
                    {messages.length === 0 && (
                        <div className="flex flex-col items-center justify-center h-[50vh] text-center opacity-60">
                            <div className="w-16 h-16 mb-4 rounded-2xl bg-neutral-900 border border-white/5 flex items-center justify-center">
                                <svg className="w-8 h-8 text-violet-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z" />
                                </svg>
                            </div>
                            <h2 className="text-lg font-medium text-white mb-1">How can I help you today?</h2>
                            <p className="text-sm text-neutral-400">Type a message or attach a file to begin.</p>
                        </div>
                    )}

                    {messages.map((m, i) => (
                        <div 
                            key={i} 
                            className={`flex w-full ${m.role === 'user' ? 'justify-end' : 'justify-start'} animate-in fade-in slide-in-from-bottom-2 duration-300`}
                        >
                            <div className={`
                                max-w-[85%] sm:max-w-[75%] px-5 py-3.5 text-[15px] leading-relaxed flex flex-col gap-2
                                ${m.role === 'user' 
                                    ? 'bg-violet-600 text-white rounded-2xl rounded-tr-sm shadow-md shadow-violet-900/20' 
                                    : 'bg-[#141414] text-neutral-200 border border-white/5 rounded-2xl rounded-tl-sm'}
                            `}>
                                {/* Attached File Display in Bubble */}
                                {m.file && (
                                    <div className={`flex items-center gap-3 p-2.5 rounded-xl border ${m.role === 'user' ? 'bg-black/20 border-white/10' : 'bg-neutral-900/50 border-white/5'}`}>
                                        <div className={`p-2 rounded-lg ${m.role === 'user' ? 'bg-white/10' : 'bg-neutral-800'}`}>
                                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                                            </svg>
                                        </div>
                                        <div className="flex flex-col min-w-0 flex-1">
                                            <span className="text-sm font-medium truncate">{m.file.name}</span>
                                            <span className="text-xs opacity-70">{formatBytes(m.file.size)}</span>
                                        </div>
                                    </div>
                                )}
                                
                                {/* Text Content */}
                                {m.text && <span>{m.text}</span>}
                            </div>
                        </div>
                    ))}

                    {/* Loading Indicator */}
                    {isLoading && (
                        <div className="flex justify-start animate-in fade-in duration-300">
                            <div className="bg-[#141414] border border-white/5 rounded-2xl rounded-tl-sm px-5 py-4 flex items-center gap-1.5">
                                <div className="w-1.5 h-1.5 bg-violet-400 rounded-full animate-bounce"></div>
                                <div className="w-1.5 h-1.5 bg-violet-400 rounded-full animate-bounce [animation-delay:-0.15s]"></div>
                                <div className="w-1.5 h-1.5 bg-violet-400 rounded-full animate-bounce [animation-delay:-0.3s]"></div>
                            </div>
                        </div>
                    )}
                    
                    <div ref={scrollEndRef} className="h-4" />
                </div>
            </main>

            {/* Input Area */}
            <footer className="flex-none p-4 sm:p-6 bg-[#0a0a0a]">
                <div className="max-w-3xl mx-auto flex flex-col gap-2">
                    
                    {/* File Preview before sending */}
                    {selectedFile && (
                        <div className="flex items-center gap-3 bg-[#141414] border border-violet-500/30 rounded-xl p-2 w-max animate-in fade-in slide-in-from-bottom-2">
                            <div className="flex items-center justify-center w-8 h-8 rounded-lg bg-violet-500/10 text-violet-400">
                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.172 7l-6.586 6.586a2 2 0 102.828 2.828l6.414-6.586a4 4 0 00-5.656-5.656l-6.415 6.585a6 6 0 108.486 8.486L20.5 13" />
                                </svg>
                            </div>
                            <div className="flex flex-col min-w-[120px] max-w-[200px]">
                                <span className="text-xs font-medium text-neutral-200 truncate">{selectedFile.name}</span>
                                <span className="text-[10px] text-neutral-500">{formatBytes(selectedFile.size)}</span>
                            </div>
                            <button 
                                onClick={removeFile}
                                className="p-1.5 ml-2 text-neutral-500 hover:text-white hover:bg-white/10 rounded-lg transition-colors"
                            >
                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
                                </svg>
                            </button>
                        </div>
                    )}

                    <div className="relative flex items-end gap-2 bg-[#141414] border border-white/10 rounded-3xl p-2 focus-within:border-violet-500/50 focus-within:ring-1 focus-within:ring-violet-500/50 transition-all">
                        
                        {/* Hidden File Input */}
                        <input 
                            type="file" 
                            ref={fileInputRef}
                            className="hidden" 
                            onChange={handleFileSelect}
                        />

                        {/* Attachment Button */}
                        <button 
                            onClick={() => fileInputRef.current?.click()}
                            className="flex-none flex items-center justify-center w-11 h-11 rounded-full text-neutral-400 hover:text-violet-400 hover:bg-violet-500/10 transition-colors"
                            title="Attach a file"
                        >
                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.172 7l-6.586 6.586a2 2 0 102.828 2.828l6.414-6.586a4 4 0 00-5.656-5.656l-6.415 6.585a6 6 0 108.486 8.486L20.5 13" />
                            </svg>
                        </button>
                        
                        <textarea
                            value={input}
                            onChange={(e) => setInput(e.target.value)}
                            onKeyDown={(e) => {
                                if (e.key === 'Enter' && !e.shiftKey) {
                                    e.preventDefault();
                                    send();
                                }
                            }}
                            placeholder="Message SupportAI..."
                            className="flex-1 max-h-32 min-h-[44px] bg-transparent border-none resize-none px-2 py-3 text-[15px] text-neutral-100 placeholder:text-neutral-500 focus:outline-none focus:ring-0"
                            rows={1}
                        />

                        {/* Send Button */}
                        <button 
                            onClick={send}
                            disabled={(!input.trim() && !selectedFile) || isLoading}
                            className={`
                                flex-none flex items-center justify-center w-11 h-11 rounded-full transition-all
                                ${(!input.trim() && !selectedFile) || isLoading 
                                    ? 'bg-white/5 text-white/20 cursor-not-allowed' 
                                    : 'bg-violet-600 text-white hover:bg-violet-500 active:scale-95 shadow-lg shadow-violet-900/30'}
                            `}
                        >
                            <svg className="w-5 h-5 -ml-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 19V5m0 0l-7 7m7-7l7 7" />
                            </svg>
                        </button>
                    </div>
                    
                    <div className="text-center mt-2">
                        <p className="text-[11px] text-neutral-600">AI can make mistakes. Verify important information.</p>
                    </div>
                </div>
            </footer>

        </div>
    );
};

export default ChatPage;