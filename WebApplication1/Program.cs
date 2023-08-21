using Autofac.Core;
using Blog.Core.Extensions.Swagger;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new Blog.Core.Extensions.Swagger.AppSettings(builder.Configuration));

builder.Services.AddControllers();
//添加Swagger
builder.Services.AddSwaggerSetup();
// 注册服务 Swaage验证登录
//builder.Services.AddSession();

var app = builder.Build();

// Swaage验证登录
//app.UseSession();
app.UseSwaggerAuthorized();
// Swagger首页
app.UseSwaggerMildd(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("WebApplication1.index.html")); 
app.UseAuthorization();
//app.UseSwagger();
//app.UseSwaggerUI();
app.MapControllers();

app.Run();
