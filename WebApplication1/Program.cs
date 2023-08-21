using Autofac.Core;
using Blog.Core.Extensions.Swagger;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new Blog.Core.Extensions.Swagger.AppSettings(builder.Configuration));

builder.Services.AddControllers();
//���Swagger
builder.Services.AddSwaggerSetup();
// ע����� Swaage��֤��¼
//builder.Services.AddSession();

var app = builder.Build();

// Swaage��֤��¼
//app.UseSession();
app.UseSwaggerAuthorized();
// Swagger��ҳ
app.UseSwaggerMildd(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("WebApplication1.index.html")); 
app.UseAuthorization();
//app.UseSwagger();
//app.UseSwaggerUI();
app.MapControllers();

app.Run();
