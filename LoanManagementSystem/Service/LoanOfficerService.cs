using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class LoanOfficerService: ILoanOfficerService
    {
        private readonly ILoanOfficerRepository _repos;
        public LoanOfficerService(ILoanOfficerRepository repos)
        {
            _repos = repos;
        }
        async Task<LoanOfficer> ILoanOfficerService.Create(LoanOfficer officer)
        {
            return await _repos.Create(officer);
        }
        async Task<LoanOfficer> ILoanOfficerService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanOfficer>> ILoanOfficerService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanOfficer> ILoanOfficerService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanOfficer> ILoanOfficerService.Update(LoanOfficer officer)
        {
            return await _repos.Update(officer);
        }



    }
}
