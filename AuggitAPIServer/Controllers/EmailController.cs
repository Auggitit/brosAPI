using System;
using System.Net;
using System.Net.Mail;

namespace AuggitAPIServer.EmailController
{
    public class EmailSenderService
    {
        public IConfiguration _configuration;
        Random random = new Random();
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string email,string body)
        {
            // var smtpHost = _configuration["SmtpHost"];
            // var smtpPort = int.Parse(_configuration["SmtpPort"]);
            // var smtpUsername = _configuration["SmtpUsername"];
            // var smtpPassword = _configuration["SmtpPassword"];
            // var FromEmail = _configuration["GeneralSetting:FromEmail"];
            // var FromName = _configuration["GeneralSetting:FromName"];
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "unitedhandstest@gmail.com";
            var smtpPassword = "akziufdbueyqmmtx";
            var FromEmail = "unitedhandstest@gmail.com";
            var FromName = "Auggit";
            try
            {
                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(FromEmail, FromName);
                        message.To.Add(new MailAddress(email));
                        message.Subject = "Otp Verification";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        await client.SendMailAsync(message);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


    }
    public class EmailModel
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
    }

}