using CRUDoperation.Models;
using CRUDoperation.EmployeeBussinessManager.IBAL;
using CRUDoperation.EmployeeDataManager.DAL;
using CRUDoperation.EmployeeDataManager.IDAL;
using CRUDoperation.Models;

namespace CRUDoperation.EmployeeBussinessManager.BAL
{
    public class EmployeeBAL : IEmployeeBAL
    {
        IEmployeeDAL _IEmployeeDAL;
        private IFormFile file;

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
        public EmployeeModel Create(EmployeeModel employee, IFormFile file)
        {
            employee.imageFile = file;

            employee.imagePath = UploadImage(employee.imageFile);

            return _IEmployeeDAL.Create(employee);

        }


        public void Delete(int id)
        {
           

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

       
    }
}