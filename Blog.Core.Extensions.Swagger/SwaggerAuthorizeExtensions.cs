#region 描述信息
/**
* 文件名：SwaggerAuthorizeExtensions
* 类  名：SwaggerAuthorizeExtensions
* 命名空间：Blog.Core.Extensions.Swagger
* 当前系统用户名：Nine
* 当前用户所在的域：NINE-DESIGNMINI
* 当前机器名称：NINE-DESIGNMINI
* 注册的组织名：
* 时间：2022/2/15 14:11:02
* CLR：4.0.30319.42000 
* GUID: a14e8436-b01d-4f7e-978f-9c8fb7543597 
* 当前系统时间：2022
* Copyright (c) 2022 何拾玖 Corporation. All rights reserved.
*┌───────────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   
*│　版权所有 何拾玖                                              
*└───────────────────────────────────────────────────────────────┘
* * Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* V0.01 2022/2/15 14:11:02 何拾玖 Nine 4.0.30319.42000 初版
**/
#endregion

using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Extensions.Swagger
{
    // 定义扩展
    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseSwaggerAuth>();
        }
    }
}
