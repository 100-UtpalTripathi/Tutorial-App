import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './Wishlist.css'; // Import custom styles

const Wishlist = () => {
  const [wishlistedCourses, setWishlistedCourses] = useState([]);
  const userEmail = localStorage.getItem('email');

  useEffect(() => {
    const fetchWishlistedCourses = async () => {
      try {
        const response = await axios.get(`https://localhost:7293/api/user/course/Wishlist/get/${userEmail}`);
        setWishlistedCourses(response.data.data || []);
      } catch (error) {
        console.error('Error fetching wishlisted courses:', error);
        toast.error('Failed to fetch wishlisted courses.');
      }
    };

    fetchWishlistedCourses();
  }, [userEmail]);

  const handleRemoveFromWishlist = async (courseId) => {
    try {
      await axios.delete('https://localhost:7293/api/user/course/Wishlist/remove', {
        headers: { 'Content-Type': 'application/json' },
        data: { userEmail, courseId }
      });
      setWishlistedCourses(wishlistedCourses.filter(course => course.courseId !== courseId));
      toast.success('Course removed from wishlist!');
    } catch (error) {
      console.error('Error removing course from wishlist:', error);
      toast.error('Failed to remove course from wishlist.');
    }
  };

  return (
    <>
      <div className="container mt-4">
        <h1>Wishlist</h1>
        <div className="row">
          {wishlistedCourses.length > 0 ? (
            wishlistedCourses.map((course) => (
              <div key={course.courseId} className="col-md-4 mb-4">
                <div className="card course-card">
                  <img src={course.courseImageUrl} className="card-img-top" alt={course.title} />
                  <div className="card-body">
                    <h5 className="card-title">{course.title}</h5>
                    <p className="card-text">{course.description}</p>
                    <p className="card-text"><strong>Category:</strong> {course.categoryName}</p>
                    <p className="card-text"><strong>Price:</strong> ${course.price}</p>
                    <p className="card-text"><strong>Instructor:</strong> {course.instructorName}</p>
                    <button
                      className="btn btn-danger"
                      onClick={() => handleRemoveFromWishlist(course.courseId)}
                    >
                      Remove from Wishlist
                    </button>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <p>No courses in your wishlist.</p>
          )}
        </div>
      </div>
      <ToastContainer />
    </>
  );
};

export default Wishlist;
