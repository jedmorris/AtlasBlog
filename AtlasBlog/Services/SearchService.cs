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

    public List<BlogPost> TermSearch(string searchTerm)
    {
        var resultSet = _dbContext.BlogPosts
            .Where(b => b.BlogPostState == Enums.BlogPostState.ProductionReady && !b.IsDeleted);
    }






}