using EmpManagement.DAL;
using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = Client.GetAll(0);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(employees);
        }


        [HttpGet]
        public IActionResult CreateUpdate(int id = 0)
        {
            try
            {
                Employee emp = new Employee();
                if (id > 0)
                {
                    emp = Client.GetAll(id)[0];
                }
                return View(emp);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public  IActionResult CreateUpdate(Employee employee)
        {
            try
            {
                if (!IsValidEmail(employee.Email))
                {
                    TempData["errorMessage"] = "Invalid email address.";
                    return View(employee);
                }
                foreach (var mobileNumber in employee.MobileNo)
                {
                    if (!IsValidMobileNumber(mobileNumber))
                    {
                        TempData["errorMessage"] = "Invalid mobile number.";
                        return View(employee);
                    }
                }

                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Please enter required field.";
                    return View(employee);
                }

                int res = Client.SaveUpdate(employee, out int Rtvalue);

                if (res > 0)
                {
                    TempData["successMessage"] = employee.EmpID > 0 ? "Update successfully" : "Save successfully";
                }
                else if (res == -1)
                {
                    TempData["errorMessage"] = "Code Alreay exist.";
                    return View(employee);
                }
                else
                {
                    TempData["errorMessage"] = employee.EmpID > 0 ? "Unable to update data." : "Unable to save data.";
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                bool res = Client.Delete(id);
                if (res) { TempData["successMessage"] = "Delete successfully"; } else { TempData["errorMessage"] = "Unable to delete."; }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidMobileNumber(string mobileNumber)
        {
            return mobileNumber.Length == 10 && mobileNumber.All(char.IsDigit);
        }
    }
}
