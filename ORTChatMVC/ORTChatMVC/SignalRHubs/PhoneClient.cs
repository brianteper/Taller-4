using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORTChatMVC.SignalRHubs
{
    public class PhoneClient
    {
        public string PhoneClientId
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