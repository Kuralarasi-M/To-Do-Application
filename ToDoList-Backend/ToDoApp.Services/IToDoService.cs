using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DataAccess;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDo>> GetAllToDos(int UserID);
        Task<ToDo?> GetById(int id, int userId);
        Task CreateToDo(ToDo todo);
        Task<bool> UpdateToDo(int id, ToDo todo,int userId);
        Task<bool> DeleteToDo(int id,int userId);
    }
}
