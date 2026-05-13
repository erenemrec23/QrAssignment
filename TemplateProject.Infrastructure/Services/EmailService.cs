////using MailKit.Net.Smtp;
////using MimeKit;
//using System.Net.Mail;
//using TemplateProject.Application.Abstractions;
//// ... using'ler

//public sealed class EmailService : IEmailService
//{
//    //private readonly EmailOptions _options; // appsettings'ten gelen ayarlar

//    public async Task SendExceptionEmailAsync(Exception exception, string requestPath, CancellationToken cancellationToken = default)
//    {
//        //var message = new MimeMessage();
//        //message.From.Add(new MailboxAddress("System Error", _options.FromEmail));
//        //message.To.Add(new MailboxAddress("Admin", _options.AdminEmail));
//        //message.Subject = $"CRITICAL ERROR: {requestPath}";

//        //message.Body = new TextPart("html")
//        //{
//        //    Text = $@"<h3>Sistemde Beklenmeyen Bir Hata Oluştu</h3>
//        //             <p><b>Yol:</b> {requestPath}</p>
//        //             <p><b>Hata:</b> {exception.Message}</p>
//        //             <p><b>Detay:</b> {exception.StackTrace}</p>"
//        //};

//        //using var client = new SmtpClient();
//        //await client.ConnectAsync(_options.Host, _options.Port, true, cancellationToken);
//        //await client.AuthenticateAsync(_options.UserName, _options.Password, cancellationToken);
//        //await client.SendAsync(message, cancellationToken);
//        //await client.DisconnectAsync(true, cancellationToken);
//    }
//}