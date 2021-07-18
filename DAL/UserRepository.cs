using Dapper;
using Authentication.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Authentication.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.DAL
{
    public class UserRepository : IUserRepository
    {
        protected readonly IConfiguration _config;
        private readonly AppSettings _appsettings;
        public UserRepository(IConfiguration config, IOptions<AppSettings> appSettings)
        {
            _config = config;
            _appsettings = appSettings.Value;
        }

        public FunctionResponse Authenticate(string userName, string password)
        {
            try
            {
                //var user = users.SingleOrDefault(x => x.userName == userName && x.password == password);

                //return null if user not found
                //if (user == null)
                //{
                //    return null;
                //}
                
                User result = null;
                using (IDbConnection dbConn = new SqlConnection(_config.GetConnectionString("ConnStr")))
                {
                    //.Open();
                    string query = @"Select * from UserTable where userName = @username and userPassword = @password";
                    result = dbConn.QueryFirstOrDefault<User>(query, new { @username = userName , @password = password});
                    if (result is null)
                        throw new Exception(message: "Invalid details");
                }

                //when user is found
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appsettings.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        //new Claim(ClaimTypes.Role = "Admin")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                result.Token = "Bearer " + jwtToken;
                return new FunctionResponse { status = "ok", result = result };
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<FunctionResponse> GetUserByIdAsync(int id)
        {
            User response = null;
            try
            {
                using (IDbConnection dbConn = new SqlConnection(_config.GetConnectionString("ConnStr")))
                {
                    //dbConn.Open();
                    string query = @"SELECT userName FROM UserTable WHERE id = @id";
                    response = await dbConn.QueryFirstOrDefaultAsync<User>(query, new { id = id });
                    if(response == null)
                    {
                        throw new Exception(message: "Invalid user ID");
                    }

                    //when user is found
                    return new FunctionResponse { status = "ok", result=response };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FunctionResponse SaveUser(User user)
        {
            try
            {
                using (IDbConnection dbConn = new SqlConnection(_config.GetConnectionString("ConnStr")))
                {
                    //.Open();
                    string query = @"INSERT INTO UserTable(userName, userEmail, userPassword) VALUES(@userName, @email, @password)";
                    dbConn.Execute(query, user);
                    return new FunctionResponse { status = "ok"};
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FunctionResponse UpdateUser(User user)
        {
            try
            {
                using (IDbConnection dbConn = new SqlConnection(_config.GetConnectionString("ConnStr")))
                {
                    dbConn.Open();
                    string query = @"UPDATE UserTable SET  userName = @userName, userEmail = @email, userPassword = @password where id = @id";
                    dbConn.Execute(query, user);
                    return new FunctionResponse { status = "ok"};
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DeleteUser(int id)
        //{
        //    try
        //    {
        //        using (IDbConnection dbConn = Connection)
        //        {
        //            dbConn.Open();
        //            string query = @"DELETE FROM UserTable WHERE id = @id";
        //            dbConn.Execute(query, new {id = id });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<IEnumerable<User>> GetAllUserAsync()
        //{
        //    try
        //    {
        //        using (IDbConnection dbConn = Connection)
        //        {
        //            dbConn.Open();
        //            string query = @"SELECT id, userName, userEmail FROM UserTable";
        //            return await dbConn.QueryAsync<User>(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<User> GetUserByIdAsync(int id)
        //{
        //    try
        //    {
        //        using (IDbConnection dbConn = Connection)
        //        {
        //            dbConn.Open();
        //            string query = @"SELECT userName FROM UserTable WHERE id = @id";
        //            return await dbConn.QueryFirstOrDefaultAsync<User>(query, new { id = id });
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<User> GetUserByUserNameAsync(string userName)
        //{
        //    try
        //    {
        //        using(IDbConnection dbConn = Connection)
        //        {
        //            dbConn.Open();
        //            string query = @"SELECT FROM UserTable WHERE userName= @userName";
        //            return await dbConn.QueryFirstOrDefaultAsync<User>(query, new { userName = userName });
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void UpdateUser(User user)
        //{
        //    try
        //    {
        //        using(IDbConnection dbConn = Connection)
        //        {
        //            dbConn.Open();
        //            string query = @"UPDATE UserTable SET userName = @userName, userEmail = @userEmail, userPassword = @userPassword";
        //            dbConn.Execute(query, user);
        //        }
        //    }catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
    //public class LoginRepository : ILoginRepository
    //{
    //    private readonly AppSettings _appsettings;
    //    public LoginRepository(IOptions<AppSettings> appSettings)
    //    {
    //        _appsettings = appSettings.Value;
    //    }
    //    private List<User> users = new List<User>()
    //    {
    //        new User {
    //            id= 1, userName ="joey",email="joey123@gmail.com", password="Joey123"
    //        }
    //    };


    //}

}
