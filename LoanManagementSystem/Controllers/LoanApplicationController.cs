using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplicationService _service;
        public LoanApplicationController(ILoanApplicationService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<LoanApplication>> Create(LoanApplication application)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(application);
                return CreatedAtAction("Get", new { id = application.ApplicationId }, result);
            }
            return BadRequest(application);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> Get()
        {
            var applications = await _service.GetAll();
            return Ok(applications);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanApplication>> Get(int id)
        {
            var application = await _service.GetById(id);
            return Ok(application);
        }
        [HttpPut]
        public async Task<ActionResult<LoanApplication>> Put(LoanApplication application)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(application);
                return Ok(result);
            }
            return BadRequest(application);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanApplication>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

    }
}
