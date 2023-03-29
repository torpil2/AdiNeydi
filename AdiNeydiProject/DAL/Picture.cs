using System;
using System.Collections.Generic;

namespace AdiNeydiProject.DAL;

public partial class Picture
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Path { get; set; }

    public int? Order { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }
}
