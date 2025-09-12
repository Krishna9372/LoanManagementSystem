using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepaymentController : ControllerBase
    {
        private readonly IRepaymentService _service;
        public RepaymentController(IRepaymentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<Repayment>> Create(Repayment repayment)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(repayment);
                return CreatedAtAction("Get", new { id = repayment.RepaymentId }, result);
            }
            return BadRequest(repayment);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repayment>>> Get()
        {
            var repayments = await _service.GetAll();
            return Ok(repayments);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Repayment>> Get(int id)
        {
            var repayment = await _service.GetById(id);
            return Ok(repayment);
        }
        [HttpPut]
        public async Task<ActionResult<Repayment>> Put(Repayment repayment)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(repayment);
                return Ok(result);
            }
            return BadRequest(repayment);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Repayment>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
