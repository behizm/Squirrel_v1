using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Service.Interfaces;
using Squirrel.Utility.Helpers;
using Squirrel.Utility.Xml;

namespace Squirrel.Service.Services
{
    internal class EmailService : BaseService, IEmailService
    {
        private readonly List<EmailSmtpModel> _smtpSettings;

        public EmailService()
        {
            _smtpSettings = XmlReadService<EmailSmtpModel>.Entity ?? new List<EmailSmtpModel>();
        }

        public async Task SendAsync(EmailSendModel model)
        {
            var smtpSetting = model.FromAddress.IsNothing()
                ? _smtpSettings.First(x => x.IsDefault)
                // ReSharper disable once SpecifyStringComparison
                : _smtpSettings.First(x => x.FromAddress.ToLower() == model.FromAddress.ToLower());
            
            if (smtpSetting == null)
            {
                Result = OperationResult.Failed(ServiceMessages.EmailService_NoSmtpSetting,
                    ServiceMessages.EmailService_SendingFailed);
                return;
            }

            model.FromAddress = smtpSetting.FromAddress.ToLower();

            var email = new MailMessage(model.FromAddress, model.ToAddress, model.Subject, model.Body);
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            try
            {
                var task = Task.Run(() =>
                {
                    using (var client = new SmtpClient())
                    {
                        client.UseDefaultCredentials = smtpSetting.UseDefaultCredentials;
                        client.Credentials = new NetworkCredential(smtpSetting.Username, smtpSetting.Password);
                        client.DeliveryMethod = smtpSetting.DeliveryMethod;
                        client.EnableSsl = smtpSetting.EnableSsl;
                        client.Host = smtpSetting.Host;
                        client.Port = smtpSetting.Port;
                        client.Timeout = smtpSetting.Timeout;
                        client.Send(email);
                    }
                });
                await task;
                Result = OperationResult.Success;
                return;
            }
            catch (Exception ex)
            {
                Result = OperationResult.Failed(ex.Message, ServiceMessages.EmailService_SendingFailed);
            }

            if (!Result.Succeeded)
            {
                var logService = new LogService();
                await logService.AddAsync(new LogAddModel
                {
                    ErrorMessage = Result.Errors.FirstOrDefault(),
                    Area = "Service Layer",
                    Controller = "EmailService",
                    Action = "SendAsync",
                    ReferredHost = "SqApp",
                    FullUrl = "-",
                    Ip = "-",
                    Port = "0",
                    UserAgent = "In App Log"
                });
            }
        }
    }
}
