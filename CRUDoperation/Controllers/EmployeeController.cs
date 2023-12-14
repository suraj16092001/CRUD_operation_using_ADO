using CRUDoperation.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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

        //public object JsonSerializer { get; private set; }

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
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
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            //const string connectionString = "Data Source=localhost;Initial Catalog=suraj;User id=root;Password=Mysql@123;";
            const string sp = "GetAllData";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(sp, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
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
                    employeemodel.imagePath = reader["image"].ToString();

                    employeeList.Add(employeemodel);
                }

            }
            Console.WriteLine(Json(employeeList));
            return Json(employeeList);
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
                command.CommandType=System.Data.CommandType.StoredProcedure;
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost,RequestSizeLimit(25 * 1000 * 1024)]
        //[RequestSizeLimit(10485760)]
        //[FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IActionResult Create(string model, IFormFile file)
        {
            //EmployeeModel employee = JsonConverter<<EmployeeModel>(model)>;
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;
            employee.imageFile = file;

            employee.imagePath = UploadImage(employee.imageFile);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string sp = "InsertData";

                    using (MySqlCommand command = new MySqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        command.Parameters.AddWithValue("@firstName", employee.firstName);
                        command.Parameters.AddWithValue("@lastName", employee.lastName);
                        command.Parameters.AddWithValue("@contactNo", employee.contactNo);
                        command.Parameters.AddWithValue("@emailId", employee.emailId);
                        command.Parameters.AddWithValue("@age", employee.age);
                        command.Parameters.AddWithValue("@imagePath", employee.imagePath);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return Json("Index");
        }

        public IActionResult Update(int? id)
        {

            EmployeeModel employeemodel = null;
            const string queryString = "GetImage";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(queryString, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employeemodel = new EmployeeModel();
                    employeemodel.id = (int)reader["emp_id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.age = reader["emp_age"].ToString();
                    employeemodel.contactNo = reader["contact"].ToString();
                    employeemodel.emailId = reader["email"].ToString();
                    employeemodel.imagePath = reader["image"].ToString();

                }

            }
            return Json(employeemodel);
        }

        [HttpPost]
        public IActionResult Update1([FromBody] EmployeeModel employee, int id)
        {
            try
            {

                string sp = "UpdateData";
                using (MySqlConnection connection1 = new MySqlConnection(connectionString))
                using (MySqlCommand command = new MySqlCommand(sp, connection1))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection1.Open();
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@firstName", employee.firstName);
                    command.Parameters.AddWithValue("@lastName", employee.lastName);
                    command.Parameters.AddWithValue("@contactNo", employee.contactNo);
                    command.Parameters.AddWithValue("@emailId", employee.emailId);
                    command.Parameters.AddWithValue("@age", employee.age);
                    command.Parameters.AddWithValue("@imagePath", employee.imagePath);
                    
                    command.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        public IActionResult Delete(int? id)
        {
            string sp = "DeleteData";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sp, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return Json("Index");
        }
    }
}
