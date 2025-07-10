using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using OJTMAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//�]�w CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    options.AddPolicy("Allow5173",
        builder => builder.WithOrigins("http://localhost:5173/").WithMethods("GET").WithHeaders("*"));
});

string? connectionString = builder.Configuration.GetConnectionString("ClassDBConnection");
builder.Services.AddDbContext<ClassDbContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); //�ҥ��R�A��Ū��

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
