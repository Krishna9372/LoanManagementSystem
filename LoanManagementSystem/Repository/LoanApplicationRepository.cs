using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly LoanManagementSystemContext _context;
        public LoanApplicationRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<LoanApplication> ILoanApplicationRepository.Create(LoanApplication loanApplication)
        {
            _context.LoanApplications.Add(loanApplication);
            await _context.SaveChangesAsync();
            return loanApplication;
        }
        async Task<LoanApplication> ILoanApplicationRepository.Delete(int id)
        {
            var existing = await _context.LoanApplications.FirstOrDefaultAsync(l => l.ApplicationId == id);
            if (existing != null)
            {
                _context.LoanApplications.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<LoanApplication>> ILoanApplicationRepository.GetAll()
        {
            return await _context.LoanApplications.ToListAsync();
        }
        async Task<LoanApplication> ILoanApplicationRepository.GetById(int id)
        {
            var existing = await _context.LoanApplications.Include(a => a.Customer).FirstOrDefaultAsync(l => l.ApplicationId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Loan Application not found");
            }
            return existing;
        }
        async Task<LoanApplication> ILoanApplicationRepository.Update(LoanApplication loanApplication)
        {
            var existing = await _context.LoanApplications.FirstOrDefaultAsync(l => l.ApplicationId == loanApplication.ApplicationId);
            if (existing != null)
            {
                existing.CustomerId = loanApplication.CustomerId;
                existing.SchemeId = loanApplication.SchemeId;
                existing.AppliedAmount = loanApplication.AppliedAmount;
                existing.TermMonths = loanApplication.TermMonths;
                existing.InterestRate = loanApplication.InterestRate;
                existing.Status = loanApplication.Status;
                existing.AssignedOfficerId = loanApplication.AssignedOfficerId;
                existing.AppliedAt = loanApplication.AppliedAt;
                existing.DecisionAt = loanApplication.DecisionAt;

                await _context.SaveChangesAsync();
            }
            return existing;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
