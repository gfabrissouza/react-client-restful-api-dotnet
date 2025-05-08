import { getLogoutCallback } from '../context/AuthContext';

import axios from 'axios'

export const baseURL = 'https://fabris-api-server.azurewebsites.net'; //'https://localhost:443';

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

export default api;