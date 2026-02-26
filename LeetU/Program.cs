using LeetU.Data.Context;
using LeetU.Data.Interfaces;
using LeetU.Data.Repositories;
using LeetU.Services;
using LeetU.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

//EVERYTHING STARTS HERE

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//old vers
//builder.Services.AddSqlite<StudentContext>("DataSource=file:Student.db;Mode=ReadWrite"); 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")??throw new Exception("Отсутсвует строка подключения");
builder.Services.AddSqlite<StudentContext>(connectionString!);
builder.Services.AddScoped<IStudentRepositoryCrud, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentCourseRepositoryCrud, StudentCourseRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
