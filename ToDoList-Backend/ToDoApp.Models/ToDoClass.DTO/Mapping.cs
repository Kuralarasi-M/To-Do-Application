using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DataAccess;

namespace ToDoApp.Models.ToDoClass.DTO
{
public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ToDo, ToDoResponseDTO>();
            CreateMap<ToDoResponseDTO, ToDo>();
        }
    }

}

