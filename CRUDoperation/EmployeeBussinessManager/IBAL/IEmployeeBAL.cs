using CRUDoperation.Models;
using CRUDoperation.Models;

namespace CRUDoperation.EmployeeBussinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        public List<EmployeeModel> GetEmployeeList();

        public string Create(EmployeeModel employee,IFormFile file);

        public bool CheckEmailExits(string email);

        public string UploadImage(IFormFile imageFile);

        public EmployeeModel Update(int id);

        public EmployeeModel Update1(int id, EmployeeModel employee, IFormFile file);

        public void Delete(int id);


    }
}