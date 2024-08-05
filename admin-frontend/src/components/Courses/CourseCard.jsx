import React from 'react';
import PropTypes from 'prop-types';
import './CourseCard.css'; // For custom styles

const CourseCard = ({ course, onEdit, onDelete }) => {
  return (
    <div className="card course-card">
      <img src={course.courseImageUrl} className="card-img-top" alt={course.title} />
      <div className="card-body">
        <h5 className="card-title">{course.title}</h5>
        <p className="card-text">{course.description}</p>
        <p className="card-text"><strong>Category:</strong> {course.categoryName}</p>
        <p className="card-text"><strong>Price:</strong> ${course.price}</p>
        <p className="card-text"><strong>Instructor:</strong> {course.instructorName}</p>
        <div className="d-flex justify-content-between">
          <button className="btn btn-warning" onClick={onEdit}>Edit</button> &nbsp; &nbsp;
          <button className="btn btn-danger" onClick={onDelete}>Delete</button>
        </div>
      </div>
    </div>
  );
};

CourseCard.propTypes = {
  course: PropTypes.shape({
    courseId: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    categoryName: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    courseImageUrl: PropTypes.string.isRequired,
    image: PropTypes.string,
    instructorName: PropTypes.string.isRequired,
  }).isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default CourseCard;
