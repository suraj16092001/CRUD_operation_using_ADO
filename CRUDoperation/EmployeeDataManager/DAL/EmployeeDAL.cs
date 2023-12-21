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
        private object employeeModel;

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
        public string GetProfileImageById(int id)
        {
            string existingImage = null;

            _dBManager.InitDbCommand("GetProfileImageById");

            _dBManager.AddCMDParam("@id", id);

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                existingImage = item["image"].ConvertJSONNullToString();
            }

            return existingImage;
        }

        public EmployeeModel Update(int id)
        {
            _dBManager.InitDbCommand("GetImage");

            EmployeeModel employeemodel = null;

            _dBManager.AddCMDParam("@id", id);


            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                employeemodel = new EmployeeModel();

                // employeeModel.Id = CommonConversion.ConvertDBNullToInt(item["emp_id"]);
                employeemodel.id = item["emp_id"].ConvertDBNullToInt();
                employeemodel.firstName = item["first_name"].ConvertDBNullToString();
                employeemodel.lastName = item["last_name"].ConvertDBNullToString();
                employeemodel.contactNo = item["contact"].ConvertJSONNullToString();
                employeemodel.emailId = item["email"].ConvertJSONNullToString();
                employeemodel.age = item["emp_age"].ConvertJSONNullToString();
                employeemodel.imagePath = item["image"].ConvertJSONNullToString();

            }

            return employeemodel;

        }

        public EmployeeModel Update1(EmployeeModel employee)
        {
            _dBManager.InitDbCommand("UpdateData");

            _dBManager.AddCMDParam("@id", employee.id);
            _dBManager.AddCMDParam("@firstName", employee.firstName);
            _dBManager.AddCMDParam("@lastName", employee.lastName);
            _dBManager.AddCMDParam("@contactNo", employee.contactNo);
            _dBManager.AddCMDParam("@emailId", employee.emailId);
            _dBManager.AddCMDParam("@age", employee.age);
            _dBManager.AddCMDParam("@imagePath", employee.imagePath);

            _dBManager.ExecuteNonQuery();

            return employee;
        }

        public bool CheckEmailExits(string email)
        {
            _dBManager.InitDbCommand("ExistsEmail");

            _dBManager.AddCMDParam("@emailID", email);

            var result = _dBManager.ExecuteScalar();

            bool emailExists = Convert.ToBoolean(result);

            return emailExists;
        }

      
    }
}