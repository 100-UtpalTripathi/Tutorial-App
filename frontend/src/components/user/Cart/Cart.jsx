import React, { useState, useEffect } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./Cart.css"; // Add your custom styles here

const Cart = () => {
  const [cartItems, setCartItems] = useState([]);
  const [selectedCourse, setSelectedCourse] = useState(null);
  const userEmail = localStorage.getItem("email");

  useEffect(() => {
    const fetchCartItems = async () => {
      try {
        const response = await axios.get(`https://tutorialappbackend.azurewebsites.net/api/user/Cart/get/${userEmail}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }

        );
        //console.log(response.data.data);
        setCartItems(response.data.data || []);
      } catch (error) {
        console.error("Error fetching cart items:", error);
      }
    };

    fetchCartItems();
  }, [userEmail]);

  const handleRemoveFromCart = async (courseId, price) => {
    try {
      await axios.delete("https://tutorialappbackend.azurewebsites.net/api/user/Cart/delete", {
        headers: { "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`
         },
        data: { userEmail, courseId, price }
      });
      setCartItems(cartItems.filter(item => item.courseId !== courseId));
      toast.success("Course removed from cart!");
    } catch (error) {
      console.error("Error removing course from cart:", error);
      toast.error("Failed to remove course from cart.");
    }
  };

  const handleBuy = (course) => {
    setSelectedCourse(course);
    const modal = new window.bootstrap.Modal(document.getElementById("buyModal"));
    modal.show();
  };

  const confirmPurchase = async () => {
    if (!selectedCourse) return;

    try {
      await axios.post("https://tutorialappbackend.azurewebsites.net/api/user/course/Enrollment/add", {
        userEmail,
        courseId: selectedCourse.courseId,
        status: "Registered"
      },
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
      );
      // Remove course from cart after successful purchase
      await handleRemoveFromCart(selectedCourse.courseId, selectedCourse.price);
      toast.success("Course purchased and removed from cart successfully!");
      setSelectedCourse(null);
      const modal = window.bootstrap.Modal.getInstance(document.getElementById("buyModal"));
      modal.hide();
    } catch (error) {
      console.error("Error purchasing course:", error);
      toast.error("Failed to purchase course.");
    }
  };

  return (
    <>
      <div className="container mt-4">
        <h2>Your Cart</h2>
        <div className="row">
          {cartItems.length > 0 ? (
            cartItems.map((course) => (
              <div key={course.courseId} className="col-md-4 mb-4">
                <div className="card course-card">
                  <img src={course.courseImageUrl} className="card-img-top" alt={course.title} />
                  <div className="card-body">
                    <h5 className="card-title">{course.title}</h5>
                    <p className="card-text">{course.description}</p>
                    <p className="card-text"><strong>Price:</strong> ${course.price}</p>
                    <div className="d-flex justify-content-between">
                      <button
                        className="btn btn-danger"
                        onClick={() => handleRemoveFromCart(course.courseId, course.price)}
                      >
                        Remove
                      </button>
                      &nbsp;
                      &nbsp;
                      <button
                        className="btn btn-primary"
                        onClick={() => handleBuy(course)}
                      >
                        Buy
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <p>No courses in your cart.</p>
          )}
        </div>
      </div>

      {/* Bootstrap Modal for Buy Confirmation */}
      <div className="modal fade" id="buyModal" tabIndex="-1" aria-labelledby="buyModalLabel" aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="buyModalLabel">Confirm Purchase</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              Are you sure you want to purchase <strong>{selectedCourse?.title}</strong>?
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="button" className="btn btn-primary" onClick={confirmPurchase}>Yes, Purchase</button>
            </div>
          </div>
        </div>
      </div>

      {/* Toast Container */}
      <ToastContainer />
    </>
  );
};

export default Cart;
