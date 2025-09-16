using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
