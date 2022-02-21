using Humanizer;
using AtlasBlog.Enums;

namespace AtlasBlog.Models;

public class Comment
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }
    
    //Reference the author of the comment
    public string? AuthorId { get; set; }
    public string? ModeratorId { get; set; }

    public string CommentBody { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    // Moderator properties
    public DateTime? ModeratedDate { get; set; }
    public ModerationReason ModerationReason { get; set; }
    public string? ModerateBody { get; set; }
    
    //Nav property that is "lazy loaded"
    public virtual BlogPost? BlogPost { get; set; } 
    public virtual BlogUser? Author { get; set; }
    public virtual BlogUser? Moderator { get; set; }
}

