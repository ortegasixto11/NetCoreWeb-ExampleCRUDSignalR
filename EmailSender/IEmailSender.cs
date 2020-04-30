using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
