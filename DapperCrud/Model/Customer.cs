using System.Text.Json.Serialization;

namespace DapperCrud.Model
{
    public class Customer
    {
 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Place { get; set; }

    }
}
