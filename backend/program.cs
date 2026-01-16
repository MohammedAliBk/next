using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// قراءة الـ connection string من env variable
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// اختبار اتصال
app.MapGet("/", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? "Connected to SQL Server!" : "Failed to connect.";
});

app.Run();

// EF Core DbContext
class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Item> Items => Set<Item>();
}

class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

