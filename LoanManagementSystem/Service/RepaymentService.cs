using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class RepaymentService:IRepaymentService
    {
        private readonly IRepaymentRepository _repos;
        public RepaymentService(IRepaymentRepository repos)
        {
            _repos = repos;
        }
        async Task<Repayment> IRepaymentService.Create(Repayment repayment)
        {
            return await _repos.Create(repayment);
        }
        async Task<Repayment> IRepaymentService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<Repayment>> IRepaymentService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<Repayment> IRepaymentService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<Repayment> IRepaymentService.Update(Repayment repayment)
        {
            return await _repos.Update(repayment);
        }
    }
}
