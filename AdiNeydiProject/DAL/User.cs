using System;
using System.Collections.Generic;

namespace AdiNeydiProject.DAL;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? LastLogin { get; set; }

    public int? UserTypeId { get; set; }

    public string? IsPhoneVerificated { get; set; }

    public byte[]? PasswordHash { get; set; }

    public DateOnly? CreatedTime { get; set; }
}
