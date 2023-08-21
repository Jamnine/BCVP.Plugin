using Autofac.Core;
using Blog.Core.Extensions.Swagger;
using System.Configuration;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new Blog.Core.Extensions.Swagger.AppSettings(builder.Configuration));

builder.Services.AddControllers();
//���Swagger
builder.Services.AddSwaggerSetup();
// ע����� Swaage��֤��¼
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
// Swaage��֤��¼
app.UseSession();
app.UseSwaggerAuthorized();
// Swagger��ҳ
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
