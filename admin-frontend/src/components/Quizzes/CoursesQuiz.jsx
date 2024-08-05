import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CourseCard from './CourseCard'; // Ensure this is in the correct folder
import { useNavigate } from 'react-router-dom';
import './CoursesQuiz.css'; // Import CSS

const CoursesQuiz = () => {
  const [courses, setCourses] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');
  const [minPrice, setMinPrice] = useState(0);
  const [maxPrice, setMaxPrice] = useState(1000); // Example max value
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const navigate = useNavigate();

  const categories = [
    "Web Development",
    "Mobile Development",
    "Programming Languages",
    "IT & Software",
    "Game Development",
    "Personal Development",
    "Database Design",
    "Software Testing",
    "Cloud Automation",
    "DevOps",
    "Health & Fitness",
    "Music",
    "Teaching & Academics",
  ];

  useEffect(() => {
    fetchCourses();
  }, []);

  const fetchCourses = async () => {
    try {
      const response = await axios.get('https://localhost:7293/api/admin/Courses',
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      setCourses(response.data.data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching courses:', error);
      setError('Error fetching courses');
      setLoading(false);
    }
  };

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const handleCategoryChange = (e) => {
    setCategoryFilter(e.target.value);
  };

  const handleMinPriceChange = (e) => {
    setMinPrice(e.target.value);
  };

  const handleMaxPriceChange = (e) => {
    setMaxPrice(e.target.value);
  };

  const filteredCourses = courses.filter(course =>
    (course.title.toLowerCase().includes(searchTerm.toLowerCase()) || 
     course.instructorName.toLowerCase().includes(searchTerm.toLowerCase())) &&
    (categoryFilter === '' || course.categoryName === categoryFilter) &&
    (course.price >= minPrice && course.price <= maxPrice)
  );

  const handleCourseClick = (courseId) => {
    navigate(`/quizzes/${courseId}`);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <div className="filters">
        <input
          type="text"
          placeholder="Search by title or instructor"
          value={searchTerm}
          onChange={handleSearchChange}
        />
        <select value={categoryFilter} onChange={handleCategoryChange}>
          <option value="">All Categories</option>
          {categories.map(category => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
        </select>
        <input
          type="number"
          placeholder="Min Price"
          value={minPrice}
          onChange={handleMinPriceChange}
          min='0'
        />
        <input
          type="number"
          placeholder="Max Price"
          value={maxPrice}
          onChange={handleMaxPriceChange}
          min='0'
        />
      </div>

      <h4>Select a course to manage quizzes: </h4>
      <div className="row">
        
        {filteredCourses.length > 0 ? (
          filteredCourses.map(course => (
            <div className="col-md-4 mb-4" key={course.courseId}>
              <CourseCard
                course={course}
                onClick={() => handleCourseClick(course.courseId)}
              />
            </div>
          ))
        ) : (
          <div>No courses available</div>
        )}
      </div>
    </div>
  );
};

export default CoursesQuiz;
