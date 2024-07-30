import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import AdminNavbar from '../components/admin/Navbar/AdminNavbar';
import Dashboard from '../components/admin/Dashboard';
import ManageCourses from '../components/admin/ManageCourses';
import ManageQuiz from '../components/admin/ManageQuiz';

const AdminRoutes = () => {
  return (
    <Router>
      <AdminNavbar />
      <Switch>
        <Route path="/admin/dashboard" component={Dashboard} />
        <Route path="/admin/manage-courses" component={ManageCourses} />
        <Route path="/admin/manage-quiz" component={ManageQuiz} />
      </Switch>
    </Router>
  );
};

export default AdminRoutes;
