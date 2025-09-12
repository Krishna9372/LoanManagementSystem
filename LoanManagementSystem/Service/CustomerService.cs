using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repos;
        public CustomerService(ICustomerRepository repos)
        {
            _repos = repos;
        }
        async Task<Customer> ICustomerService.Create(Customer customer)
        {
            return await _repos.Create(customer);
        }

        async Task<Customer> ICustomerService.Delete(int id)
        {
            return await _repos.Delete(id);
        }

        async Task<IEnumerable<Customer>> ICustomerService.GetAll()
        {
            return await _repos.GetAll();
        }

        async Task<Customer> ICustomerService.GetById(int id)
        {
            return await _repos.GetById(id);
        }

        async Task<Customer> ICustomerService.Update(Customer customer)
        {
            return await _repos.Update(customer);
        }
    }
}
