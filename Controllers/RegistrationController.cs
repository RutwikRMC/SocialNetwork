
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SocialNetwork.Models;
using SocialNetwork.Repository;

namespace SocialNetwork.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationManager _registrationManager;
        public RegistrationController(IConfiguration configuration, IRegistrationManager registrationManager)
        {
            _configuration = configuration;
            _registrationManager = registrationManager;
        }

        [HttpPost]
        [Route("Registration")]
        public Response Registration([FromBody] Registration registration)
        {   
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            response = _registrationManager.Registration(registration, connection);
            return response;
        }

        [HttpPost]
        [Route("Login")]
        public Response Login([FromBody] Registration registration)
        {
            Response response;
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString()))
            {
                connection.Open();
                response = _registrationManager.Login(registration, connection);
            }
            return response;
        }

        [HttpPost]
        [Route("UserApproval")]
        public Response UserApproval(Registration registration)
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                response = _registrationManager.UserAproval(registration, connection);
            }
            return response;
        }

        [HttpPost]
        [Route("StaffRegistration")]

        public Staff StaffRegistration(Staff staff)
        {
            Response response = new Response();

            using(SqlConnection conneciton = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                response = _registrationManager.StaffRegistration(staff, conneciton);
            }
            return staff;
        }

        [HttpDelete]
        [Route("DeleteStaff/{id}")]
        public Response DeleteStaff(int id)
        {
            using(SqlConnection connection  = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                return _registrationManager.DeleteStaff(id, connection);
            }
        }

        [HttpPost]
        [Route("GetRegistrations")]
        public Response GetRegistrations(Registration reg)
        {
            Response res = new Response();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                res = _registrationManager.GetRegList(reg, connection);
            }
            return res;

        }

        [HttpGet]
        [Route("StaffList")]
        public Response GetStaffDetails()
        {
            Response res = new Response();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                res = _registrationManager.GetStaffList(conn);

            }

            return res;
        }

    }
}
