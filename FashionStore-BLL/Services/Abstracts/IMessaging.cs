using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore_BLL.Services.Abstracts
{
        public class MessageTemplate
        {
            public string From { get; set; }
            public string To { get; set; }
            public string MessageBody { get; set; }
            public string MessageSubject { get; set; }


        }

        public interface IMessaging
        {
            bool IsSucceed { get; }
            void SendMessage(MessageTemplate msj);
        }
}
