using System;
using System.Net;
using System.Net.Mail;

namespace MapBul.SharedClasses
{
    public static class MailProvider
    {
        public static void SendMailWithCredintails(string password, string firstName, string middleName, string email, string lang="ru")
        {
            string subject;
            string body;
            if (lang == "ru")
            {
                subject = "Регистрация";
                body = "Добрый день, " + firstName + " " + middleName + "!<br/>" +
                       "Для Вас создана учетная запись <br/>" +
                       "Логин: " + email + "<br/>" +
                       "Пароль: " + password;
            }
            else
            {
                subject = "Registration";
                body = "Hello " + firstName + " " + middleName + "!<br/>" +
                       "Your credentials <br/>" +
                       "Login: " + email + "<br/>" +
                       "Password: " + password;
            }

            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body))
            {
                SendMain(subject, body, email);
            }


            //try
            //{
            //    MailMessage message = new MailMessage {From = new MailAddress("MapBul<mapbulapp@yandex.ru>")};

            //    message.To.Add(new MailAddress(email));
            //    message.Subject = "Регистрация на сервисе MapBul";
            //    message.Body = "Добрый день, " + firstName + " " + middleName + "!<br/>" +
            //                   "Для Вас создана учетная запись в MapBul <br/>" +
            //                   "Логин: " + email + "<br/>" +
            //                   "Пароль: " + password;
            //    message.IsBodyHtml = true;
            //    SmtpClient client = new SmtpClient("smtp.yandex.ru", 25)
            //    {
            //        EnableSsl = true,
            //        Credentials = new NetworkCredential("mapbulapp@yandex.ru", "mapbulbul")
            //    };
            //    client.EnableSsl = true;
            //    client.Send(message);
            //}
            //catch (Exception e)
            //{
            //    // ignored
            //}
        }

        public static void SendMailRecoveryPassword(string password, string firstName, string middleName, string email, string lang="ru")
        {
            string subject;
            string body;
            if (lang == "ru")
            {
                subject = "Восстановление пароля";
                body = "Добрый день, " + firstName + " " + middleName + "!<br/>" +
                       "Для Вас создан новый пароль <br/>" +
                       "Логин: " + email + "<br/>" +
                       "Пароль: " + password;
            }
            else
            {
                subject = "Password recovery";
                body = "Hello " + firstName + " " + middleName + "!<br/>" +
                       "New credentials: <br/>" +
                       "Login: " + email + "<br/>" +
                       "Password: " + password;
            }

            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body))
            {
                SendMain(subject, body, email);
            }
            //try
            //{
            //    MailMessage message = new MailMessage { From = new MailAddress("MapBul<mapbulapp@yandex.ru>") };

            //    message.To.Add(new MailAddress(email));
            //    message.Subject = "Восстановление пароля от сервиса MapBul";
            //    message.Body = "Добрый день, " + firstName + " " + middleName + "!<br/>" +
            //                   "Для Вас создан новый пароль в MapBul <br/>" +
            //                   "Логин: " + email + "<br/>" +
            //                   "Пароль: " + password;
            //    message.IsBodyHtml = true;
            //    SmtpClient client = new SmtpClient("smtp.yandex.ru", 25)
            //    {
            //        EnableSsl = true,
            //        Credentials = new NetworkCredential("mapbulapp@yandex.ru", "mapbulbul")
            //    };
            //    client.EnableSsl = true;
            //    client.Send(message);
            //}
            //catch (Exception e)
            //{
            //    // ignored
            //}
        }

        private static void SendMain(string subject, string body, string email)
        {
            try
            {
                MailMessage message = new MailMessage { From = new MailAddress("MapBul<mapbulapp@yandex.ru>") };

                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.yandex.ru", 25)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("mapbulapp@yandex.ru", "mapbulbul")
                };
                client.EnableSsl = true;
                client.Send(message);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
