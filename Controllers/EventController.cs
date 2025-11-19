using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Repository;
using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEventsManager eventsManager;
        public EventController(IConfiguration configuration, IEventsManager manager)
        {
            _configuration = configuration;
            eventsManager = manager;
        }

        [HttpPost]
        [Route("AddEvent")]
        public Response AddEvent(Events event_)
        {
            Response response = new Response();
            using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                response = eventsManager.AddEvent(event_, connection);

            }
            return response;
        }


        [HttpPost]
        [Route("AllEvents")]
        public Response GetAllEvents()
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                response = eventsManager.GetAllEvents(connection);

            }
            return response;
        }
    }
}
