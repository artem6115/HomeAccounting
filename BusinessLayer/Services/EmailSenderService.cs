using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    public class EmailSenderService
    {
        private readonly ILogger<EmailSenderService> _logger;
        public EmailSenderService(ILogger<EmailSenderService> logger) => _logger=logger;
        public async Task<bool> Send(string UserAdress,string message,string subject="")
        {
            MailAddress fromadress = new MailAddress("artem.mikov.2003@mail.ru","HomeAccounting");
            MailAddress toadress = new MailAddress(UserAdress);

            MailMessage massage = new MailMessage(fromadress, toadress);

            massage.Subject = subject;
            massage.Body =message;

            SmtpClient smtpc = new SmtpClient();

            smtpc.Host = "smtp.mail.ru";
            smtpc.Port = 25;
            smtpc.EnableSsl = true;
            smtpc.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpc.UseDefaultCredentials = false;
            smtpc.Credentials = new NetworkCredential(fromadress.Address, "6fYFHt7Gb4RYVqepk1pi");
            try
            {
                smtpc.Send(massage);
                _logger.LogInformation("Message send to email adress");

            }
            catch(Exception e) {
            _logger.LogError($"EmailSernder Error : {e.Message}");
             return false;
            }
            return true;

        }
    }
}
