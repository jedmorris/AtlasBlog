#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AtlasBlog.Data;
using AtlasBlog.Models;
using AtlasBlog.Services;
using AtlasBlog.Services.Interfaces;

namespace AtlasBlog.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly SlugService _slugService;

        public BlogPostController(ApplicationDbContext context, IImageService imageService, SlugService slugService)
        {
            _context = context;
            _imageService = imageService;
            _slugService = slugService;
        }

        // GET: BlogPost
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogPosts.Include(b => b.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Blog)
                .Include(c => c.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPost/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "BlogName");
            return View();
        }

        // POST: BlogPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,BlogPostState,Body")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                var slug = _slugService.UrlFriendly(blogPost.Title, 100);
                //Ensure slug is unique before storing in Db
                var isUnique = !_context.BlogPosts.Any(b => b.Slug == slug);
                if (isUnique)
                {
                    blogPost.Slug = slug;
                }
                else
                {
                    //throw slug error
                    ModelState.AddModelError("Title", "Duplicate title. Please select a unique title.");
                    ModelState.AddModelError("", "Error on page");
                    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "BlogName", blogPost.BlogId);
                    return View(blogPost);
                }
                
                blogPost.Created = DateTime.UtcNow;
                
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "BlogName", blogPost.BlogId);
            return View(blogPost);
        }

        // GET: BlogPost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "BlogName", blogPost.BlogId);
            return View(blogPost);
        }

        // POST: BlogPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Slug,Title,Slug,IsDeleted,Abstract,BlogPostState,Body,Created")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogPost.Created = DateTime.SpecifyKind(blogPost.Created, DateTimeKind.Utc);
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "BlogName", blogPost.BlogId);
            return View(blogPost);
        }

        // GET: BlogPost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
