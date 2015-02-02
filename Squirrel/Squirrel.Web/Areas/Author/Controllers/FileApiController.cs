using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Squirrel.Service;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class FileApiController : ApiController
    {
        private IFileService _fileService;
        protected IFileService FileService
        {
            get { return _fileService ?? (_fileService = ServiceIOC.Get<IFileService>()); }
        }


        public IHttpActionResult Get(int id)
        {
            return Ok("HI hi : " + id);
        }

        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Json(new { result = false, message = "هیچ فایلی مشخص نشده است." });
            }

            const string tempPath = "~/Content/Files/temp";
            var physicalTempPath = HttpContext.Current.Server.MapPath(tempPath);
            var folderName = FileService.CreateTempSubDirectory(physicalTempPath);
            if (folderName == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.First() });
            }
            var saveFolder = string.Format("{0}\\{1}\\", physicalTempPath, folderName);
            var streamProvider = new MultipartFormDataStreamProvider(saveFolder);
            try
            {
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        return Json(new { result = false, message = "هنگام انجام درخواست خطا رخ داد." });

                    if (t.IsCanceled)
                        return Json(new { result = false, message = "انجام درخواست کنسل شد." });

                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        return string.Format("{0}/{1}/{2}", tempPath, folderName, info.Name);
                    });

                    return Json(new { result = true, message = fileInfo.FirstOrDefault() });
                });
                return await task;
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "هنگام انجام درخواست خطا رخ داد." });
            }
        }

        //public Task<IEnumerable<string>> Post()
        //{
        //    //throw new Exception("Custom error thrown for script error handling test!");  

        //    if (Request.Content.IsMimeMultipartContent())
        //    {
        //        //Simulate large file upload  
        //        //System.Threading.Thread.Sleep(5000);


        //        string fullPath = HttpContext.Current.Server.MapPath("~/Content/Files/temp");
        //        CustomMultipartFormDataStreamProvider streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);
        //        try
        //        {
        //            var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
        //            {
        //                if (t.IsFaulted || t.IsCanceled)
        //                    throw new HttpResponseException(HttpStatusCode.InternalServerError);

        //                var fileInfo = streamProvider.FileData.Select(i =>
        //                {
        //                    var info = new FileInfo(i.LocalFileName);
        //                    return "File saved as " + info.FullName + " (" + info.Length + ")";
        //                });
        //                return fileInfo;

        //            });
        //            return task;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message));
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
        //    }
        //}
    }
}
