using System.Net;
using System.Net.Mail;

namespace MapBul.SharedClasses
{
    public static class  MailProvider
    {
        public static void SendMailWithCredintails(string password, string firstName, string middleName, string email)
        {
            try
            {
                MailMessage message = new MailMessage {From = new MailAddress("MapBul<andrei_kryuchkov@bk.ru>")};

                message.To.Add(new MailAddress(email));
                message.Subject = "Регистрация на сервисе MapBul";
                message.Body = "Добрый день, " + firstName + " " + middleName + "!<br/>" +
                               "Для Вас создана учетная запись в MapBul <br/>" +
                               "Логин: " + email + "<br/>" +
                               "Пароль: " + password;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.mail.ru", 25)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("andrei_kryuchkov@bk.ru", "dblyjtgkr823")
                };
                client.Send(message);
            }
            catch
            {
                // ignored
            }
        }


    }
}
