using System.Threading.Tasks;
using ToDoApp.DataAccess;

namespace ToDoApp.Services
{

    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository repository;

        public ToDoService(IToDoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ToDo>> GetAllToDos(int UserID)
        {
            return await repository.GetAll(UserID);
        }

        public async Task<ToDo?> GetById(int id,int userId)
        {
            return await repository.GetToDoById(id,userId);
        }

        public async Task CreateToDo(ToDo todo)
        {
           await repository.Post(todo);
        }

        public async Task<bool> UpdateToDo(int id, ToDo todo,int userId)
        {
            bool exist = await repository.UpdateById(id, todo,userId);
            return  exist;
        }

        public async Task<bool> DeleteToDo(int id,int userId )
        {
            bool exist =await  repository.DeleteToDoById(id,userId);
            return exist;
        }
    }
}
