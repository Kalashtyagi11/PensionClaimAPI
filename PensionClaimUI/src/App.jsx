import { BrowserRouter as Router, Routes, Route, Link, useLocation } from 'react-router-dom';
import { useState } from 'react';
import './App.css';
import WorkflowSettings from './components/WorkflowSettings';

// Components
import ClaimForm from './components/ClaimForm';
import ClaimManagerDashboard from './components/ClaimManagerDashboard';
import ApproverDashboard from './components/ApproverDashboard';

function NavLink({ to, children }) {
  const location = useLocation();
  const isActive = location.pathname === to;
  
  return (
    <Link to={to} className={isActive ? 'active' : ''}>
      {children}
    </Link>
  );
}

function App() {
  return (
    <Router>
      <div className="app">
        <nav className="navbar">
          <Link to="/" className="nav-brand">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"/>
            </svg>
            Pension Claim System
          </Link>
          <div className="nav-links">
            <NavLink to="/">Home</NavLink>
            <NavLink to="/claim">Submit Claim</NavLink>
            <NavLink to="/manager">Claim Manager</NavLink>
            <NavLink to="/approver">Approver</NavLink>
            <NavLink to="/workflow-settings">Workflow Notifs.</NavLink>
          </div>
        </nav>

        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/claim" element={<ClaimForm />} />
            <Route path="/manager" element={<ClaimManagerDashboard />} />
            <Route path="/approver" element={<ApproverDashboard />} />
            <Route path="/workflow-settings" element={<WorkflowSettings />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

function Home() {
  return (
    <div className="home">
      <h1>Welcome to Pension Claim System</h1>
      <p>
        Streamline your pension claim process with our easy-to-use platform.
        Submit claims, track status, and manage approvals all in one place.
      </p>
      <div className="home-actions">
        <Link to="/claim" className="btn btn-primary">
          Submit New Claim
        </Link>
        <Link to="/manager" className="btn btn-secondary">
          View Claims
        </Link>
      </div>
    </div>
  );
}

export default App;
