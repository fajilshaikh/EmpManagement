using EmpManagement.Models;
using System.Data;
using System.Data.SqlClient;

namespace EmpManagement.DAL
{
    public class Client
    {
        public static IConfiguration configuration { get; set; }
        public static SqlConnection _connection;
        public static SqlCommand _command;

        public static string GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configuration = builder.Build();
            return configuration.GetConnectionString("DefaultConnection");
        }

        public static List<Employee> GetAll(int id)
        {
            List<Employee> employee = new List<Employee>();
            using (_connection = new SqlConnection(GetConnection()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "GetEmployee";
                _command.Parameters.Add(new SqlParameter("@Id", id));
                _connection.Open();

                SqlDataReader dr = _command.ExecuteReader();
                while (dr.Read())
                {
                    Employee emp = new Employee();
                    emp.EmpID = Convert.ToInt32(dr["EmpID"]);
                    emp.EmployeeCode = dr["EmployeeCode"].ToString();
                    emp.FirstName = dr["FirstName"].ToString();
                    emp.LastName = dr["LastName"].ToString();
                    emp.Address = dr["Address"].ToString();
                    emp.Email = dr["Email"].ToString();

                    string mobileNumbersString = dr["MobileNo"].ToString();
                    if (!string.IsNullOrEmpty(mobileNumbersString))
                    {
                        emp.MobileNo = mobileNumbersString.Replace(" ","").Split(',').ToList();
                    }
                    else
                    {
                        emp.MobileNo = new List<string>();
                    }

                    employee.Add(emp);
                }
            }
            return employee;
        }
        public static int SaveUpdate(Employee emp, out int RtValue)
        {
            using (_connection = new SqlConnection(GetConnection()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "EmployeeInsertUpdate";
                _command.Parameters.Add(new SqlParameter("@EmpID", emp.EmpID));
                _command.Parameters.Add(new SqlParameter("@EmployeeCode", emp.EmployeeCode));
                _command.Parameters.Add(new SqlParameter("@FirstName", emp.FirstName));
                _command.Parameters.Add(new SqlParameter("@LastName", emp.LastName));
                _command.Parameters.Add(new SqlParameter("@Address", emp.Address));
                _command.Parameters.Add(new SqlParameter("@Email", emp.Email));
                _command.Parameters.Add(new SqlParameter("@MobileNo", string.Join(",", emp.MobileNo)));

                _command.Parameters.Add("@ReturnVal", SqlDbType.Int, int.MaxValue);
                _command.Parameters["@ReturnVal"].Direction = ParameterDirection.Output;
                _connection.Open();

                _command.ExecuteNonQuery();
                RtValue = Convert.ToInt32(_command.Parameters["@ReturnVal"].Value);
                return RtValue;
            }
        }

        public static bool Delete(int id)
        {
            using (_connection = new SqlConnection(GetConnection()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "DeleteEmployee";
                _command.Parameters.Add(new SqlParameter("@id", id));
                _connection.Open();

                int result = _command.ExecuteNonQuery();
                return result > 0 ? true : false;
            }
        }
    }
}
