using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.ToDoClass.DTO
{
    public class RegisterDto
    {
        [MinLength(3)]
        public string UserName { get; set; } = null!;
        [MinLength(4)]
        public string Password { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

    }
}
