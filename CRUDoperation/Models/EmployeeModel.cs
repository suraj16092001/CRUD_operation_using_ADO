using System.ComponentModel.DataAnnotations;

namespace CRUDoperation.Models
{
    public class EmployeeModel
    {
        [Key]
        public int id { get; set; }


        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20,ErrorMessage ="First Name not be exceed")]
        public string firstName { get; set; }



        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(20, ErrorMessage = "Last Name not be exceed")]
        public string lastName { get; set; }


        // [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact No should be a 10-digit number")]
        public string contactNo { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string emailId { get; set; }


        [Range(18, 99, ErrorMessage = "Age must be between 18 and 99")]
        public string age { get; set; }


        [Display(Name = "Profile Image")]
        public string imagePath { get; set; }

        [Display(Name = "Upload Image")]
        [Required(ErrorMessage = "Please choose a file.")]

        public IFormFile imageFile { get; set; }
    }
}
