using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagementSystem.Service
{
    public class AuthService:IAuthService
    {
        private readonly ICustomerService _customerService;
        private readonly ILoanOfficerService _officerService;
        private readonly IAuthRepository _repos;
        private readonly IConfiguration _config;
        public AuthService(IAuthRepository repos,IConfiguration config,ICustomerService customerService, ILoanOfficerService officerService)
        {
            _repos = repos;
            _config = config;
            _customerService = customerService;
            _officerService= officerService;
        }
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            return await _repos.Login(loginRequest);
        }

        async Task<string> IAuthService.GetToken(User user)
        {
            int userId = user.UserId;
            string role = user.Role.ToString();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecurityKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                new Claim(ClaimTypes.Role,role)
            };
            if (user.Role == Role.Customer)
            {
                var customer = await _customerService.GetById(userId);
                if (customer != null)
                {
                    claims.Add(new Claim("CustomerId", customer.Customer_Id.ToString()));
                    claims.Add(new Claim("CustomerName", customer.Customer_Name));
                }
            }
            if (user.Role == Role.LoanOfficer)
            {
                var loanOfficer = await _officerService.GetByUserId(userId);
                if (loanOfficer != null)
                {
                    claims.Add(new Claim("OfficerId", loanOfficer.OfficerId.ToString()));
                    claims.Add(new Claim("OfficerName", loanOfficer.FullName));
                }
            }
            if (user.Role == Role.Admin)
            {
                claims.Add(new Claim("AdminId", userId.ToString()));
                claims.Add(new Claim("AdminName", user.Username));
            }
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:DurationInMinutes"])),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
