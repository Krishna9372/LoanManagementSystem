
using LoanManagementSystem.Data;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Service;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<LoanManagementSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection")));

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ILoanSchemeRepository, LoanSchemeRepository>();
            builder.Services.AddScoped<ILoanSchemeService, LoanSchemeService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILoanOfficerRepository, LoanOfficerRepository>();
            builder.Services.AddScoped<ILoanOfficerService, LoanOfficerService>();
            builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
            builder.Services.AddScoped<IRepaymentRepository, RepaymentRepository>();
            builder.Services.AddScoped<IRepaymentService, RepaymentService>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();

            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
