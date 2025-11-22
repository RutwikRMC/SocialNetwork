using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using SocialNetwork.Repository;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IArticleManager articleRepository;
        public ArticleController(IConfiguration configuration, IArticleManager article)
        {
            _configuration = configuration;
            articleRepository = article;
            // push on github 1
        }

        [HttpPost]
        [Route("AddArticle")]
        public Response AddArticle([FromForm] Article article, IFormFile ImageFile)
        {
            Response res = new Response();
            try
            {
                // Save the image if uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Ensure folder exists
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    // Save image file
                    var filePath = Path.Combine(imagesFolder, ImageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Save relative path to DB
                    article.Image = "/images/" + ImageFile.FileName;
                }

                // Save to DB
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    sqlConnection.Open();
                    res = articleRepository.AddArticle(article, sqlConnection);
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.StatusMessage = $"Error while adding article: {ex.Message}";
            }

            return res;
        }


        [HttpPost]
        [Route("ArticleList")]
        public Response GetAllArticles([FromBody] Article  article)
        {
            Response res = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            res = articleRepository.GetArticles(article,connection);
            return res;
        }

        [HttpPost]
        [Route("ArticleApproval")]
        public Response ArticleApproval(Article article)
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                response = articleRepository.ArticleAproval(article, connection);
            }
            return response;
        }
    }
}
