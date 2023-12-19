using CRUDoperation.Models;
using CRUDoperation.Models;

namespace CRUDoperation.EmployeeBussinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        public List<EmployeeModel> GetEmployeeList();
        public EmployeeModel Create(EmployeeModel employee,IFormFile file);
        public void Delete(int id);
    }
}