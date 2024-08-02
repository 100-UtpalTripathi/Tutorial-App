// index.js or main entry file
import React from 'react';
import ReactDOM from "react-dom/client";
import { BrowserRouter as Router } from 'react-router-dom';
import App from './App';
import '../src/assets/styles/index.css'; // if you have any global styles
import { AuthProvider } from './contexts/AuthContext';

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <Router>
      <AuthProvider>
        <App />
      </AuthProvider>
    </Router>  
  </React.StrictMode>
);