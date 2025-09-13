using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LoanManagementSystemContext _context;
        public AuthRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

            if (user != null)
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    User = user,
                    Role = user.Role.ToString(),
                    Token=""
                };
            }

            // If login fails
            return new LoginResponse
            {
                IsSuccess = false,
                User = null,
                Role = null,
                Token = null
            };
        }

       
    }
}
