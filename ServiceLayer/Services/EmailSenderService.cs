﻿using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using ServiceLayer.Common.Interfaces;
using ServiceLayer.ErrorHandling.Exceptions;
using ServiceLayer.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ServiceLayer.Common.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private SendGridClient _client;

        /// <summary>
        /// Sets up client based on saved SendGrid Api key, that is expected to be saved in configuration.
        /// </summary>
        /// <param name="config"></param>
        public EmailSenderService(IConfiguration config)
            => _client = new SendGridClient(config["SendGridApiKey"]);

        /// <summary>
        /// Sends email on specified address with proper design.
        /// </summary>
        /// <param name="sendEmailData">Details about email, such as sender's email, receiver's email...</param>
        ///<exception cref="EmailSenderException"></exception>
        public async Task SendTemplateEmailAsync(TemplateEmail sendEmailData)
        {
            EmailAddress fromAddress = new EmailAddress(sendEmailData.From);
            EmailAddress toAddress = new EmailAddress(sendEmailData.To);
            SendGridMessage msg = new SendGridMessage
            {
                From = fromAddress,
                TemplateId = sendEmailData.TemplateId
            };
            msg.AddTo(toAddress);
            msg.SetTemplateData(sendEmailData.TemplateData);
            try
            {
                Response response = await _client.SendEmailAsync(msg);
                if (response.StatusCode != HttpStatusCode.Accepted)
                    throw new EmailSenderException(await response.Body.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                // SendGrid did not document what exception is thrown in case of an error
                throw new EmailSenderException("Unable to send email via SendGrid", ex);
            }
        }
    }
}
