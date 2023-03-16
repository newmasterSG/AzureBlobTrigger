using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using SendGrid.Helpers.Mail;
using System.Text;

namespace AzureBlobTrigger
{
    public class BlobTrigger
    {
        [FunctionName("BlobTrigger")]
        public void Run([BlobTrigger("azuretestingdemocontainer/{name}", Connection = "AzureBlobStorageKey")] Stream myBlob, string name, ILogger log)
        {
            // Send email notification
            SendEmail(GetEmail(name));
        }

        string GetEmail(string name)
        {
            name = name.Substring(0, name.IndexOf("_"));
            return name;
        }

        async Task SendEmail(string enail)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridApiKey"));
            var from = new EmailAddress("testing.project.ts@gmail.com", "Sender Name");
            var to = new EmailAddress(enail, "Recipient Name");
            var subject = "New file uploaded to BLOB storage";
            var body = $"The file was successfully uploaded to BLOB storage.";
            var message = MailHelper.CreateSingleEmail(from, to, subject, body, null);
            await client.SendEmailAsync(message);
        }
    }
}
