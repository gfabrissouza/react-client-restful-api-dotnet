import { getLogoutCallback } from '../context/AuthContext';

import axios from 'axios'

export const baseURL = 'https://localhost:443';

const api = axios.create({
    baseURL: baseURL,
    withCredentials: true
})

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response && error.response.status === 401) {
            const logout = getLogoutCallback();
            if (logout) logout(); // call logout using context
        }
        return Promise.reject(error);
    }
);

export async function refreshAccessToken(): Promise<boolean> {
    try {
        const response = await api.post('/api/auth/v1/refresh');
        return response.status === 204;
    } catch (error) {
        console.error('Error to try refresh access token:', error);
        const logout = getLogoutCallback();
        if (logout) logout(); // automatic fallback
        return false;
    }
}

export async function handleLogin() {
    try {
        const response = await api.get('/api/auth/v1/me');
        console.log(response);
    } catch (error) {
        console.log(error);
    }
}

export default api;