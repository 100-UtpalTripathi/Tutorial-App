import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from '../../../hooks/useFetch';
import './Login.css';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [triggerFetch, setTriggerFetch] = useState(false);
  const [options, setOptions] = useState(null);
  const navigate = useNavigate();

  const { data, error: fetchError, loading: fetchLoading } = useFetch(
    triggerFetch ? 'https://localhost:7293/api/Auth/user/login' : null,
    options
  );

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    setOptions({
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });

    setTriggerFetch(true);
  };

  useEffect(() => {
    if (data) {
      const { token, email: userEmail, role } = data;
      localStorage.setItem('token', token);
      localStorage.setItem('email', userEmail);
      localStorage.setItem('role', role);
      navigate('/');
    } else if (fetchError) {
      setError(fetchError);
    }
    setLoading(fetchLoading);
  }, [data, fetchError, fetchLoading, navigate]);

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Login</h2>
        {error && <p className="error-message">{error}</p>}
        <div className="form-group">
          <label htmlFor="email">Email address</label>
          <input
            type="email"
            className="form-control"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            className="form-control"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Logging in...' : 'Login'}
        </button>
      </form>
    </div>
  );
};

export default Login;
