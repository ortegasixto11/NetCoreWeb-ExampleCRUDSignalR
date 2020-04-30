﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSender
{
    // https://code-maze.com/send-email-with-attachments-aspnetcore-2/
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
