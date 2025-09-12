using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class RepaymentRepository: IRepaymentRepository
    {
        private readonly LoanManagementSystemContext _context;
        public RepaymentRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<Repayment> IRepaymentRepository.Create(Repayment repayment)
        {
            _context.Repayments.Add(repayment);
            await _context.SaveChangesAsync();
            return repayment;
        }
        async Task<Repayment> IRepaymentRepository.Delete(int id)
        {
            var existing = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == id);
            if (existing != null)
            {
                _context.Repayments.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<Repayment>> IRepaymentRepository.GetAll()
        {
            return await _context.Repayments.ToListAsync();
        }
        async Task<Repayment> IRepaymentRepository.GetById(int id)
        {
            var existing = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Repayment not found");
            }   
            return existing;
        }
        async Task<Repayment> IRepaymentRepository.Update(Repayment repayment)
        {
            var existing = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == repayment.RepaymentId);
            if (existing != null)
            {
                existing.ApplicationId = repayment.ApplicationId;
                existing.InstallmentNumber = repayment.InstallmentNumber;
                existing.DueDate = repayment.DueDate;
                existing.AmountDue = repayment.AmountDue;
                existing.PaidDate = repayment.PaidDate;
                existing.Status = repayment.Status;
                existing.IsOverdue = repayment.IsOverdue;
                existing.PenaltyAmount = repayment.PenaltyAmount;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
    }
}
