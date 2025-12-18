using System;
using System.Collections.Generic;

namespace ToDoApp.DataAccess;

public partial class User
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int UserId { get; set; }
}
