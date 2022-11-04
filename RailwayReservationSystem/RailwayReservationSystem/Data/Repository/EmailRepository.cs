using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace RailwayReservationSystem.Data.Repository
{
    public class EmailRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailRepository> _log;

        public EmailRepository(IConfiguration config, ILogger<EmailRepository> log)
        {
            _config = config;
            _log = log;
        }


        public async Task<bool> SendEmail(MailDetails mail, int pnr = -1, int userId = -1, bool cancelled = false)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_config["MailConfig:Sender"]));
                message.To.Add(MailboxAddress.Parse(mail.To));
                message.Subject = "PRTC - " + mail.Subject;

                BodyBuilder body = new BodyBuilder();
                if (cancelled)
                    body.HtmlBody = "<center><h1> Railway Ticket Cancelled for- </h1><h2> PNR Number: <b><u>" + pnr + "<u><b></ h2 ></center>";
                else if (userId != -1)
                    body.HtmlBody = "<center><h1> Registration Successfull </h1><h2> Your UserId: <b><u>" + userId + "<u><b></h2></center>";
                else if (pnr != -1)
                    body.HtmlBody = "<center><h1> Railway Ticket </h1><h2> PNR Number: <b><u>" + pnr + "<u><b></ h2 ></center>";
                else
                    return false;

                message.Body = body.ToMessageBody();


                SmtpClient client = new SmtpClient();
                client.Connect(_config["MailConfig:Host"], Convert.ToInt32(_config["MailConfig:Port"]), Convert.ToBoolean(_config["MailConfig:EnableSSL"]));
                client.Authenticate(_config["MailConfig:UserName"], _config["MailConfig:Password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch(Exception error)
            {
                _log.LogError(error.Message);
                return false;
            }
            return true;
            
        }
    }
}
