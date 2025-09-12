using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class LoanApplicationService: ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _repos;
        public LoanApplicationService(ILoanApplicationRepository repos)
        {
            _repos = repos;
        }
        async Task<LoanApplication> ILoanApplicationService.Create(LoanApplication application)
        {
            return await _repos.Create(application);
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
