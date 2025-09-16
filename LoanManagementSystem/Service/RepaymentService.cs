using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Service
{
    public class RepaymentService : IRepaymentService
    {
        private readonly IRepaymentRepository _repos;
        private readonly ILoanApplicationRepository _appRepos;
        public RepaymentService(IRepaymentRepository repos, ILoanApplicationRepository appRepos)
        {
            _repos = repos;
            _appRepos = appRepos;
        }
        public async Task<Repayment> PayInstallment(int applicationId, decimal paidAmount)
        {
            if (paidAmount <= 0)
                throw new Exception("Paid amount should be greater than 0.");

            var repayments = (await _repos.GetAllByApplicationId(applicationId))
                              .OrderBy(r => r.InstallmentNumber)
                              .ToList();

            var application = await _appRepos.GetById(applicationId);
            if (application == null)
                throw new Exception("Loan application not found.");

            // Generate repayment schedule if none exists
            if (!repayments.Any())
            {
                await GenerateRepaymentSchedule(application);
                repayments = (await _repos.GetAllByApplicationId(applicationId))
                               .OrderBy(r => r.InstallmentNumber)
                               .ToList();
            }

            // Get first unpaid installment
            var currentRepayment = repayments.FirstOrDefault(r => r.AmountDue > 0);

            if (currentRepayment == null)
            {
                // Loan already fully paid
                application.Status = ApplicationStatus.Closed;
                await _appRepos.Update(application);
                throw new Exception("Loan fully repaid. No more installments left.");
            }

            // Partial or full EMI payment
            if (paidAmount >= currentRepayment.AmountDue)
            {
                // Full EMI paid
                decimal extraPayment = paidAmount - currentRepayment.AmountDue;

                currentRepayment.AmountPaid += currentRepayment.AmountDue;
                currentRepayment.AmountDue = 0;
                currentRepayment.Status = RepaymentStatus.Paid;
                currentRepayment.PaidDate = DateTime.UtcNow;

                await _repos.Update(currentRepayment);

                if (extraPayment > 0)
                {
                    // Reduce principal by extra payment
                    application.AppliedAmount -= extraPayment;
                    if (application.AppliedAmount < 0) application.AppliedAmount = 0;
                    await _appRepos.Update(application);

                    // Delete old unpaid schedule and regenerate with remaining months
                    int paidInstallments = repayments.Count(r => r.Status == RepaymentStatus.Paid);
                    int remainingMonths = application.TermMonths - paidInstallments;

                    if (remainingMonths > 0)
                    {
                        await _repos.DeleteAllByApplicationId(applicationId);
                        await GenerateRepaymentSchedule(application, remainingMonths, paidInstallments);
                    }
                    else
                    {
                        // All installments cleared
                        application.Status = ApplicationStatus.Closed;
                        await _appRepos.Update(application);
                    }
                }
            }
            else
            {
                // Partial EMI payment
                currentRepayment.AmountPaid += paidAmount;
                currentRepayment.AmountDue -= paidAmount;
                currentRepayment.Status = RepaymentStatus.Pending;
                await _repos.Update(currentRepayment);
            }

            // Check if all EMIs are paid → close loan
            bool allPaid = (await _repos.GetAllByApplicationId(applicationId))
                             .All(r => r.Status == RepaymentStatus.Paid);

            if (allPaid)
            {
                application.Status = ApplicationStatus.Closed;
                await _appRepos.Update(application);
            }

            return currentRepayment;
        }


        private decimal CalculateEMI(decimal principal, decimal monthlyRate, int months)
        {
            decimal numerator = principal * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months);
            decimal denominator = (decimal)Math.Pow((double)(1 + monthlyRate), months) - 1;
            return Math.Round(numerator / denominator, 2);
        }

        private decimal CalculateDailyPenalty(Repayment repayment)
        {
            if (repayment.AmountDue <= 0)
                return 0m;

            int overdueDays = (DateTime.UtcNow - repayment.DueDate).Days;
            if (overdueDays <= 0)
                return 0m;

            decimal dailyPenaltyRate = 0.02m;
            return Math.Round(repayment.AmountDue * dailyPenaltyRate * overdueDays, 2);
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

        public async Task GenerateRepaymentSchedule(LoanApplication application, int remainingMonths = 0, int paidInstallments = 0)
        {
            if (remainingMonths == 0) remainingMonths = application.TermMonths - paidInstallments;

            decimal principalRemaining = application.AppliedAmount;
            decimal monthlyRate = application.InterestRate / 12 / 100;

            var newSchedule = new List<Repayment>();

            for (int i = 1; i <= remainingMonths; i++)
            {
                decimal emi = CalculateEMI(principalRemaining, monthlyRate, remainingMonths - i + 1);

                decimal interest = principalRemaining * monthlyRate;
                decimal principalComponent = emi - interest;
                principalRemaining -= principalComponent;
                if (principalRemaining < 0) principalRemaining = 0;

                var repayment = new Repayment
                {
                    ApplicationId = application.ApplicationId,
                    InstallmentNumber = paidInstallments + i,
                    AmountEMI = emi,
                    AmountPaid = 0,
                    AmountDue = emi,
                    DueDate = application.AppliedAt.AddMonths(paidInstallments + i),
                    Status = RepaymentStatus.Pending,
                    IsOverdue = false,
                    PenaltyAmount = 0
                };

                newSchedule.Add(repayment);
            }

            // Save new schedule
            await _repos.BulkInsert(newSchedule);
        }


        // EMI Calculation
    }
}
