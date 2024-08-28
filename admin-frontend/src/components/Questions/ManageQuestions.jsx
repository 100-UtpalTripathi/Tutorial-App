import React, { useState, useEffect } from "react";
import axios from "axios";
import "./ManageQuestions.css"; // Import CSS for this component
import { useParams } from "react-router-dom";

const ManageQuestions = () => {
  const { quizId } = useParams();
  const [questions, setQuestions] = useState([]);
  const [formData, setFormData] = useState({
    quizId: parseInt(quizId),
    text: "",
    optionA: "",
    optionB: "",
    optionC: "",
    optionD: "",
    correctAnswer: "",
  });
  const [editingQuestion, setEditingQuestion] = useState(null);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchQuestions();
  }, [quizId]);

  const fetchQuestions = async () => {
    try {
      const response = await axios.get(
        `https://tutorialappbackend.azurewebsites.net/api/Question/get/${quizId}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }

      );
      //console.log(response.data.data);
      setQuestions(response.data.data);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching questions:", error);
      setError("Error fetching questions");
      setLoading(false);
    }
  };

  const handleCreateClick = () => {
    setEditingQuestion(null);
    setShowCreateForm(true);
  };

  const handleClose = () => {
    setShowCreateForm(false);
    setFormData({
      quizId: quizId,
      text: "",
      optionA: "",
      optionB: "",
      optionC: "",
      optionD: "",
      correctAnswer: "",
    });
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    const apiUrl = editingQuestion
      ? `https://tutorialappbackend.azurewebsites.net/api/Question/update/${editingQuestion.questionId}`
      : "https://tutorialappbackend.azurewebsites.net/api/Question/add";
  
    try {
      setFormData({ ...formData, quizId: parseInt(quizId) }); // Ensure quizId is an integer
  
      if (editingQuestion) {
        // Use PUT for updating
        await axios.put(apiUrl, formData, {
          headers: {
            'Content-Type': 'application/json', // Ensure JSON content type
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
          }
        });
      } else {
        // Use POST for creating
        await axios.post(apiUrl, formData, {
          headers: {
            'Content-Type': 'application/json', // Ensure JSON content type
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
          }
        });
      }
      
      fetchQuestions();
      handleClose();
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };
  
  
  const handleEdit = (question) => {
    setEditingQuestion(question);
    setFormData({
      text: question.text,
      optionA: question.optionA,
      optionB: question.optionB,
      optionC: question.optionC,
      optionD: question.optionD,
      correctAnswer: question.correctAnswer,
    });
    setShowCreateForm(true);
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`https://tutorialappbackend.azurewebsites.net/api/Question/delete/${id}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      fetchQuestions();
    } catch (error) {
      console.error("Error deleting question:", error);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <button className="btn btn-primary mb-3" onClick={handleCreateClick}>
        Add New Question
      </button>
      <div className="row">
        {questions.length > 0 ? (
          questions.map((question) => (
            <div className="col-md-4 mb-4" key={question.questionId}>
              <div className="question-card">
                <h4>{question.text}</h4>
                <p>Options:</p>
                <ul>
                  <li>A: {question.optionA}</li>
                  <li>B: {question.optionB}</li>
                  <li>C: {question.optionC}</li>
                  <li>D: {question.optionD}</li>
                </ul>
                <p>Correct Answer: {question.correctAnswer}</p>
                <button
                  className="btn btn-primary"
                  onClick={() => handleEdit(question)}
                >
                  Edit
                </button>
                <br />
                <br />
                <button
                  className="btn btn-danger"
                  onClick={() => handleDelete(question.questionId)}
                >
                  Delete
                </button>
              </div>
            </div>
          ))
        ) : (
          <div>No questions available</div>
        )}
      </div>

      {/* Modal for creating/editing a question */}
      {showCreateForm && (
        <div className="modal show d-block" tabIndex="-1" role="dialog">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {editingQuestion ? "Edit Question" : "Create New Question"}
                </h5>
                <button type="button" className="close" onClick={handleClose}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form onSubmit={handleSubmit}>
                  <div className="form-group">
                    <label htmlFor="formText">Question</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formText"
                      name="text"
                      value={formData.text}
                      onChange={handleInputChange}
                      placeholder="Enter question text"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formOptionA">Option A</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formOptionA"
                      name="optionA"
                      value={formData.optionA}
                      onChange={handleInputChange}
                      placeholder="Enter option A"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formOptionB">Option B</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formOptionB"
                      name="optionB"
                      value={formData.optionB}
                      onChange={handleInputChange}
                      placeholder="Enter option B"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formOptionC">Option C</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formOptionC"
                      name="optionC"
                      value={formData.optionC}
                      onChange={handleInputChange}
                      placeholder="Enter option C"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formOptionD">Option D</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formOptionD"
                      name="optionD"
                      value={formData.optionD}
                      onChange={handleInputChange}
                      placeholder="Enter option D"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formCorrectAnswer">Correct Answer</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formCorrectAnswer"
                      name="correctAnswer"
                      value={formData.correctAnswer}
                      onChange={handleInputChange}
                      placeholder="Enter correct answer"
                      required
                    />
                  </div>
                  <button type="submit" className="btn btn-primary mt-3">
                    {editingQuestion ? "Update Question" : "Create Question"}
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageQuestions;
