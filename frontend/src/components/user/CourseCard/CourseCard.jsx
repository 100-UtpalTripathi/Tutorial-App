import React, { useState, useEffect } from "react";
import axios from "axios";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css'; // Import CSS for react-toastify
import "./CourseCard.css"; // Create this CSS file for card-specific styles

const CourseCard = ({ course }) => {
  const [isWishlisted, setIsWishlisted] = useState(false);
  const userEmail = localStorage.getItem("email");

  useEffect(() => {
    const checkWishlistStatus = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7293/api/user/course/Wishlist/get/${userEmail}`
        );
        const wishlistedCourses = response.data.data;
        if (!wishlistedCourses) return;
        setIsWishlisted(
          wishlistedCourses.some((c) => c.courseId === course.courseId)
        );
      } catch (error) {
        console.error("Error fetching wishlist status:", error);
      }
    };

    checkWishlistStatus();
  }, [course.courseId, userEmail]);

  const handleWishlistToggle = async () => {
    try {
      if (isWishlisted) {
        await axios.delete(
          "https://localhost:7293/api/user/course/Wishlist/remove",
          {
            headers: {
              "Content-Type": "application/json",
            },
            data: {
              userEmail: userEmail,
              courseId: course.courseId,
            },
          }
        );
        toast.success('Course removed from wishlist!');
      } else {
        await axios.post(
          "https://localhost:7293/api/user/course/Wishlist/add",
          {
            userEmail,
            courseId: course.courseId,
          }
        );
        toast.success('Course added to wishlist!');
      }
      setIsWishlisted(!isWishlisted);
    } catch (error) {
      console.error("Error toggling wishlist status:", error);
      toast.error('Error updating wishlist status.');
    }
  };

  const handleAddToCart = async () => {
    try {
      const response = await axios.post("https://localhost:7293/api/user/Cart/add", {
        userEmail,
        courseId: course.courseId,
        price: course.price,
      },
      {
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );
      if(response.data.data)
        toast.success('Course added to cart!');
      else
        toast.error('Course already in cart!');
    } catch (error) {
      console.error("Error adding course to cart:", error);
      toast.error('Error adding course to cart.');
    }
  };

  return (
    <div className="card course-card">
      <img
        src={course.courseImageUrl}
        className="card-img-top"
        alt={course.title}
      />
      <div className="card-body">
        <h5 className="card-title">{course.title}</h5>
        <p className="card-text">{course.description}</p>
        <p className="card-text">
          <strong>Category:</strong> {course.categoryName}
        </p>
        <p className="card-text">
          <strong>Price:</strong> ${course.price}
        </p>
        <p className="card-text">
          <strong>Instructor:</strong> {course.instructorName}
        </p>
        <div className="d-flex justify-content-between align-items-center">
          <button
            className={`wishlist-btn ${isWishlisted ? "wishlisted" : ""}`}
            onClick={handleWishlistToggle}
          >
            {isWishlisted ? "❤️" : "🤍"}
          </button>{" "}
          &nbsp; &nbsp;
          <button className="btn btn-primary" onClick={handleAddToCart}>
            Add to Cart
          </button>
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
