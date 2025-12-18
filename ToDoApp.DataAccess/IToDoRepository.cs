using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.DataAccess
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetAll(int UserID);
        Task<ToDo?> GetToDoById(int id, int userId);
        Task<ToDo> Post(ToDo toDo);
        Task<bool> UpdateById(int id, ToDo todo,int userId);
        Task<bool> DeleteToDoById(int id, int userId);
    }
}
