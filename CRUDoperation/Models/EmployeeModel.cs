namespace CRUDoperation.Models
{
    public class EmployeeModel
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactNo { get; set; }
        public string emailId { get; set; }
        public string age { get; set; }

        public string imagePath { get; set; }

        public IFormFile imageFile { get; set; }
    }
}
