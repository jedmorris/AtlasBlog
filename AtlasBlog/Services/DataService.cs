using AtlasBlog.Data;
using AtlasBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AtlasBlog.Services;

public class DataService
{
   // Calling a method or instruction that executes the migrations
   private readonly ApplicationDbContext _context;
   private readonly RoleManager<IdentityRole> _roleManager;
   private readonly UserManager<BlogUser> _userManager;
   private readonly IConfiguration _configuration;

   public DataService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager, IConfiguration configuration)
   {
      _context = context;
      _roleManager = roleManager;
      _userManager = userManager;
      _configuration = configuration;
   }

   public async Task SetupDbAsync()
   {
      //Run the migrations async
      await _context.Database.MigrateAsync();
      
      //Add Role to the system
      await SeedRolesAsync();
     
      //Assign users to roles
      await SeedUsersAsync();


   }

   private async Task SeedRolesAsync()
   {
      if (!_roleManager.Roles.Any())
      {
         await _roleManager.CreateAsync(new IdentityRole("Administrator"));
         await _roleManager.CreateAsync(new IdentityRole("Moderator"));
      }
   }

   private async Task SeedUsersAsync()
   {
      try
      {
         BlogUser user = new()
         {
            UserName = "sloth@admin.com",
            Email = "sloth@admin.com",
            FirstName = "Jed",
            LastName = "Morris",
            DisplayName = "Sloth Admin",
            PhoneNumber = "6199779546",
            EmailConfirmed = true
         };

         var newUser = await _userManager.FindByEmailAsync(user.Email);
         if (newUser is null)
         {
            var adminPassword = _configuration["DataService:AdminPassword"];
            await _userManager.CreateAsync(user, adminPassword);
            await _userManager.AddToRoleAsync(user, "Administrator");
         }
         
         user = new()
         {
            UserName = "sloth@mod.com",
            Email = "sloth@mod.com",
            FirstName = "Jed",
            LastName = "Morris",
            DisplayName = "Sloth Moderator",
            PhoneNumber = "6199779546",
            EmailConfirmed = true
         };
         
         newUser = await _userManager.FindByEmailAsync(user.Email);
         if (newUser is null)
         {
            var modPassword = _configuration["DataService:ModPassword"];
            await _userManager.CreateAsync(user, _configuration["DataService:ModPassword"]);
            await _userManager.AddToRoleAsync(user, "Moderator");
         }
      }
      catch (Exception e)
      {
         Console.WriteLine(e);
         throw;
      }
      
      


   }
}