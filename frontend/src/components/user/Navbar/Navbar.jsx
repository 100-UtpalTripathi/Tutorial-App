import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

// Dummy function to simulate fetching courses based on query
const fetchCourses = async (query) => {
  // Replace with your actual API call
  const response = await fetch(`/api/courses?search=${query}`);
  const data = await response.json();
  return data;
};

const Navbar = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [suggestions, setSuggestions] = useState([]);

  useEffect(() => {
    if (searchQuery) {
      fetchCourses(searchQuery).then(data => {
        setSuggestions(data);
      });
    } else {
      setSuggestions([]);
    }
  }, [searchQuery]);

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <Link className="navbar-brand" to="/">Tutor Ninja</Link>
      <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span className="navbar-toggler-icon"></span>
      </button>
      <div className="collapse navbar-collapse" id="navbarNav">
        <ul className="navbar-nav mr-auto">
          <li className="nav-item">
            <Link className="nav-link" to="/">Home</Link>
          </li>
          <li className="nav-item dropdown">
            <Link className="nav-link dropdown-toggle" to="#" id="navbarDropdown" role="button">
              Categories
            </Link>
            <div className="dropdown-menu" aria-labelledby="navbarDropdown">
              <Link className="dropdown-item" to="/category1">Category 1</Link>
              <Link className="dropdown-item" to="/category2">Category 2</Link>
              <Link className="dropdown-item" to="/category3">Category 3</Link>
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
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;
