import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

// Utility function to check if user is logged in
const isLoggedIn = () => {
  // You can check token in local storage, cookies, or a global state
  return !!localStorage.getItem('token'); // Adjust as per your storage method
};

const Navbar = () => {
  const loggedIn = isLoggedIn(); // Check if the user is logged in

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container-fluid">
        <Link className="navbar-brand" to="/">Admin Dashboard</Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link" to="/courses">Courses</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/quizzes">Quizzes</Link>
            </li>
            {!loggedIn && (
              <li className="nav-item">
                <Link className="nav-link" to="/login">Login</Link>
              </li>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
