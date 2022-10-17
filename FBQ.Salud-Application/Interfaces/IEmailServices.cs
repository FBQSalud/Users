using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Application.Interfaces
{
    public interface IEmailServices
    {
        //Task Send(string email, string subject, string htmlContent);
        //Task Send(string fromEmail, string subject, string body);
        Task Send(string fromEmail, string toEmail, string subject, string body);
    }
}
