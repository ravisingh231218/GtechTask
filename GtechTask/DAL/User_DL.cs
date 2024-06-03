//using GtechTask.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Data;

//namespace GtechTask.DAL
//{
//    public class User_DL
//    {
//        private readonly string _connectionString;

//        public User_DL(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("Conn");
//        }

//        public void InsertUserPlan(Users users)
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    using (var command = new SqlCommand("InsertPlan", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;

//                        command.Parameters.AddWithValue("@PlaneName", users.PlaneName);
//                        command.Parameters.AddWithValue("@TenureMonths", users.TenureMonths);
//                        command.Parameters.AddWithValue("@ROI", users.ROI);

//                        connection.Open();
//                        command.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (SqlException ex)
//            {

//                throw new Exception("An error occurred while inserting the user plan.", ex);
//            }
//        }
//    }
//}


using GtechTask.Models;
using Microsoft.Data.SqlClient;

using System.Data;

namespace GtechTask.DAL
{
    public class User_DL
    {
        private readonly string _connectionString;

        public User_DL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conn");
        }

        public List<Users> GetAllPlans()
        {
            var plans = new List<Users>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT Id, PlaneName, TenureMonths, ROI FROM InvestPlans", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        plans.Add(new Users
                        {
                            Id = reader.GetInt32(0),
                            PlaneName = reader.GetString(1),
                            TenureMonths = reader.GetInt32(2),
                            ROI = reader.GetDecimal(3)
                        });
                    }
                }
            }

            return plans;
        }

        public Users GetPlanDetails(string planeName)
        {
            Users plan = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetPlanDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PlaneName", planeName);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        plan = new Users
                        {
                            PlaneName = reader.GetString(0),
                            TenureMonths = reader.GetInt32(1),
                            ROI = reader.GetDecimal(2)
                        };
                    }
                }
            }

            return plan;
        }

        public void InsertUserPlan(Users users)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("InsertPlan", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PlaneName", users.PlaneName);
                        command.Parameters.AddWithValue("@TenureMonths", users.TenureMonths);
                        command.Parameters.AddWithValue("@ROI", users.ROI);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while inserting the user plan.", ex);
            }
        }
    }
}

