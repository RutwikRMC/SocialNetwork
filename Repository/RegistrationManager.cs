using SocialNetwork.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocialNetwork.Repository
{
    public class RegistrationManager : IRegistrationManager
    {
        public Response Registration(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                string query = @"INSERT INTO Registration (Name, Email, Password, PhoneNo, IsActive, IsApproved, UserType) 
                                 VALUES (@Name, @Email, @Password, @PhoneNo, @IsActive, @IsApproved, @UserType)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", registration.Name);
                    cmd.Parameters.AddWithValue("@Email", registration.Email);
                    cmd.Parameters.AddWithValue("@Password", registration.Password);
                    cmd.Parameters.AddWithValue("@PhoneNo", registration.PhoneNo);
                    cmd.Parameters.AddWithValue("@IsActive", 0);
                    cmd.Parameters.AddWithValue("@IsApproved", 0);
                    cmd.Parameters.AddWithValue("@UserType", registration.UserType);


                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Registration Successfull";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Registration Failed";
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;  
                response.StatusMessage = ex.Message;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return response;
        }

        public Response Login(Registration registration, SqlConnection connection)
        {
            Response response = new Response();

            try
            {
                string query = "SELECT * FROM Registration WHERE Email = @Email AND Password = @Password";

                SqlDataAdapter ad = new SqlDataAdapter(query, connection);
                ad.SelectCommand.Parameters.AddWithValue("@Email", registration.Email);
                ad.SelectCommand.Parameters.AddWithValue("@Password", registration.Password);

                DataTable dt = new DataTable();
                ad.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Login Successful";

                    Registration reg = new Registration();
                    reg.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    reg.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    reg.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    
                    response.Registration = reg;
                }
                else
                {
                    response.StatusCode = 401; // Unauthorized
                    response.StatusMessage = "Invalid Email or Password";
                    response.Registration = null;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.StatusMessage = ex.Message;
                response.Registration = null ;
            }

            return response;
        }

       public Response UserAproval(Registration registration, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                string query = "UPDATE Registration SET IsApproved = 1 WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", registration.Id);

                    if (connection != null && connection.State != ConnectionState.Open)
                        connection.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "User Approved";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "User Approval Failed";
                    }

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "User Approval Failed: " + ex.Message; 
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return response;
        }

        public Response StaffRegistration(Staff Staffregistration, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                string query = @"INSERT INTO Staff (Name, Email, Password, IsActive) 
                                 VALUES (@Name, @Email, @Password, @IsActive)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", Staffregistration.Name);
                    cmd.Parameters.AddWithValue("@Email", Staffregistration.Email);
                    cmd.Parameters.AddWithValue("@Password", Staffregistration.Password);
                    cmd.Parameters.AddWithValue("@IsActive", Staffregistration.IsActive);

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Staff Registration Successfull";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = " Staff Registration Failed";
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return response;
        }

        public Response DeleteStaff(int id , SqlConnection conn)
        {
           Response res = new Response();

            try
            {
                string query = "Delete from Staff Where Id = @Id And IsActive = 1";

                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        res.StatusCode = 200;
                        res.StatusMessage = "Staff Deleted Sucessfully";
                    }
                    else
                    {
                        res.StatusCode = 100;
                        res.StatusMessage = " Failed To Delete Staff";
                    }

                }
            }
            catch(Exception ex)
            {
                res.StatusCode = 500;
                res.StatusMessage = ex.Message;
            }
           return res;
        }

        public Response GetRegList(Registration registration,SqlConnection conn)
        {
            Response res = new Response();
            List<Registration> registrations = new List<Registration>();

            try
            {
                string query = "SELECT * FROM Registration Where UserType = 'User'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserType", registration.UserType);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) 
                        {
                            Registration reg = new Registration
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                Name = reader["Name"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                Password = reader["Password"]?.ToString(),
                                PhoneNo = reader["PhoneNo"]?.ToString(),
                                IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToInt32(reader["IsActive"]) : 0,
                                IsApproved = reader["IsApproved"] != DBNull.Value ? Convert.ToInt32(reader["IsApproved"]) : 0
                            };

                            registrations.Add(reg);
                        }
                    }
                }

                res.StatusCode = 200;
                res.StatusMessage = "Data retrieved successfully.";
                res.ListRegistration = registrations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res.StatusCode = 100;
                res.StatusMessage = ex.Message;
                res.ListRegistration = null;
            }

            return res;
        }

        public Response GetStaffList(SqlConnection connection)
        {
            Response res = new Response();
            List<Staff> staffList = new List<Staff>();

            string query = "SELECT Id, Name, Email, Password, IsActive FROM Staff";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Staff staff = new Staff()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    IsActive = Convert.ToInt32(reader["IsActive"])
                };
                staffList.Add(staff);
            }
            reader.Close();

            res.StatusCode = 200;
            res.StatusMessage = "Staff list fetched successfully.";
            res.ListStaff = staffList;
            return res;
        }


    }
}
