import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CourseCard from '../CourseCard/CourseCard';
import './CoursesByCategory.css'; // Ensure this CSS file handles layout and responsiveness
import { useParams } from 'react-router-dom';

const CoursesByCategory = () => {
  const { categoryId } = useParams();
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const decodedCategory = decodeURIComponent(categoryId);
        const response = await axios.get(`https://localhost:7293/api/User/courses/get/${decodedCategory}`);
        setCourses(response.data.data);
        setLoading(false);
      } catch (err) {
        setError(err);
        setLoading(false);
      }
    };

    fetchCourses();
  }, [categoryId]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error fetching courses: {error.message}</p>;

  return (
    <div className="courses-container">
      {courses.length > 0 ? (
        courses.map(course => (
          <CourseCard key={course.courseId} course={course} />
        ))
      ) : (
        <p>No courses available for this category.</p>
      )}
    </div>
  );
};

export default CoursesByCategory;
