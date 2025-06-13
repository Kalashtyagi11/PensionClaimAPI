import React, { useState, useEffect } from 'react';
import WorkflowStep from './WorkflowStep';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5239'; // Updated to match your backend port

const WorkflowSettings = () => {
    const [activeTab, setActiveTab] = useState('ageBenefit');
    const [workflowData, setWorkflowData] = useState({
        ageBenefit: [],
        employmentInjuryBenefit: [],
    });

    useEffect(() => {
        const fetchWorkflowData = async () => {
            try {
                const response = await axios.get(`${API_BASE_URL}/api/EmailPreference`);
                const allNotifications = response.data;
                console.log("Fetched all notifications:", allNotifications); // Debugging

                const organizedData = {
                    ageBenefit: [
                        {
                            id: 1,
                            step: 'On Claim Submission',
                            backendStep: 'CLAIM_SUBMISSION',
                            recipients: [],
                            applicantNotification: null,
                        },
                        {
                            id: 2,
                            step: 'On Field Officer Confirmation',
                            backendStep: 'FIELD_OFFICER_CONFIRMATION',
                            recipients: [],
                            applicantNotification: null,
                        },
                        {
                            id: 3,
                            step: 'On Approval From Finance Department',
                            backendStep: 'FINANCE_APPROVAL',
                            recipients: [],
                            applicantNotification: null,
                        },
                        {
                            id: 4,
                            step: 'On Disbursement',
                            backendStep: 'DISBURSEMENT',
                            recipients: [],
                            applicantNotification: null,
                        },
                    ],
                    employmentInjuryBenefit: [
                        {
                            id: 5,
                            step: 'On Claim Submission',
                            backendStep: 'EI_CLAIM_SUBMISSION',
                            recipients: [],
                            applicantNotification: null,
                        },
                        {
                            id: 6,
                            step: 'On Field Officer Confirmation',
                            backendStep: 'EI_FIELD_OFFICER_CONFIRMATION',
                            recipients: [],
                            applicantNotification: null,
                        },
                    ],
                };

                allNotifications.forEach(notification => {
                    Object.keys(organizedData).forEach(tabKey => {
                        organizedData[tabKey].forEach(step => {
                            if (step.backendStep === notification.workflowStep) {
                                if (notification.recipientType === 'APPLICANT') {
                                    step.applicantNotification = {
                                        id: notification.id,
                                        sendNotification: notification.sendNotification,
                                        recipientEmail: notification.recipientEmail, 
                                        recipientType: notification.recipientType,
                                        workflowStep: notification.workflowStep,
                                    };
                                } else {
                                    // Only add recipient if it's not the APPLICANT type and has a recipient email
                                    if (notification.recipientEmail && notification.recipientType !== 'APPLICANT') {
                                        step.recipients.push({
                                            name: notification.recipientType.replace(/_/g, ' ').replace(/\b\w/g, c => c.toUpperCase()), // Basic conversion for display
                                            email: notification.recipientEmail,
                                            selected: notification.sendNotification,
                                            id: notification.id, // Store backend ID for updates
                                            recipientType: notification.recipientType, // Store recipient type for updates
                                        });
                                    }
                                }
                            }
                        });
                    });
                });
                setWorkflowData(organizedData);
            } catch (error) {
                console.error('Error fetching workflow data:', error);
            }
        };

        fetchWorkflowData();
    }, []);

    const handleRecipientChange = async (tab, stepId, recipientIndex) => {
        const updatedWorkflowData = { ...workflowData };
        const step = updatedWorkflowData[tab].find(s => s.id === stepId);
        if (step) {
            const recipient = step.recipients[recipientIndex];
            if (recipient) {
                recipient.selected = !recipient.selected;
                setWorkflowData(updatedWorkflowData);

                try {
                    await axios.put(`${API_BASE_URL}/api/EmailPreference`, {
                        id: recipient.id,
                        workflowStep: step.backendStep,
                        recipientType: recipient.recipientType,
                        recipientEmail: recipient.email,
                        sendNotification: recipient.selected,
                    });
                } catch (error) {
                    console.error('Error updating recipient preference:', error);
                    // Revert UI on error
                    recipient.selected = !recipient.selected;
                    setWorkflowData(updatedWorkflowData);
                }
            }
        }
    };

    const handleApplicantUpdateChange = async (tab, stepId, checked) => {
        const updatedWorkflowData = { ...workflowData };
        const step = updatedWorkflowData[tab].find(s => s.id === stepId);
        if (step && step.applicantNotification) {
            step.applicantNotification.sendNotification = checked;
            setWorkflowData(updatedWorkflowData);

            try {
                await axios.put(`${API_BASE_URL}/api/EmailPreference`, {
                    id: step.applicantNotification.id, 
                    workflowStep: step.applicantNotification.workflowStep,
                    recipientType: step.applicantNotification.recipientType,
                    recipientEmail: step.applicantNotification.recipientEmail, 
                    sendNotification: checked,
                });
            } catch (error) {
                console.error('Error updating applicant notification:', error);
                step.applicantNotification.sendNotification = !checked;
                setWorkflowData(updatedWorkflowData);
            }
        }
    };

    return (
        <div className="workflow-settings-container">
            <h2>Workflow Settings</h2>
            <div className="tabs">
                <button 
                    className={activeTab === 'ageBenefit' ? 'active' : ''}
                    onClick={() => setActiveTab('ageBenefit')}
                >
                    Age Benefit
                </button>
                <button
                    className={activeTab === 'employmentInjuryBenefit' ? 'active' : ''}
                    onClick={() => setActiveTab('employmentInjuryBenefit')}
                >
                    Employment Injury Benefit
                </button>
            </div>

            <div className="workflow-content">
                {workflowData[activeTab] && workflowData[activeTab].map((step, index) => (
                    <WorkflowStep
                        key={step.id}
                        stepNumber={index + 1}
                        stepTitle={step.step}
                        recipients={step.recipients}
                        sendUpdateToApplicant={step.applicantNotification ? step.applicantNotification.sendNotification : false}
                        onRecipientChange={(recipientIndex) => handleRecipientChange(activeTab, step.id, recipientIndex)}
                        onApplicantUpdateChange={(checked) => handleApplicantUpdateChange(activeTab, step.id, checked)}
                    />
                ))}
            </div>
        </div>
    );
};

export default WorkflowSettings; 