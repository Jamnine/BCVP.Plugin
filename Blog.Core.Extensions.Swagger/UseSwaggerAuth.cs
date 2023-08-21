#region 描述信息
/**
* 文件名：UseSwaggerAuth
* 类  名：UseSwaggerAuth
* 命名空间：Blog.Core.Extensions.Swagger
* 当前系统用户名：Nine
* 当前用户所在的域：NINE-DESIGNMINI
* 当前机器名称：NINE-DESIGNMINI
* 注册的组织名：
* 时间：2022/2/15 14:09:25
* CLR：4.0.30319.42000 
* GUID: b09dc497-f307-4798-90ca-c93631c95cd8 
* 当前系统时间：2022
* Copyright (c) 2022 何拾玖 Corporation. All rights reserved.
*┌───────────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   
*│　版权所有 何拾玖                                              
*└───────────────────────────────────────────────────────────────┘
* * Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* V0.01 2022/2/15 14:09:25 何拾玖 Nine 4.0.30319.42000 初版
**/
#endregion

using Microsoft.AspNetCore.Http;
using SqlSugar.Extensions;
using System.Net;
using System.Text;

namespace Blog.Core.Extensions.Swagger
{
    /// <summary>
    /// 实现类：UseSwaggerAuth
    /// </summary>	
    public class UseSwaggerAuth
    {
        #region 私有变量
        private readonly RequestDelegate requestDelegate;
        #endregion

        /// <summary>
        /// 构造函数，Autoface依赖注入
        /// </summary>
        public UseSwaggerAuth(RequestDelegate _requestDelegate)
        {
            this.requestDelegate = _requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettings.app(new string[] { "Middleware", "IsLocalRequestSwgger", "Enabled" }).ObjToBool())
            {
                if (IsLocalRequest(context))
                {
                    await requestDelegate.Invoke(context);
                }
                else
                {
                    //此段代码提取方法报错，应该是context转发被修改发起request导致,姑且重复
                    if (context.Request.Path.Value.ToLower().Contains("index.html"))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
            }
            else
            {
                if (AppSettings.app("AppSettings", "HideSwagger", "Enabled").ObjToBool())
                {
                    if (context.Request.Path.Value.ToLower().Contains(AppSettings.app("AppSettings", "HideSwagger", "RoutePrefix").ToString()))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
                else
                {
                    if (context.Request.Path.Value.ToLower().Contains("index.html"))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
            }

        }

        public bool IsAuthorized(HttpContext context)
        {
            // 使用session模式 可以使用其他的
            return context.Session.GetString("swagger-code") == "success";
        }

        /// <summary>
        /// 判断是不是本地访问
        /// 本地不用swagger拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsLocalRequest(HttpContext context)
        {
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            {
                return true;
            }
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }
            return false;
        }
    }
}
