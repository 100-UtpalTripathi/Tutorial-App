import React from "react";
import { Route, Routes } from "react-router-dom";
import Navbar from "./components/user/Navbar/Navbar";
import Home from "./components/user/Home/Home";
import MyLearning from "./components/user/MyLearning/MyLearning";
import Wishlist from "./components/user/Wishlist/Wishlist";
import Cart from "./components/user/Cart/Cart";
import Profile from "./components/user/Profile/Profile";
import Login from "./components/user/Login/Login";
import Signup from "./components/user/Signup/Signup";
import CourseDetails from "./components/user/CourseDetails/CourseDetails";
import CoursesByCategory from "./components/user/Navbar/CoursesByCategory"; // Import the new component
import ProtectedRoute from "./ProtectedRoute";

const App = () => {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/course/:id" element={<CourseDetails />} />
        <Route
          path="/mylearning"
          element={
            <ProtectedRoute>
              <MyLearning />
            </ProtectedRoute>
          }
        />
        <Route
          path="/wishlist"
          element={
            <ProtectedRoute>
              <Wishlist />
            </ProtectedRoute>
          }
        />
        <Route
          path="/cart"
          element={
            <ProtectedRoute>
              <Cart />
            </ProtectedRoute>
          }
        />
        <Route
          path="/profile"
          element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          }
        />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/category/:categoryId" element={<CoursesByCategory />} /> 
      </Routes>
    </>
  );
};

export default App;
