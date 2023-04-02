using System;
using System.Collections.Generic;

namespace AdiNeydiProject.DAL;

public partial class Post
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? TypeId { get; set; }

    public int? CategoryId { get; set; }

    public int? UserId { get; set; }

    public string? IsApproved { get; set; }

    public DateTime? CreatedTime { get; set; }
}
