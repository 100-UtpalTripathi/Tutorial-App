import React, { useState, useEffect } from "react";
import axios from "axios";
import CourseCard from "./CourseCard";
import "./Courses.css"; // For custom styles

const Courses = () => {
  const [courses, setCourses] = useState([]);
  const [show, setShow] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [selectedCourse, setSelectedCourse] = useState(null);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    categoryName: "",
    courseURL: "",
    price: 0,
    image: null,
    instructorName: "",
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Filter state
  const [searchTerm, setSearchTerm] = useState("");
  const [categoryFilter, setCategoryFilter] = useState("");
  const [minPrice, setMinPrice] = useState(0);
  const [maxPrice, setMaxPrice] = useState(Infinity);

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(3);

  const categories = [
    "Web Development",
    "Mobile Development",
    "Programming Languages",
    "IT & Software",
    "Game Development",
    "Personal Development",
    "Database Design",
    "Software Testing",
    "Cloud Automation",
    "DevOps",
    "Health & Fitness",
    "Music",
    "Teaching & Academics",
  ];

  useEffect(() => {
    fetchCourses();
  }, []);

  const fetchCourses = async () => {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.get(
        "https://tutorialappbackend.azurewebsites.net/api/admin/Courses",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      setCourses(response.data.data);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching courses:", error);
      setError("Error fetching courses");
      setLoading(false);
    }
  };

  const handleShow = (course = null) => {
    if (course) {
      setEditMode(true);
      setSelectedCourse(course);
      setFormData({
        title: course.title,
        description: course.description,
        categoryName: course.categoryName,
        courseURL: course.courseURL,
        price: course.price,
        image: null,
        instructorName: course.instructorName,
      });
    } else {
      setEditMode(false);
      setSelectedCourse(null);
      setFormData({
        title: "",
        description: "",
        categoryName: "",
        courseURL: "",
        price: 0,
        image: null,
        instructorName: "",
      });
    }
    setShow(true);
  };

  const handleClose = () => setShow(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleFileChange = (e) => {
    setFormData({
      ...formData,
      image: e.target.files[0],
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const data = new FormData();
    data.append("title", formData.title);
    data.append("description", formData.description);
    data.append("categoryName", formData.categoryName);
    data.append("courseURL", formData.courseURL);
    data.append("price", formData.price);
    data.append("image", formData.image);
    data.append("instructorName", formData.instructorName);

    try {
      if (editMode) {
        await axios.put(
          `https://tutorialappbackend.azurewebsites.net/api/admin/Courses/update/${selectedCourse.courseId}`,
          data,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              "Authorization": `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
      } else {
        await axios.post(
          "https://tutorialappbackend.azurewebsites.net/api/admin/Courses/create",
          data,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              "Authorization": `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
      }
      fetchCourses();
      handleClose();
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(
        `https://tutorialappbackend.azurewebsites.net/api/admin/Courses/delete/${id}`, {
          headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );
      fetchCourses();
    } catch (error) {
      console.error("Error deleting course:", error);
    }
  };

  // Filter logic
  const filteredCourses = courses.filter((course) => {
    return (
      (searchTerm === "" ||
        course.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
        course.instructorName
          .toLowerCase()
          .includes(searchTerm.toLowerCase())) &&
      (categoryFilter === "" || course.categoryName === categoryFilter) &&
      course.price >= minPrice &&
      course.price <= maxPrice
    );
  });

  // Pagination logic
  const indexOfLastCourse = currentPage * itemsPerPage;
  const indexOfFirstCourse = indexOfLastCourse - itemsPerPage;
  const currentCourses = filteredCourses.slice(indexOfFirstCourse, indexOfLastCourse);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  // Calculate total pages
  const totalPages = Math.ceil(filteredCourses.length / itemsPerPage);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <div className="filters mb-3">
        <input
          type="text"
          placeholder="Search by title or instructor"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="form-control mb-2"
        />
        <select
          value={categoryFilter}
          onChange={(e) => setCategoryFilter(e.target.value)}
          className="form-control mb-2"
        >
          <option value="">All Categories</option>
          {categories.map((cat, index) => (
            <option key={index} value={cat}>
              {cat}
            </option>
          ))}
        </select>
        <div className="d-flex mb-2">
          <input
            type="number"
            placeholder="Min price"
            value={minPrice === 0 ? "" : minPrice}
            onChange={(e) =>
              setMinPrice(e.target.value ? parseInt(e.target.value) : 0)
            }
            min='0'
            className="form-control mr-3"
          /> &nbsp; _ &nbsp;
          <input
            type="number"
            placeholder="Max price"
            value={maxPrice === Infinity ? "" : maxPrice}
            onChange={(e) =>
              setMaxPrice(e.target.value ? parseInt(e.target.value) : Infinity)
            }
            min='0'
            className="form-control ml-3"
          />
        </div>
      </div>
            &nbsp;&nbsp; &nbsp;
      <button className="btn btn-primary mb-3" onClick={() => handleShow()}>
        Add New Course
      </button>
      <div className="row">
        {Array.isArray(currentCourses) && currentCourses.length > 0 ? (
          currentCourses.map((course) => (
            <div className="col-md-4 mb-4" key={course.courseId}>
              <CourseCard
                course={course}
                onEdit={() => handleShow(course)}
                onDelete={() => handleDelete(course.courseId)}
              />
            </div>
          ))
        ) : (
          <div>No courses available</div>
        )}
      </div>

      {/* Pagination */}
      <nav aria-label="Page navigation">
        <ul className="pagination">
          <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
            <button className="page-link" onClick={() => paginate(currentPage - 1)}>&laquo;</button>
          </li>
          {[...Array(totalPages).keys()].map(number => (
            <li key={number + 1} className={`page-item ${currentPage === number + 1 ? 'active' : ''}`}>
              <button className="page-link" onClick={() => paginate(number + 1)}>{number + 1}</button>
            </li>
          ))}
          <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
            <button className="page-link" onClick={() => paginate(currentPage + 1)}>&raquo;</button>
          </li>
        </ul>
      </nav>

      {/* Modal for form */}
      {show && (
        <div className="modal show d-block" tabIndex="-1" role="dialog">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {editMode ? "Edit Course" : "Add New Course"}
                </h5>
                <button type="button" className="close" onClick={handleClose}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form onSubmit={handleSubmit}>
                  <div className="form-group">
                    <label htmlFor="formTitle">Title</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formTitle"
                      name="title"
                      value={formData.title}
                      onChange={handleInputChange}
                      placeholder="Enter title"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formDescription">Description</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formDescription"
                      name="description"
                      value={formData.description}
                      onChange={handleInputChange}
                      placeholder="Enter description"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formCategory">Category</label>
                    <select
                      className="form-control"
                      id="formCategory"
                      name="categoryName"
                      value={formData.categoryName}
                      onChange={handleInputChange}
                      required
                    >
                      <option value="">Select category</option>
                      {categories.map((cat, index) => (
                        <option key={index} value={cat}>
                          {cat}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="form-group">
                    <label htmlFor="formCourseURL">Course URL</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formCourseURL"
                      name="courseURL"
                      value={formData.courseURL}
                      onChange={handleInputChange}
                      placeholder="Enter course URL"
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formPrice">Price</label>
                    <input
                      type="number"
                      className="form-control"
                      id="formPrice"
                      name="price"
                      value={formData.price}
                      onChange={handleInputChange}
                      placeholder="Enter price"
                      min='0'
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formImage">Image</label>
                    <input
                      type="file"
                      className="form-control"
                      id="formImage"
                      name="image"
                      onChange={handleFileChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="formInstructorName">Instructor Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formInstructorName"
                      name="instructorName"
                      value={formData.instructorName}
                      onChange={handleInputChange}
                      placeholder="Enter instructor name"
                      required
                    />
                  </div>
                  <button type="submit" className="btn btn-primary mt-3">
                    {editMode ? "Update Course" : "Add Course"}
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Courses;
