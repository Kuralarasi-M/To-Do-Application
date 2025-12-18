using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.ContextFolder;

namespace ToDoApp.DataAccess
{
    public class TokenDataAccess : ITokenDataAccess
    {
        private readonly ToDoContext _dbContext;

        public TokenDataAccess(ToDoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateNewUser(User user)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (existingUser != null) return "User already exists";

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return "Added successfully";
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task<User?> GetUserById(int userId)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            return existingUser;
        }
    }
}
