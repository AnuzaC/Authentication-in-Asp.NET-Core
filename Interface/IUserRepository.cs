using Authentication.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interface
{
    public interface IUserRepository
    {
        FunctionResponse Authenticate(string userName, string password);
        //Task<IEnumerable<User>> GetAllUserAsync();
        Task<FunctionResponse> GetUserByIdAsync(int id);
        //Task<User> GetUserByUserNameAsync(string userName);
        FunctionResponse SaveUser(User user);
        FunctionResponse UpdateUser(User user);
        //void DeleteUser(int id);
    }
    //public interface ILoginRepository
    //{
    //    User Authenticate(string userName, string password);
    //}
}
