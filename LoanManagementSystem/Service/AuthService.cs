using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagementSystem.Service
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerService _customerService;
        private readonly ILoanOfficerService _officerService;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        public AuthService(IAuthRepository authRepository, IConfiguration config, ICustomerService customerService, ILoanOfficerService officerService)
        {
            _authRepository = authRepository;
            _config = config;
            _customerService = customerService;
            _officerService = officerService;
        }
        async Task<LoginResponse> IAuthService.Login(LoginRequest model)
        {
            var response = await _authRepository.Login(model);
            if (response.IsSuccess)
            {
                response.Token = GenerateToken(response.User);
            }
            return response;
        }

        private string GenerateToken(User user)
        {
            var Secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var SigningCredentials = new SigningCredentials(Secretkey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
            new Claim(ClaimTypes.MobilePhone,user.Phone),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role.ToString())


        };
            var tokenOptions = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: SigningCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

    }

}
