using Enterprise.Services;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add Services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EnterpriseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultStrings")));
builder.Services.AddScoped<IHeadmasterRepository, HeadmasterRepository>();
builder.Services.AddScoped<IWorkmanRepository, WorkmanRepository>();
builder.Services.AddScoped<ISubdivisionMasterRepository, SubdivisionMasterRepository>();
builder.Services.AddScoped<IInspectorRepository, InspectorRepository>();
builder.Services.AddScoped<ISelectionSubdivision, SubdivisionRepository>();


var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.Run();