using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public interface INewsRepository
    {
        Response GetAllActiveNews(SqlConnection connection);
        Response AddNews(News news, SqlConnection connection);
    }
}