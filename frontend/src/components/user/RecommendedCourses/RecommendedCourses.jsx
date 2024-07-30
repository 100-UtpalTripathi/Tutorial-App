import React from 'react';
import CourseCard from '../CourseCard/CourseCard';
import './RecommendedCourses.css';

const RecommendedCourses = () => {
  // Mock data, replace with real data fetching logic
  const courses = [
    { id: 1, title: 'Course 1', description: 'Description 1' },
    { id: 2, title: 'Course 2', description: 'Description 2' },
    { id: 3, title: 'Course 3', description: 'Description 3' },
  ];

  return (
    <div className="row">
      {courses.map(course => (
        <div className="col-md-4" key={course.id}>
          <CourseCard course={course} />
        </div>
      ))}
    </div>
  );
};

export default RecommendedCourses;
