using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTChatWP8.SignalR
{
    public class SignalREventArgs : EventArgs
    {
        public string ChatMessageFromServer
        {
            get;
            set;
        }
    }
}
