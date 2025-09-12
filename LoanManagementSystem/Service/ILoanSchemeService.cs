using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanSchemeService
    {
        Task<LoanScheme> Create(LoanScheme scheme);
        Task<LoanScheme> GetById(int id);
        Task<LoanScheme> Delete(int id);
        Task<IEnumerable<LoanScheme>> GetAll();
        Task<LoanScheme> Update(LoanScheme scheme);
    }
}
