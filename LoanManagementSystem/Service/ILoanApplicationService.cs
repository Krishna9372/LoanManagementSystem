using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanApplicationService
    {
        Task<LoanApplication> Create(LoanApplication application);
        Task<LoanApplication> GetById(int id);
        Task<IEnumerable<LoanApplication>> GetAll();
        Task<LoanApplication> Update(LoanApplication application);
        Task<LoanApplication> Delete(int id);
    }
}
