import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Quiz = () => {
  const { courseId } = useParams(); // Access the courseId parameter from the URL
  const [questions, setQuestions] = useState([]);
  const [answers, setAnswers] = useState({});
  const [showResults, setShowResults] = useState(false);
  const [result, setResult] = useState({ score: 0, total: 0 });

  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await axios.get(`https://localhost:7293/api/course/Quiz/get/${courseId}`);
        //console.log(response.data.data.questions);
        setQuestions(response.data.data.questions || []);
        
        if (response.data.data == null || response.data.data.questions.length === 0) {
          toast.error("No quiz available for this course.");
        }
      } catch (error) {
        console.error("Error fetching quiz:", error);
        toast.error("Failed to fetch quiz.");
      }
    };

    fetchQuiz();
  }, [courseId]);

  const handleAnswerChange = (questionId, answer) => {
    setAnswers((prev) => ({ ...prev, [questionId]: answer }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    evaluateResults();
    setShowResults(true); // Show results modal
  };

  const evaluateResults = () => {
    let score = 0;
    const total = questions.length;

    questions.forEach((question) => {
      if (question.correctAnswer === answers[question.questionId]) {
        score++;
      }
    });

    setResult({ score, total });
  };

  return (
    <div className="container mt-4">
      <h2>Check Your Knowledge : </h2>
      {questions.length > 0 ? (
        <form onSubmit={handleSubmit}>
          {questions.map((question) => (
            <div key={question.questionId} className="mb-3">
              <p>{question.text}</p>
              <div>
                {question.optionA && (
                  <div>
                    <input
                      type="radio"
                      id={`q${question.questionId}a`}
                      name={`q${question.questionId}`}
                      value={question.optionA}
                      onChange={() => handleAnswerChange(question.questionId, question.optionA)}
                    />
                    <label htmlFor={`q${question.questionId}a`}>{question.optionA}</label>
                  </div>
                )}
                {question.optionB && (
                  <div>
                    <input
                      type="radio"
                      id={`q${question.questionId}b`}
                      name={`q${question.questionId}`}
                      value={question.optionB}
                      onChange={() => handleAnswerChange(question.questionId, question.optionB)}
                    />
                    <label htmlFor={`q${question.questionId}b`}>{question.optionB}</label>
                  </div>
                )}
                {question.optionC && (
                  <div>
                    <input
                      type="radio"
                      id={`q${question.questionId}c`}
                      name={`q${question.questionId}`}
                      value={question.optionC}
                      onChange={() => handleAnswerChange(question.questionId, question.optionC)}
                    />
                    <label htmlFor={`q${question.questionId}c`}>{question.optionC}</label>
                  </div>
                )}
                {question.optionD && (
                  <div>
                    <input
                      type="radio"
                      id={`q${question.questionId}d`}
                      name={`q${question.questionId}`}
                      value={question.optionD}
                      onChange={() => handleAnswerChange(question.questionId, question.optionD)}
                    />
                    <label htmlFor={`q${question.questionId}d`}>{question.optionD}</label>
                  </div>
                )}
              </div>
            </div>
          ))}
          <button type="submit" className="btn btn-primary">Submit</button>
        </form>
      ) : (
        <p>No quiz available for this course.</p>
      )}

      {/* Results Modal */}
      <div className={`modal fade ${showResults ? 'show' : ''}`} tabIndex="-1" aria-labelledby="resultsModalLabel" aria-hidden="true" style={{ display: showResults ? 'block' : 'none' }}>
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="resultsModalLabel">Quiz Results</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <p>Your score: {result.score} out of {result.total}</p>
              <p>{result.score / result.total * 100}%</p> {/* Display percentage if needed */}
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-bs-dismiss="modal" onClick={() => setShowResults(false)}>Close</button>
            </div>
          </div>
        </div>
      </div>

      
    </div>
  );
};

export default Quiz;
