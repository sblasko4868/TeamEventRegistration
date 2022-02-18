using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeamEventRegistration.Core.Services
{
    public class EmailService : IEmailSender
    {
        Uri _sendEndpoint { get; set; }
        string _key { get; set; }
        public EmailService(Uri sendEndpoint, string key)
        {
            _sendEndpoint = sendEndpoint;
            _key = key;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmailAsync(new EmailSendOptions {
                api_key = _key,
                sender = "noreply@teameventregistration.app",
                to = new List<string> { email },
                subject = subject,
                html_body = htmlMessage
                }
            );
        }

        public async Task<HttpResponseMessage> SendEmailAsync(EmailSendOptions options)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(_sendEndpoint.AbsoluteUri, new StringContent(JsonSerializer.Serialize(options), Encoding.UTF8, "application/json"))) 
                {
                    return response;
                }
            }
        }
    }

    public class EmailSendOptions 
    {
        public string api_key { get; set; }
        public string sender { get; set; }
        public ICollection<string> to { get; set; }
        public ICollection<string> cc { get; set; }
        public ICollection<string> bcc { get; set; }
        public string subject { get; set; }
        public string text_body { get; set; }
        public string html_body { get; set; }
        public ICollection<HttpHeaders> custom_headers { get; set; }
    }
}
