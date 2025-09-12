using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanOfficerController : ControllerBase
    {
        private readonly ILoanOfficerService _service;
        public LoanOfficerController(ILoanOfficerService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<LoanOfficer>> Create(LoanOfficer officer)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(officer);
                return CreatedAtAction("Get", new { id = officer.OfficerId }, result);
            }
            return BadRequest(officer);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanOfficer>>> Get()
        {
            var officers = await _service.GetAll();
            return Ok(officers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanOfficer>> Get(int id)
        {
            var officer = await _service.GetById(id);
            return Ok(officer);
        }
        [HttpPut]
        public async Task<ActionResult<LoanOfficer>> Put(LoanOfficer officer)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(officer);
                return Ok(result);
            }
            return BadRequest(officer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanOfficer>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
