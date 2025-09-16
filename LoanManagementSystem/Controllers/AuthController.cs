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
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {

            LoginResponse response;
            if (ModelState.IsValid)
            {
                response = await _authService.Login(loginRequest);
                var token = response.Token;
                if (response.IsSuccess && response.User != null)
                {
                    return Ok(new { token });
                }
                return Unauthorized(new { Message = "Invalid email or password" });
            }
            return BadRequest();
        }
    }
}


