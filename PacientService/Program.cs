using Microsoft.EntityFrameworkCore;
using PacientService.Data;
using PacientService.Facade;
using PacientService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPacientRepository, PacientRepository>();
builder.Services.AddScoped<PacientFacadeService>();

builder.Services.AddDbContext<PacientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PacientDBConnString")));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
