using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            if(ModelState.IsValid)
            {
                var newCustomer= await _service.Create(customer);
                return CreatedAtAction("Get", new { id = newCustomer.Customer_Id }, newCustomer);
            }
            return BadRequest(customer);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            var authors = await _service.GetAll();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var authors=await _service.GetById(id);
            return Ok(authors);
        }
        [HttpPut]
        public async Task<ActionResult<Customer>> Put(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var updated = await _service.Update(customer);
                return Ok(updated);
            }
            return BadRequest(customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            return Ok(deleted);
        }
    }
}
