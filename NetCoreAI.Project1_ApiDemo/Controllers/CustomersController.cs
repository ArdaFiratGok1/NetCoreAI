using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project1_ApiDemo.Context;
using NetCoreAI.Project1_ApiDemo.Entities;

namespace NetCoreAI.Project1_ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomersController(ApiContext context)//Dependency Injection
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CustomerList()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("Customer added successfully");
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} was not found.");

            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok("Customer removed successfully");
        }

        [HttpPut]
        public IActionResult UpdateCustomer(Customer customer)
        {
           
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Customer updated successfully");
        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} was not found.");

            }
            return Ok(customer);
        }



    }
}
