using System;
using System.Collections.Generic;

namespace ToDoApp.DataAccess;

public partial class ToDo
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public int? UserId { get; set; }
}
