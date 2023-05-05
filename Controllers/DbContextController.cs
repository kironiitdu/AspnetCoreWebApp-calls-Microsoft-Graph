using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Controllers
{
    public class DbContextController : Controller
    {
        private readonly AppDbContext _context;

        public DbContextController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> LOG()
        {
            try
            {
                int exists = 0;
                var userId = "admin";
                var password = "123456";
                await Task.Run(() =>
                {
                    using (var connection = _context.Database.GetDbConnection())
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_LOGIN";
                        command.Parameters.Add(new SqlParameter("@LOGIN", userId));
                        command.Parameters.Add(new SqlParameter("@PASSWORD", password));
                        exists = (int)command.ExecuteScalar();
                        command.Dispose();
                        connection.Close();

                    }
                });

                if (exists > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AnotherWay()
        {
            try
            {
                var userId = "admin";
                var password = "123456";
                int role_id = 0;
                string role_name = "";
                //using (var connection = new SqlConnection("Server=WX-6899;Database=WsAttendance;Trusted_Connection=True;MultipleActiveResultSets=true"))
                using (var connection = new SqlConnection("Server=SQL5079.site4now.net;Database=db_a82594_stockmanagement; User Id=db_a82594_stockmanagement_admin;Password=MSSQL@2014;integrated security=False;Trusted_Connection=False;MultipleActiveResultSets=True;"))
                {
                 
                    connection.Open();
                    var command = connection.CreateCommand();
                  //  command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "	select role_id,role_name  from role ";
                    
                    SqlDataReader reader =  command.ExecuteReader();
                     while (reader.Read())
                    {
                        role_id = Convert.ToInt32( reader["role_id"]); // Remember Type Casting is required here it has to be according to database column data type
                        role_name = Convert.ToString( reader["role_name"]); // Remember Type Casting is required here it has to be according to database column data type

                    }
                    reader.Close();
                    command.Dispose();
                    connection.Close();

                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


