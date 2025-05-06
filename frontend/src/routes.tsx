import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from './pages/Login';
import Books from './pages/Books';
import Book from "./pages/Book";
import Callback from "./pages/Login/callback";
import { AuthProvider } from "./context/AuthContext";

export default function AppRoutes() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Login />} />
                    <Route path="/books" element={<Books />} />
                    <Route path="/new/:bookId" element={<Book />} />
                    <Route path="/callback" element={<Callback />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}
