# Tutorial App

## Features

### Admin Functionality

1. **Admin Login**
   - Admins can log in using email and password.

2. **Course Management (CRUD)**
   - Admins can create new courses, including title, description, and modules.
   - Admins can edit existing courses.
   - Admins can delete courses.
   - Admins can view a list of all courses.

### User Functionality

3. **Navigation**
   - **Home Page Navigation**
     - A brand logo in the navbar redirects to the home page.
   - **Search Bar**
     - Users can type in the search bar to see a dropdown of related courses.
     - Clicking on a suggested course redirects to that course's detail page.
   - **Categories**
     - A "Categories" option in the navbar shows all related courses from the selected category.

4. **Course Viewing and Purchasing**
   - **Course Details Page**
     - Users can click on a course to view its details and modules.
     - Users can purchase courses directly from the course details page.
   - **My Learning Menu**
     - Users have a "My Learning" menu item in the navbar.
     - Clicking on "My Learning" opens a page showing all purchased courses marked as registered, started, or completed.

5. **Course Completion and Quiz**
   - **Mark as Completed**
     - Users can mark a course as completed.
     - Upon marking as completed, a quiz is triggered to check the user's learning.
     - Users can view their quiz results after completion.

6. **Wishlist**
   - **Wishlist Icon**
     - A heart icon in the navbar shows all wishlisted courses.
     - Users can view wishlisted courses from the "My Learning" page as well.

7. **Cart**
   - **Cart Icon**
     - A cart icon in the navbar shows all courses in the cart.
     - Users can view and manage their cart contents.

8. **User Authentication and Profile**
   - **Login/Logout and Registration**
     - Navbar shows login/register options if the user is not authenticated (based on JWT).
     - Navbar shows logout and view profile options if the user is authenticated.
   - **View Profile**
     - Users can view and edit their profile information.

## Non-functional Requirements

9. **Security and Data Privacy**
   - User passwords should be securely hashed.
   - JWT should be used for secure authentication.
   - Ensure proper access control for admin functionalities.

10. **Usability**
    - The interface should be intuitive and easy to navigate.
    - Responsive design to ensure usability on various devices.

11. **Performance**
    - The application should load quickly and handle concurrent users efficiently.
