import React, { useState, useEffect } from "react";
import axios from "axios";
import CourseCard from "../CourseCard/CourseCard";
import "./CoursesByCategory.css"; // Ensure this CSS file handles layout and responsiveness
import { useParams } from "react-router-dom";

const CoursesByCategory = () => {
  const { categoryId } = useParams();
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Filter state
  const [searchTerm, setSearchTerm] = useState("");
  const [minPrice, setMinPrice] = useState(0);
  const [maxPrice, setMaxPrice] = useState(Infinity);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const decodedCategory = decodeURIComponent(categoryId);
        const response = await axios.get(
          `https://localhost:7293/api/User/courses/get/${decodedCategory}`
        );
        setCourses(response.data.data);
        setLoading(false);
      } catch (err) {
        setError(err);
        setLoading(false);
      }
    };

    fetchCourses();
  }, [categoryId]);

  // Filter logic
  const filteredCourses = courses.filter((course) => {
    return (
      (searchTerm === "" ||
        course.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
        course.instructorName
          .toLowerCase()
          .includes(searchTerm.toLowerCase())) &&
      course.price >= minPrice &&
      course.price <= maxPrice
    );
  });

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error fetching courses: {error.message}</p>;

  return (
    <>
    <br></br>
    <br></br>
      <div className="filters mb-3">
        <input
          type="text"
          placeholder="Search by title or instructor"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="form-control mb-2"
        />
        <div className="d-flex mb-2">
          <input
            type="number"
            placeholder="Min price"
            value={minPrice === 0 ? "" : minPrice}
            onChange={(e) =>
              setMinPrice(e.target.value ? parseInt(e.target.value) : 0)
            }
            className="form-control mr-3"
          />{" "}
          &nbsp; _ &nbsp;
          <input
            type="number"
            placeholder="Max price"
            value={maxPrice === Infinity ? "" : maxPrice}
            onChange={(e) =>
              setMaxPrice(e.target.value ? parseInt(e.target.value) : Infinity)
            }
            className="form-control ml-3"
          />
        </div>
      </div>
      <div className="courses-container">
        {filteredCourses.length > 0 ? (
          filteredCourses.map((course) => (
            <CourseCard key={course.courseId} course={course} />
          ))
        ) : (
          <p>No courses available for this category.</p>
        )}
      </div>
    </>
  );
};

export default CoursesByCategory;
