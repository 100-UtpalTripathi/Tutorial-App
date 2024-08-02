import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Dashboard.css'; // Import the CSS file for styling

const Dashboard = () => {
  const [coursesCount, setCoursesCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCoursesCount();
  }, []);

  const fetchCoursesCount = async () => {
    try {
      const response = await axios.get('https://localhost:7293/api/admin/Courses');
      setCoursesCount(response.data.data.length); // Assuming the API returns an array of courses
      setLoading(false);
    } catch (error) {
      console.error('Error fetching courses:', error);
      setError('Error fetching courses');
      setLoading(false);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="dashboard">
      
      <div className="dashboard-overview">
        <div className="dashboard-card">
          <h2>Courses</h2>
          <p>Total Courses Registered: {coursesCount}</p>
        </div>
        {/* Other dashboard cards */}
      </div>
      {/* Other dashboard sections */}
    </div>
  );
};

export default Dashboard;
