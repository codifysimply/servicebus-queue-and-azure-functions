using CS.Services.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Mail.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mail);
    }
}
