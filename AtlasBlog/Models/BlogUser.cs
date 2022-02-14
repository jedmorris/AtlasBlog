using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AtlasBlog.Models;

public class BlogUser : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? DisplayName { get; set; }

    [NotMapped]
    public string FullName
    {
        get { return $"{LastName}, {FirstName}"; }
    }

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}