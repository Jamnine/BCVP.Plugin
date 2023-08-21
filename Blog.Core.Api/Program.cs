using Autofac.Core;
using Blog.Core.Extensions.Swagger;
using System.Configuration;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new Blog.Core.Extensions.Swagger.AppSettings(builder.Configuration));

builder.Services.AddControllers();
//添加Swagger
builder.Services.AddSwaggerSetup();
// 注册服务 Swaage验证登录
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
// Swaage验证登录
app.UseSession();
app.UseSwaggerAuthorized();
// Swagger首页
app.UseSwaggerMildd(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("Blog.Core.Api.index.html"));
app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    //endpoints.MapHub<ChatHub>("/api2/chatHub");
});
app.Run();
