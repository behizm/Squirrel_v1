using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Squirrel.Web.Models;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class FileApiController : ApiController
    {
        public IHttpActionResult Get(int id)
        {
            return Ok("HI hi : " + id);
        }

        public Task<IEnumerable<string>> Post()
        {
            //throw new Exception("Custom error thrown for script error handling test!");  

            if (Request.Content.IsMimeMultipartContent())
            {
                //Simulate large file upload  
                System.Threading.Thread.Sleep(5000);


                string fullPath = HttpContext.Current.Server.MapPath("~/Uploads");
                CustomMultipartFormDataStreamProvider streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        return "File saved as " + info.FullName + " (" + info.Length + ")";
                    });
                    return fileInfo;

                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
        }  
    }
}
