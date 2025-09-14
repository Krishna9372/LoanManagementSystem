using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.Identity.Client;

namespace LoanManagementSystem.Service
{
    public class LoanApplicationService: ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _repos;
        private readonly ILoanOfficerService _officerService;
        public LoanApplicationService(ILoanApplicationRepository repos,ILoanOfficerService officerService)
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
    }
}
