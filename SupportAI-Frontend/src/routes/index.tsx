import React, { useState } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Loginpage } from '../pages/Loginpage'
import ChatPage from '../pages/ChatPage'
import RegisterPage from '../pages/RegisterPage'
import { GuestRoute, ProtectedRoute } from './RouteGuards'

const AppRoutes = () => {
    const isAuthenticated = !!localStorage.getItem("token");
    // const [isAuthenticated, setIsAuthenticated] = useState(false);

  return (
    <BrowserRouter>
        <Routes>
            <Route element={<GuestRoute isAuthenticated={isAuthenticated}/>}>
                <Route path='/' element={<Loginpage />}/>
                <Route path='/register' element={<RegisterPage />}/>
            </Route>

            <Route element={<ProtectedRoute isAuthenticated={isAuthenticated}/>}>
                <Route path='/chat' element={<ChatPage />}/>
            </Route>
{/* 
            <Route path='/' element={<Loginpage />}/>
            <Route path='/chat' element={<ChatPage />}/>
            <Route path='/register' element={<RegisterPage />}/> */}
            <Route path='*' element={<p>Page not Found</p>}/>
        </Routes>
    </BrowserRouter>
  )
}

export default AppRoutes