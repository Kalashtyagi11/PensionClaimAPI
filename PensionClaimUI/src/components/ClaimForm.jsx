import { useState } from 'react';
import './ClaimForm.css';

function ClaimForm() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    companyName: '',
    address: '',
    phoneNumber: '',
    employmentStartDate: '',
    employmentEndDate: '',
  });

  const [status, setStatus] = useState({ type: '', message: '' });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    setStatus({ type: '', message: '' });
    
    try {
      const response = await fetch('http://localhost:5239/api/claims', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });

      if (response.ok) {
        setStatus({
          type: 'success',
          message: 'Claim submitted successfully! You will receive an email confirmation shortly.'
        });
        setFormData({
          name: '',
          email: '',
          companyName: '',
          address: '',
          phoneNumber: '',
          employmentStartDate: '',
          employmentEndDate: '',
        });
      } else {
        const error = await response.json();
        throw new Error(error.message || 'Error submitting claim');
      }
    } catch (error) {
      setStatus({
        type: 'error',
        message: error.message || 'Error submitting claim. Please try again.'
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="claim-form-container">
      <h2>Submit Pension Claim</h2>
      <form onSubmit={handleSubmit} className="claim-form">
        <div className="form-group">
          <label htmlFor="name">Full Name</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            placeholder="Enter your full name"
          />
        </div>

        <div className="form-group">
          <label htmlFor="email">Email Address</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            placeholder="Enter your email address"
          />
        </div>

        <div className="form-group">
          <label htmlFor="companyName">Company Name</label>
          <input
            type="text"
            id="companyName"
            name="companyName"
            value={formData.companyName}
            onChange={handleChange}
            required
            placeholder="Enter your company name"
          />
        </div>

        <div className="form-group">
          <label htmlFor="phoneNumber">Phone Number</label>
          <input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
            placeholder="Enter your phone number"
          />
        </div>

        <div className="form-group full-width">
          <label htmlFor="address">Address</label>
          <textarea
            id="address"
            name="address"
            value={formData.address}
            onChange={handleChange}
            required
            placeholder="Enter your full address"
          />
        </div>

        <div className="form-group">
          <label htmlFor="employmentStartDate">Employment Start Date</label>
          <input
            type="date"
            id="employmentStartDate"
            name="employmentStartDate"
            value={formData.employmentStartDate}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="employmentEndDate">Employment End Date</label>
          <input
            type="date"
            id="employmentEndDate"
            name="employmentEndDate"
            value={formData.employmentEndDate}
            onChange={handleChange}
            required
          />
        </div>

        {status.message && (
          <div className={`form-status ${status.type}`}>
            {status.message}
          </div>
        )}

        <div className="form-actions">
          <button
            type="submit"
            className="submit-button"
            disabled={isSubmitting}
          >
            {isSubmitting ? (
              <>
                <svg className="animate-spin" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Submitting...
              </>
            ) : (
              <>
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                </svg>
                Submit Claim
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}

export default ClaimForm; 