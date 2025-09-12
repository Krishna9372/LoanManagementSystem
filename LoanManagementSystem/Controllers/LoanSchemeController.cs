using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanSchemeController : ControllerBase
    {
        private readonly ILoanSchemeService _service;
        public LoanSchemeController(ILoanSchemeService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<LoanScheme>> Create(LoanScheme scheme)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(scheme);
                return CreatedAtAction("Get", new { id = scheme.SchemeId }, result);
            }
            return BadRequest(scheme);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanScheme>>> Get()
        {
            var schemes = await _service.GetAll();
            return Ok(schemes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanScheme>> Get(int id)
        {
            var scheme = await _service.GetById(id);
            return Ok(scheme);
        }
        [HttpPut]
        public async Task<ActionResult<LoanScheme>> Put(LoanScheme scheme)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(scheme);
                return Ok(result);
            }
            return BadRequest(scheme);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanScheme>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
