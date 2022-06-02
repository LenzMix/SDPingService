using SDPingService.Classes;
using System;
using System.Net;
using System.Net.Mail;

namespace SDPingService.Modules
{
    internal class MailSender
    {

        internal bool SendMessage(LogClass newLog)
        {
            try
            {
                MailAddress from = new MailAddress(Program._config.configuration.SMTPfromMail, "SD Ping Service");
                MailAddress to = new MailAddress(Program._config.configuration.SMTPtoMail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Отчёт по отклику сайтов";
                m.Body = newLog.HTMLLog();
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(Program._config.configuration.SMTPserver, Program._config.configuration.SMTPport);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Program._config.configuration.SMTPlogin, Program._config.configuration.SMTPpassword);
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
