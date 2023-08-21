using BCVP.Plugin.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar.Extensions;

namespace Blog.Core.Api.Controllers
{
    /// <summary>
    /// GlobaDomainController
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GlobaDomainController : BaseApiController
    {
        /// <summary>
        /// swagger登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<dynamic> SwaggerLogin([FromBody] SwaggerLoginRequest loginRequest)
        {
            try
            {

                //var st1 = testViewServices.QuerySql("SELECT* FROM \"TestView\"");
                //var st1q = testViewServices.QuerySql("SELECT* FROM TestView");
                //var st = testViewServices.Query();
                string url;
                if (AppSettings.app("AppSettings", "HideSwagger", "Enabled").ObjToBool())
                {
                    url = AppSettings.app("AppSettings", "HideSwagger", "RoutePrefix").ToString() + "/index.html";
                }
                else
                {
                    url = "/index.html";
                }
                HttpContext.Session.SetString("swagger-code", "success");
                return new { result = true, token = "Bearer ", location = url };

            }
            catch (Exception ex)
            {
                return new { result = false };
            }
        }
    }
    public class SwaggerLoginRequest
    {
        public string name { get; set; }
        public string pwd { get; set; }
    }
}
