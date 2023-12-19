using CRUDoperation.Models;

namespace CRUDoperation.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        public List<EmployeeModel> GetEmployeeList();
        public EmployeeModel Create(EmployeeModel employee);

        public void Delete(int id);
    }
}