using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace MVC卓越项目.Controller.FileController
{
    /// <summary>
    /// 文件上传模块
    /// </summary>
    [RoutePrefix("api")]
    public class FileController : ApiController
    {
        private string UpPath = ConfigurationManager.AppSettings["UploadFilePath"];
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<HttpResponseMessage> UploadPhoto(string type = "")
        {
            try
            {
                string guid = Guid.NewGuid().ToString(); ;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new ApiException(500, "不支持的媒体类型");
                }

                string root = UpPath + type;
                string uploadFolderPath = HostingEnvironment.MapPath(root);

                string path = uploadFolderPath + guid;

                //如果路径不存在，创建路径
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                List<string> files = new List<string>();
                var provider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath, guid);


                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.FileData)
                {
                    //接收文件                 
                    files.Add(root.Replace("~/staticResource","") + "/" + Path.GetFileName(file.LocalFileName));
                }

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", files.FirstOrDefault());
                dic.Add("Code", 200);
                //结果转为JSON消息格式
                return new HttpResponseMessage()
                {

                    Content = new ObjectContent<Dictionary<string, object>>(
                          dic,
                         new JsonMediaTypeFormatter(),
                         "application/json"
                       )
                };
            }
            catch (Exception ex)
            {
                throw new ApiException(500, "上传图片错误");

            }
        }
    }
}