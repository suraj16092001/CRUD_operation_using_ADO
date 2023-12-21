using CRUDoperation.EmployeeBussinessManager.IBAL;
using CRUDoperation.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRUDoperation.Controllers
{
    public class EmployeeController : Controller
    {
        IConfiguration _configuration;
        string connectionString ;
        IEmployeeBAL _IEmployeeBAL;

        public EmployeeController(IConfiguration configuration, IEmployeeBAL employeeBAL)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _IEmployeeBAL = employeeBAL;
        }


        public IActionResult Index()//used for getting all records in list
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            //const string connectionString = "Data Source=localhost;Initial Catalog=suraj;User id=root;Password=Mysql@123;";
            const string sp = "Getalldata";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(sp, connection))
            {
                command.CommandType= CommandType.StoredProcedure;
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeModel employeemodel = new EmployeeModel(); 
                    employeemodel.id =(int)reader["emp_id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.age = reader["emp_age"].ToString();
                    employeemodel.contactNo = reader["contact"].ToString();
                    employeemodel.emailId = reader["email"].ToString();
                    employeemodel.imagePath = reader["image"].ToString();

                    employeeList.Add(employeemodel);


                }

            }
            return View(employeeList);
        }

        public IActionResult EmployeeList()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost,RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Create(string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            

            var result = _IEmployeeBAL.Create(employee,file);

            if (result == "EmailExists")
            {
                return Json("Email Id Already Exists!");
            }

            return Json("Index");

        }
        private string UploadImage(IFormFile imageFile)
        {
            try
            {
                string uniqueFileName = null;

                if (imageFile != null)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Create the directory if it doesn't exist

                    Console.WriteLine(Directory.GetCurrentDirectory());

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                }
                else
                {
                    Console.WriteLine("Image file path is null");
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public IActionResult Update(int id)
        {
            return Json(_IEmployeeBAL.Update(id));
        }

        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Update1(int id, string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            _IEmployeeBAL.Update1(id, employee, file);

            return Json("Index");

        }
        
        public IActionResult Delete(int id)
        {
            _IEmployeeBAL.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult searchdata(string search)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            //const string connectionString = "Data Source=localhost;Initial Catalog=suraj;User id=root;Password=Mysql@123;";
            string queryString = "SearchData(search)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(queryString, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeModel employeemodel = new EmployeeModel();
                    employeemodel.id = (int)reader["emp_id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.age = reader["emp_age"].ToString();
                    employeemodel.contactNo = reader["contact"].ToString();
                    employeemodel.emailId = reader["email"].ToString();

                    employeeList.Add(employeemodel);
                }
            }
            return View("index", employeeList);
        }
    }
}
