using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class LoanSchemeService: ILoanSchemeService
    {
        private readonly ILoanSchemeRepository _repos;
        public LoanSchemeService(ILoanSchemeRepository repos)
        {
            _repos = repos;
        }
        async Task<LoanScheme> ILoanSchemeService.Create(LoanScheme scheme)
        {
            return await _repos.Create(scheme);
        }
        async Task<LoanScheme> ILoanSchemeService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanScheme>> ILoanSchemeService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanScheme> ILoanSchemeService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanScheme> ILoanSchemeService.Update(LoanScheme scheme)
        {
            return await _repos.Update(scheme);
        }

    }
}
