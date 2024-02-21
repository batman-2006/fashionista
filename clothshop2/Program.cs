using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using clothshop2.Data;
using clothshop2.Models;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("clothshop2ContextConnection") ?? throw new InvalidOperationException("Connection string 'clothshop2ContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<clothshop2.Areas.Identity.Data.clothshop2User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<clothshop2.Models.appDbContext>();
// Add services to the container.
builder.Services.AddSession(
    options => { options.IdleTimeout = TimeSpan.FromMinutes(10);
    
    }
 );

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{name?}");
app.MapRazorPages();


app.Run();
