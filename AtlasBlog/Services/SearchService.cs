using AtlasBlog.Data;
using AtlasBlog.Models;

namespace AtlasBlog.Services;

public class SearchService
{
    private readonly ApplicationDbContext _dbContext;

    public SearchService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<BlogPost> TermSearch(string searchTerm)
    {
        var resultSet = _dbContext.BlogPosts
            .Where(b => b.BlogPostState == Enums.BlogPostState.ProductionReady && !b.IsDeleted).AsQueryable();
        
        // If a search term is supplied, look for it in the resultSet
        if (!string.IsNullOrEmpty(searchTerm))
        {
            resultSet = resultSet.Where(
                b => b.Title.ToLower().Contains(searchTerm) ||
                     b.Abstract.ToLower().Contains(searchTerm) ||
                     b.Body.ToLower().Contains(searchTerm) ||
                     b.Comments.Any(c =>
                         c.CommentBody.ToLower().Contains(searchTerm) ||
                         c.Author!.FirstName.ToLower().Contains(searchTerm) ||
                         c.Author.LastName.ToLower().Contains(searchTerm) ||
                         c.Author.Email.ToLower().Contains(searchTerm) ||
                         c.ModerateBody!.ToLower().Contains(searchTerm)));
        }

        return resultSet.OrderByDescending(r => r.Created);
    }






}