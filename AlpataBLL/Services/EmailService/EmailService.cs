using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.BaseResult.Abstracts;

namespace AlpataBLL.Services.EmailService
{
    public class EmailService : IEmailService
    {

        public async Task<IBaseResult> SendEmail(EmailModel EmailRequestModel)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("info@yorumingo.com", "ALPATA TEKNOLOJİ"); 
                mail.To.Add(EmailRequestModel.Receiver);
                mail.Subject = EmailRequestModel.Subject;
                mail.IsBodyHtml = true;
                mail.Body = EmailRequestModel.Content;

                using (var smtp = new SmtpClient
                {
                    Host = "mail.yorumingo.com",
                    Port = 587,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("info@yorumingo.com", "115792Aa.")
                })
                {
                    await smtp.SendMailAsync(mail);
                    return new SuccessResult() { Success = true };
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult() { Message = ex.Message, Success = false };
            }
        }
    }
}
