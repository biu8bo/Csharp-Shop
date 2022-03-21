using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;

namespace MVC卓越项目.Controller.FileController
{
    public class WithExtensionMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private string UpPath = ConfigurationManager.AppSettings["UploadFilePath"];
        public string guid { get; set; }

        public WithExtensionMultipartFormDataStreamProvider(string rootPath, string guidStr) : base(rootPath)
        {
            guid = guidStr;
        }
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            string fileName = string.IsNullOrEmpty(guid) ? headers.ContentDisposition.Name.Replace("\"", "") : guid;
            string extension = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? Path.GetExtension(GetValidFileName(headers.ContentDisposition.FileName)) : "";
            string fileName2 = fileName;
            int i = 2;
            while (File.Exists(HostingEnvironment.MapPath(UpPath + fileName2 + extension)))
            {
                fileName2 = fileName + "_$" + i;
                i++;
            }
            return fileName2 + extension;
        }
        private string GetValidFileName(string filePath)
        {
            char[] invalids = System.IO.Path.GetInvalidFileNameChars();
            return String.Join("_", filePath.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}