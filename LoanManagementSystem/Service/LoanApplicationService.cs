using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.Identity.Client;

namespace LoanManagementSystem.Service
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _repos;
        private readonly ILoanOfficerService _officerService;
        public LoanApplicationService(ILoanApplicationRepository repos, ILoanOfficerService officerService)
        {
            _repos = repos;
            _officerService = officerService;
        }

        public async Task<LoanApplication> Create(LoanApplication application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application), "Application cannot be null");

            application.Status = ApplicationStatus.Pending;
            application.AppliedAt = DateTime.UtcNow;

            application.AssignedOfficerId = null;
            // 1️⃣ Save application first
            var createdApplication = await _repos.Create(application);

            // 2️⃣ Assign officer now that ApplicationId exists
            var officer = await _officerService.AssignApplication(createdApplication.ApplicationId);
            createdApplication.AssignedOfficerId = officer.OfficerId;

            // 3️⃣ Save again
            await _repos.SaveChangesAsync();

            return createdApplication;
        }

        async Task<LoanApplication> ILoanApplicationService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanApplication>> ILoanApplicationService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanApplication> ILoanApplicationService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanApplication> ILoanApplicationService.Update(LoanApplication application)
        {
            return await _repos.Update(application);
        }
        async Task<LoanApplication> ILoanApplicationService.Approve(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException("Application not found or not in a pending state.");
            }
            application.Status = ApplicationStatus.Approved;
            application.DecisionAt = DateTime.UtcNow;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Reject(int applicationId, string reason)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Pending && application.Status != ApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException("Application not found or not in a pending state.");
            }
            application.Status = ApplicationStatus.Rejected;
            application.RejectionReason = reason;
            application.DecisionAt = DateTime.UtcNow;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Close(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Disbursed)
            {
                throw new InvalidOperationException("Application not found or not in an approved state.");
            }
            application.Status = ApplicationStatus.Closed;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Disburse(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException("Application not found or not in an approved state.");
            }
            application.Status = ApplicationStatus.Disbursed;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.UnderReview(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Pending)
            {
                throw new InvalidOperationException("Application not found or not in a pending state.");
            }
            application.Status = ApplicationStatus.UnderReview;
            await _repos.SaveChangesAsync();
            return application;
        }


        private async Task ValidateOfficerAssigned(LoanApplication application)
        {
            if (application == null)
                throw new InvalidOperationException("Application not found.");

            if (application.AssignedOfficerId == null)
                throw new InvalidOperationException("No officer assigned to this application.");

            var officer = await _officerService.GetById(application.AssignedOfficerId.Value);
            if (officer == null || !officer.Active)
                throw new UnauthorizedAccessException("Assigned officer is not active.");
        }

    }
}