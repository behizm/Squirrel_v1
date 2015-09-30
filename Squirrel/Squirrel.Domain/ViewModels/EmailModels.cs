using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Xml.Serialization;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class EmailSmtpModel
    {
        [XmlElement("FromAddress")]
        public string FromAddress { get; set; }

        [XmlElement("Username")]
        public string Username { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }

        [XmlElement("UseDefaultCredentials")]
        public bool UseDefaultCredentials { get; set; }

        [XmlElement("DeliveryMethod")]
        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        [XmlElement("DeliveryFormat")]
        public SmtpDeliveryFormat DeliveryFormat { get; set; }

        [XmlElement("EnableSsl")]
        public bool EnableSsl { get; set; }

        [XmlElement("Host")]
        public string Host { get; set; }

        [XmlElement("Port")]
        public int Port { get; set; }

        [XmlElement("Timeout")]
        public int Timeout { get; set; }

        [XmlElement("IsDefault")]
        public bool IsDefault { get; set; }
    }

    public class EmailSendModel
    {
        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }

    public class ContactUsViewModel
    {
        [Display(Name = @"آدرس ایمیل")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"نام")]
        public string Name { get; set; }

        [Display(Name = @"عنوان پیام")]
        public string Subject { get; set; }

        [Display(Name = @"پیام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Body { get; set; }

        [Display(Name = @"عبارت امنیتی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string CaptchaText { get; set; }
    }

    public class AdminEmailViewModel
    {
        [Display(Name = @"آدرس ایمیل")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"عنوان پیام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Subject { get; set; }

        [Display(Name = @"متن پیام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Body { get; set; }
    }
}
