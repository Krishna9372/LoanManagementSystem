using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IRepaymentRepository
    {
        Task<Repayment> Create(Repayment repayment);
        Task<Repayment> GetById(int id);
        Task<Repayment> Update(Repayment repayment);
        Task<Repayment> Delete(int id);
        Task<IEnumerable<Repayment>> GetAll();
    }
}
