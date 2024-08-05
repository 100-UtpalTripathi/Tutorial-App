import React, { useState, useEffect } from 'react';
import axios from 'axios';
import QuizCard from './QuizCard'; // Ensure this is in the correct folder
import './Quizzes.css'; // Import CSS
import { useParams } from 'react-router-dom';

const Quizzes = () => {
  const { courseId } = useParams();
  const [quiz, setQuiz] = useState(null);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [formData, setFormData] = useState({ title: '', courseId: courseId });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchQuiz();
  }, [courseId]);

  const fetchQuiz = async () => {
    try {
      const response = await axios.get(`https://localhost:7293/api/course/Quiz/get/${courseId}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      //console.log(response.data.data);
      setQuiz(response.data.data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching quiz:', error);
      setError('Error fetching quiz');
      setLoading(false);
    }
  };

  const handleCreateClick = () => {
    setShowCreateForm(true);
  };

  const handleClose = () => {
    setShowCreateForm(false);
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
    try {
      await axios.post('https://localhost:7293/api/course/Quiz/create', formData,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      fetchQuiz();
      handleClose();
    } catch (error) {
      console.error('Error creating quiz:', error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`https://localhost:7293/api/course/Quiz/delete/${id}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      fetchQuiz();
    } catch (error) {
      console.error('Error deleting quiz:', error);
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
      {quiz ? (
        <div className="row">
          <div className="col-md-4 mb-4">
            <QuizCard quiz={quiz} onDelete={() => handleDelete(quiz.quizId)} />
          </div>
        </div>
      ) : (
        <button className="btn btn-primary mb-3" onClick={handleCreateClick}>
          Create Quiz
        </button>
      )}

      {/* Modal for creating a quiz */}
      {showCreateForm && (
        <div className="modal show d-block" tabIndex="-1" role="dialog">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Create New Quiz</h5>
                <button type="button" className="close" onClick={handleClose}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form onSubmit={handleSubmit}>
                  <div className="form-group">
                    <label htmlFor="formTitle">Title</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formTitle"
                      name="title"
                      value={formData.title}
                      onChange={handleInputChange}
                      placeholder="Enter quiz title"
                      required
                    />
                  </div>
                  <button type="submit" className="btn btn-primary mt-3">
                    Create Quiz
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

export default Quizzes;
