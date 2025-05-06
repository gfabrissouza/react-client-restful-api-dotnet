import './styles.css';
import logo from '../../assets/react.svg';
import api from '../../services/api';

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { FiPower, FiEdit, FiTrash2 } from 'react-icons/fi';

interface Book {
    id: number;
    title: string;
    author: string;
    price: number;
    launchDate: string;
}

export default function Books(){

    const [books, setBooks] = useState<Book[]>([]);
    const[page, setPage] = useState(0);

    const username = localStorage.getItem('userName');
    //const accessToken = localStorage.getItem('accessToken');

    // const authorization = {
    //     headers: {
    //         Authorization: `Bearer ${accessToken}`
    //     }
    // }

    const navigate = useNavigate();

    useEffect(() => {
        fetchMoreBooks();
    },[]);

    async function fetchMoreBooks(){
        const response = await api.get('/api/book/v1');
        setBooks(books => [...books, ...response.data]);
        setPage(page => page + 1);
    };

    async function logout(){
        try {
            await api.get('/api/auth/v1/revoke');
            localStorage.clear();
            navigate('/');
        } catch (error) {
            console.log(error);
            alert('Logout failed! Try Again!');
        }
    };

    async function editBook(id: number){
        try {
            navigate(`/new/${id}`);
        } catch (error) {
            alert('Delete failed! Try Again!');
        }
    };

    async function deleteBook(id: number){
        try {
            await api.delete(`/api/book/v1/${id}`);
            setBooks(books.filter(book => book.id !== id)); 
        } catch (error) {
            alert('Delete failed! Try Again!');
        }
    };

    return (
        <div className='book-container'>
            <header>
                <img src={logo} alt="Logo" />
                <span>Welcome, <strong>{username?.toUpperCase()}</strong></span>
                <Link className='button' to='/new/0'>Add New Book</Link>
                <button type='button' onClick={logout}>
                    <FiPower size={18} color='#251fc5' />
                </button>                    
            </header>

            <h1>Registered Books</h1>
            <ul>
                {books.map(book => (
                    <li key={book.id}>
                        <strong>Title:</strong>
                        <p>{book.title}</p>
                        <strong>Author:</strong>
                        <p>{book.author}</p>
                        <strong>Price:</strong>
                        <p>{Intl.NumberFormat('pt-BR', {style: 'currency', currency: 'BRL'}).format(book.price)}</p>
                        <strong>Release Date:</strong>
                        <p>{Intl.DateTimeFormat('pt-BR').format(new Date(book.launchDate))}</p>

                        <button type='button' onClick={() => {editBook(book.id)} }>
                            <FiEdit size={20} color='#251fc5'/>
                        </button>
                        <button type='button' onClick={() => { deleteBook(book.id)} }>
                            <FiTrash2 size={20} color='#251fc5'/>
                        </button>
                    </li>
                ))}
            </ul>
            <button className='button' type='button' onClick={fetchMoreBooks}>Load More</button>
        </div>
    );
};