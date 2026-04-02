import { Navigate, Outlet } from "react-router-dom"

export const GuestRoute = ({ isAuthenticated } : { isAuthenticated : boolean}) => {
    if (isAuthenticated){
        return <Navigate to="/Chat" replace/>
    }

    return <Outlet />;
}

export const ProtectedRoute = ({isAuthenticated}:{isAuthenticated: boolean}) => {
    if (!isAuthenticated){
        return <Navigate to="login" replace/>
    }

    return <Outlet />;
}