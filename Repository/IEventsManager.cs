using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public interface IEventsManager
    {
        Response AddEvent(Events _event, SqlConnection connection);
        Response GetAllEvents(SqlConnection connection);
    }
}