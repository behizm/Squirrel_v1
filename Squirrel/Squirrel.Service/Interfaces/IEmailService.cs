using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Interfaces
{
    public interface IEmailService : IBaseService
    {
        Task SendAsync(EmailSendModel model);
    }
}
