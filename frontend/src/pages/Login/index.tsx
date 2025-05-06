import './styles.css';
import logo from '../../assets/react.svg';
import api, { baseURL } from '../../services/api';

import useAuth from '../../hooks/useAuth';

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Login() {

    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();
    const { setAuthenticated } = useAuth();

    async function login(event: React.FormEvent) {
        event.preventDefault(); // evita o reload

        const data = {
            username: username,
            password: password
        };
        
        try {
            //const response = await api.post('/api/auth/v1/signin', data);

            await api.post('/api/auth/v1/signin', data);

            const userResponse = await api.get('/api/auth/v1/me');

            const userName = userResponse.data.name;
            localStorage.setItem('userName', userName);
            //localStorage.setItem('accessToken', response.data.accessToken);
            //localStorage.setItem('refreshToken', response.data.refreshToken);

            setAuthenticated(true);
            navigate('/books');
        } catch (error) {
            alert('Login failed! Try again!');
        }
    };

    async function googleAuthentication(){
        try {
            window.location.href = 'https://localhost:443/api/auth/v1/start'; //`${baseURL}/api/auth/v1/start`;
        } catch (error) {
            alert('Google authentication failed!');
        }
    }

    return (
        <div className='login-container'>
            <section className='form'>
                <img src={logo} alt='React Logo'/>
                <form onSubmit={login}>
                    <h1>
                        Access your account
                    </h1>

                    <input 
                        placeholder='Username'
                        value={username}
                        onChange={e => setUserName(e.target.value)}
                    />
                    <input 
                        type='password' 
                        placeholder='Password'
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                    />

                    <button 
                        className='button' 
                        type='submit'
                    >
                        Login
                    </button>
                </form>
                <button 
                    style={{ background: '#000000', padding: '0 66px' }}
                    className='button' 
                    type='button'
                    onClick={googleAuthentication}
                >
                    <img 
                        src="https://www.svgrepo.com/show/475656/google-color.svg" 
                        alt="Google Icon" 
                        style={{ width: '25px', height: '25px', marginRight: '22px'}} 
                    />
                    Entrar com Google
                </button>
            </section>
        </div>
    );
};