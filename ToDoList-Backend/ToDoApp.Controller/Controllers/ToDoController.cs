using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.DataAccess;
using ToDoApp.Services;
using ToDoApp.Models.ToDoClass.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ToDoApp.Controller.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class ToDoController : ControllerBase
    {
        

        private readonly IToDoService service;
        public ToDoController(IToDoService service)
        {
            this.service = service;
        }

        // GET: api/<ToDoDemoController>
       // [AllowAnonymous]
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ToDoResponseDTO>>> GetAll()
        {
            try
            {
                var userID = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier);
                if (userID == null)
                {
                    return Ok(new { message = "Login Again!" });
                }

                int UserID = int.Parse(userID.Value);  


                var toDo = await service.GetAllToDos(UserID);
                if (toDo == null || !toDo.Any())

                {
                    return NotFound(new { message = $"No ToDo found!." });
                }
                var todo = toDo.Select(t => new ToDoResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Date = t.Date
                    
                });
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }


        // GET api/<ToDoDemoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoResponseDTO>> GetById(int id)
        {
            try
            {
                var userID = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userID == null)
                {
                    return Ok(new { message = "Login Again!" });
                }

                int UserID = int.Parse(userID.Value);


                var todo = await service.GetById(id,UserID);
                
                if (todo == null)
                    return NotFound(new { message = $"No ToDo found with Id = {id}" });
                var task = new ToDoResponseDTO
                {
                    Id=todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Date = todo.Date,
                };
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }


        // POST api/<ToDoDemoController>
        [HttpPost]
        public async Task<ActionResult<ToDoResponseDTO>> Post([FromBody] TodoClassDTO dto)
        {
            var Id = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
            if (Id == null)
            {
                return Ok(new { message = "Login Again!" });
            }
            int userId = int.Parse(Id.Value);
            var todo = new ToDo
            {
                UserId=userId,
                Title = dto.Title,
                Description = dto.Description,
                Date = DateTime.Now
            };

            await service.CreateToDo(todo);

            var task = new ToDoResponseDTO
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Date = todo.Date
                
            };

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, task);
        }

        // PUT api/<ToDoDemoController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoClassDTO>> Update(int id, [FromBody] TodoClassDTO toDo)
        {
            try
            {
                var Id = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (Id == null)
                {
                    return Ok(new { message = "Login Again!" });
                }
                int userId = int.Parse(Id.Value);
                var todo = new ToDo
                {
                    UserId = userId,
                    Title = toDo.Title,
                    Description = toDo.Description,
                    Date = DateTime.Now,
                };

                bool updated = await service.UpdateToDo(id, todo,userId);
                if (!updated)
                {
                    return NotFound(new { message = $"No ToDo found with Id = {id}" });
                }

                var task = new ToDoResponseDTO
                {
                    Id = id,
                    Title = todo.Title,
                    Description = toDo.Description,
                    Date = todo.Date,
                };
                return Ok(new { message = $"ToDo with Id = {id} updated successfully.",data=task });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }




        }

        // DELETE api/<ToDoDemoController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> DeleteToById(int id)
        {
            try
            {
                var Id = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (Id == null)
                {
                    return Ok(new { message = "Login Again!" });
                }
                int userId = int.Parse(Id.Value);
                bool deleted = await service.DeleteToDo(id,userId);
                if (!deleted)
                {
                    return NotFound(new { message = $"No ToDo found with Id = {id}" });
                }

                return Ok(new { message = $"ToDo with Id = {id} Deleted successfully." });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }
    }
}
