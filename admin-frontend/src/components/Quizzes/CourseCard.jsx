import React from 'react';
import './CourseCard.css'; // Use appropriate CSS file

const CourseCard = ({ course, onClick }) => {
  return (
    <div className="course-card" onClick={() => onClick(course.courseId)}>
      <h3>{course.title}</h3>
      <p>{course.instructorName}</p>
      <p>Price: ${course.price}</p>
      <p>Category: {course.categoryName}</p>
      <button className="btn btn-primary">Manage Quizzes</button>
    </div>
  );
};

export default CourseCard;
