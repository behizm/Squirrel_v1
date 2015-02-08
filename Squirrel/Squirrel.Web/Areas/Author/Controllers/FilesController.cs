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
using Squirrel.Domain.Resources;
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
                OrderByKeySelector = x => x.Name,
                Skip = (searchPage - 1) * 10,
                Take = 10,
            };

            model.Username = User.Identity.Name;
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

        public async Task<ActionResult> Details(Guid id)
        {
            var item = await FileService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            if (!item.IsPublic && User.Identity.UserId != item.UserId && !User.Identity.IsAdmin)
            {
                ViewBag.ErrorMessage = "شما دسترسی مشاهده این فایل را ندارید.";
                return PartialView("_Message");
            }

            ViewData.Model = item;
            return PartialView();
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var item = await FileService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            if (User.Identity.UserId != item.UserId && !User.Identity.IsAdmin)
            {
                ViewBag.ErrorMessage = "شما دسترسی ویرایش این فایل را ندارید.";
                return PartialView("_Message");
            }

            ViewData.Model = new FileEditModel
            {
                Category = item.Category,
                Id = item.Id,
                IsPublic = item.IsPublic,
                Name = item.Name,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FileEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            model.Username = User.Identity.Name;
            await FileService.EditAsync(model);
            if (FileService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "فایل با موفقیت ویرایش شد.";
                ViewBag.JsMethod = string.Format("CompleteEdit('{0}');", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var item = await FileService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            if (User.Identity.UserId != item.UserId && !User.Identity.IsAdmin)
            {
                ViewBag.ErrorMessage = "شما دسترسی حذف این فایل را ندارید.";
                return PartialView("_Message");
            }

            var fullPath = await EncryptAsync(Server.MapPath(item.Address));
            if (fullPath == null)
            {
                ViewBag.ErrorMessage = ServiceMessages.General_ErrorAccurred;
                return PartialView("_Message");
            }

            ViewData.Model = new FileDeleteModel
            {
                Id = item.Id,
                FullFilePath = fullPath,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(FileDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده قابل قبول نیست.";
                return PartialView(model);
            }

            var fullPath = await DecryptAsync(model.FullFilePath);
            if (fullPath == null)
            {
                ViewBag.ErrorMessage = ServiceMessages.General_ErrorAccurred;
                return PartialView("_Message");
            }

            model.Username = User.Identity.Name;
            model.FullFilePath = fullPath;
            await FileService.RemoveAsync(model);
            if (FileService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "فایل با موفقیت حذف شد.";
                ViewBag.JsMethod = "CompleteDelete();";
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        public async Task<ActionResult> Replace(Guid id)
        {
            var item = await FileService.FindByIdAsync(id);
            if (item == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            if (User.Identity.UserId != item.UserId && !User.Identity.IsAdmin)
            {
                ViewBag.ErrorMessage = "شما دسترسی ویرایش این فایل را ندارید.";
                return PartialView("_Message");
            }

            ViewData.Model = new FileReplaceModel
            {
                Id = item.Id,
            };
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Replace(FileReplaceModel model)
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

            var file = await FileService.FindByIdAsync(model.Id);
            if (file == null)
            {
                ViewBag.ErrorMessage = ServiceMessages.FileService_FileNotFount;
                return PartialView(model);
            }

            if (fileType != file.Type)
            {
                ViewBag.ErrorMessage = "نوع فایل آپلود شده با فایل اصلی همخوانی ندارد.";
                return PartialView(model);
            }

            model.OldFilePath = Server.MapPath(file.Address);
            model.NewFilePath = filePath;
            model.Username = User.Identity.Name;
            await FileService.ReplaceAsync(model);
            if (FileService.Result.Succeeded)
            {
                ViewBag.SuccessMessage = "فایل با موفقیت جایگذین شد.";
                ViewBag.JsMethod = string.Format("LoadDetails('{0}')", model.Id);
                return PartialView("_Message");
            }

            ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> RequestFile(FileRequestModel filterModel)
        {
            filterModel.Page = filterModel.Page < 1 ? 1 : filterModel.Page;
            var orderingModel = new OrderingModel<Domain.Enititis.File>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate.ToString(),
                Skip = (filterModel.Page - 1) * 12,
                Take = 12,
            };

            var searchModel = new FileSearchModel
            {
                Category = filterModel.Category,
                Name = filterModel.Name,
                Type = filterModel.Type,
                Username = User.Identity.Name,
            };
            var itemsTask = FileService.SearchAsync(searchModel, orderingModel);
            var countTask = FileService2.CountAsync(searchModel);

            var items = await itemsTask;
            if (items == null)
            {
                ViewBag.ErrorMessage = FileService.Result.Errors.FirstOrDefault();
                return PartialView("_Message");
            }

            var count = await countTask;
            if (!count.HasValue)
                return PartialView("Request", items);

            ViewBag.Paging = new PagingModel
            {
                CurrentPage = filterModel.Page,
                PageCount = count.Value % 12 == 0 ? count.Value / 12 : (count.Value / 12) + 1,
                PagingMethod = "LoadRequestFileList(#)"
            };
            ViewBag.RequestFileFilter = filterModel;
            return PartialView("Request", items);
        }



        // Json

        public async Task<JsonResult> CategoryJson(string id)
        {
            var items =
                (await FileService.CategoriesAsync(id, 0, 5)) ?? new List<string>();
            return Json(items.Select(x => new { Name = x }), JsonRequestBehavior.AllowGet);
        }

    }
}