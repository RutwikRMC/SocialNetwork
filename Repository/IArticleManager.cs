using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public interface IArticleManager
    {
        Response AddArticle(Article news, SqlConnection connection);

        Response GetArticles(Article article, SqlConnection connection);

        Response ArticleAproval(Article artiel, SqlConnection connection);
    }
}