import React from "react";
import { Route, Routes } from "react-router-dom";
import Navbar from "./components/Navbar/Navbar"; // Updated to Navbar.jsx
import Dashboard from "./components/Dashboard/Dashboard";
import CoursesQuiz from "./components/Quizzes/CoursesQuiz"; // Updated to CoursesQuiz
import Quizzes from "./components/Quizzes/Quizzes";
import Login from "./components/Login/Login";
import ProtectedRoute from "./ProtectedRoute";
import Courses from "./components/Courses/Courses";
import ManageQuestions from "./components/Questions/ManageQuestions"; // Updated to ManageQuestions.jsx

const App = () => {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route
          path="/courses"
          element={
            <ProtectedRoute>
              <Courses/>
            </ProtectedRoute>
          }
        />
        <Route
          path="/quizzes/" // Updated to handle courseId as a parameter
          element={
            <ProtectedRoute>
              <CoursesQuiz />
            </ProtectedRoute>
          }
        />
        <Route
          path="/quizzes/:courseId" // Updated to handle courseId as a parameter
          element={
            <ProtectedRoute>
              <Quizzes />
            </ProtectedRoute>
          }
        />

        <Route path="/manage-questions/:quizId" 
          element={
            <ProtectedRoute>
              <ManageQuestions />
            </ProtectedRoute>
          }
        />
        <Route path="/login" element={<Login />} />
      </Routes>
    </>
  );
};

export default App;
