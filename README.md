# Tutorial App

## Features

### Admin Functionality

#### Admin Login
- Admins can log in using email and password.

#### Course Management (CRUD)
- Create, edit, and delete courses, including managing titles, descriptions, and modules.
- View a list of all courses.

#### Quiz Management
- Create, edit, and delete self-evaluation quizzes related to courses.
- Manage quiz questions by adding, editing, and deleting them.
- View all quizzes associated with a specific course.

### User Functionality

#### Navigation

- **Home Page Navigation**  
  - A brand logo in the navbar redirects to the home page.

- **Search Bar**  
  - Type in the search bar to see a dropdown of related courses.
  - Click on a suggested course to view its details.

- **Categories**  
  - Browse courses by category using the "Categories" option in the navbar.

#### Course Viewing and Purchasing

- **Course Details Page**  
  - View detailed information and modules of a course.
  - Purchase courses directly from the course details page.

- **My Learning Menu**  
  - Access purchased courses marked as registered, started, or completed.

#### Course Completion and Quiz

- **Mark as Completed**  
  - Mark a course as completed to trigger a quiz.
  - Take quizzes after course completion and view your performance results.

#### Wishlist

- **Wishlist Icon**  
  - View all wishlisted courses using the heart icon in the navbar.
  - Access wishlisted courses from the "My Learning" page.

#### Cart

- **Cart Icon**  
  - View and manage courses in the cart.

#### User Authentication and Profile

- **Login/Logout and Registration**  
  - The navbar shows login/register options if the user is not authenticated (based on JWT).
  - If authenticated, the navbar shows logout and view profile options.

- **View Profile**  
  - View and edit your profile information.

#### Business Logic

- **Course Progress Tracking**  
  - Track user progress as registered, started, or completed.

- **Discounts**  
  - Earn a 10% discount on your next purchase after completing 3 courses.

### Non-functional Requirements

#### Security and Data Privacy

- Securely hash user passwords.
- Use JWT for secure authentication.
- Implement proper access control for admin functionalities.

#### Usability

- Design an intuitive and easy-to-navigate interface.
- Ensure a responsive design for usability on various devices.

#### Performance

- Optimize the application for quick loading and efficient handling of concurrent users.
