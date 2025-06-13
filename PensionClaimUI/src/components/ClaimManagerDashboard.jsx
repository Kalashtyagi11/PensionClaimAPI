import { useState, useEffect } from 'react';
import './ClaimManagerDashboard.css';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5239'; // Confirm this is your backend port

function ClaimManagerDashboard() {
  const [claims, setClaims] = useState([]);
  const [loading, setLoading] = useState(true);
  const [receiveEmails, setReceiveEmails] = useState(true);
  const [managerNotificationId, setManagerNotificationId] = useState(null);

  useEffect(() => {
    fetchClaims();
    fetchManagerNotificationPreference();
  }, []);

  const fetchManagerNotificationPreference = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/api/EmailPreference/CLAIM_SUBMISSION/CLAIM_MANAGER_ANISH`);
      const data = response.data;
      setReceiveEmails(data.sendNotification);
      setManagerNotificationId(data.id);
    } catch (error) {
      console.error('Error fetching manager notification preference:', error);
    }
  };

  const updateManagerNotificationPreference = async (checked) => {
    try {
      await axios.put(`${API_BASE_URL}/api/EmailPreference`, {
        id: managerNotificationId,
        workflowStep: "CLAIM_SUBMISSION",
        recipientType: "CLAIM_MANAGER_ANISH",
        recipientEmail: "anish.s@mishainfotech.com", // Assuming Anish Singh is the manager
        sendNotification: checked,
      });
      setReceiveEmails(checked);
    } catch (error) {
      console.error('Error updating manager notification preference:', error);
    }
  };

  const fetchClaims = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/api/claims`);
      setClaims(response.data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching claims:', error);
      setLoading(false);
    }
  };

  const handleApprove = async (claimId) => {
    try {
      const response = await axios.put(`${API_BASE_URL}/api/claims/${claimId}/approve`);

      if (response.status === 200) {
        fetchClaims();
      } else {
        alert('Error approving claim');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('Error approving claim');
    }
  };

  const handleReject = async (claimId) => {
    try {
      const response = await axios.put(`${API_BASE_URL}/api/claims/${claimId}/reject`);

      if (response.status === 200) {
        fetchClaims();
      } else {
        alert('Error rejecting claim');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('Error rejecting claim');
    }
  };

  if (loading) {
    return <div className="loading">Loading claims...</div>;
  }

  return (
    <div className="dashboard-container">
      <h2>Claim Manager Dashboard</h2>
      <div className="email-preference">
        <label className="email-checkbox">
          <input
            type="checkbox"
            checked={receiveEmails}
            onChange={(e) => updateManagerNotificationPreference(e.target.checked)}
          />
          Receive Email Notifications
        </label>
      </div>
      <div className="claims-list">
        {claims.length === 0 ? (
          <p className="no-claims">No claims to review</p>
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
              </div>
              {claim.status === 'PENDING' && (
                <div className="claim-actions">
                  <button
                    className="approve-button"
                    onClick={() => handleApprove(claim.id)}
                  >
                    Approve
                  </button>
                  <button
                    className="reject-button"
                    onClick={() => handleReject(claim.id)}
                  >
                    Reject
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

export default ClaimManagerDashboard; 