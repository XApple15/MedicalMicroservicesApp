using AppointmentService.Data;
using AppointmentService.Factory;
using AppointmentService.Repository;
using AppointmentService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAppointmentFactory, AppointmentFactory>();
builder.Services.AddScoped<IAppointmentService, AppointmentService.Service.AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentDBConnString")));

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
