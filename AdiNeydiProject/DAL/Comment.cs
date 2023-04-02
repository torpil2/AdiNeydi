using System;
using System.Collections.Generic;

namespace AdiNeydiProject.DAL;

public partial class Comment
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public bool? TrueComment { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public int? CommentOrder { get; set; }

    public int? RepliedCommentId { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? IpAddress { get; set; }
}
