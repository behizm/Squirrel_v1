using System;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class LogAddModel
    {
        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ReferredHost { get; set; }

        public bool IsAjax { get; set; }

        public string FullUrl { get; set; }

        public string ReferredUrl { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public string Port { get; set; }

        public bool IsPostMethod { get; set; }

        public string ErrorMessage { get; set; }

        public int LineNumber { get; set; }

        public string Username { get; set; }
    }

    public class LogRemoveModel
    {
        public Guid Id { get; set; }
    }

    public class LogSearchModel
    {
        [Display(Name = @"اِریا")]
        public string AreaProp { get; set; }

        [Display(Name = @"کنترلر")]
        public string ControllerProp { get; set; }

        [Display(Name = @"اکشن")]
        public string ActionProp { get; set; }

        public string ReferredHost { get; set; }

        [Display(Name = @"ایجکس باشد؟")]
        public bool? IsAjax { get; set; }

        public string FullUrl { get; set; }

        public string ReferredUrl { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public string Port { get; set; }

        public bool? IsPostMethod { get; set; }

        [Display(Name = @"ارور باشد؟")]
        public bool? IsErrorLog { get; set; }

        public string ErrorMessage { get; set; }

        public string Username { get; set; }

        public Guid? UserId { get; set; }

        [Display(Name = @"از تاریخ")]
        public DateTime? CreateDateFrom { get; set; }

        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}")]
        public string CreatePersianDateFrom { get; set; }

        [Display(Name = @"تا تاریخ")]
        public DateTime? CreateDateTo { get; set; }

        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}")]
        public string CreatePersianDateTo { get; set; }
    }

    public class LogCleanModel
    {
        public DateTime? CleanDateFrom { get; set; }

        [Display(Name = @"تاریخ شروع")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string CleanPersianDateFrom { get; set; }

        public DateTime? CleanDateTo { get; set; }

        [Display(Name = @"تاریخ پایان")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string CleanPersianDateTo { get; set; }
    }
}
