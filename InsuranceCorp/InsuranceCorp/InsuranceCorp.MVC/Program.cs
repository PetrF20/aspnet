using InsuranceCorp.Data;
using InsuranceCorp.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//ApplicationDbContext - lok�ln� DB, kde se ukl�daj� ��ty
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//identity u�ivatele
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>(); //�daje se maj� ukl�dat do DB, kter� je vytvo�ena dle connectionstring
builder.Services.AddControllersWithViews();


//sezn�men�, �e existuje DBkontext
builder.Services.AddDbContext<InsCorpDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //načítá styly , javascripty, obrázky a další statické soubory

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default", //pojmenov�n� cesty, m��u se na n� d�le odkazovat
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
