using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Repository;
using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly INewsRepository newsRepository;
        public NewsController(IConfiguration configuration, INewsRepository repoNews)
        {
            _configuration = configuration;
            newsRepository = repoNews;
        }
        [HttpPost]
        [Route("AddNews")]
        public Response AddNews(News news)
        {
            Response res = new Response();

            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            sqlConnection.Open();
            res = newsRepository.AddNews(news, sqlConnection);
            return res;
        }

        [HttpGet]
        [Route("NewsList")]
        public Response GetAllNews()
        {
            Response res = new Response();
            SqlConnection connection  = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            res = newsRepository.GetAllActiveNews(connection);
            return res;
        }
    }
}
