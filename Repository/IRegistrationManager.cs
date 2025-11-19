using SocialNetwork.Models;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public interface IRegistrationManager
    {
        Response Registration(Registration registration, SqlConnection connection);

        Response Login(Registration registration, SqlConnection connection);

        Response UserAproval(Registration registration , SqlConnection connection);


        Response StaffRegistration(Staff stafregistration, SqlConnection connection);

        Response DeleteStaff(int id, SqlConnection conn);

        Response GetRegList(Registration registration,SqlConnection conn);

        Response GetStaffList(SqlConnection conn);

    }
}