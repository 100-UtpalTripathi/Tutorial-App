import React from "react";
import { Link } from "react-router-dom";
import "./CourseCard.css"; // Import the CSS for styling

const CourseCard = ({ course, onMarkAsCompleted, onTakeQuiz }) => {
  const isCompleted = course.status === "Completed";

  return (
    <div className="card course-card">
      <img src={course.courseImageUrl} className="card-img-top" alt={course.title} />
      <div className="card-body">
        <h5 className="card-title">{course.title}</h5>
        <p className="card-text">{course.description}</p>
        <p className="card-text"><strong>Instructor:</strong> {course.instructorName}</p>
        <div className="d-flex justify-content-between">
          {isCompleted ? (
            <Link to={`/quiz/${course.courseId}`} className="btn btn-primary">
              Take Quiz
            </Link>
          ) : (
            <button
              className="btn btn-success"
              onClick={() => onMarkAsCompleted(course)}
            >
              Mark as Completed
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
