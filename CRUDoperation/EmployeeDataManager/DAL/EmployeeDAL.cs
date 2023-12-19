using CRUDoperation.Models;
using CRUDoperation.CommonCode;
using CRUDoperation.EmployeeDataManager.IDAL;
using MySql.Data.MySqlClient;
using System.Data;

namespace CRUDoperation.EmployeeDataManager.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {

        readonly IDBManager _dBManager;
        public EmployeeDAL(IDBManager dBManager)
        {
            _dBManager = dBManager;
            //_configuration = configuration;
            //_connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<EmployeeModel> GetEmployeeList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            _dBManager.InitDbCommand("GetAllData");

            DataSet ds = _dBManager.ExecuteDataSet();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                EmployeeModel employeemodel = new EmployeeModel();

                //employeemodel.Id = (int)reader["id"];
                employeemodel.id = item["emp_id"].ConvertDBNullToInt();
                employeemodel.firstName = item["first_name"].ConvertDBNullToString().ToString();
                employeemodel.lastName = item["last_name"].ConvertDBNullToString().ToString();
                employeemodel.contactNo = item["contact"].ConvertDBNullToString().ToString();
                employeemodel.emailId = item["email"].ConvertDBNullToString().ToString();
                employeemodel.age = item["emp_age"].ConvertDBNullToString().ToString();
                employeemodel.imagePath = item["image"].ConvertDBNullToString().ToString();
               
                //employeemodel.contactNumber = reader["contact_number"].ToString();
                //employeemodel.emailId = reader["emailid"].ToString();
                //employeemodel.age = reader["age"].ToString();
                //employeemodel.imagePath = reader["image"].ToString();

                employeeList.Add(employeemodel);

            }
           
            return employeeList;
        }

        public EmployeeModel Create(EmployeeModel employee)
        {

            _dBManager.InitDbCommand("InsertData");

            _dBManager.AddCMDParam("@firstName", employee.firstName);
            _dBManager.AddCMDParam("@lastName", employee.lastName);
            _dBManager.AddCMDParam("@contactNo", employee.contactNo);
            _dBManager.AddCMDParam("@emailId", employee.emailId);
            _dBManager.AddCMDParam("@age", employee.age);
            _dBManager.AddCMDParam("@imagePath", employee.imagePath);


            _dBManager.ExecuteNonQuery();

            return employee;
        }

        public void Delete(int id)
        {
            
            _dBManager.InitDbCommand("DeleteData");
            _dBManager.AddCMDParam("@id", id);
            _dBManager.ExecuteNonQuery();

           
        }
    }
}