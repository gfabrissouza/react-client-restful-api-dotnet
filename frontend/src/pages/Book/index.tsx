import './styles.css';
import logo from '../../assets/react.svg';
import api from '../../services/api';

import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { FiArrowLeft } from 'react-icons/fi';

export default function Book(){

    const [id, setId] = useState(0);
    const [author, setAuthor] = useState('');
    const [title, setTitle] = useState('');
    const [launchDate, setLauchDate] = useState('');
    const [price, setPrice] = useState('');

    const { bookId } = useParams();

    const navigate = useNavigate();

    useEffect(() => {
        if (bookId === '0') return;
        else loadBook();
    }, [bookId]);

    async function loadBook(){
        try {
            const response = await api.get(`/api/book/v1/${bookId}`)

            let ajustedDate = response.data.launchDate.split('T')[0];

            setId(response.data.id);
            setAuthor(response.data.author);
            setTitle(response.data.title);
            setPrice(response.data.price);
            setLauchDate(ajustedDate);
        } catch (error) {
            alert('Error Recovering Book! Try Again!');
            navigate('/books');
        }
    }

    async function saveOrUpdateBook(event: React.FormEvent){
        event.preventDefault(); // avoid page reload

        const data = {
            id: id,
            title: title,
            author: author,
            launchDate: launchDate,
            price: price
        };

        try {
            if (bookId === '0'){ 
                await api.post('/api/book/v1', data);
            } else{
                await api.put('/api/book/v1', data);
            } 
        } catch (error) {
            alert('Error while recording book! Try again!');
        }

        navigate('/books');
    };

    return (
        <div className='new-book-container'>
            <div className='content'>
                <section className='form'>
                    <img src={logo} alt='React'/>
                    <h1>{bookId === '0' ? 'Add New' : 'Update'} Book</h1>
                    <p>Enter the Book Information and Click on {bookId === '0' ? `'Add'` : `'Update'`}</p>
                    <Link className='back-link' to='/books'>
                        <FiArrowLeft size={16} color='#251fc5'/>
                        Back to Books
                    </Link>
                </section>
                <form onSubmit={saveOrUpdateBook}>
                    <input 
                        placeholder='Title'
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                    />
                    <input 
                        placeholder='Author'
                        value={author}
                        onChange={e => setAuthor(e.target.value)}
                    />
                    <input 
                        type='date'
                        value={launchDate}
                        onChange={e => setLauchDate(e.target.value)}
                    />
                    <input 
                        placeholder='Price'
                        value={price}
                        onChange={e => setPrice(e.target.value)}
                    />

                    <button className='button' type='submit'>{bookId === '0' ? 'Add' : 'Update'}</button>
                </form>
            </div>
        </div>
    );
};