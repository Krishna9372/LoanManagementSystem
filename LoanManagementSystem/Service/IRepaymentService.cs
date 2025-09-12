using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IRepaymentService
    {
        Task<Repayment> Create(Repayment repayment);
        Task<Repayment> GetById(int id);
        Task<IEnumerable<Repayment>> GetAll();
        Task<Repayment> Update(Repayment repayment);
        Task<Repayment> Delete(int id);
    }
}
