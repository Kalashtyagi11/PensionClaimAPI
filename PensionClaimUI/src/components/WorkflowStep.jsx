import React from 'react';

const WorkflowStep = ({ stepNumber, stepTitle, recipients, sendUpdateToApplicant, onRecipientChange, onApplicantUpdateChange }) => {
    return (
        <div className="workflow-step">
            <div className="step-header">
                <span className="step-number">{stepNumber}.</span>
                <h3>{stepTitle}</h3>
            </div>
            <div className="recipients-list">
                {recipients.map((recipient, index) => (
                    <div key={index} className="recipient-item">
                        <input
                            type="checkbox"
                            checked={recipient.selected}
                            onChange={() => onRecipientChange(index)}
                        />
                        <div className="recipient-info">
                            <span className="recipient-name">{recipient.name}</span>
                            <span className="recipient-email">{recipient.email}</span>
                        </div>
                        <span className="dropdown-arrow"></span>
                    </div>
                ))}
                <div className="add-recipient">
                    <span className="add-icon">+</span>
                </div>
            </div>
            <div className="send-update">
                <input
                    type="checkbox"
                    checked={sendUpdateToApplicant}
                    onChange={(e) => onApplicantUpdateChange(e.target.checked)}
                />
                <span>Send Update to the Applicant?</span>
            </div>
        </div>
    );
};

export default WorkflowStep; 