// src/contexts/AuthContext.js
import React, { createContext, useState, useEffect } from 'react';

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState({
        token: localStorage.getItem('token') || null,
        email: localStorage.getItem('email') || null,
        role: localStorage.getItem('role') || null,
    });

    useEffect(() => {
        if (auth.token) {
            localStorage.setItem('token', auth.token);
            localStorage.setItem('email', auth.email);
            localStorage.setItem('role', auth.role);
        } else {
            localStorage.removeItem('token');
            localStorage.removeItem('email');
            localStorage.removeItem('role');
        }
    }, [auth]);

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    );
};

export { AuthContext, AuthProvider };
