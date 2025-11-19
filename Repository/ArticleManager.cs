using SocialNetwork.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public class ArticleManager : IArticleManager
    {
        public Response AddArticle(Article news, SqlConnection connection)
        {
            Response res = new Response();

            string query = " Insert Into Article (Title, Content, Email, Image, IsActive, IsApproved) Values(@Title, @Content, @Email, @Image, @IsActive, @IsApproved)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", news.Title);
                    cmd.Parameters.AddWithValue("@Content", news.Content);
                    cmd.Parameters.AddWithValue("@Email", news.Email);
                    cmd.Parameters.AddWithValue("@Image", news.Image);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@IsApproved", 0);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        res.StatusCode = 200;
                        res.StatusMessage = "Article Added Sucessfully";
                    }
                    else
                    {
                        res.StatusCode = 100;
                        res.StatusMessage = "Article Added Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.StatusMessage = "Article Added Failed";
            }


            return res;
        }


        public Response GetArticles(Article article ,SqlConnection connection)
        {
            Response response = new Response();
            string query = string.Empty;
            if (article.Type == "User")
            {
                 query = "Select * From Article Where Email = '" + article.Email +"' And IsActive = 1";

            }
            if(article.Type  == "Page")
            {
                 query = "Select * From Article Where  IsActive = 1";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "All Article";
                List<Article> ArticleList = new List<Article>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Article art = new Article();
                    art.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    art.Title = Convert.ToString(dt.Rows[i]["Title"]);
                    art.Content = Convert.ToString(dt.Rows[i]["Content"]);
                    art.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    art.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    art.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                    //art.IsApproved = Convert.ToInt32(dt.Rows[i]["IsApproved"]);
                    ArticleList.Add(art);
                }
                if (ArticleList.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "News Data Found";
                    response.ListArticles = ArticleList;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "News Data Not Found";
                    response.ListArticles = null; ;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "News Data Not Found";
                response.ListArticles = null;
            }


            return response;
        }

        public Response ArticleAproval(Article artiel, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                string query = "UPDATE Article SET IsApproved = 1 WHERE Id = @Id AND IsActive = 1";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", artiel.Id);
                    connection.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Article Approved";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Article Approval Failed";
                    }

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "Article Approval Failed: " + ex.Message;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return response;
        }

    }
}
