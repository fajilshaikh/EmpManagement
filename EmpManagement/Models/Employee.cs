using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmpManagement.Models
{
    public class Employee
    {
        public int EmpID { get; set; }
        [Required(ErrorMessage = "Please enter valid Code")]
        [DisplayName("Code")]
        public string EmployeeCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public List<string> MobileNo { get; set; }

    }
  
}
