import { useEffect } from 'react';
import { handleLogin } from '../../services/api';
//import { useNavigate } from 'react-router-dom';

//import api from '../../services/api';
//import useAuth from '../../hooks/useAuth';

export default function Callback() {
    //const navigate = useNavigate();
    //const { setAuthenticated } = useAuth();

    useEffect(() => {
        handleLogin();
    }, []);

    return <p>Autenticando...</p>;
}