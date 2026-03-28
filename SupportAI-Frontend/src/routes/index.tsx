import React from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Loginpage } from '../pages/Loginpage'
import ChatPage from '../pages/ChatPage'

const AppRoutes = () => {
  return (
    <BrowserRouter>
        <Routes>
            <Route path='/' element={<Loginpage />}/>
            <Route path='/chat' element={<ChatPage />}/>
            <Route />
        </Routes>
    </BrowserRouter>
  )
}

export default AppRoutes