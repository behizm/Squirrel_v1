using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Service;
using Squirrel.Utility.FarsiTools;
using Squirrel.Utility.Helpers;
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(FileSearchModel model, int searchPage = 1)
        {
            var orderingModel = new OrderingModel<Domain.Enititis.File>
            {
                IsAscending = true,
                KeySelector = x => x.Name,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            var itemsTask = FileService.SearchAsync(model, orderingModel);
            var countTask = FileService2.CountAsync(model);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "LoadList(#)"
                };
            }
            return PartialView("list", items);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FileAddModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            var filePath = System.Web.HttpContext.Current.Server.MapPath(model.FileAddress);
            if (!System.IO.File.Exists(filePath))
            {
                ViewBag.ErrorMessage = "فایل خود را مجددا آپلود کنید.";
                return PartialView(model);
            }
            var fileInfo = new FileInfo(filePath);
            var fileType = FileService.GetFileTypeByFileName(fileInfo.Name);
            if (fileType == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView(model);
            }

            var maxSize = FileService.GetFileTypeSize(fileType.Value);
            if (maxSize == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView(model);
            }
            if (fileInfo.Length > maxSize)
            {
                ViewBag.ErrorMessage =
                    string.Format("برای فایلهای از دسته {0} آپلود بیش از {1}  مگا بایت امکان پذیر نیست.",
                        fileType.Value.Description(), (maxSize.Value / (1024 * 1024)).FaDigit());
                return PartialView(model);
            }

            const string mainPath = "~/Content/Files";
            var physicalMainPath = System.Web.HttpContext.Current.Server.MapPath(mainPath);
            var newDirPath = FileService.MoveFromTempToMain(filePath, physicalMainPath);
            if (newDirPath == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView(model);
            }
            var newFilePath = string.Format("{0}/{1}", mainPath, newDirPath);
            var physicalNewFilePath = System.Web.HttpContext.Current.Server.MapPath(newFilePath);
            if (!System.IO.File.Exists(physicalNewFilePath))
            {
                ViewBag.ErrorMessage = "فایل خود را مجددا آپلود کنید.";
                return PartialView(model);
            }
            var newFileInfo = new FileInfo(physicalNewFilePath);

            model.FileAddress = newFilePath;
            model.Filename = newFileInfo.Name;
            model.Size = (int)newFileInfo.Length;
            model.Type = fileType;
            model.Username = User.Identity.Name;
            await FileService.AddAsync(model);
            if (FileService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "فایل با موفقیت افزوده شد.";
                ViewBag.JsMethod = "ReloadList()";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
            return PartialView(model);
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



        // Json

        public async Task<JsonResult> CategoryJson(string id)
        {
            var items =
                (await FileService.Categories(id,0,5)) ?? new List<string>();
            return Json(items.Select(x => new { Name = x }), JsonRequestBehavior.AllowGet);
        }

    }
}