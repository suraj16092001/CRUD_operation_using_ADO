using CRUDoperation.Models;
using CRUDoperation.EmployeeBussinessManager.IBAL;
using CRUDoperation.EmployeeDataManager.DAL;
using CRUDoperation.EmployeeDataManager.IDAL;
using CRUDoperation.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CRUDoperation.EmployeeBussinessManager.BAL
{
    public class EmployeeBAL : IEmployeeBAL
    {
        IEmployeeDAL _IEmployeeDAL;

        public EmployeeBAL(IDBManager dBManager)
        {
            _IEmployeeDAL = new EmployeeDAL(dBManager);
        }

        //Get all employee list
        List<EmployeeModel> IEmployeeBAL.GetEmployeeList()
        {
            return _IEmployeeDAL.GetEmployeeList();
        }

        //Add employee data to database
        public string Create(EmployeeModel employee, IFormFile file)
        {
            employee.imageFile = file;

            employee.imagePath = UploadImage(employee.imageFile);

            bool emailExists = CheckEmailExits(employee.emailId);

            if (emailExists)
            {
                return "EmailExists";
            }
            else 
            {
                _IEmployeeDAL.Create(employee);
                return "Success";
            }

        }

        public bool CheckEmailExits(string email)
        {
            return _IEmployeeDAL.CheckEmailExits(email);
        }

        public string UploadImage(IFormFile imageFile)
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

        //Delete existing file from images folder and delete data 
        public void Delete(int id)
        {
            string existingImage = _IEmployeeDAL.GetProfileImageById(id);

            if (!string.IsNullOrEmpty(existingImage))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _IEmployeeDAL.Delete(id);

        }

        public EmployeeModel Update(int id)
        {
            return _IEmployeeDAL.Update(id);
        }

        public EmployeeModel Update1(int id, EmployeeModel employee, IFormFile file)
        {
            employee.id = id;

            employee.imageFile = file;

                string existingImage = _IEmployeeDAL.GetProfileImageById(id); 

                if (employee.imageFile != null)
                {
                    employee.imagePath = UploadImage(employee.imageFile);
                }
                else
                {
                    employee.imagePath = existingImage;
                }

                return _IEmployeeDAL.Update1(employee);
        }
    }
}