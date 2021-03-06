﻿using System;
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
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = FileService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            var count = await countTask;
            if (count.HasValue)
            {
                ViewBag.Paging = new PagingModel
                {
                    CurrentPage = searchPage,
                    PageCount = count.Value % 10 == 0 ? count.Value / 10 : (count.Value / 10) + 1,
                    PagingMethod = "searchingFile(#)"
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
        public async Task<JsonResult> Add(FileAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData },
                 JsonRequestBehavior.AllowGet);
            }

            var filePath = System.Web.HttpContext.Current.Server.MapPath(model.FileAddress);
            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { result = false, message = "فایل خود را مجددا آپلود کنید." },
                 JsonRequestBehavior.AllowGet);
            }
            var fileInfo = new FileInfo(filePath);
            var fileType = FileService.GetFileTypeByFileName(fileInfo.Name);
            if (fileType == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
            }

            var maxSize = FileService.GetFileTypeSize(fileType.Value);
            if (maxSize == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
            }
            if (fileInfo.Length > maxSize)
            {
                var errorMessage =
                    string.Format("برای فایلهای از دسته {0} آپلود بیش از {1}  مگا بایت امکان پذیر نیست.",
                        fileType.Value.Description(), (maxSize.Value / (1024 * 1024)).FaDigit());

                return Json(new { result = false, message = errorMessage },
                 JsonRequestBehavior.AllowGet);
            }

            const string mainPath = "~/Content/Files";
            var physicalMainPath = System.Web.HttpContext.Current.Server.MapPath(mainPath);
            var newDirPath = FileService.MoveFromTempToMain(filePath, physicalMainPath);
            if (newDirPath == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
            }
            var newFilePath = string.Format("{0}/{1}", mainPath, newDirPath);
            var physicalNewFilePath = System.Web.HttpContext.Current.Server.MapPath(newFilePath);
            if (!System.IO.File.Exists(physicalNewFilePath))
            {
                return Json(new { result = false, message = "فایل خود را مجددا آپلود کنید." },
                 JsonRequestBehavior.AllowGet);
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
                return Json(new { result = true, message = "فایل با موفقیت افزوده شد." },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault() },
                 JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var item = await FileService.FindByIdAsync(id);
            if (item == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = FileService.Result.Errors.FirstOrDefault(),
                };
                return PartialView("_HandledError");
            }

            if (!item.IsPublic && User.Identity.UserId != item.UserId && !User.Identity.IsAdmin)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = "شما دسترسی مشاهده این فایل را ندارید.",
                };
                return PartialView("_HandledError");
            }

            var fullPath = await EncryptAsync(Server.MapPath(item.Address));
            if (fullPath == null)
            {
                ViewData.Model = new ErrorViewModel
                {
                    Topic = "خطا",
                    Message = ServiceMessages.General_ErrorAccurred,
                };
                return PartialView("_HandledError");
            }

            ViewBag.FullPathFileAddress = fullPath;
            ViewData.Model = item;
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(FileEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            await FileService.EditAsync(model);
            if (FileService.Result.Succeeded)
            {
                return Json(new { result = true, message = "فایل با موفقیت ویرایش شد.", id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault(), id = model.Id },
             JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Replace(FileReplaceModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            var filePath = System.Web.HttpContext.Current.Server.MapPath(model.FileAddress);
            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { result = false, message = "فایل خود را مجددا آپلود کنید.", id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }
            var fileInfo = new FileInfo(filePath);
            var fileType = FileService.GetFileTypeByFileName(fileInfo.Name);
            if (fileType == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault(), id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }

            var maxSize = FileService.GetFileTypeSize(fileType.Value);
            if (maxSize == null)
            {
                return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault(), id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }
            if (fileInfo.Length > maxSize)
            {
                var error = string.Format("برای فایلهای از دسته {0} آپلود بیش از {1}  مگا بایت امکان پذیر نیست.",
                    fileType.Value.Description(), (maxSize.Value / (1024 * 1024)).FaDigit());

                return Json(new { result = false, message = error, id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }

            var file = await FileService.FindByIdAsync(model.Id);
            if (file == null)
            {

                return Json(new { result = false, message = ServiceMessages.FileService_FileNotFount, id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }

            if (fileType != file.Type)
            {

                return Json(new { result = false, message = "نوع فایل آپلود شده با فایل اصلی همخوانی ندارد.", id = model.Id },
                  JsonRequestBehavior.AllowGet);
            }

            model.OldFilePath = Server.MapPath(file.Address);
            model.NewFilePath = filePath;
            model.Username = User.Identity.Name;
            await FileService.ReplaceAsync(model);
            if (FileService.Result.Succeeded)
            {
                return Json(new { result = true, message = "فایل با موفقیت جایگذین شد.", id = model.Id },
                     JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault(), id = model.Id },
              JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(FileDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = ServiceMessages.General_LackOfInputData, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            var fullPath = await DecryptAsync(model.FullFilePath);
            if (fullPath == null)
            {
                return Json(new { result = false, message = ServiceMessages.General_ErrorAccurred, id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            model.Username = User.Identity.Name;
            model.FullFilePath = fullPath;
            await FileService.RemoveAsync(model);
            if (FileService.Result.Succeeded)
            {
                return Json(new { result = true, message = "فایل با موفقیت حذف شد.", id = model.Id },
                 JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = FileService.Result.Errors.FirstOrDefault(), id = model.Id },
                 JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<ActionResult> Popup(FileRequestModel filterModel)
        {
            const int countInPage = 8;
            filterModel.Page = filterModel.Page < 1 ? 1 : filterModel.Page;
            var orderingModel = new OrderingModel<Domain.Enititis.File>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.CreateDate.ToString(),
                Skip = (filterModel.Page - 1) * countInPage,
                Take = countInPage,
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
                return PartialView(items);

            ViewBag.Paging = new PagingModel
            {
                CurrentPage = filterModel.Page,
                PageCount = count.Value % countInPage == 0 ? count.Value / countInPage : (count.Value / countInPage) + 1,
                PagingMethod = "LoadPopupFileList(#)"
            };
            ViewBag.RequestFileFilter = filterModel;
            return PartialView("ChoosePopup", items);
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