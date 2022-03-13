using System.Web.Http;
using WebActivatorEx;
using MVC卓越项目;
using Swashbuckle.Application;
using System.Reflection;
using MVC卓越项目.Swagger;
using System.Linq;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace MVC卓越项目
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "MVC卓越项目");
                        string xmlFile = string.Format("{0}/bin/ShoppingApis.xml", System.AppDomain.CurrentDomain.BaseDirectory);
                        c.IncludeXmlComments(xmlFile);
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                        c.CustomProvider((defaultProvider) => new SwaggerHelpTools(defaultProvider, xmlFile));
                        c.OperationFilter<HttpAuthHeaderFilter>();

                    })
                .EnableSwaggerUi(c =>
                {
                    c.InjectJavaScript(Assembly.GetExecutingAssembly(), "MVC卓越项目.Swagger.swaggerHelper.js");
                });
        }
        /// <summary>
        /// 全局token配置
        /// </summary>
        public class HttpAuthHeaderFilter : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter { name = "Authorization", @in = "header", description = "授权", required = false, type = "header" });

            }
        }
    }
}
