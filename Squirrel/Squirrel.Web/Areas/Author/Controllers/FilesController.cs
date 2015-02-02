using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Service;
using Squirrel.Utility.EnumHelpers;
using Squirrel.Utility.FarsiTools;
using Squirrel.Web.Controllers;
using Squirrel.Web.Models;
using WebGrease.Css.Extensions;

namespace Squirrel.Web.Areas.Author.Controllers
{
    public class FilesController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            if (Request.Files["ufile"] == null)
            {
                return Json(new { result = false, message = "هیچ فایلی مشخص نشده است." }, JsonRequestBehavior.AllowGet);
            }

            var file = Request.Files["ufile"];
            var fileType = FileService.GetFileTypeByFileName(file.FileName);
            if (fileType == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.First() },
                    JsonRequestBehavior.AllowGet);
            }

            var maxSize = FileService.GetFileTypeSize(fileType.Value);
            if (maxSize == null)
            {
                return Json(
                    new { result = false, message = FileService.Result.Errors.First() },
                    JsonRequestBehavior.AllowGet);
            }
            if (file.ContentLength > maxSize)
            {
                return Json(
                    new
                    {
                        result = false,
                        message =
                            string.Format("برای فایلهای از دسته {0} آپلود بیش از {1}  مگا بایت امکان پذیر نیست.",
                                fileType.Value.Description(), (maxSize.Value / (1024 * 1024)).FaDigit())
                    },
                    JsonRequestBehavior.AllowGet);
            }

            const string tempPath = "~/Content/Files/temp";
            var physicalTempPath = System.Web.HttpContext.Current.Server.MapPath(tempPath);
            var folderName = FileService.CreateTempSubDirectory(physicalTempPath);
            if (folderName == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.First() },
                    JsonRequestBehavior.AllowGet);
            }
            var savePath = string.Format("{0}\\{1}\\{2}", physicalTempPath, folderName, file.FileName);
            try
            {
                file.SaveAs(savePath);
                return
                    Json(
                        new
                        {
                            result = true,
                            //message = file.ContentType,
                            message = "فایل با موفقیت آپلود شد.",
                            path = string.Format("{0}/{1}/{2}", tempPath, folderName, file.FileName)
                        }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "هنگام انجام درخواست خطا رخ داد." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ResetUpload()
        {
            return PartialView("Upload");
        }

        public JsonResult FileIsValid(string ext, long size)
        {
            var type = FileService.GetFileTypeByExtention(ext);
            if (type == null)
            {
                return Json(
                    new { result = false, message = FileService.Result.Errors.First() },
                    JsonRequestBehavior.AllowGet);
            }

            var maxSize = FileService.GetFileTypeSize(type.Value);
            if (maxSize == null)
            {
                return Json(
                    new { result = false, message = FileService.Result.Errors.First() },
                    JsonRequestBehavior.AllowGet);
            }
            if (size > maxSize)
            {
                return Json(
                    new
                    {
                        result = false,
                        message =
                            string.Format("برای فایلهای از دسته {0} آپلود بیش از {1}  مگا بایت امکان پذیر نیست.",
                                type.Value.Description(), (maxSize.Value / (1024 * 1024)).FaDigit())
                    },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(
                new { result = true, message = "" },
                JsonRequestBehavior.AllowGet);
        }

    }
}