using System.Diagnostics;
using AtlasBlog.Data;
using Microsoft.AspNetCore.Mvc;
using AtlasBlog.Models;
using X.PagedList;

namespace AtlasBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(int? pageNum)
    {
        pageNum ??= 1;
        
        // PagedLists always need to know what page to display and order them explicitly
        var blogs = await _context.Blogs
            .OrderByDescending(b => b.Created)
            .ToPagedListAsync(pageNum, 5);
        
        return View(blogs);
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}