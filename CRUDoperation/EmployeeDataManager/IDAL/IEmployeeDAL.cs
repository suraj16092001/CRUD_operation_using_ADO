using CRUDoperation.Models;

namespace CRUDoperation.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        public List<EmployeeModel> GetEmployeeList();
        public EmployeeModel Create(EmployeeModel employee);

        public void Delete(int id);

        //Delete existing file from images folder 
        public string GetProfileImageById(int id);

        public EmployeeModel Update(int id);

        public EmployeeModel Update1(EmployeeModel employee);
    }
}