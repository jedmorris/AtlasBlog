using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AtlasBlog.Models;

public class Blog
{
   public int Id { get; set; }
   
   [Required]
   [Display(Name = "Blog Name")]
   [StringLength(100, ErrorMessage = "The {0} must be at most {1} and at least {2} characters long.", MinimumLength = 2)]
   public string BlogName { get; set; } = "";

   [Required]
   [StringLength(300, ErrorMessage = "The {0} must be at most {1} and at least {2} characters long.", MinimumLength = 2)]
   public string Description { get; set; } = "";
   
   [DataType(DataType.Date)]
   public DateTime Created { get; set; }
   
   public DateTime? Updated { get; set; }

   [Display(Name = "Choose Image")]
   public byte[] ImageData { get; set; } = Array.Empty<byte>();

   public string ImageType { get; set; } = "";
   
   // This model should have a list of Posts as children
   public ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
}