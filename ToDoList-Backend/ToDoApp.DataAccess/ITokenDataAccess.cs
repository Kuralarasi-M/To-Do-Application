using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.DataAccess
{
    public interface ITokenDataAccess
    {
        Task<string> CreateNewUser(User user);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserById(int userId);
    }
}
