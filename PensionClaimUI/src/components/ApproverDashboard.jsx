import { useState, useEffect } from 'react';
import './ApproverDashboard.css';

function ApproverDashboard() {
  const [claims, setClaims] = useState([]);
  const [loading, setLoading] = useState(true);
  const [receiveEmails, setReceiveEmails] = useState(true);

  useEffect(() => {
    fetchClaims();
    fetchEmailPreference();
  }, []);

  const fetchEmailPreference = async () => {
    try {
      const response = await fetch('http://localhost:5239/api/emailpreference/APPROVER');
      const data = await response.json();
      setReceiveEmails(data.receiveEmails);
    } catch (error) {
      console.error('Error fetching email preference:', error);
    }
  };

  const updateEmailPreference = async (checked) => {
    try {
      await fetch('http://localhost:5239/api/emailpreference/APPROVER', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ role: 'APPROVER', receiveEmails: checked }),
      });
      setReceiveEmails(checked);
    } catch (error) {
      console.error('Error updating email preference:', error);
    }
  };

  const fetchClaims = async () => {
    try {
      const response = await fetch('http://localhost:5239/api/claims/approved');
      const data = await response.json();
      setClaims(data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching claims:', error);
      setLoading(false);
    }
  };

  const handleFinalApprove = async (claimId) => {
    try {
      const response = await fetch(`http://localhost:5239/api/claims/${claimId}/final-approve`, {
        method: 'PUT',
      });

      if (response.ok) {
        fetchClaims();
      } else {
        alert('Error in final approval');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('Error in final approval');
    }
  };

  const handleFinalReject = async (claimId) => {
    try {
      const response = await fetch(`http://localhost:5239/api/claims/${claimId}/final-reject`, {
        method: 'PUT',
      });

      if (response.ok) {
        fetchClaims();
      } else {
        alert('Error in final rejection');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('Error in final rejection');
    }
  };

  if (loading) {
    return <div className="loading">Loading claims...</div>;
  }

  return (
    <div className="dashboard-container">
      <h2>Approver Dashboard</h2>
      <div className="email-preference">
        <label className="email-checkbox">
          <input
            type="checkbox"
            checked={receiveEmails}
            onChange={(e) => updateEmailPreference(e.target.checked)}
          />
          Receive Email Notifications
        </label>
      </div>
      <div className="claims-list">
        {claims.length === 0 ? (
          <p className="no-claims">No claims pending final approval</p>
        ) : (
          claims.map((claim) => (
            <div key={claim.id} className="claim-card">
              <div className="claim-header">
                <h3>{claim.name}</h3>
                <span className={`status ${claim.status.toLowerCase()}`}>
                  {claim.status}
                </span>
              </div>
              <div className="claim-details">
                <p><strong>Email:</strong> {claim.email}</p>
                <p><strong>Company:</strong> {claim.companyName}</p>
                <p><strong>Employment Period:</strong> {claim.employmentStartDate} to {claim.employmentEndDate}</p>
                <p><strong>Initial Approval Date:</strong> {claim.initialApprovalDate}</p>
              </div>
              {claim.status === 'APPROVED_BY_MANAGER' && (
                <div className="claim-actions">
                  <button
                    className="approve-button"
                    onClick={() => handleFinalApprove(claim.id)}
                  >
                    Final Approve
                  </button>
                  <button
                    className="reject-button"
                    onClick={() => handleFinalReject(claim.id)}
                  >
                    Final Reject
                  </button>
                </div>
              )}
            </div>
          ))
        )}
      </div>
    </div>
  );
}

export default ApproverDashboard; 