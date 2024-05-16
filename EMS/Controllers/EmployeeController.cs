using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Project1.Modal;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {

        [HttpGet]
        [Route("GetEmployees")]
        public JsonResult GetEmployees()
        {
            var emp = new List<Employee>();

            const string connectionUri = "mongodb+srv://aarundas707:Arun123@cluster0.omzekzn.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            try
            {
                var client = new MongoClient(settings);
                var database = client.GetDatabase("mongodb");
                var empData = database.GetCollection<Employee>("EmployeeDetails");          
                var allEmployees = empData.Find(new BsonDocument()).ToList();
                foreach (var employee in allEmployees)
                {
                    Console.WriteLine($"EmployeeID: {employee.EmployeeID}, Name: {employee.Name}, Age: {employee.Age}, Department: {employee.Department}, Address: {employee.Address}, Phone: {employee.Phone}");
                }
                emp = allEmployees;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");            
            }
            
            return new JsonResult(emp);

        }

        [HttpPost]
        [Route("AddUpdateEmployee")]
        public IActionResult AddUpdateEmployee(Employee data)
        {
            var responseData = new
            {
                Status = "Success", Message = "Success"
            };
            const string connectionUri = "mongodb+srv://aarundas707:Arun123@cluster0.omzekzn.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            try
            {
                var client = new MongoClient(settings);
                var database = client.GetDatabase("mongodb");
                var empData = database.GetCollection<Employee>("EmployeeDetails");

                if(data.EmployeeID > 0)
                {
                    var filter = Builders<Employee>.Filter.Eq(e => e.EmployeeID, data.EmployeeID);
                    var employee = empData.Find(filter).FirstOrDefault();

                    if (employee != null)
                    {
                        var update = Builders<Employee>.Update
                        .Set(e => e.Name, data.Name)
                        .Set(e => e.Age, data.Age)
                        .Set(e => e.Department, data.Department)
                        .Set(e => e.Address, data.Address)
                        .Set(e => e.Phone, data.Phone);

                        var result = empData.UpdateOne(filter, update);
                    }
                }
                else
                {
                    var lastEmployee = empData.Find(FilterDefinition<Employee>.Empty)
                   .SortByDescending(e => e.EmployeeID) 
                   .FirstOrDefault();
                    var empId = lastEmployee != null ? lastEmployee.EmployeeID + 1 : 0;
                    var newEmployee = new Employee
                    {
                        EmployeeID = empId,
                        Name = data.Name,
                        Age = data.Age,
                        Department = data.Department,
                        Address = data.Address,
                        Phone = data.Phone
                    };

                    empData.InsertOne(newEmployee);
                }
                                            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");             
            }

            return Json(responseData);
        }

        [HttpPost]
        [Route("DeleteEmployee")]
        public JsonResult DeleteEmployee(Employee data)
        {
            var responseData = new
            {
                Status = "Success",
                Message = "Success"
            };
            const string connectionUri = "mongodb+srv://aarundas707:Arun123@cluster0.omzekzn.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            try
            {
                var client = new MongoClient(settings);
                var database = client.GetDatabase("mongodb");
                var empData = database.GetCollection<Employee>("EmployeeDetails");
                var filter = Builders<Employee>.Filter.Eq(e => e.EmployeeID, data.EmployeeID);
                var result = empData.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return Json(responseData);
        }

        [HttpPost]
        [Route("GetEmployeesById")]
        public JsonResult GetEmployeesById(Employee data)
        {
            var emp = new Employee();

            const string connectionUri = "mongodb+srv://aarundas707:Arun123@cluster0.omzekzn.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            try
            {
                var client = new MongoClient(settings);
                var database = client.GetDatabase("mongodb");
                var empData = database.GetCollection<Employee>("EmployeeDetails");
                var filter = Builders<Employee>.Filter.Eq(e => e.EmployeeID, data.EmployeeID);
                var employee = empData.Find(filter).FirstOrDefault();

                if (employee != null)
                {
                    emp.Id = employee.Id;
                    emp.EmployeeID = employee.EmployeeID;
                    emp.Name = employee.Name;
                    emp.Age = employee.Age;
                    emp.Address = employee.Address;
                    emp.Department = employee.Department;
                    emp.Phone = employee.Phone;
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new JsonResult(emp);

        }

    }
}
