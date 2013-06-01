using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORTChatMVC.SignalRHubs
{
    public class Client
    {
        public string ClientId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }
}