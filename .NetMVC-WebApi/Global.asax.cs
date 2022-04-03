using Commons.Quartz;
using MVC卓越项目.Commons.Fillter;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Service.SeckillService.Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC卓越项目
{
    public class WebApiApplication : HttpApplication
    {
        
        /// <summary>
       /// 跨域设置
        /// </summary>
        public void Application_BeginRequest()
        {
            //解决预检请求跨域问题
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod.Equals("OPTIONS"))
            {
                //尝试获取orgin头
                IEnumerable<string> strings = Request.Headers.GetValues("Origin");
                Response.Headers.Add("Access-Control-Allow-Origin", strings.FirstOrDefault());
                Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Content-Length,Authorization,Accept,X-Requested-With");
                Response.Headers.Add("Access-Control-Allow-Methods", "Get,Post,Put,Options,Delete");
                Response.Headers.Add("Access-Control-Expose-Headers", "Cache-Control,Content-Language,Content-Type,Expires,Last-Modified,Pragma");
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                Response.Headers.Add("Access-Control-Max-Age", "60");
                //对输出的内容进行缓冲
                Response.Flush();
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),        //小驼峰命名法,格式化日期时间
            //    Formatting = Formatting.Indented,
            //DateFormatString = "yyyy-MM-dd HH:mm:ss"
            //};
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            //{
            //    //驼峰命名
            //    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    //日期类型默认格式化处理
            //    settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    settings.Formatting = Formatting.Indented;
            //    //空值处理
            //    settings.NullValueHandling = NullValueHandling.Ignore;
            //    return settings;
            //});
            //载入log4j配置文件
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/log4net.config")));
            //初始化IOC容器
            Bootstrapper.Initialise();
            //调度扫描秒杀商品  每隔5秒执行一次
            QuartzManager.JoinToQuartz(typeof(SeckillQuartz));
        }

    }
}
