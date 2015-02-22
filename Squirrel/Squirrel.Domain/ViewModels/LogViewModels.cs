using System;

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

        public string Username { get; set; }
    }

    public class LogRemoveModel
    {
        public Guid Id { get; set; }
    }

    public class LogSearchModel
    {
        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ReferredHost { get; set; }

        public bool? IsAjax { get; set; }

        public string FullUrl { get; set; }

        public string ReferredUrl { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public string Port { get; set; }

        public bool? IsPostMethod { get; set; }

        public string ErrorMessage { get; set; }

        public string Username { get; set; }

        public Guid? UserId { get; set; }
    }
}
