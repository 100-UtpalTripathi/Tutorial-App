import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Navbar from './components/user/Navbar/Navbar';
import Home from './components/user/Home/Home';
import MyLearning from './components/user/MyLearning/MyLearning';
import Wishlist from './components/user/Wishlist/Wishlist';
import Cart from './components/user/Cart/Cart';
import Profile from './components/user/Profile/Profile';
import Login from './components/user/Login/Login';
import Signup from './components/user/Signup/Signup';

const App = () => {
  return (
    <div>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/mylearning" element={<MyLearning />} />
        <Route path="/wishlist" element={<Wishlist />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/profile" element={<Profile />} />
      </Routes>
    </div>
  );
}

export default App;
