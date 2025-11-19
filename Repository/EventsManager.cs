using SocialNetwork.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public class EventsManager : IEventsManager
    {
        public Response AddEvent(Events _event, SqlConnection connection)
        {
            Response res = new Response();
            string query = " Insert Into Events (Title, Content, Email, IsActive, CreatedOn) Values(@Title, @Content, @Email, @IsActive, @CreatedOn)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", _event.Title);
                    cmd.Parameters.AddWithValue("@Content", _event.Content);
                    cmd.Parameters.AddWithValue("@Email", _event.Email);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        res.StatusCode = 200;
                        res.StatusMessage = "Event Added Sucessfully";
                    }
                    else
                    {
                        res.StatusCode = 100;
                        res.StatusMessage = "Event Added Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.StatusMessage = "Event Added Failed";
            }
            return res;
        }

        public Response GetAllEvents(SqlConnection connection)
        {
            Response response = new Response();
            string query = "Select * From Events Where IsActive = 1";
            SqlDataAdapter da = new SqlDataAdapter(query, connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "All Events";
                List<News> newsList = new List<News>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    News news = new News();
                    news.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    news.Title = Convert.ToString(dt.Rows[i]["Title"]);
                    news.Content = Convert.ToString(dt.Rows[i]["Content"]);
                    news.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                    news.CreatedOn = Convert.ToString(dt.Rows[i]["CreatedOn"]);
                    newsList.Add(news);
                }
                if (newsList.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Events Data Found";
                    response.ListNews = newsList;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Events Data Not Found";
                    response.ListNews = null; ;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Events Data Not Found";
                response.ListNews = null;
            }


            return response;
        }
    }
}
