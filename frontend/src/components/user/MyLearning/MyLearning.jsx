import React, { useState, useEffect } from "react";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import CourseCard from "./CourseCard";
import "react-toastify/dist/ReactToastify.css";
import "./MyLearning.css"; // Add your custom styles here

const MyLearning = () => {
  const [enrolledCourses, setEnrolledCourses] = useState([]);
  const [selectedCourse, setSelectedCourse] = useState(null);
  const userEmail = localStorage.getItem("email");

  useEffect(() => {
    const fetchEnrolledCourses = async () => {
      try {
        const response = await axios.get(`https://localhost:7293/api/user/course/Enrollment/get/${userEmail}`);
        setEnrolledCourses(response.data.data || []);
      } catch (error) {
        console.error("Error fetching enrolled courses:", error);
        toast.error("Failed to fetch enrolled courses.");
      }
    };

    fetchEnrolledCourses();
  }, [userEmail]);

  const handleMarkAsCompleted = async (course) => {
    setSelectedCourse(course);
    const modal = new window.bootstrap.Modal(document.getElementById("completionModal"));
    modal.show();
  };

  const confirmCompletion = async () => {
    if (!selectedCourse) return;

    try {
      await axios.put("https://localhost:7293/api/user/course/Enrollment/status", {
        userEmail,
        courseId: selectedCourse.courseId,
        status: "Completed"
      });

      setEnrolledCourses(enrolledCourses.map(course =>
        course.courseId === selectedCourse.courseId
          ? { ...course, status: "Completed" }
          : course
      ));
      toast.success("Course marked as completed!");
      setSelectedCourse(null);
      const modal = window.bootstrap.Modal.getInstance(document.getElementById("completionModal"));
      modal.hide();
    } catch (error) {
      console.error("Error marking course as completed:", error);
      toast.error("Failed to mark course as completed.");
    }
  };

  return (
    <>
      <div className="container mt-4">
        <h2>Your Learnings</h2>
        <div className="row">
          {enrolledCourses.length > 0 ? (
            enrolledCourses.map((course) => (
              <div key={course.courseId} className="col-md-4 mb-4">
                <CourseCard
                  course={course}
                  onMarkAsCompleted={handleMarkAsCompleted}
                />
              </div>
            ))
          ) : (
            <p>No courses enrolled.</p>
          )}
        </div>
      </div>

      {/* Bootstrap Modal for Mark as Completed Confirmation */}
      <div className="modal fade" id="completionModal" tabIndex="-1" aria-labelledby="completionModalLabel" aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="completionModalLabel">Confirm Completion</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              Are you sure you want to mark <strong>{selectedCourse?.title}</strong> as completed?
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="button" className="btn btn-primary" onClick={confirmCompletion}>Yes, Mark as Completed</button>
            </div>
          </div>
        </div>
      </div>

      {/* Toast Container */}
      <ToastContainer />
    </>
  );
};

export default MyLearning;
