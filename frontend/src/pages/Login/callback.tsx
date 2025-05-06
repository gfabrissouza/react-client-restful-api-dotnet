import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import api from '../../services/api';
import useAuth from '../../hooks/useAuth';

export default function Callback() {
    const navigate = useNavigate();
    const { setAuthenticated } = useAuth();

    useEffect(() => {
        async function handleLogin() {
            try {
                const response = await api.get('/api/auth/v1/me');

                const userName = response.data.name;
                localStorage.setItem('userName', userName);
                
                setAuthenticated(true); 
                navigate('/books');
            } catch (error) {
                alert('Authentication failed! Try again!');
                navigate('/');
            }
        }

        handleLogin();
    }, []);

    return <p>Autenticando...</p>;
}