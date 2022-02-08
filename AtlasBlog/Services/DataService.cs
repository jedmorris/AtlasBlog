using AtlasBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasBlog.Services;

public class DataService
{
   // Calling a method or instruction that executes the migrations
   private readonly ApplicationDbContext _context;

   public DataService(ApplicationDbContext context)
   {
      _context = context;
   }

   public async Task SetupDbAsync()
   {
      //Run the migrations async
      await _context.Database.MigrateAsync();
   }

}