import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Dashboard.css'; // Import the CSS file for styling

const Dashboard = () => {
  const [coursesCount, setCoursesCount] = useState(0);
  const [enrollments, setEnrollments] = useState([]);
  const [courses, setCourses] = useState([]); // Add state for courses
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCoursesCount();
    fetchEnrollments();
    fetchCourses(); // Fetch courses data
  }, []);

  const fetchCoursesCount = async () => {
    try {
      const response = await axios.get('https://localhost:7293/api/admin/Courses', {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });
      setCoursesCount(response.data.data.length); // Assuming the API returns an array of courses
    } catch (error) {
      console.error('Error fetching courses:', error);
      setError('Error fetching courses');
    }
  };

  const fetchEnrollments = async () => {
    try {
      const userEmail = 'example@example.com'; // Replace with actual user email
      const response = await axios.get(`https://localhost:7293/api/user/course/Enrollment/get/${userEmail}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });
      setEnrollments(response.data.data || []); // Set empty array if data is null
    } catch (error) {
      console.error('Error fetching enrollments:', error);
      setError('Error fetching enrollments');
    }
  };

  const fetchCourses = async () => {
    try {
      const response = await axios.get('https://localhost:7293/api/admin/Courses', {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });
      setCourses(response.data.data || []); // Set empty array if data is null
    } catch (error) {
      console.error('Error fetching courses:', error);
      setError('Error fetching courses');
    } finally {
      setLoading(false);
    }
  };

  // Category statistics calculation
  const categories = [
    'Web Development',
    'Mobile Development',
    'Programming Languages',
    'IT & Software',
    'Game Development',
    'Personal Development',
    'Database Design',
    'Software Testing',
    'Cloud Automation',
    'DevOps',
    'Health & Fitness',
    'Music',
    'Teaching & Academics'
  ];

  const calculateCategoryStats = (enrollments, courses) => {
    const stats = {};

    // Initialize stats object
    categories.forEach(category => {
      stats[category] = { courseCount: 0, enrollmentCount: 0 };
    });

    // Calculate course counts
    courses.forEach(course => {
      const categoryName = course.categoryName || 'Uncategorized';
      if (stats[categoryName]) {
        stats[categoryName].courseCount += 1;
      }
    });

    // Calculate enrollment counts
    enrollments.forEach(enrollment => {
      const categoryName = enrollment.categoryName || 'Uncategorized';
      if (stats[categoryName]) {
        stats[categoryName].enrollmentCount += 1;
      }
    });

    return stats;
  };

  const categoryStats = calculateCategoryStats(enrollments, courses);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="dashboard">
      <div className="dashboard-overview">
        <div className="dashboard-card card bg-dark text-light">
          <div className="card-body">
            <h5 className="card-title">Total Courses Registered</h5>
            <p className="card-text">{coursesCount}</p>
            <h5 className="card-title">Total Enrollments done</h5>
            <p className="card-text">{enrollments.length}</p>
          </div>
        </div>
        {categories.map(category => (
          <div key={category} className="dashboard-card card bg-dark text-light">
            <div className="card-body">
              <h5 className="card-title">{category}</h5>
              <p className="card-text">Courses: {categoryStats[category]?.courseCount || 0}</p>
              <p className="card-text">Enrollments: {categoryStats[category]?.enrollmentCount || 0}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Dashboard;
