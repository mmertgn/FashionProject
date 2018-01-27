using FashionStore.Entity.Entities;
using FashionStore_BLL.Services.Abstracts;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using FashionStore.Repository.Repositories.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class MailManager : IMessaging
    {
        private readonly IUnitOfWork _unitOfWork;
        public MailManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool IsSucceed { get; private set; }

        public void SendMessage(MessageTemplate msj)
        {
            var _emailAccount = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
            var message = new MailMessage
            {
                From = new MailAddress(msj.From),
                IsBodyHtml = true,
                Body = msj.MessageBody,
                Subject = msj.MessageSubject,
            };
            message.To.Add(msj.To);


            var client = new SmtpClient
            {
                UseDefaultCredentials = _emailAccount.UseDefaultCredentials,
                Credentials = new NetworkCredential(_emailAccount.Email, _emailAccount.Password),
                Host = _emailAccount.Host,
                Port = _emailAccount.Port,
                EnableSsl = _emailAccount.EnableSsl
            };

            try
            {
                client.Send(message);
                IsSucceed = true;

            }
            catch (Exception exception)
            {
                var asd = exception;
                IsSucceed = false;
            }
        }

    }
}
