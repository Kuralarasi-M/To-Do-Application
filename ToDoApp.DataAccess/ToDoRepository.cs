using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.ContextFolder;

namespace ToDoApp.DataAccess
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoContext _dbContext;

        public ToDoRepository(ToDoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ToDo>> GetAll(int UserID)
        {
            return await  _dbContext.ToDos.Where(x=> x.UserId == UserID).ToListAsync();
        }

        public async Task<ToDo?> GetToDoById(int id, int userId)
        {
            return await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<ToDo> Post(ToDo toDo)
        {
              _dbContext.ToDos.Add(toDo);
            await _dbContext.SaveChangesAsync();
            return  toDo;
        }

        public async Task<bool> UpdateById(int id, ToDo toDo,int userId)
        {

            var existingToDo = await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (existingToDo == null) return false;

            existingToDo.Title = toDo.Title;
            existingToDo.Description = toDo.Description;
            existingToDo.Date = toDo.Date;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteToDoById(int id,int userId)
        {

            var existingToDo = await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (existingToDo == null) return false;

            _dbContext.ToDos.Remove(existingToDo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
