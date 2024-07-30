// index.js or main entry file
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router } from 'react-router-dom';
import App from './App';
import '../src/assets/styles/index.css'; // if you have any global styles

ReactDOM.render(
  <Router>
    <App />
  </Router>,
  document.getElementById('root')
);
