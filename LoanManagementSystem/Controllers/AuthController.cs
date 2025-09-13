using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _authService.Login(loginRequest);
            if (loginResponse.IsSuccess && loginResponse.User != null)
            {
                var token = await _authService.GetToken(loginResponse.User);
                loginResponse.Token = token;
                return Ok(loginResponse);
            }
            return Unauthorized(new { Message = "Invalid email or password" });
        }
    }
}
