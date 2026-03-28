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
        <div className="p-4 max-w-2xl mx-auto">
            <div className="h-[70vh] overflow-y-auto border p-4 mb-4">
                {messages.map((m, i) => (
                    <div key={i} className="mb-2">
                        <b>{m.role}</b> {m.text}
                    </div>
                ))}
            </div>

            <div className="flex gap-2">
                <input
                    className="flex-1 border p-2"
                    value={input}
                    onChange={(e) => setInput(e.target.value)}/>

                <button className='bg-black text-white px-4' onClick={send}>
                    Send
                </button>
            </div>
        </div>
    )
}

export default ChatPage