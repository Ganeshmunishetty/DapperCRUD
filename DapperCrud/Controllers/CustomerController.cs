
using Dapper;
using DapperCrud.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{

    [Route("api/Controller")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Customer> customers = await selectallcustomers(connection);
            return Ok(customers);
        }

        
        [HttpGet("customerId")]
        public async Task<ActionResult<List<Customer>>> Gethero(int customerId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var customer = await connection.QueryFirstAsync<Customer>("select * from details where ID =@ID",
                new {Id= customerId });
            return Ok(customer);
        }
        [HttpPost]
        public async Task<ActionResult<List<Customer>>> AddCustomer(Customer customer)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var storedProcedureName = "SaveCustomerDetails";
            var parameters = new
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Place = customer.Place
            };
            await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            //await connection.ExecuteAsync("insert into Details (FirstName,LastName,Email,Place) values (@FirstName,@LastName,@Email,@Place)", customer);
            return Ok(customer);
        }
        [HttpPut]
        public async Task<ActionResult<List<Customer>>> UpdateCustomer(Customer customer)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update Details set FirstName=@FirstName,LastName=@LastName,Email=@Email,Place=@Place where ID=@ID", customer);
            return Ok(customer);
        }
        [HttpDelete]
        public async Task<ActionResult<List<Customer>>> DeleteCustomer(int customerId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Delete from Details where ID=@ID", new {ID= customerId });
            return Ok(customerId);
        }
        private static async Task<IEnumerable<Customer>> selectallcustomers(SqlConnection connection)
        {
            return await connection.QueryAsync<Customer>("select * from Details");
        }
    }
}
