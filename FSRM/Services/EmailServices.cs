//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FSRM.Services
{
    public class EmailServices
    {
        public Boolean SendMail(string eBody, string eAdd)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                smtp.UseDefaultCredentials = false;
                message.From = new MailAddress("MPN@Domain.com");
                message.To.Add(new MailAddress(eAdd));
                message.Subject = "درخواست ثبت دسترسی FSRM";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = eBody;
                smtp.Port = 25;// 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //await smtp.SendMailAsync(message);
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}