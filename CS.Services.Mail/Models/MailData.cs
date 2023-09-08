using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Mail.Models
{
    public class MailData
    {
        public List<string> To { get; set; } = new List<string>();
        public List<string> Bcc { get; set; } = new List<string>();
        public List<string> Cc { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
}
