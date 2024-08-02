import React from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import './QuizCard.css'; // Ensure CSS file path is correct

const QuizCard = ({ quiz, onDelete }) => {
  const navigate = useNavigate();

  const handleManageClick = () => {
    navigate(`/manage-questions/${quiz.quizId}`);
  };

  return (
    <div className="quiz-card">
      <h3>{quiz.title}</h3>
      <div className="quiz-card-actions">
        <button className="btn btn-primary" onClick={handleManageClick}>
          Manage Questions
        </button> &nbsp; &nbsp;
        <button className="btn btn-danger" onClick={() => onDelete(quiz.quizId)}>
          Delete
        </button>
      </div>
    </div>
  );
};

// PropTypes for better type checking
QuizCard.propTypes = {
  quiz: PropTypes.shape({
    quizId: PropTypes.number.isRequired, // Ensure this matches the API response
    title: PropTypes.string.isRequired,
  }).isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default QuizCard;
