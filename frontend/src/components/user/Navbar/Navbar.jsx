import React, { useState, useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Navbar.css';
import { AuthContext } from '../../../contexts/AuthContext';

const categories = [
  'Web Development',
  'Mobile Development',
  'Programming Languages',
  'IT & Software',
  'Game Development',
  'Personal Development',
  'Database Design',
  'Software Testing',
  'Cloud Automation',
  'DevOps',
  'Health & Fitness',
  'Music',
  'Teaching & Academics'
];

const Navbar = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [suggestions, setSuggestions] = useState([]);
  const { auth, setAuth } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchSuggestions = async () => {
      if (searchQuery) {
        try {
          const response = await axios.get(`/api/courses?search=${searchQuery}`);
          setSuggestions(response.data);
        } catch (error) {
          console.error('Error fetching courses:', error);
          setSuggestions([]);
        }
      } else {
        setSuggestions([]);
      }
    };

    fetchSuggestions();
  }, [searchQuery]);

  const handleLogout = () => {
    // Clear authentication data
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('role');
    setAuth({});  // Clear context auth
    navigate('/'); // Redirect to login page

    // Close the modal programmatically
    const modal = document.getElementById('logoutModal');
    if (modal) {
      const bsModal = window.bootstrap.Modal.getInstance(modal);
      if (bsModal) {
        bsModal.hide();
      }
    }
  };

  const handleLogoutClick = () => {
    const modal = new window.bootstrap.Modal(document.getElementById('logoutModal'));
    modal.show();
  };

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <Link className="navbar-brand" to="/">Tutor Ninja</Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav mr-auto">
            <li className="nav-item active">
              <Link className="nav-link" to="/">Home</Link>
            </li>
            <li className="nav-item dropdown">
              <Link className="nav-link dropdown-toggle" to="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Categories
              </Link>
              <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                {categories.map((category, index) => (
                  <Link 
                    key={index} 
                    className="dropdown-item" 
                    to={`/category/${encodeURIComponent(category)}`} // Encodes spaces as %20
                  >
                    {category}
                  </Link>
                ))}
              </div>
            </li>
          </ul>
          <form className="form-inline my-2 my-lg-0">
            <input 
              className="form-control mr-sm-2" 
              type="search" 
              placeholder="Search" 
              aria-label="Search"
              value={searchQuery}
              onChange={e => setSearchQuery(e.target.value)}
            />
            {suggestions.length > 0 && (
              <div className="search-suggestions">
                {suggestions.map(course => (
                  <Link 
                    key={course.id} 
                    to={`/course/${course.id}`} 
                    className="suggestion-item"
                  >
                    {course.title}
                  </Link>
                ))}
              </div>
            )}
          </form>
          <ul className="navbar-nav ml-auto">
            {auth.token ? (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/mylearning">My Learning</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/wishlist"><i className="bi bi-heart"></i></Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/cart"><i className="bi bi-cart"></i></Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/profile">Profile</Link>
                </li>
                <li className="nav-item">
                  <button className="nav-link btn btn-link" onClick={handleLogoutClick}>Logout</button>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <Link className="nav-link" to="/login">Login</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to="/signup">Register</Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </nav>

      {/* Logout Confirmation Modal */}
      <div className="modal fade" id="logoutModal" tabIndex="-1" aria-labelledby="logoutModalLabel" aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="logoutModalLabel">Confirm Logout</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              Are you sure you want to logout?
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="button" className="btn btn-primary" onClick={handleLogout}>Logout</button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Navbar;
