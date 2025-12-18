using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.ToDoClass.DTO
 {
    public class ToDoResponseDTO
    {
        public int Id { get; set; }
       
        public string? Title { get; set; }
        
        public string? Description { get; set; }
     
        public DateTime? Date { get; set; }
       
    }
}
