import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { handleLogin, refreshAccessToken } from '../services/api';

interface AuthContextData {
    isAuthenticated: boolean;
    setAuthenticated: (value: boolean) => void;
    logout: () => void;
}

let logoutCallback: (() => void) | null = null;

const AuthContext = createContext<AuthContextData | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        if (!isAuthenticated) return;

        const intervalId = setInterval(() => {
            refreshAccessToken();
        }, 10 * 60 * 1000);
        return () => clearInterval(intervalId);
    }, [isAuthenticated]);

    useEffect(() => {
        handleLogin();
        if (!logoutCallback) {
            registerLogoutCallback(logout);
        }
    }, []);

    const logout = () => {
        localStorage.clear(); 
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, setAuthenticated: setIsAuthenticated, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

function registerLogoutCallback(cb: () => void) {
    logoutCallback = cb;
}

export function getLogoutCallback() {
    return logoutCallback;
}

export function useAuthContext() {
    const context = useContext(AuthContext);
    if (!context) throw new Error('useAuthContext must be used inside AuthProvider');
    return context;
}
